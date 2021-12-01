using Compentio.SourceMapper.Attributes;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.SourceMapper.Metadata
{
    /// <summary>
    /// Encapsulates a method from mapper that is used for mappings
    /// </summary>
    internal interface IMethodMetadata : IMetadata
    {
        /// <summary>
        /// Method return type
        /// </summary>
        ITypeMetadata ReturnType { get; }

        /// <summary>
        /// Parameters of the method
        /// </summary>
        IEnumerable<ITypeMetadata> Parameters { get; }

        /// <summary>
        /// Method full name with return type and its namespace and parameters.
        /// This name is ready to be used in code generation.
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Name of the method inverse for this method
        /// </summary>
        string InverseMethodName { get; }

        /// <summary>
        /// Attributes that used for mappings
        /// </summary>
        IEnumerable<MappingAttribute> MappingAttributes { get; }
    }

    internal class MethodMetadata : IMethodMetadata
    {
        private readonly IMethodSymbol _methodSymbol;

        private readonly SymbolDisplayFormat _methodFullNameFormat =
            new(parameterOptions: SymbolDisplayParameterOptions.IncludeName | SymbolDisplayParameterOptions.IncludeType,
                   memberOptions: SymbolDisplayMemberOptions.IncludeParameters,
                   typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);

        internal MethodMetadata(IMethodSymbol methodSymbol)
        {
            _methodSymbol = methodSymbol;
        }

        public string Name
        {
            get
            {
                var parts = _methodSymbol.ToDisplayParts(SymbolDisplayFormat.CSharpShortErrorMessageFormat)
                    .Where(s => s.Kind == SymbolDisplayPartKind.MethodName);
                return $"{string.Join(string.Empty, parts)}";
            }
        }

        public ITypeMetadata ReturnType => new TypeMetadata(_methodSymbol.ReturnType);
        public IEnumerable<ITypeMetadata> Parameters => _methodSymbol.Parameters.Select(p => new ParameterTypeMetadata(p));

        public string FullName => $"{ReturnType.FullName} {_methodSymbol?.ToDisplayString(_methodFullNameFormat)}";

        public IEnumerable<MappingAttribute> MappingAttributes => _methodSymbol.GetAttributes()
                    .Where(attribute => attribute is not null && attribute.AttributeClass?.Name == nameof(MappingAttribute))
                    .Select(attribute =>
                    {
                        var sourceConstant = attribute.NamedArguments.FirstOrDefault(x => x.Key == nameof(MappingAttribute.Source)).Value;
                        var targetConstant = attribute.NamedArguments.FirstOrDefault(x => x.Key == nameof(MappingAttribute.Target)).Value;
                        var expressionConstant = attribute.NamedArguments.FirstOrDefault(x => x.Key == nameof(MappingAttribute.Expression)).Value;

                        var mappingAttr = new MappingAttribute
                        {
                            Source = sourceConstant.Value as string ?? string.Empty,
                            Target = targetConstant.Value as string ?? string.Empty,
                            Expression = expressionConstant.Value as string ?? string.Empty,
                        };
                        return mappingAttr;
                    });

        public string InverseMethodName
        {
            get
            {
                var attribute = _methodSymbol.GetAttributes().FirstOrDefault(attribute => attribute is not null && attribute.AttributeClass?.Name == nameof(InverseMappingAttribute));
                var mapperAttribute = attribute?.NamedArguments.FirstOrDefault(arg => arg.Key == nameof(InverseMappingAttribute.InverseMethodName));
                return mapperAttribute?.Value.Value as string;
            }
        }

        public Location? Location => _methodSymbol.Locations.FirstOrDefault();
    }
}