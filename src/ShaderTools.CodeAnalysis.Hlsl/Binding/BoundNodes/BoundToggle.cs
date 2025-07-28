using System.Collections.Immutable;
using ShaderTools.CodeAnalysis.Hlsl.Symbols;

namespace ShaderTools.CodeAnalysis.Hlsl.Binding.BoundNodes
{
    internal sealed class BoundToggle : BoundNode
    {
        public ToggleSymbol ToggleSymbol { get; }
        public bool DefaultValue { get; }

        public BoundToggle(ToggleSymbol toggleSymbol, bool defaultValue)
            : base(BoundNodeKind.BoundToggleExpression)
        {
            ToggleSymbol = toggleSymbol;
            DefaultValue = defaultValue;
        }
    }
}