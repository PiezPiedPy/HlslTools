// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using Microsoft;
using Microsoft.VisualStudio.ComponentModelHost;

namespace ShaderTools.VisualStudio.LanguageServices.Utilities.Extensions
{
    internal static class ServiceProviderExtensions
    {
        public static T GetMefService<T>(this IServiceProvider serviceProvider) where T : class
        {
            var componentModel = (IComponentModel) serviceProvider.GetService(typeof(SComponentModel));
            Assumes.Present(componentModel);
            return componentModel.GetService<T>();
        }
    }
}
