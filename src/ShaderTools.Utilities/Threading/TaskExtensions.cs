﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ShaderTools.Utilities.ErrorReporting;

namespace ShaderTools.Utilities.Threading
{
    internal static class TaskExtensions
    {
        public static T WaitAndGetResult<T>(this Task<T> task, CancellationToken cancellationToken)
        {
#if DEBUG
            var threadKind = ForegroundThreadDataInfo.CurrentForegroundThreadDataKind;
            if (threadKind == ForegroundThreadDataKind.Unknown)
            {
                // If you hit this when running tests then your code is in error.  WaitAndGetResult
                // should only be called from a foreground thread.  There are a few ways you may 
                // want to fix this.
                //
                // First, if you're actually calling this directly *in test code* then you could 
                // either:
                //
                //  1) Mark the test with [WpfFact].  This is not preferred, and should only be
                //     when testing an actual UI feature (like command handlers).
                //  2) Make the test actually async (preferred).
                //
                // If you are calling WaitAndGetResult from product code, then that code must
                // be a foreground thread (i.e. a command handler).  It cannot be from a threadpool
                // thread *ever*.
                throw new InvalidOperationException($"{nameof(WaitAndGetResult)} can only be called from a 'foreground' thread.");
            }
#endif

            return WaitAndGetResult_CanCallOnBackground(task, cancellationToken);
        }

        // Only call this *extremely* special situations.  This will synchronously block a threadpool
        // thread.  In the future we are going to be removing this and disallowing its use.
        public static T WaitAndGetResult_CanCallOnBackground<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            task.Wait(cancellationToken);
            return task.Result;
        }

        // NOTE(cyrusn): Once we switch over to .Net 4.5 we can make our SafeContinueWith overloads
        // simply call into task.ContinueWith(..., TaskContinuationOptions.LazyCancellation, ...) as
        // that will have the semantics that we want.  From the TPL guys:
        //
        //   In this situation:
#if false
        Task A = Task.Run(...);
        Task B = A.ContinueWith(..., cancellationToken);
        Task C = B.ContinueWith(...);
#endif
        // If "cancellationToken" is signaled, B completes immediately (if it has not yet started).
        // Which means that C can start before A completes, which would seem to violate the rules of
        // the dependency chain.
        //
        // We've added TaskContinuationOptions.LazyCancellation option to signify "this continuation
        // will not complete due to cancellation until its antecedent has completed".  We considered
        // simply changing the default underlying behavior, but rejected that idea because there was
        // a good chance that existing users had already drawn a dependency on the current behavior.

        public static Task SafeContinueWith(
            this Task task,
            Action<Task> continuationAction,
            TaskContinuationOptions continuationOptions,
            TaskScheduler scheduler,
            CancellationToken cancellationToken)
        {
            Func<Task, bool> continuationFunction = antecedent =>
            {
                continuationAction(antecedent);
                return true;
            };

            return task.SafeContinueWith(continuationFunction, continuationOptions, scheduler, cancellationToken);
        }

        public static Task<TResult> SafeContinueWith<TInput, TResult>(
            this Task<TInput> task,
            Func<Task<TInput>, TResult> continuationFunction,
            TaskContinuationOptions continuationOptions,
            TaskScheduler scheduler,
            CancellationToken cancellationToken)
        {
            return task.SafeContinueWith<TResult>(
                (Task antecedent) => continuationFunction((Task<TInput>)antecedent), continuationOptions, scheduler, cancellationToken);
        }

        public static Task SafeContinueWith<TInput>(
            this Task<TInput> task,
            Action<Task<TInput>> continuationAction,
            TaskContinuationOptions continuationOptions,
            TaskScheduler scheduler,
            CancellationToken cancellationToken)
        {
            return task.SafeContinueWith(
                (Task antecedent) => continuationAction((Task<TInput>)antecedent), continuationOptions, scheduler, cancellationToken);
        }

