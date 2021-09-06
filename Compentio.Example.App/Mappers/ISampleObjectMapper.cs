using Compentio.Example.App.Entities;
using Compentio.SourceMapper.Attributes;

namespace Compentio.Example.App.Mappers
{
    [Mapper]
    interface ISampleObjectMapper
    {
        [Mapping(Source = "PageTitle", Target ="Title")]
        NoteDto MapToRest(NoteDao source);
    }
}

