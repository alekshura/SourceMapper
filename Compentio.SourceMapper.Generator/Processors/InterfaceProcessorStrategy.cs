using Compentio.SourceMapper.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;

namespace Compentio.SourceMapper.Processors
{
    internal class InterfaceProcessorStrategy : IProcessorStrategy
    {
        public string GenerateCode(ISourceMetadata sourceMetadata)
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

        private string GenerateMethods(ISourceMetadata sourceMetadata)
        {
            var methods = string.Empty;

            foreach(var method in sourceMetadata.MethodsMap)
            {
                methods += @$"public {method.Key}
                    {{
                        {GenerateMethodBody(method.Value)}
                    }}";
            }

            return methods;
        }

        private string GenerateMethodBody(IMethodSymbol methodSymbol)
        {
            var mapperSourceParameter = methodSymbol.Parameters.First().Type;
            var mapperTargetParameter = methodSymbol.ReturnType;

            var methodBody = $"var target = new {mapperTargetParameter}();";
            var mappingAttributes = methodSymbol.GetAttributes()
                .Where(attribute => attribute is not null && attribute.AttributeClass?.Name == nameof(MappingAttribute))
                .Select(attribute =>
                 {
                     var mappingAttr = new MappingAttribute();
                     mappingAttr.Source = attribute.NamedArguments.FirstOrDefault(x => x.Key == nameof(mappingAttr.Source)).Value.Value.ToString();
                     mappingAttr.Target = attribute.NamedArguments.FirstOrDefault(x => x.Key == nameof(mappingAttr.Target)).Value.Value.ToString();
                     return mappingAttr;
                 });


            foreach (var targetMember in mapperTargetParameter.GetMembers().Where(member => member.Kind == SymbolKind.Property && !member.IsStatic))
            {
                var matchedField = mapperSourceParameter.GetMembers().FirstOrDefault(p => p.Name == targetMember.Name);

                if (matchedField is not null)
                {
                    methodBody += "\n";
                    methodBody += $"target.{matchedField.Name} = source.{matchedField.Name};";
                }

                var matchedAttribute = mappingAttributes.FirstOrDefault(attribute => attribute?.Target == targetMember.Name);
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
