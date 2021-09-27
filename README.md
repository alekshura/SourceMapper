# SourceMapper

# Introduction
`SourceMapper` is code generator for mappings based on attributes defined in interfaces or abstrat classes. 
It is based on .NetCore [Source Generators](https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.md) 
engine and generates classes for mappers during build time of you project.
That is the main difference between `SourceMapper` and [Automapper](https://automapper.org/): you can see, reuse or inherit from generated code after app build process.

# Installation
Install using nuget `Install-Package SourceMapper`.
In the project where it is installed add `OutputItemType="Analyzer"`

```
<ItemGroup>
    <ProjectReference xxx OutputItemType="Analyzer" />
 </ItemGroup>
```

# How to use
To define mapping we have to mark mapping abstract class or interface with `MapperAttribute`:

```csharp
[Mapper]
public interface INotesMapper
{
    NoteDto MapToDto(NoteDao source);
}
```
This will generate mapping class with default class name `NotesMapper` for properties that names are the same for `NoteDto` and `NoteDao` classes.
When the names are different than we can use `Source` and `Target` names of the properties:

```csharp
[Mapper(ClassName = "InterfaceUserMapper")]
public interface IUserMapper
{
    [Mapping(Source = nameof(UserDao.FirstName), Target = nameof(UserInfo.Name))]
    UserInfo MapToDomainMoodel(UserDao userDao);       
}
```

The `ClassName` property in `MappeAttribute` is responsible for name of the generated mapping class. 
For default `MappeAttribute` interface prefix `I` is truncated or `Impl` added to the class name if there is no `I` prefix
in the mapping interface or abstract class name.

## Interface mapping
Use interfaces to prepare basic mapping. 
In a case when mapped object contains another objects, e.g.:

```csharp
public class NoteDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public NoteDocumentDto Document { get; set; }
}
```

and

```csharp
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
it is enough to add mapping method to the interface for these types and the code generation processor will match and generate mappings for 
`NoteDto MapToDto(NoteDao source)` method:

```csharp
[Mapper]
public interface INotesMapper
{
    [Mapping(Source = nameof(NoteDao.PageTitle), Target = nameof(NoteDto.Title))]
    NoteDto MapToDto(NoteDao source);

    [Mapping(Source = nameof(NoteDocumentDao.Metadata.CreatorFirstName), Target = nameof(NoteDocumentDto.Autor))]
    NoteDocumentDto MapToDto(NoteDocumentDao source);
}
```

the output will be:

```csharp
 public class NotesMapper : INotesMapper
{
    public static NotesMapper Create() => new();
    public virtual Compentio.Example.App.Entities.NoteDto MapToDto(Compentio.Example.App.Entities.NoteDao source)
    {
        var target = new Compentio.Example.App.Entities.NoteDto();
        target.Id = source.Id;
        target.Title = source.PageTitle;
        target.Description = source.Description;
        target.Document = MapDocumentToDto(source.Document);
        return target;
    }

    public virtual Compentio.Example.App.Entities.NoteDocumentDto MapDocumentToDto(Compentio.Example.App.Entities.NoteDocumentDao source)
    {
        var target = new Compentio.Example.App.Entities.NoteDocumentDto();
        target.Id = source.Id;
        target.Title = source.Title;
        return target;
    }
}
```
> All methds are marked as `virtual`, so there is a possibility to override them in own mappers code. 


## Class mapping
For more complicated convertings 


## Embedded objects

Example of generated code for this mapping:


The generated class is in the same namespace as its base abstract class of interface. It can be found in Visual Studio: //TODO

## Dependency injection

# Roadmap
