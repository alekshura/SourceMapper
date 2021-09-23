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
        /// TODO: get DI used in target project
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

        private SourcesMetadata() 
        { 
        }
        static SourcesMetadata()
        {
        }

        public IReadOnlyCollection<IMapperMetadata> Mappers => _mappers.AsReadOnly();

        public DependencyInjection DependencyInjection => new() 
        {
            DependencyInjectionType = DependencyInjectionType.DotNetCore
        };

        public static ISourcesMetadata Create() => new SourcesMetadata();

        public void AddOrUpdate(IMapperMetadata mapperMetadata)
        {
            var mapper = _mappers.FirstOrDefault(m => m.MapperName == mapperMetadata.MapperName);
            if (mapper is not null)
            {
                mapper = mapperMetadata;
            }
            else
            {
                _mappers.Add(mapperMetadata);
            }            
        }
    }
}
