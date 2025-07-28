using ShaderTools.CodeAnalysis.Hlsl.Symbols;

namespace ShaderTools.CodeAnalysis.Hlsl.Binding.BoundNodes
{
    internal sealed class BoundToggleExpression : BoundExpression
    {
        public BoundToggleExpression(ToggleSymbol variableSymbol, BoundScalarType type)
            : base(BoundNodeKind.BoundToggleExpression)
        {
            Symbol = variableSymbol;
            Type = type.TypeSymbol;
        }
        public ToggleSymbol Symbol { get; }

        public override TypeSymbol Type { get; }
    }
}