        public static Task<TResult> SafeContinueWith<TResult>(
            this Task task,
            Func<Task, TResult> continuationFunction,
            TaskContinuationOptions continuationOptions,
            TaskScheduler scheduler,
            CancellationToken cancellationToken)
        {
            // So here's the deal.  Say you do the following:
#if false
            // CancellationToken ct1 = ..., ct2 = ...;
 
            // Task A = Task.Factory.StartNew(..., ct1);
            // Task B = A.ContinueWith(..., ct1);
            // Task C = B.ContinueWith(..., ct2);
#endif
            // If ct1 is cancelled then the following may occur: 
            // 1) Task A can still be running (as it hasn't responded to the cancellation request
            //    yet).
            // 2) Task C can start running.  How?  Well if B hasn't started running, it may
            //    immediately transition to the 'Cancelled/Completed' state.  Moving to that state will
            //    immediately trigger C to run.
            //
            // We do not want this, so we pass the LazyCancellation flag to the TPL which implements
            // the behavior we want.

            Func<Task, TResult> outerFunction = t =>
            {
                try
                {
                    return continuationFunction(t);
                }
                catch (Exception e) when (FatalError.ReportUnlessCanceled(e))
                {
                    throw ExceptionUtilities.Unreachable;
                }
            };

            // This is the only place in the code where we're allowed to call ContinueWith.
            return task.ContinueWith(outerFunction, cancellationToken, continuationOptions | TaskContinuationOptions.LazyCancellation, scheduler);
        }

        // Code provided by Stephen Toub.
        public static Task<TResult> ContinueWithAfterDelay<TInput, TResult>(
            this Task<TInput> task,
            Func<Task<TInput>, TResult> continuationFunction,
            int millisecondsDelay,
            TaskContinuationOptions taskContinuationOptions,
            TaskScheduler scheduler,
            CancellationToken cancellationToken)
        {
            return task.SafeContinueWith(t =>
                Task.Delay(millisecondsDelay, cancellationToken).SafeContinueWith(
                    _ => continuationFunction(t), TaskContinuationOptions.None, scheduler, cancellationToken),
                taskContinuationOptions, scheduler, cancellationToken).Unwrap();
        }

        public static Task<TNResult> ContinueWithAfterDelay<TNResult>(
            this Task task,
            Func<Task, TNResult> continuationFunction,
            int millisecondsDelay,
            TaskContinuationOptions taskContinuationOptions,
            TaskScheduler scheduler,
            CancellationToken cancellationToken)
        {
            return task.SafeContinueWith(t =>
                Task.Delay(millisecondsDelay, cancellationToken).SafeContinueWith(
                    _ => continuationFunction(t), TaskContinuationOptions.None, scheduler, cancellationToken),
                taskContinuationOptions, scheduler, cancellationToken).Unwrap();
        }

        public static Task ContinueWithAfterDelay(
            this Task task,
            Action continuationAction,
            int millisecondsDelay,
            TaskContinuationOptions taskContinuationOptions,
            TaskScheduler scheduler,
            CancellationToken cancellationToken)
        {
            return task.SafeContinueWith(t =>
                Task.Delay(millisecondsDelay, cancellationToken).SafeContinueWith(
                    _ => continuationAction(), TaskContinuationOptions.None, scheduler, cancellationToken),
                taskContinuationOptions, scheduler, cancellationToken).Unwrap();
        }

        public static Task<TResult> SafeContinueWithFromAsync<TInput, TResult>(
            this Task<TInput> task,
            Func<Task<TInput>, Task<TResult>> continuationFunction,
            TaskContinuationOptions continuationOptions,
            TaskScheduler scheduler,
            CancellationToken cancellationToken)
        {
            return task.SafeContinueWithFromAsync<TResult>(
                (Task antecedent) => continuationFunction((Task<TInput>)antecedent), continuationOptions, scheduler, cancellationToken);
        }

        public static Task<TResult> SafeContinueWithFromAsync<TResult>(
            this Task task,
            Func<Task, Task<TResult>> continuationFunction,
            TaskContinuationOptions continuationOptions,
            TaskScheduler scheduler,
            CancellationToken cancellationToken)
        {
            // So here's the deal.  Say you do the following:
#if false
            // CancellationToken ct1 = ..., ct2 = ...;
 
            // Task A = Task.Factory.StartNew(..., ct1);
            // Task B = A.ContinueWith(..., ct1);
            // Task C = B.ContinueWith(..., ct2);
#endif
            // If ct1 is cancelled then the following may occur: 
            // 1) Task A can still be running (as it hasn't responded to the cancellation request
            //    yet).
            // 2) Task C can start running.  How?  Well if B hasn't started running, it may
            //    immediately transition to the 'Cancelled/Completed' state.  Moving to that state will
            //    immediately trigger C to run.
            //
            // We do not want this, so we pass the LazyCancellation flag to the TPL which implements
            // the behavior we want.
            // This is the only place in the code where we're allowed to call ContinueWith.
            var nextTask = task.ContinueWith(continuationFunction, cancellationToken, continuationOptions | TaskContinuationOptions.LazyCancellation, scheduler).Unwrap();

            nextTask.ContinueWith(ReportFatalError, continuationFunction,
               CancellationToken.None,
               TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously,
               TaskScheduler.Default);

            return nextTask;
        }

