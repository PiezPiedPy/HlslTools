using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Workspace;
using ShaderTools.CodeAnalysis.NavigateTo;

namespace ShaderTools.LanguageServer.Handlers
{
    internal sealed class WorkspaceSymbolsHandler(LanguageServerWorkspace workspace) : IWorkspaceSymbolsHandler
    {
        private readonly WorkspaceSymbolRegistrationOptions _registrationOptions = new();

        public async Task<Container<WorkspaceSymbol>> Handle(WorkspaceSymbolParams request, CancellationToken cancellationToken)
        {
            var searchService = workspace.Services.GetService<INavigateToSearchService>();

            var symbols = ImmutableArray.CreateBuilder<WorkspaceSymbol>();

            foreach (var document in workspace.CurrentDocuments.Documents)
            {
                await Helpers.FindSymbolsInDocumentAsync(searchService, document, request.Query, cancellationToken, symbols);
            }

            return new Container<WorkspaceSymbol>(symbols);
        }

        public WorkspaceSymbolRegistrationOptions GetRegistrationOptions(WorkspaceSymbolCapability capability,
            ClientCapabilities clientCapabilities)
        {
            return _registrationOptions;
        }
    }
}
