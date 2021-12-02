using Compentio.SourceMapper.Processors.DependencyInjection;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
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

        private SourcesMetadata(IEnumerable<AssemblyIdentity> assemblies)
        {
            _dependencyInjection = new DependencyInjection(assemblies);
        }

        static SourcesMetadata()
        {
        }

        public IReadOnlyCollection<IMapperMetadata> Mappers => _mappers.AsReadOnly();

        public DependencyInjection DependencyInjection => _dependencyInjection;

        public static ISourcesMetadata Create(IEnumerable<AssemblyIdentity> assemblies) => new SourcesMetadata(assemblies);

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
    }
}