using System.Collections.Immutable;
using ShaderTools.CodeAnalysis.Hlsl.Symbols;

namespace ShaderTools.CodeAnalysis.Hlsl.Binding.BoundNodes
{
    internal sealed class BoundToggle : BoundNode
    {
        public ToggleNameSymbol ToggleSymbol { get; }
        public bool DefaultValue { get; }

        public BoundToggle(ToggleNameSymbol toggleSymbol, bool defaultValue)
            : base(BoundNodeKind.BoundToggleExpression)
        {
            ToggleSymbol = toggleSymbol;
            DefaultValue = defaultValue;
        }
    }
}