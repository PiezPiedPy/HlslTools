using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using ShaderTools.CodeAnalysis;
using ShaderTools.CodeAnalysis.ReferenceHighlighting;

namespace ShaderTools.LanguageServer.Handlers
{
    internal sealed class DocumentHighlightHandler : IDocumentHighlightHandler
    {
        private readonly LanguageServerWorkspace _workspace;
        private readonly DocumentHighlightRegistrationOptions _registrationOptions;

        public DocumentHighlightHandler(LanguageServerWorkspace workspace, DocumentHighlightRegistrationOptions registrationOptions)
        {
            _workspace = workspace;
            _registrationOptions = registrationOptions;
        }

        DocumentHighlightRegistrationOptions IRegistration<DocumentHighlightRegistrationOptions>.GetRegistrationOptions() => _registrationOptions;

        public async Task<DocumentHighlightContainer> Handle(DocumentHighlightParams request, CancellationToken token)
        {
            var (document, position) = _workspace.GetLogicalDocument(request);

            var documentHighlightsService = _workspace.Services.GetService<IDocumentHighlightsService>();

            var documentHighlightsList = await documentHighlightsService.GetDocumentHighlightsAsync(
                document, position,
                ImmutableHashSet<Document>.Empty, // TODO
                token);

            var result = new List<DocumentHighlight>();

            foreach (var documentHighlights in documentHighlightsList)
            {
                if (documentHighlights.Document != document)
                {
                    continue;
                }

                foreach (var highlightSpan in documentHighlights.HighlightSpans)
                {
                    result.Add(new DocumentHighlight
                    {
                        Kind = highlightSpan.Kind == HighlightSpanKind.Definition
                            ? DocumentHighlightKind.Write
                            : DocumentHighlightKind.Read,
                        Range = Helpers.ToRange(document.SourceText, highlightSpan.TextSpan)
                    });
                }
            }

            return result;
        }

        public void SetCapability(DocumentHighlightCapability capability) { }
    }
}
