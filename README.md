# SourceMapper

# Introduction
`SourceMapper` is code generator for mappings based on attributes defined in interfaces or abstrat classes. 
It is based on .NetCore [Source Generators](https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.md) 
engine and generates classes for mappers during build time of you project.
That is the main difference between `SourceMapper` and [Automapper](https://automapper.org/): you can see, reuse or inherit from generated code after app build process.

# How to use
To define mapping we have to mark mapping abstract class or interface with `MapperAttribute`:

``` 
[Mapper]
public interface INotesMapper
{
    NoteDto MapToDto(NoteDao source);
}
```
This will generate mapping class for properties that names are the same for `NoteDto` and `NoteDao` classes.
When the names are different than we can use `Source` and `Target` names of the properties:

```
[Mapper(ClassName = "InterfaceUserMapper")]
public interface IUserMapper
{
    [Mapping(Source = nameof(UserDao.FirstName), Target = nameof(UserInfo.Name))]
    UserInfo MapToDomainMoodel(UserDao userDao);       
}
```

## Interface mapping 

## Class mapping

`ClassName` are responsible for name of the generated mapping class name. If it is empty dotnet interface `I` truncated or `Impl` added to abstract class name

## Embedded objects
Lats assume, that we have "more complicated objects" to map `NoteDto MapToDto(NoteDao source)` with embedded objects:

```
public class NoteDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public NoteDocumentDto Document { get; set; }
}
```

and

```
public class NoteDao
{
    public long Id { get; set; }
    public string PageTitle { get; set; }
    public string Description { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public string CreatedBy { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public NoteDocumentDao Document { get; set; }
}
```

than it is enough to add mapper for these types of objects and it will be matched and used in `NoteDto MapToDto(NoteDao source)` method:

```
[Mapper]
public interface INotesMapper
{
    [Mapping(Source = nameof(NoteDao.PageTitle), Target = nameof(NoteDto.Title))]
    NoteDto MapToDto(NoteDao source);

    [Mapping(Source = nameof(NoteDocumentDao.Metadata.CreatorFirstName), Target = nameof(NoteDocumentDto.Autor))]
    NoteDocumentDto MapToDto(NoteDocumentDao source);
}
```
Example of generated code for this mapping:

```
 public class NotesMapper : INotesMapper
{
    public static NotesMapper Create() => new();
    public virtual Compentio.Example.App.Entities.NoteDto MapToDto(Compentio.Example.App.Entities.NoteDao source)
    {
        var target = new Compentio.Example.App.Entities.NoteDto();
        target.Id = source.Id;
        target.Title = source.PageTitle;
        target.Description = source.Description;
        target.Document = MapToDto(source.Document);
        return target;
    }

    public virtual Compentio.Example.App.Entities.NoteDocumentDto MapToDto(Compentio.Example.App.Entities.NoteDocumentDao source)
    {
        var target = new Compentio.Example.App.Entities.NoteDocumentDto();
        target.Id = source.Id;
        target.Title = source.Title;
        target.Metadata = MapDocumentToDto(source.Metadata);
        return target;
    }

    public virtual Compentio.Example.App.Entities.NoteDao MapToDao(Compentio.Example.App.Entities.NoteDto source)
    {
        var target = new Compentio.Example.App.Entities.NoteDao();
        target.Id = source.Id;
        target.PageTitle = source.Title;
        target.Description = source.Description;
        return target;
    }
}

```
The generated class is in the same namespace as its base abstract class of interface. It can be found in Visual Studio: //TODO

## Dependency injection

# Roadmap
