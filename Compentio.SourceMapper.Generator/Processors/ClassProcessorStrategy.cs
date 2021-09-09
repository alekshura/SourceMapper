using Compentio.SourceMapper.Metadata;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;

namespace Compentio.SourceMapper.Processors
{
    internal class ClassProcessorStrategy : AbstractProcessorStrategy
    {
        public override string GenerateCode(ISourceMetadata sourceMetadata)
        {
            var result = @$"// <mapper-source-generated />

            using System;

            {(string.IsNullOrWhiteSpace(sourceMetadata.Namespace) ? null : $"namespace {sourceMetadata.Namespace}")}
            {{
               public class {sourceMetadata.MapperName}
               {{                  
                   { GenerateMethods(sourceMetadata) }                  
               }}
            }}
            ";

            var tree = CSharpSyntaxTree.ParseText(result);
            var root = tree.GetRoot().NormalizeWhitespace();
            return root.ToFullString();
        }
    }
}
