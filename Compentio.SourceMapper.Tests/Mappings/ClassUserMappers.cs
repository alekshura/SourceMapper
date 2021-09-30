using Compentio.SourceMapper.Attributes;
using Compentio.SourceMapper.Tests.Entities;
using System;
using System.Linq;

namespace Compentio.SourceMapper.Tests.Mappings
{
    /// <summary>
    /// Abstract class mapper with examples of expressions for additional mappings
    /// </summary>
    [Mapper(ClassName = "ClassUserDaoMapper")]
    public abstract class UserMapper
    {
        [Mapping(Source = nameof(UserDao.UserId), Target = nameof(UserInfo.Id), Expression = nameof(ConvertUserId))]
        [Mapping(Target = nameof(UserInfo.Name), Expression = nameof(ConvertUserName))]
        [Mapping(Source = nameof(UserDao.UserGender), Target = nameof(UserInfo.Sex), Expression = nameof(ConvertUserGender))]
        public abstract UserInfo MapToDomainMoodel(UserDao userDao);

        protected static int ConvertUserId(long id)
        {
            return Convert.ToInt32(id);
        }
        protected static string ConvertUserName(UserDao userDao) => $"{userDao.FirstName} {userDao.LastName}";
        protected readonly Func<UserGender, Sex> ConvertUserGender = gender => gender == UserGender.Female ? Sex.W : Sex.M;
    }

    [Mapper(ClassName = "ClassUserDaoArrayMapper")]
    public abstract class UserDataArrayMapper
    {
        [Mapping(Source = nameof(UserWithArrayDao.UserAddresses), Target = nameof(UserInfoWithArray.Addresses), Expression = nameof(ConvertAddresses))]
        public abstract UserInfoWithArray MapToDomainModel(UserWithArrayDao userWithArrayDao);

        protected Address[] ConvertAddresses(AddressDao[] addresses)
        {
            return addresses.Select(a => MapAddress(a)).ToArray();
        }

        public abstract Address MapAddress(AddressDao addressDao);

        public abstract Region MapRegion(RegionDao regionDao);
    }
}
