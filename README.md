# <img src="/Compentio.Assets/Logo.png" align="left" width="50"> SourceMapper

[![NuGet](http://img.shields.io/nuget/v/Compentio.SourceMapper.svg)](https://www.nuget.org/packages/Compentio.SourceMapper)
![Sonar Quality Gate](https://img.shields.io/sonar/quality_gate/alekshura_SourceMapper?server=https%3A%2F%2Fsonarcloud.io)
[![Test](https://github.com/alekshura/SourceMapper/actions/workflows/pr-tests.yml/badge.svg)](https://github.com/alekshura/SourceMapper/actions/workflows/pr-tests.yml)
[![Build](https://github.com/alekshura/SourceMapper/actions/workflows/main.yml/badge.svg)](https://github.com/alekshura/SourceMapper/actions/workflows/main.yml)
![Nuget](https://img.shields.io/nuget/dt/Compentio.SourceMapper)
![GitHub](https://img.shields.io/github/license/alekshura/SourceMapper)
![GitHub top language](https://img.shields.io/github/languages/top/alekshura/SourceMapper)

# Introduction
`SourceMapper` is a code generator that uses attributes placed in interfaces or abstract classes: 
during build time it generates mapping classes and methods for mappings based on "rules" defined in these attributes. 

It is based on [Source Generators](https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.md) feature
that has been intoduced with `C# 9.0` and brings a possibility to  generate code during build time.

:point_right:
After configuring you mappers you can see, control and override the generated code for the mappings.
:point_left:

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
        if (source == null)
            return source;
        var target = new Compentio.Example.App.Entities.NoteDto();
        target.Id = source.Id;
        target.Title = source.PageTitle;
        target.Description = source.Description;
        target.Document = MapDocumentToDto(source.Document);
        return target;
    }

    public virtual Compentio.Example.App.Entities.NoteDocumentDto MapDocumentToDto(Compentio.Example.App.Entities.NoteDocumentDao source)
    {
        if (source == null)
            return source;
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

For more examples see [Wiki examples](https://github.com/alekshura/SourceMapper/wiki/Examples#mapping-collections).

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
The `Compentio.SourceMapper` searches for 3 main dependency container packages (`Microsoft.Extensions.DependencyInjection`, `Autofac.Extensions.DependencyInjection`, and `StructureMap.Microsoft.DependencyInjection`) and generates extension code. If there no any container packages found, Dependency Injection extension class is not generated.


