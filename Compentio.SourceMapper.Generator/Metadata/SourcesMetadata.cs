using Compentio.SourceMapper.Processors.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.SourceMapper.Metadata
{
    /// <summary>
    /// Encapsulates all defined mappers in assembly
    /// </summary>
    interface ISourcesMetadata
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
        private readonly DependencyInjectionType _dependencyInjectionType;

        private SourcesMetadata(DependencyInjectionType dependencyInjectionType) 
        {
            _dependencyInjectionType = dependencyInjectionType;
        }
        static SourcesMetadata()
        {
        }

        public IReadOnlyCollection<IMapperMetadata> Mappers => _mappers.AsReadOnly();

        public DependencyInjection DependencyInjection => new() 
        {
            DependencyInjectionType = _dependencyInjectionType
        };

        public static ISourcesMetadata Create(DependencyInjectionType dependencyInjectionType) => new SourcesMetadata(dependencyInjectionType);

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
