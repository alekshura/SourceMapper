using Compentio.Example.App.Entities;
using Compentio.SourceMapper.Attributes;

namespace Compentio.Example.App.Mappers
{
    [Mapper]
    public interface INotesMapper
    {
        [Mapping(Source = "PageTitle", Target ="Title")]
        NoteDto MapToDto(NoteDao source);

        [Mapping(Source = "Title", Target = "PageTitle")]
        NoteDao MapToDao(NoteDto source);
    }
}

