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
    public abstract partial class UserMapper
    {
        [Mapping(Source = nameof(UserDao.UserId), Target = nameof(UserInfo.Id), Expression = nameof(ConvertUserId), 
            InverseSource = nameof(UserInfo.Id), InverseTarget = nameof(UserDao.UserId), InverseExpression = nameof(ConvertId))]
        [Mapping(Target = nameof(UserInfo.Name), Expression = nameof(ConvertUserName))]
        [Mapping(Source = nameof(UserDao.UserGender), Target = nameof(UserInfo.Sex), Expression = nameof(ConvertUserGender),
            InverseSource = nameof(UserInfo.Sex), InverseTarget = nameof(UserDao.UserGender), InverseExpression = nameof(ConvertSex))]
        [Mapping(CreateInverse = true, InverseMethodName = "MapToDatabaseModel")]
        public abstract UserInfo MapToDomainModel(UserDao source);

        protected static int ConvertUserId(long id)
        {
            return Convert.ToInt32(id);
        }

        protected static long ConvertId(int id)
        {
            return Convert.ToInt64(id);
        }

        protected static string ConvertUserName(UserDao userDao) => $"{userDao.FirstName} {userDao.LastName}";
        protected readonly Func<UserGender, Sex> ConvertUserGender = gender => gender == UserGender.Female ? Sex.W : Sex.M;
        protected readonly Func<Sex, UserGender> ConvertSex = sex => sex == Sex.M ? UserGender.Male : UserGender.Female;
    }

    [Mapper(ClassName = "ClassUserDaoArrayMapper")]
    public abstract partial class UserDataArrayMapper
    {
        [Mapping(Source = nameof(UserWithArrayDao.UserAddresses), Target = nameof(UserInfoWithArray.Addresses), Expression = nameof(ConvertAddresses),
            InverseSource = nameof(UserInfoWithArray.Addresses), InverseTarget = nameof(UserWithArrayDao.UserAddresses), InverseExpression = nameof(ConvertAddressesDao))]
        [Mapping(CreateInverse = true, InverseMethodName = "MapToDatabaseModel")]
        public abstract UserInfoWithArray MapToDomainModel(UserWithArrayDao userWithArrayDao);

        protected Address[] ConvertAddresses(AddressDao[] addresses)
        {
            return addresses.Select(a => MapAddress(a)).ToArray();
        }

        protected AddressDao[] ConvertAddressesDao(Address[] addresses)
        {
            return addresses.Select(a => MapFromAddress(a)).ToArray();
        }

        [Mapping(CreateInverse = true, InverseMethodName = "MapFromAddress")]
        public abstract Address MapAddress(AddressDao source);

        [Mapping(CreateInverse = true, InverseMethodName = "MapFromRegion")]
        public abstract Region MapRegion(RegionDao source);
    }
}
