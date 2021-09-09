using Compentio.Example.App.Entities;
using Compentio.SourceMapper.Attributes;

namespace Compentio.Example.App.Mappers
{
    [Mapper]
    public interface INotesMapper
    {
        [Mapping(Source = "PageTitle", Target ="Title")]
        //[Mapping(Source = "BinaryDocument", Target = "Document")]
        NoteDto MapToDto(NoteDao source);

        NoteDocumentDto MapToDto(NoteDocumentDao source);

        [Mapping(Source = "Title", Target = "PageTitle")]
        NoteDao MapToDao(NoteDto source);
    }
}

