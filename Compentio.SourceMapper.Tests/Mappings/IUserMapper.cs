using Compentio.SourceMapper.Attributes;
using Compentio.SourceMapper.Tests.Entities;
using System;

namespace Compentio.SourceMapper.Tests.Mappings
{
    [Mapper(ClassName = "InterfaceUserMapper")]
    public interface IUserMapper
    {
        [Mapping(Source = nameof(UserDao.FirstName), Target = nameof(UserInfo.Name))]
        UserInfo MapToDomainMoodel(UserDao userDao);       
    }

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
}
