using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Compentio.SourceMapper.Processors
{
    internal class ClassProcessorStrategy : AbstractProcessorStrategy
    {
        public override string GenerateCode()
        {
            var result = @$"// <mapper-source-generated />

            using System;

            {(string.IsNullOrWhiteSpace(_sourceMetadata.Namespace) ? null : $"namespace {_sourceMetadata.Namespace}")}
            {{
               public class {_sourceMetadata.MapperName}
               {{                  
                   { GenerateMethods(_sourceMetadata) }                  
               }}
            }}
            ";

            var tree = CSharpSyntaxTree.ParseText(result);
            var root = tree.GetRoot().NormalizeWhitespace();
            return root.ToFullString();
        }
    }
}
