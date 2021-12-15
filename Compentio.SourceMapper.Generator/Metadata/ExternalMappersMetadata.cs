using Compentio.SourceMapper.Attributes;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.SourceMapper.Metadata
{
    /// <summary>
    /// Encapsulates all data for list of abstract classes or/and interfaces that defines mappings in referenced assemblies
    /// </summary>
    internal interface IExternalMappersMetadata
    {
        /// <summary>
        /// Collection of referenced non public assemblies (only from current solution)
        /// </summary>
        IReadOnlyCollection<IAssemblySymbol> ExternalAssemblies { get; }

        /// <summary>
        /// Collection of referenced mappers <see cref="MapperMetadata"/>
        /// </summary>
        IReadOnlyCollection<IMapperMetadata> ExternalMappers { get; }
    }

    internal class ExternalMappersMetadata : IExternalMappersMetadata
    {
        private readonly IEnumerable<IAssemblySymbol> _assemblySymbols;

        public ExternalMappersMetadata(IEnumerable<IAssemblySymbol> assemblySymbols)
        {
            _assemblySymbols = assemblySymbols;
        }

        public IReadOnlyCollection<IAssemblySymbol> ExternalAssemblies => _assemblySymbols?.Where(a => a.Identity?.HasPublicKey == false).ToList().AsReadOnly();

        public IReadOnlyCollection<IMapperMetadata> ExternalMappers
        {
            get
            {
                var nSpaceCollection = new List<INamespaceSymbol>();

                foreach (var assembly in ExternalAssemblies)
                {
                    nSpaceCollection.AddRange(GetNamespaces(assembly.GlobalNamespace.GetNamespaceMembers()));
                }

                var typesCollection = nSpaceCollection?.SelectMany(n => n.GetTypeMembers());
                var mappersCollection = typesCollection?.Where(t => t.GetAttributes().Any(attribute => attribute is not null && attribute.AttributeClass?.Name == nameof(MapperAttribute)));

                return mappersCollection?.Select(t =>
                {
                    return new MapperMetadata(t, true);
                }).ToList().AsReadOnly();
            }
        }

        private IEnumerable<INamespaceSymbol> GetNamespaces(IEnumerable<INamespaceSymbol> namespaceSymbols)
        {
            var resultSymbols = new List<INamespaceSymbol>();

            foreach (var nSpace in namespaceSymbols)
            {
                resultSymbols.Add(nSpace);
                resultSymbols.AddRange(GetNamespaces(nSpace.GetNamespaceMembers()));
            }

            return resultSymbols;
        }
    }
}