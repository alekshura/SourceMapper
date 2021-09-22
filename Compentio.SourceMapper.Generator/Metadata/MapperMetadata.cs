using Compentio.SourceMapper.Attributes;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.SourceMapper.Metadata
{
    /// <summary>
    /// Encapsulates all data for one abstract class or interface that define mappings.
    /// </summary>
    interface IMapperMetadata
    {
        /// <summary>
        /// Type of the class or interface that defines the mappings. 
        /// For now used: TypeKind.Class and TypeKind.Interface
        /// </summary>
        TypeKind TypeKind { get; }
        /// <summary>
        /// File name for generated mappings class 
        /// </summary>
        string FileName { get; }
        /// <summary>
        /// Namespace of generated mapping class
        /// </summary>
        string Namespace { get; }
        /// <summary>
        /// Name of abstract class or interface that define the mappings
        /// </summary>
        string MapperName { get; }
        /// <summary>
        /// The name of the generated class with mappings
        /// </summary>
        string TargetClassName { get; }
        IEnumerable<IMethodMetadata> MethodsMetadata { get; }
        /// <summary>
        /// Returns method data that defined in mapping corresponds to source and target types.
        /// This method can be used in mappings od nested types.
        /// </summary>
        /// <param name="source">Source property metadata</param>
        /// <param name="target">Target property metadata</param>
        /// <returns></returns>
        IMethodMetadata FindDefinedMethod(IPropertyMetadata source, IPropertyMetadata target);
    }

    internal class MapperMetadata : IMapperMetadata
    {
        private readonly ITypeSymbol _typeSymbol;

        public MapperMetadata(ITypeSymbol typeSymbol)
        {
            _typeSymbol = typeSymbol;
        }
        public TypeKind TypeKind => _typeSymbol.TypeKind;
        public string FileName => $"{TargetClassName}.cs";
        public string MapperName => _typeSymbol.Name;
        public string Namespace => _typeSymbol.ContainingNamespace.ToString();
        
        public string TargetClassName
        {
            get
            {
                var attribute = _typeSymbol.GetAttributes().FirstOrDefault(attribute => attribute is not null && attribute.AttributeClass?.Name == nameof(MapperAttribute));
                var mapperAttribute  = attribute?.NamedArguments.FirstOrDefault(arg => arg.Key == nameof(MapperAttribute.ClassName));
                var className = mapperAttribute?.Value.Value as string;

                if (!string.IsNullOrWhiteSpace(className))
                    return className;

                className = MapperName.TrimStart('I', 'i');
                if (className.Equals(MapperName, StringComparison.InvariantCultureIgnoreCase)) 
                {
                    className = $"{MapperName}Impl";
                }
                return className;
            }
        }

        public IEnumerable<IMethodMetadata> MethodsMetadata => _typeSymbol.GetMembers()
                    .Where(field => field.Kind == SymbolKind.Method && field.IsAbstract)
                    .Select(method =>
                    {
                        return new MethodMetadata(method as IMethodSymbol);
                    });

        public IMethodMetadata FindDefinedMethod(IPropertyMetadata source, IPropertyMetadata target)
        {
            var method = MethodsMetadata.FirstOrDefault(m =>
                m.ReturnType.FullName == target.FullName && m.Parameters.FirstOrDefault().FullName == source.FullName);
            return method;
        }
    }
}
