using Compentio.SourceMapper.Attributes;
using Compentio.SourceMapper.Tests.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.SourceMapper.Tests.Mappings
{
    /// <summary>
    /// Abstract class mapper with examples of expressions for additional mappings
    /// </summary>
    [Mapper(ClassName = "ClassUserDaoMapper")]
    public abstract partial class UserMapper
    {
        [Mapping(Source = nameof(UserDao.UserId), Target = nameof(UserInfo.Id), Expression = nameof(ConvertUserId))]
        [Mapping(Source = nameof(UserDao.UserGender), Target = nameof(UserInfo.Sex), Expression = nameof(ConvertUserGender))]
        [Mapping(Source = nameof(UserDao), Target = nameof(UserInfo.Name), Expression = nameof(ConvertUserName))]
        [InverseMapping(InverseMethodName = "MapToDatabaseModel")]
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

    [Mapper(ClassName = "ClassUserDataDaoMapper")]
    public abstract partial class UserDataDaoMapper
    {
        [Mapping(Source = nameof(UserDataDao.FirstName), Target = nameof(UserInfo.Name))]
        public abstract UserInfo MapToDomainModel(UserDataDao source);
    }

    [Mapper(ClassName = "ClassUserDaoArrayMapper")]
    public abstract partial class UserDataArrayMapper
    {
        [Mapping(Source = nameof(UserWithArrayDao.UserAddresses), Target = nameof(UserInfoWithArray.Addresses), Expression = nameof(ConvertAddresses))]
        [InverseMapping(InverseMethodName = "MapToDatabaseModel")]
        public abstract UserInfoWithArray MapToDomainModel(UserWithArrayDao source);

        protected Address[] ConvertAddresses(AddressDao[] addresses)
        {
            return addresses.Select(a => MapAddress(a)).ToArray();
        }

        protected AddressDao[] ConvertAddressesDao(Address[] addresses)
        {
            return addresses.Select(a => MapFromAddress(a)).ToArray();
        }

        [InverseMapping(InverseMethodName = "MapFromAddress")]
        public abstract Address MapAddress(AddressDao source);

        [InverseMapping(InverseMethodName = "MapFromRegion")]
        public abstract Region MapRegion(RegionDao source);
    }

    public class CustomClassUserDaoArrayMapper : ClassUserDaoArrayMapper
    {
        public override UserWithArrayDao MapToDatabaseModel(UserInfoWithArray source)
        {
            var result = base.MapToDatabaseModel(source);
            result.UserAddresses = ConvertAddressesDao(source.Addresses);
            return result;
        }
    }

    [Mapper(ClassName = "ClassUserDaoListMapper")]
    public abstract partial class UserDataListMapper
    {
        [Mapping(Source = nameof(UserWithListDao.UserAddresses), Target = nameof(UserInfoWithList.Addresses), Expression = nameof(ConvertAddresses))]
        [InverseMapping(InverseMethodName = "MapToDatabaseModel")]
        public abstract UserInfoWithList MapToDomainModel(UserWithListDao source);

        protected IList<Address> ConvertAddresses(IList<AddressDao> addresses)
        {
            return addresses.Select(a => MapAddress(a)).ToList();
        }

        protected IList<AddressDao> ConvertAddressesDao(IList<Address> addresses)
        {
            return addresses.Select(a => MapFromAddress(a)).ToList();
        }

        [InverseMapping(InverseMethodName = "MapFromAddress")]
        public abstract Address MapAddress(AddressDao source);

        [InverseMapping(InverseMethodName = "MapFromRegion")]
        public abstract Region MapRegion(RegionDao source);
    }

    public class CustomClassUserDaoListMapper : ClassUserDaoListMapper
    {
        public override UserWithListDao MapToDatabaseModel(UserInfoWithList source)
        {
            var result = base.MapToDatabaseModel(source);
            result.UserAddresses = ConvertAddressesDao(source.Addresses);
            return result;
        }
    }
}