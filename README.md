# SourceMapper

# Introduction
`SourceMapper` is code generator for mappings based on attributes defined in interfaces or abstrat classes. 
It is based on .NetCore [Source Generators](https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.md) 
engine and generates classes for mappers during build time of you project.
That is the main difference between `SourceMapper` and [Automapper](https://automapper.org/): you can see, reuse or inherit from generated code after app build process.

# Attributes

``` namespace Compentio.Example.App.Mappers
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
```


# How to use



# Roadmap
