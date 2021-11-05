using Compentio.Example.DotNetCore.App.Entities;
using Compentio.SourceMapper.Attributes;
using System;

namespace Compentio.Example.DotNetCore.App.Mappers
{
    [Mapper]
    public interface INotesMapper
    {
        [Mapping(Source = nameof(NoteDao.PageTitle), Target = nameof(NoteDto.Title))]
        NoteDto MapToDto(NoteDao source);

        [Mapping(Source = nameof(NoteDocumentDao.Metadata.CreatorFirstName), Target = nameof(NoteDocumentDto.Autor))]
        NoteDocumentDto MapToDto(NoteDocumentDao source);

        [Mapping(Source = nameof(NoteDto.Title), Target = nameof(NoteDao.PageTitle))]
        NoteDao MapToDao(NoteDto source);

        NoteDocumentMetadataDto MapDocumentToDto(NoteDocumentMetadataDao source);
    }
}

