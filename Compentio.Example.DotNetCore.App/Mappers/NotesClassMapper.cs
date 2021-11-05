﻿using Compentio.Example.DotNetCore.App.Entities;
using Compentio.SourceMapper.Attributes;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Compentio.Example.DotNetCore.App.Mappers
{
    [ExcludeFromCodeCoverage]
    [Mapper(ClassName = "NotesMappings")]
    public abstract class NotesClassMapper
    {
        [Mapping(Source = nameof(NoteDao.PageTitle), Target = nameof(NoteDto.Title))]
        public abstract NoteDto MapToDto(NoteDao source);

        [Mapping(Target = nameof(NoteDocumentDto.Autor), Expression = nameof(ConvertAuthor))]
        public abstract NoteDocumentDto MapToDto(NoteDocumentDao source);

        protected readonly Func<NoteDocumentDao, string> ConvertAuthor = s => s.Metadata.CreatorFirstName + s.Metadata.CreatorLastName;

        [Mapping(Source = nameof(NoteDto.Title), Target = nameof(NoteDao.PageTitle))]
        public abstract NoteDao MapToDao(NoteDto source);
    }
}

