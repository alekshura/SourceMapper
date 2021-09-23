using Compentio.SourceMapper.Metadata;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Compentio.SourceMapper.Processors
{
    interface IProcessorStrategy
    {
        string GenerateCode(IMapperMetadata mapperMetadata);           
    }

    internal abstract class AbstractProcessorStrategy : IProcessorStrategy
    {
        public abstract string GenerateCode(IMapperMetadata mapperMetadata);
        protected string FormatCode(string code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = tree.GetRoot().NormalizeWhitespace();
            return root.ToFullString();
        }
        protected void Warning()
        {

        }
        protected void Error()
        {

        }
    }
}