        public static Task SafeContinueWithFromAsync(
            this Task task,
            Func<Task, Task> continuationFunction,
            TaskContinuationOptions continuationOptions,
            TaskScheduler scheduler,
            CancellationToken cancellationToken)
        {
            // So here's the deal.  Say you do the following:
#if false
            // CancellationToken ct1 = ..., ct2 = ...;
 
            // Task A = Task.Factory.StartNew(..., ct1);
            // Task B = A.ContinueWith(..., ct1);
            // Task C = B.ContinueWith(..., ct2);
#endif
            // If ct1 is cancelled then the following may occur: 
            // 1) Task A can still be running (as it hasn't responded to the cancellation request
            //    yet).
            // 2) Task C can start running.  How?  Well if B hasn't started running, it may
            //    immediately transition to the 'Cancelled/Completed' state.  Moving to that state will
            //    immediately trigger C to run.
            //
            // We do not want this, so we pass the LazyCancellation flag to the TPL which implements
            // the behavior we want.
            // This is the only place in the code where we're allowed to call ContinueWith.
            var nextTask = task.ContinueWith(continuationFunction, cancellationToken, continuationOptions | TaskContinuationOptions.LazyCancellation, scheduler).Unwrap();
            ReportFatalError(nextTask, continuationFunction);
            return nextTask;
        }

        public static Task<TNResult> ContinueWithAfterDelayFromAsync<TNResult>(
            this Task task,
            Func<Task, Task<TNResult>> continuationFunction,
            int millisecondsDelay,
            TaskContinuationOptions taskContinuationOptions,
            TaskScheduler scheduler,
            CancellationToken cancellationToken)
        {
            return task.SafeContinueWith(t =>
                Task.Delay(millisecondsDelay, cancellationToken).SafeContinueWithFromAsync(
                    _ => continuationFunction(t), TaskContinuationOptions.None, scheduler, cancellationToken),
                taskContinuationOptions, scheduler, cancellationToken).Unwrap();
        }

        public static Task ContinueWithAfterDelayFromAsync(
            this Task task,
            Func<Task, Task> continuationFunction,
            int millisecondsDelay,
            TaskContinuationOptions taskContinuationOptions,
            TaskScheduler scheduler,
            CancellationToken cancellationToken)
        {
            return task.SafeContinueWith(t =>
                Task.Delay(millisecondsDelay, cancellationToken).SafeContinueWithFromAsync(
                    _ => continuationFunction(t), TaskContinuationOptions.None, scheduler, cancellationToken),
                taskContinuationOptions, scheduler, cancellationToken).Unwrap();
        }

        internal static void ReportFatalError(Task task, object continuationFunction)
        {
            task.ContinueWith(ReportFatalErrorWorker, continuationFunction,
               CancellationToken.None,
               TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously,
               TaskScheduler.Default);
        }

        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        private static void ReportFatalErrorWorker(Task task, object continuationFunction)
        {
            var exception = task.Exception;
            var methodInfo = ((Delegate)continuationFunction).GetMethodInfo();
            exception.Data["ContinuationFunction"] = methodInfo.DeclaringType.FullName + "::" + methodInfo.Name;

            // In case of a crash with ExecutionEngineException w/o call stack it might be possible to get the stack trace using WinDbg:
            // > !threads // find thread with System.ExecutionEngineException
            //   ...
            //   67   65 4760 692b5d60   1029220 Preemptive  CD9AE70C:FFFFFFFF 012ad0f8 0     MTA (Threadpool Worker) System.ExecutionEngineException 03c51108 
            //   ...
            // > ~67s     // switch to thread 67
            // > !dso     // dump stack objects
            FatalError.Report(exception);
        }
    }
}
