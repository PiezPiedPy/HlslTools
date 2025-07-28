using ShaderTools.CodeAnalysis.Hlsl.Syntax;
using ShaderTools.CodeAnalysis.Symbols;
using ShaderTools.CodeAnalysis.Syntax;
using ShaderTools.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace ShaderTools.CodeAnalysis.Hlsl.Symbols
{
    public class ToggleSymbol : Symbol
    {
        public ToggleDefinitionSyntax Syntax { get; }

        public ToggleSymbol(ToggleDefinitionSyntax syntax) : base(SymbolKind.Toggle, syntax.Name.Name.Text, $"Current Value: {syntax.GetDefaultValue()}", null)
        {
            Syntax = syntax;
            SourceTree = syntax.SyntaxTree;
            Locations = ImmutableArray.Create(syntax.Name.SourceRange);
            DeclaringSyntaxNodes = ImmutableArray.Create((SyntaxNodeBase)syntax);
        }

        public override SyntaxTreeBase SourceTree { get; }

        public override ImmutableArray<SourceRange> Locations { get; }

        public override ImmutableArray<SyntaxNodeBase> DeclaringSyntaxNodes { get; }
    }
}
