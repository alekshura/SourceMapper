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

`ClassName` are responsible for name of the generated mapping class name. If it is empty dotnet interface `I` truncated or `Impl` added to abstract class name

## Embedded objects
Lats assume, that we want to map `UserDao` -> `UserInfo`, where those ones defained as:

```
public class UserDao
{
    public long UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public UserGender UserGender { get; set; }
    public string City { get; set; }
    public string House { get; set; }
    public string ZipCode { get; set; }
    public string District { get; set; }
    public string State { get; set; }
    public DateTime BirthDate { get; set; }
}    
    
public class UserInfo
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Gender Gender { get; set; }
    public Address Address { get; set; }
    public DateTime BirthDate { get; set; }
}

public enum Gender { Male, Female }
public enum UserGender { Female, Male, Unknown }

```

than it can be done:

```
[Mapper(ClassName = "ClassUserMapper")]
public abstract class UserMapper
{
    [Mapping(Source = nameof(UserDao.UserId), Target = nameof(UserInfo.Id), Expression = nameof(ConvertUserId))]
    [Mapping(Source = nameof(UserDao.FirstName), Target = nameof(UserInfo.Name))]
    [Mapping(Source = nameof(UserDao.UserGender), Target = nameof(UserInfo.Gender), Expression = nameof(ConvertUserGender))]
    public abstract UserInfo MapToDomainMoodel(UserDao userDao);

    protected int ConvertUserId(long id)
    {
        return Convert.ToInt32(id);
    }
    protected readonly Func<UserGender, Gender> ConvertUserGender = gender => gender == UserGender.Female ? Gender.Female : Gender.Male;
}
```



# Roadmap
