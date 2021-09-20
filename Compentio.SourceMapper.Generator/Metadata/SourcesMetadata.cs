using System;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.SourceMapper.Metadata
{
    interface ISourcesMetadata
    {
        void AddOrUpdate(IMapperMetadata mapperMetadata);
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
