using Compentio.Example.DotNetCore.App.Entities;
using Compentio.SourceMapper.Attributes;

namespace Compentio.Example.DotNetCore.App.Mappers
{
    [Mapper]
    public partial interface INotesMapper
    {
        [Mapping(Source = nameof(NoteDao.PageTitle), Target = nameof(NoteDto.Title))]
        //[InverseMapping(InverseMethodName = "MapToDao")]
        NoteDto MapToDto(NoteDao source);

        [Mapping(Source = nameof(NoteDocumentDao.Metadata.CreatorFirstName), Target = nameof(NoteDocumentDto.Autor))]
        //[InverseMapping(InverseMethodName = "MapToDao")]
        NoteDocumentDto MapToDto(NoteDocumentDao source);

        //[InverseMapping(InverseMethodName = "MapDocumentToDao")]
        NoteDocumentMetadataDto MapDocumentToDto(NoteDocumentMetadataDao source);
    }
}