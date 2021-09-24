using Compentio.SourceMapper.Attributes;
using Compentio.SourceMapper.Tests.Entities;

namespace Compentio.SourceMapper.Tests.Mappings
{
    [Mapper(ClassName = "InterfaceUserMapper")]
    public interface IUserMapper
    {
        [Mapping(Source = nameof(UserDao.UserId), Target = nameof(UserInfo.Id))]
        [Mapping(Source = nameof(UserDao.FirstName), Target = nameof(UserInfo.Name))]
        [Mapping(Source = nameof(UserDao.UserGender), Target = nameof(UserInfo.Gender))]
        UserInfo MapToDomainMoodel(UserDao userDao);       
    }
}
