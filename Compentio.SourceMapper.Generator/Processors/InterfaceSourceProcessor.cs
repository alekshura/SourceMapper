using Compentio.SourceMapper.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;

namespace Compentio.SourceMapper.Processors
{
    internal class InterfaceSourceProcessor : ISourceProcessor
    {
        private readonly ISourceMetadata _sourceMetadata;

        internal InterfaceSourceProcessor(ISourceMetadata sourceMetadata)
        {
            _sourceMetadata = sourceMetadata;
        }

        public string FileName => _sourceMetadata.FileName;

        public string GenerateCode()
        {
            var result = @$"// <mapper-source-generated />

            using System;

            {(string.IsNullOrWhiteSpace(_sourceMetadata.Namespace) ? null : $"namespace {_sourceMetadata.Namespace}")}
            {{
               public class {_sourceMetadata.TargetClassName} : {_sourceMetadata.MapperName}
               {{
                  public static {_sourceMetadata.TargetClassName} Create() => new();
                  
                   { GenerateMethods() }                  
               }}
            }}
            ";

            var tree = CSharpSyntaxTree.ParseText(result);
            var root = tree.GetRoot().NormalizeWhitespace();
            return root.ToFullString();
        }

        private string GenerateMethods()
        {
            var methods = string.Empty;

            foreach(var method in _sourceMetadata.MethodsMap)
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
