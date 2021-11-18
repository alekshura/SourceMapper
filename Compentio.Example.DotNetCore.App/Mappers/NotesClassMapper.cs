using Compentio.Example.DotNetCore.App.Entities;
using Compentio.SourceMapper.Attributes;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Compentio.Example.DotNetCore.App.Mappers
{
    [ExcludeFromCodeCoverage]
    [Mapper(ClassName = "NotesMappings")]
    public abstract partial class NotesClassMapper
    {
        [Mapping(Source = nameof(NoteDao.PageTitle), Target = nameof(NoteDto.Title))]
        [Mapping(CreateInverse = true, InverseMethodName = "MapToDao")]
        public abstract NoteDto MapToDto(NoteDao source);

        [Mapping(Target = nameof(NoteDocumentDto.Autor), Expression = nameof(ConvertAuthorToDto), InverseExpression = nameof(ConvertAuthorToDao), InverseTarget = nameof(NoteDocumentDao.Author))]
        [Mapping(CreateInverse = true, InverseMethodName = "MapToDao")]
        public abstract NoteDocumentDto MapToDto(NoteDocumentDao source);

        protected readonly Func<NoteDocumentDao, string> ConvertAuthorToDto = s => s.Metadata.CreatorFirstName + s.Metadata.CreatorLastName;
        protected readonly Func<NoteDocumentDto, string> ConvertAuthorToDao = s => s.Metadata.CreatorLastName + s.Metadata.CreatorFirstName;
    }
}