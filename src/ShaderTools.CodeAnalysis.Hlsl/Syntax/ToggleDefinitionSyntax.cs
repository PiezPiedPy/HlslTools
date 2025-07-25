using System;
using System.Collections.Generic;
using System.Text;

namespace ShaderTools.CodeAnalysis.Hlsl.Syntax
{
    public partial class ToggleDefinitionSyntax : InitializerSyntax
    {
        public VariableDeclarationStatementSyntax declaration { set; get; }
    }
}
