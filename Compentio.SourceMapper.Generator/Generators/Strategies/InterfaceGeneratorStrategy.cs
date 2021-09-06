using Compentio.SourceMapper.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.SourceMapper.Generators.Strategies
{
    internal class InterfaceGeneratorStrategy : AbstractGeneratorStrategy
    {
        private ITypeSymbol _mapperInterface;

        private string Namespace => _mapperInterface.ContainingNamespace.ToString();

        protected override string ClassName 
        { 
            get
            {
                var className = _mapperInterface.Name.TrimStart('I', 'i');
                if (className.Equals(InterfaceName, StringComparison.InvariantCultureIgnoreCase))
                {
                    className = $"{InterfaceName}Impl";
                }
                return className;
            }
        }
        private string InterfaceName => _mapperInterface.Name;


        internal override string GenerateCode(ITypeSymbol typeSymbol)
        {
            _mapperInterface = typeSymbol;

            var result = @$"// <mapper-source-generated />

            using System;

            {(string.IsNullOrWhiteSpace(Namespace) ? null : $"namespace {Namespace}")}
            {{
               public class {ClassName} : {InterfaceName}
               {{
                  public static {ClassName} Create() => new();
                  
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

            foreach(var method in GetMethodSignatures())
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

        private IDictionary<string, IMethodSymbol> GetMethodSignatures()
        {
            var methodFormat = new SymbolDisplayFormat(parameterOptions: SymbolDisplayParameterOptions.IncludeName | SymbolDisplayParameterOptions.IncludeType,
               memberOptions: SymbolDisplayMemberOptions.IncludeParameters,
               typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);

            return _mapperInterface.GetMembers()
                .Where(field => field.Kind == SymbolKind.Method)
                .Select(method => 
                {
                    method.ToDisplayString(methodFormat);
                    var mapperInterfaceMethod = method as IMethodSymbol;
                    return new KeyValuePair<string, IMethodSymbol>($"{mapperInterfaceMethod?.ReturnType.ToDisplayString()} {method.ToDisplayString(methodFormat)}", mapperInterfaceMethod);
                })
                .ToDictionary(x => x.Key, x => x.Value);
        }      
    }
}
