using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using MediatR;
using OmniSharp.Extensions.JsonRpc;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
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
    
    [Parallel]
    [Method("textDocument/completion", Direction.ClientToServer)]
    public interface ITextureRegisterHandler : 
        IJsonRpcRequestHandler<CompletionParams, CompletionList>,
        IRequestHandler<CompletionParams, CompletionList>,
        IJsonRpcHandler,
        IRegistration<CompletionRegistrationOptions>,
        ICapability<CompletionCapability>
    {
    }

    internal class TextureRegisterHandler : ITextureRegisterHandler
    {
        public async Task<CompletionList> Handle(CompletionParams request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public CompletionRegistrationOptions GetRegistrationOptions()
        {
            throw new NotImplementedException();
        }

        public void SetCapability(CompletionCapability capability)
        {
            throw new NotImplementedException();
        }
    }
}
#pragma warning restore 618