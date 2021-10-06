# SourceMapper

# Introduction
`SourceMapper` is code generator for mappings based on attributes defined in interfaces or abstrat classes. 
It is based on .NetCore [Source Generators](https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.md) 
engine and generates classes for mappers during build time of you project.
That is the main difference between `SourceMapper` and [Automapper](https://automapper.org/): you can see, reuse or inherit from generated code after app build process.

# Installation
Install using nuget package manager:

```console
Install-Package Compentio.SourceMapper
```

or `.NET CLI`:

```console
dotnet add package Compentio.SourceMapper
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
The generated class is in the same namespace as its base abstract class of interface. It can be found in project in Visual Studio: 
> Dependencies -> Analyzers -> Compentio.SourceMapper.Generators.MainSourceGenerator.

When the names are different than we can use `Source` and `Target` names of the properties:

```csharp
[Mapper(ClassName = "InterfaceUserMapper")]
public interface IUserMapper
{
    [Mapping(Source = nameof(UserDao.FirstName), Target = nameof(UserInfo.Name))]
    UserInfo MapToDomainMoodel(UserDao userDao);       
}
```

The `ClassName` property in `MapperAttribute` is responsible for name of the generated mapping class. 
For default `MapperAttribute` interface prefix `I` is removed or `Impl` suffix added to the generated class name if there is no `I` prefix
in the mapping interface name.

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
For more complicated mapings use abstract class to define mappers. The main difference between abstract class mapper and interface, that `Expression`
property can be used in `MappingAttribute`:

```csharp
[Mapper(ClassName = "NotesMappings")]
public abstract class NotesClassMapper
{
    [Mapping(Target = nameof(NoteDocumentDto.Autor), Expression = nameof(ConvertAuthor))]
    public abstract NoteDocumentDto MapToDto(NoteDocumentDao source);

    protected readonly Func<NoteDocumentDao, string> ConvertAuthor = s => s.Metadata.CreatorFirstName + s.Metadata.CreatorLastName;
}

```

`Expression` - it is a name of mapping function, that can be used for additional properties mapping. 
> It must be `public` or `protected`, since it is used in generated mapper class that implements abstract mapping class.

### Mapping collections
Lets assume we need to map two entities:

```csharp
public class UserDao
{
    public long UserId { get; set; }
    public AddressDao[] UserAddresses { get; set; }
}
```
to 

```csharp
public class UserInfo
{
    public int Id { get; set; }
    public Address[] Addresses { get; set; }
}
```
It can be achieved using abstract class mapper:

```csharp
[Mapper(ClassName = "UserDataMapper")]
public abstract class UserMapper
{
    [Mapping(Source = nameof(UserDao.UserAddresses), Target = nameof(UserInfo.Addresses), Expression = nameof(ConvertAddresses))]
    [Mapping(Source = nameof(UserDao.UserId), Target = nameof(UserInfo.Id), Expression = nameof(ConvertUserId))]
    public abstract UserInfoWithArray MapToDomainModel(UserWithArrayDao userWithArrayDao);

    protected Address[] ConvertAddresses(AddressDao[] addresses)
    {
        return addresses.Select(a => MapAddress(a)).ToArray();
    }
    
    protected static int ConvertUserId(long id)
    {
        return Convert.ToInt32(id);
    }

    public abstract Address MapAddress(AddressDao addressDao);
}
```

More examples can be found in repo code:
https://github.com/alekshura/SourceMapper/blob/master/Compentio.SourceMapper.Tests/Mappings/ClassUserMappers.cs and
https://github.com/alekshura/SourceMapper/blob/master/Compentio.SourceMapper.Tests/Mappings/InterfaceUserMappers.cs


## Dependency injection
To simplify adding dependency injection for mappers `MappersDependencyInjectionExtensions` class is generated, that can be used (for now only for
`Microsoft.Extensions.DependencyInjection`) by adding `AddMappers()` that adds all mappers defined in the project:

```csharp
 Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services
                    //.here you services
                    //
                    .AddMappers());
```
The `Compentio.SourceMapper` searches for 3 main dependency container packages (`Microsoft.Extensions.DependencyInjection`, `Autofac.Extensions.DependencyInjection`, and `StructureMap.Microsoft.DependencyInjection`) and generates extension code. If there no any container packages found, Dependency Injection extension class in not generated.

# Roadmap & development
| Status | Description |
| --- |---|
|[✔] |Basic interface and abstract class mapper
|[✔]|Collections mappings
|[❌]|Add Using property to `MapperAttribute` to use mappings from another mappers
|[❌]|Inverse mapping - `MappingAttribute` property that automaticly generates inverse mapping 
|[❔] |<del>Automatic casting</del> manual casing Attribute of the properties
|[✔] |Dependency injection containers automatic recognize container type and generating extensions methods for mappers
|[❌] |Dependency injection containers diagnostics
|[❔]|Linq extensions - generate extensions for mapping collections, e.g.: `IEnumerable<NoteDocumentDto> documentDtos = documentsDaos.MapToDto()`

