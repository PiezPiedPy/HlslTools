using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using ShaderTools.CodeAnalysis.NavigateTo;

namespace ShaderTools.LanguageServer.Handlers
{
    internal sealed class DocumentSymbolsHandler : IDocumentSymbolHandler
    {
        private readonly LanguageServerWorkspace _workspace;
        private readonly DocumentSymbolRegistrationOptions _registrationOptions;

        public DocumentSymbolsHandler(LanguageServerWorkspace workspace, DocumentSymbolRegistrationOptions registrationOptions)
        {
            _workspace = workspace;
            _registrationOptions = registrationOptions;
        }

        DocumentSymbolRegistrationOptions IRegistration<DocumentSymbolRegistrationOptions>.GetRegistrationOptions() => _registrationOptions;

        public async Task<SymbolInformationOrDocumentSymbolContainer> Handle(DocumentSymbolParams request, CancellationToken token)
        {
            var document = _workspace.GetDocument(request.TextDocument.Uri.ToUri());

            var searchService = _workspace.Services.GetService<INavigateToSearchService>();

            var symbols = ImmutableArray.CreateBuilder<SymbolInformation>();

            await Helpers.FindSymbolsInDocument(searchService, document, string.Empty, token, symbols);

            var symbolsResult = ImmutableArray.CreateRange(
                symbols.ToImmutable(), 
                x => new SymbolInformationOrDocumentSymbol(x));

            return new SymbolInformationOrDocumentSymbolContainer(symbolsResult);
        }

        public void SetCapability(DocumentSymbolCapability capability) { }
    }
}
