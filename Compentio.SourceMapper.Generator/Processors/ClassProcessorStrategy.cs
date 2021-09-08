using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;

namespace Compentio.SourceMapper.Processors
{
    internal class ClassProcessorStrategy : IProcessorStrategy
    {
        public string GenerateCode(ISourceMetadata sourceMetadata)
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

        private string GenerateMethods(ISourceMetadata sourceMetadata)
        {
            var methods = string.Empty;

            foreach(var methodMetadata in sourceMetadata.MethodsMetadata)
            {
                methods += @$"public virtual {methodMetadata.MethodFullName}
                    {{
                        {GenerateMethodBody(methodMetadata)}
                    }}";
            }

            return methods;
        }

        private string GenerateMethodBody(MethodMetadata metadata)
        {
            var methodBody = $"var target = new {metadata.ReturnType}();";

            foreach (var targetMember in metadata.ReturnType.GetMembers().Where(member => member.Kind == SymbolKind.Property && !member.IsStatic))
            {
                var matchedField = metadata.Parameters.First().GetMembers().FirstOrDefault(p => p.Name == targetMember.Name);

                if (matchedField is not null)
                {
                    methodBody += "\n";
                    methodBody += $"target.{matchedField.Name} = source.{matchedField.Name};";
                }

                var matchedAttribute = metadata.MappingAttributes.FirstOrDefault(attribute => attribute?.Target == targetMember.Name);
                if (matchedAttribute is not null)
                {
                    methodBody += "\n";
                    methodBody += $"target.{matchedAttribute.Target} = source.{matchedAttribute.Source};";
                }
            }

            methodBody += "\n";
            methodBody += $"return target;";
            return methodBody;
        }
    }
}
