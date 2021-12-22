using Compentio.SourceMapper.Attributes;
using Compentio.SourceMapper.Processors.DependencyInjection;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Compentio.SourceMapper.Metadata
{
    /// <summary>
    /// Encapsulates all defined mappers in assembly
    /// </summary>
    internal interface ISourcesMetadata
    {
        /// <summary>
        /// Dependency injection container type used in source project
        /// </summary>
        DependencyInjection DependencyInjection { get; }

        /// <summary>
        /// Upsert method for mapper metadata. Used during mappers setup.
        /// </summary>
        /// <param name="mapperMetadata">mapper metadata object</param>
        void AddOrUpdate(IMapperMetadata mapperMetadata);

        /// <summary>
        /// List of defined mappers
        /// </summary>
        IReadOnlyCollection<IMapperMetadata> Mappers { get; }
    }

    internal class SourcesMetadata : ISourcesMetadata
    {
        private readonly List<IMapperMetadata> _mappers = new();
        private readonly DependencyInjection _dependencyInjection;
        private readonly IEnumerable<IAssemblySymbol> _assemblySymbols;

        private SourcesMetadata(Compilation compilation)
        {
            _dependencyInjection = new DependencyInjection(compilation.ReferencedAssemblyNames);
            _assemblySymbols = compilation.SourceModule.ReferencedAssemblySymbols;
            AddReferencedMappers(_referencedMappers);
        }

        private SourcesMetadata(IEnumerable<AssemblyIdentity> assemblyIdentities, IEnumerable<IAssemblySymbol> assemblySymbols)
        {
            _dependencyInjection = new DependencyInjection(assemblyIdentities);
            _assemblySymbols = assemblySymbols;
            AddReferencedMappers(_referencedMappers);
        }

        public IReadOnlyCollection<IMapperMetadata> Mappers => _mappers.AsReadOnly();

        public DependencyInjection DependencyInjection => _dependencyInjection;

        public static ISourcesMetadata Create(Compilation compilation) => new SourcesMetadata(compilation);

        public static ISourcesMetadata Create(IEnumerable<AssemblyIdentity> assemblyIdentities, IEnumerable<IAssemblySymbol> assemblySymbols) => 
            new SourcesMetadata(assemblyIdentities, assemblySymbols);

        public void AddOrUpdate(IMapperMetadata mapperMetadata)
        {
            var mapper = _mappers.FirstOrDefault(m => m.Name == mapperMetadata.Name);
            if (mapper is not null)
            {
                _mappers.Where(m => m == mapper).ToList().ForEach(m => m = mapperMetadata);
            }
            else
            {
                _mappers.Add(mapperMetadata);
            }
        }

        /// <summary>
        /// Collection of referenced assemblies <see cref="IAssemblySymbol" />
        /// </summary>
        private IReadOnlyCollection<IAssemblySymbol> _referencedAssemblies => _assemblySymbols?.Where(a => a.Identity?.HasPublicKey == false).ToList().AsReadOnly();

        /// <summary>
        /// Collection of referenced mappers <see cref="MapperMetadata" />
        /// </summary>
        private IReadOnlyCollection<IMapperMetadata> _referencedMappers
        {
            get
            {
                var nSpaceCollection = new List<INamespaceSymbol>();

                foreach (var assembly in _referencedAssemblies)
                {
                    nSpaceCollection.AddRange(FlattenNamespaces(assembly.GlobalNamespace.GetNamespaceMembers()));
                }

                var typesCollection = nSpaceCollection?.SelectMany(n => n.GetTypeMembers());
                var mappersCollection = typesCollection?.Where(t => t.GetAttributes().Any(attribute => attribute is not null && attribute.AttributeClass?.Name == nameof(MapperAttribute)));

                return mappersCollection?.Select(t =>
                {
                    return new MapperMetadata(t, true);
                }).ToList().AsReadOnly();
            }
        }

        /// <summary>
        /// Method returns recursively flatt collection of <see cref="INamespaceSymbol" />
        /// </summary>
        /// <param name="namespaceSymbols"></param>
        /// <returns></returns>
        private IEnumerable<INamespaceSymbol> FlattenNamespaces(IEnumerable<INamespaceSymbol> namespaceSymbols) =>
            namespaceSymbols.SelectMany(n =>  FlattenNamespaces(n.GetNamespaceMembers())).Concat(namespaceSymbols);

        /// <summary>
        /// Method that add collection of mappers <see cref="MapperMetadata">
        /// </summary>
        /// <param name="mapperMetadatas"></param>
        private void AddReferencedMappers(IEnumerable<IMapperMetadata> mapperMetadatas)
        {
            if (mapperMetadatas is null) return;

            foreach (var mapper in (mapperMetadatas))
            {
                AddOrUpdate(mapper);
            }
        }
    }
}