using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.Text;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using ShaderTools.CodeAnalysis;
using ShaderTools.CodeAnalysis.Classification;
using ShaderTools.CodeAnalysis.Hlsl.Syntax;
using ShaderTools.CodeAnalysis.ReferenceHighlighting;
using ShaderTools.CodeAnalysis.Shared.Extensions;
using ShaderTools.CodeAnalysis.Syntax;


namespace ShaderTools.LanguageServer.Handlers;

//internal class DocumentFormattingHandler : DocumentRangeFormattingHandler
//{
//    private readonly LanguageServerWorkspace _workspace;

//    public DocumentFormattingHandler(LanguageServerWorkspace workspace, DocumentSelector documentSelector) : base(
//        new DocumentRangeFormattingRegistrationOptions()
//        {
//            DocumentSelector = documentSelector
//        })
//    {
//        _workspace = workspace;
//    }

//    public override async Task<TextEditContainer> Handle(DocumentRangeFormattingParams request,
//        CancellationToken cancellationToken)
//    {
//        var document = _workspace.GetDocument(request.TextDocument.Uri);

//        var formattingService = document?.LanguageServices.GetService<IEditorFormattingService>();

//        if (formattingService != null)
//        {
//            var changes = await formattingService.GetFormattingChangesAsync(document,
//                Helpers.ToSpan(document.SourceText, request.Range),
//                cancellationToken);

//            //var a=new TextEditContainer(changes.Select(x=>x.NewText))

//            //return changes;
//        }

//        return null;
//    }
//}