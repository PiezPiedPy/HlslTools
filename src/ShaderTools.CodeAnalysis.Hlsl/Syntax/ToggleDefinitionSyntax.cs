using System;
using System.Collections.Generic;
using System.Text;

namespace ShaderTools.CodeAnalysis.Hlsl.Syntax
{
    public sealed partial class ToggleDefinitionSyntax : InitializerSyntax
    {
        public VariableDeclarationStatementSyntax declaration { set; get; }

        public string GetDefaultValue()
        {
            var value = false;
            foreach (var property in StateInitializer.Properties)
            {
                if (property.Name.Text.Equals("Default"))
                {
                    if (property.Value.Kind == SyntaxKind.TrueLiteralExpression)
                    {
                        value = true;
                    }
                }
            }
            return value.ToString();
        }
    }

    public sealed partial class ToggleNameSyntax : NameSyntax
    {
        public override IdentifierNameSyntax GetUnqualifiedName()
        {
            return new IdentifierNameSyntax(name);
        }
    }
}
