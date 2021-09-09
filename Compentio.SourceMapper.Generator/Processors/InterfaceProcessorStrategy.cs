using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.SourceMapper.Processors
{
    internal class InterfaceProcessorStrategy : AbstractProcessorStrategy
    {
        public override string GenerateCode(ISourceMetadata sourceMetadata)
        {
            var result = @$"// <mapper-source-generated />

            using System;

            {(string.IsNullOrWhiteSpace(sourceMetadata.Namespace) ? null : $"namespace {sourceMetadata.Namespace}")}
            {{
               public class {sourceMetadata.TargetClassName} : {sourceMetadata.MapperName}
               {{
                  public static {sourceMetadata.TargetClassName} Create() => new();
                  
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
