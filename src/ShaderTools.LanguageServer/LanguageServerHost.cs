using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Server;
using Serilog;
using ShaderTools.CodeAnalysis;
using ShaderTools.CodeAnalysis.Diagnostics;
using ShaderTools.CodeAnalysis.Host.Mef;
using ShaderTools.LanguageServer.Handlers;

namespace ShaderTools.LanguageServer
{
    internal sealed class LanguageServerHost : IDisposable
    {
        private ILanguageServer _server;

        private readonly MefHostServices _exportProvider;
        private LanguageServerWorkspace _workspace;
        private DiagnosticNotifier _diagnosticNotifier;

        private readonly LoggerFactory _loggerFactory;
        private readonly Serilog.Core.Logger _logger;
        private readonly LogLevel _minLogLevel;

        private LanguageServerHost(
            MefHostServices exportProvider,
            Serilog.Core.Logger logger,
            LogLevel minLogLevel)
        {
            _exportProvider = exportProvider;
            _logger = logger;
            _minLogLevel = minLogLevel;
        }

        public static async Task<LanguageServerHost> Create(
            Stream input,
            Stream output,
            string logFilePath,
            LogLevel minLogLevel)
        {
            var exportProvider = CreateHostServices();

            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.File(logFilePath)
                .CreateLogger();

            var result = new LanguageServerHost(exportProvider, logger, minLogLevel);

            await result.InitializeAsync(input, output);

            return result;
        }

        private static MefHostServices CreateHostServices()
        {
            var assemblies = MefHostServices.DefaultAssemblies
                .Union(new[] { typeof(LanguageServerHost).GetTypeInfo().Assembly });

            return MefHostServices.Create(assemblies);
        }

        private async Task InitializeAsync(Stream input, Stream output)
        {
            _server = await OmniSharp.Extensions.LanguageServer.Server.LanguageServer.From(options => options
                .WithInput(input)
                .WithOutput(output)
                .ConfigureLogging(x => x.AddSerilog(_logger).SetMinimumLevel(_minLogLevel).AddLanguageProtocolLogging(_minLogLevel))
                .OnInitialized(OnInitialized));

            var diagnosticService = _workspace.Services.GetService<IDiagnosticService>();
            _diagnosticNotifier = new DiagnosticNotifier(_server, diagnosticService);

            var documentSelector = new DocumentSelector(
                LanguageNames.AllLanguages
                    .Select(x => new DocumentFilter
                    {
                        Language = x.ToLowerInvariant()
                    }));

            var registrationOptions = new TextDocumentRegistrationOptions
            {
                DocumentSelector = documentSelector
            };

            var definitionRegistrationOptions = new DefinitionRegistrationOptions
            {
                DocumentSelector = documentSelector
            };

            var highlightRegistrationOptions = new DocumentHighlightRegistrationOptions
            {
                DocumentSelector = documentSelector
            };

            var symbolRegistrationOptions = new DocumentSymbolRegistrationOptions
            {
                DocumentSelector = documentSelector
            };

            var hoverRegistrationOptions = new HoverRegistrationOptions
            {
                DocumentSelector = documentSelector
            };

            _server.Register(s => { s.AddHandlers(
                new TextDocumentSyncHandler(_workspace, registrationOptions),
                new CompletionHandler(_workspace, registrationOptions),
                new DefinitionHandler(_workspace, definitionRegistrationOptions),
                new WorkspaceSymbolsHandler(_workspace),
                new DocumentHighlightHandler(_workspace, highlightRegistrationOptions),
                new DocumentSymbolsHandler(_workspace, symbolRegistrationOptions),
                new HoverHandler(_workspace, hoverRegistrationOptions),
                new SignatureHelpHandler(_workspace, registrationOptions));});
        }

        private Task OnInitialized(ILanguageServer server, InitializeParams request, InitializeResult response, CancellationToken cancellationToken)
        {
            _workspace = new LanguageServerWorkspace(_exportProvider, request.RootPath);

            return Task.CompletedTask;
        }

        public Task WaitForExit => _server.WaitForExit;

        public void Dispose()
        {
            _diagnosticNotifier?.Dispose();

            _server.Dispose();

            _logger.Dispose();
            _loggerFactory.Dispose();
        }
    }
}
