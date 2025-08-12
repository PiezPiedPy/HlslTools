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
using OmniSharp.Extensions.LanguageServer.Protocol.Document.Proposals;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Models.Proposals;
using ShaderTools.CodeAnalysis;
using ShaderTools.CodeAnalysis.Classification;
using ShaderTools.CodeAnalysis.Hlsl.Syntax;
using ShaderTools.CodeAnalysis.ReferenceHighlighting;
using ShaderTools.CodeAnalysis.Shared.Extensions;
using ShaderTools.CodeAnalysis.Syntax;


#pragma warning disable 618
namespace ShaderTools.LanguageServer.Handlers
{
    internal class SemanticTokenHandler : SemanticTokensHandlerBase
    {
        static class HlslClassificationTypeNames
        {
            public const string Punctuation = "Hlsl.Punctuation";
            public const string Semantic = "Hlsl.Semantic";
            public const string PackOffset = "Hlsl.PackOffset";
            public const string RegisterLocation = "Hlsl.RegisterLocation";
            public const string NamespaceIdentifier = "Hlsl.Namespace";
            public const string GlobalVariableIdentifier = "Hlsl.GlobalVariable";
            public const string FieldIdentifier = "Hlsl.Field";
            public const string LocalVariableIdentifier = "Hlsl.LocalVariable";
            public const string ConstantBufferVariableIdentifier = "Hlsl.ConstantBufferVariable";
            public const string ParameterIdentifier = "Hlsl.Parameter";
            public const string FunctionIdentifier = "Hlsl.Function";
            public const string MethodIdentifier = "Hlsl.Method";
            public const string ClassIdentifier = "Hlsl.Class";
            public const string StructIdentifier = "Hlsl.Struct";
            public const string InterfaceIdentifier = "Hlsl.Interface";
            public const string ConstantBufferIdentifier = "Hlsl.ConstantBuffer";
            public const string MacroIdentifier = "Hlsl.Macro";
            public const string ToggleIdentifier = "Hlsl.Toggle";
            public const string AnnotationIdentifier = "Hlsl.AnnotationIdentifier";
        }

        public static readonly Dictionary<string, SemanticTokenType> ClassificationMapping =
            new()
            {
                {
                    HlslClassificationTypeNames.MacroIdentifier, SemanticTokenType.Macro
                }
            };

        public static (SemanticTokenType, List<SemanticTokenModifier>) GetClassificationType(
            string classificationTypeNames) => classificationTypeNames switch
            {
                HlslClassificationTypeNames.Punctuation => (SemanticTokenType.Label, []),
                HlslClassificationTypeNames.Semantic => (SemanticTokenType.Interface, [SemanticTokenModifier.Readonly]),
                HlslClassificationTypeNames.PackOffset => (SemanticTokenType.EnumMember, [SemanticTokenModifier.Readonly]),
                HlslClassificationTypeNames.RegisterLocation => (SemanticTokenType.EnumMember,
                    [SemanticTokenModifier.Readonly]),
                HlslClassificationTypeNames.NamespaceIdentifier => (SemanticTokenType.Namespace, []),
                HlslClassificationTypeNames.GlobalVariableIdentifier => (SemanticTokenType.Property,
                    [SemanticTokenModifier.Declaration]),
                HlslClassificationTypeNames.FieldIdentifier => (SemanticTokenType.Variable, [SemanticTokenModifier.Readonly]),
                HlslClassificationTypeNames.LocalVariableIdentifier => (SemanticTokenType.Variable, [SemanticTokenModifier.Modification]),
                HlslClassificationTypeNames.ParameterIdentifier => (SemanticTokenType.Parameter, [SemanticTokenModifier.Modification]),
                HlslClassificationTypeNames.FunctionIdentifier => (SemanticTokenType.Function, [SemanticTokenModifier.Readonly]),
                HlslClassificationTypeNames.MethodIdentifier => (SemanticTokenType.Function, []),
                HlslClassificationTypeNames.ClassIdentifier => (SemanticTokenType.Class, []),
                HlslClassificationTypeNames.StructIdentifier => (SemanticTokenType.Struct, []),
                HlslClassificationTypeNames.InterfaceIdentifier => (SemanticTokenType.Interface, []),
                HlslClassificationTypeNames.ConstantBufferVariableIdentifier => (SemanticTokenType.Variable, [SemanticTokenModifier.Declaration]),
                HlslClassificationTypeNames.ConstantBufferIdentifier => (SemanticTokenType.Class, []),
                HlslClassificationTypeNames.MacroIdentifier => (SemanticTokenType.Macro, []),
                HlslClassificationTypeNames.ToggleIdentifier => (SemanticTokenType.Macro, []),
                HlslClassificationTypeNames.AnnotationIdentifier => (SemanticTokenType.Keyword, []),
                _ => (SemanticTokenType.Label, [])
            };

        private readonly LanguageServerWorkspace _workspace;

        public SemanticTokenHandler(LanguageServerWorkspace workspace, DocumentSelector documentSelector) : base(
            new SemanticTokensRegistrationOptions()
            {
                DocumentSelector = documentSelector,
                Full = true,
                Range = true,
                Legend = new SemanticTokensLegend
                {
                    TokenTypes = SemanticTokenType.Defaults.Select(x => x.ToString()).ToArray(),
                    TokenModifiers = SemanticTokenModifier.Defaults.Select(x => x.ToString()).ToArray()
                }
            })
        {
            _workspace = workspace;
        }


        protected override async Task Tokenize(SemanticTokensBuilder builder, ITextDocumentIdentifierParams identifier,
            CancellationToken cancellationToken)
        {
            var document = _workspace.GetDocument(identifier.TextDocument.Uri);
            var ast = await document.GetSyntaxTreeAsync(cancellationToken);
            var classificationService = document?.LanguageServices.GetService<IClassificationService>();

            if (classificationService == null)
            {
                return;
            }

            var syntaxTree = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

            var classifiedSpans = new List<ClassifiedSpan>();

            classificationService.AddSemanticClassifications(
                syntaxTree,
                new TextSpan(0, document.SourceText.Length),
                _workspace,
                classifiedSpans,
                cancellationToken);

            //var unified = classifiedSpans.Select(x => x.ClassificationType)
            //    .Distinct()
            //    .ToList();

            foreach (var classifiedSpan in classifiedSpans)
            {
                if (classifiedSpan.ClassificationType == ClassificationTypeNames.WhiteSpace)
                {
                    continue;
                }

                var range = Helpers.ToRange(document.SourceText, classifiedSpan.TextSpan);
                var content = document.SourceText.GetSubText(classifiedSpan.TextSpan).ToString();
                try
                {
                    var (semanticTokenType, semanticTokenModifiers) = GetClassificationType(classifiedSpan.ClassificationType);
                    builder.Push(range, semanticTokenType, semanticTokenModifiers);
                }
                catch (Exception e)
                {
                    var a = e.Message;
                }
            }
        }

        protected override async Task<SemanticTokensDocument> GetSemanticTokensDocument(
            ITextDocumentIdentifierParams @params, CancellationToken cancellationToken)
        {
            return new SemanticTokensDocument(GetRegistrationOptions().Legend);
        }
    }
}
#pragma warning restore 618