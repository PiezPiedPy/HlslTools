﻿using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Workspace;
using ShaderTools.CodeAnalysis.NavigateTo;

namespace ShaderTools.LanguageServer.Handlers
{
    internal sealed class WorkspaceSymbolsHandler : IWorkspaceSymbolsHandler
    {
        private readonly LanguageServerWorkspace _workspace;

        public WorkspaceSymbolsHandler(LanguageServerWorkspace workspace)
        {
            _workspace = workspace;
        }

        public async Task<Container<SymbolInformation>> Handle(WorkspaceSymbolParams request, CancellationToken token)
        {
            var searchService = _workspace.Services.GetService<INavigateToSearchService>();

            var symbols = ImmutableArray.CreateBuilder<SymbolInformation>();

            foreach (var document in _workspace.CurrentDocuments.Documents)
            {
                await Helpers.FindSymbolsInDocument(searchService, document, request.Query, token, symbols);
            }

            return new Container<SymbolInformation>(symbols);
        }

        public void SetCapability(WorkspaceSymbolCapability capability) { }

        WorkspaceSymbolRegistrationOptions IRegistration<WorkspaceSymbolRegistrationOptions>.GetRegistrationOptions()
        {
            return null;
        }
    }
}
