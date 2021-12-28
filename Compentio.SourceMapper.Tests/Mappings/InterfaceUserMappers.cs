using Compentio.SourceMapper.Attributes;
using Compentio.SourceMapper.Tests.Entities;
using System.Linq;

namespace Compentio.SourceMapper.Tests.Mappings
{
    [Mapper(ClassName = "InterfaceUserDaoMapper")]
    public partial interface IUserDaoMapper
    {
        [Mapping(Source = nameof(UserDao.FirstName), Target = nameof(UserInfo.Name))]
        [InverseMapping(InverseMethodName = "MapToDatabaseModel")]
        UserInfo MapToDomainModel(UserDao source);
        [InverseMapping(InverseMethodName = "MapFromAddress")]
        Address MapAddress(UserDao source);
        [InverseMapping(InverseMethodName = "MapFromRegion")]
        Region MapRegion(UserDao source);
    }

    [Mapper(ClassName = "InterfaceUserDataDaoMapper")]
    public partial interface IUserDataDaoMapper
    {
        [Mapping(Source = nameof(UserDataDao.FirstName), Target = nameof(UserInfo.Name))]
        [Mapping(Source = nameof(UserDataDao.UserAddress), Target = nameof(UserInfo.Address))]
        [InverseMapping(InverseMethodName = "MapToDatabaseModel")]
        UserInfo MapToDomainModel(UserDataDao source);

        [InverseMapping(InverseMethodName = "MapFromAddress")]
        Address MapAddress(AddressDao addressDao);

        [InverseMapping(InverseMethodName = "MapFromRegion")]
        Region MapRegion(RegionDao regionDao);
    }

    public class CustomUserDataDaoMapper : InterfaceUserDataDaoMapper
    {
        public override UserInfo MapToDomainModel(UserDataDao userDataDao)
        {
            var result = base.MapToDomainModel(userDataDao);
            result.Id = (int)userDataDao.UserId;
            result.Name = $"{userDataDao.FirstName} {userDataDao.LastName}";  
            return result;
        }

        public override UserDataDao MapToDatabaseModel(UserInfo source)
        {
            var result = base.MapToDatabaseModel(source);
            result.UserId = source.Id;
            result.FirstName = source.Name.Split(" ").FirstOrDefault();
            result.LastName = source.Name.Split(" ").LastOrDefault();
            return result;
        }
    }

    [Mapper(ClassName = "InterfaceUserDaoArrayMapper")]
    public partial interface IUserDataArrayMapper
    {
        [Mapping(Source = nameof(UserWithArrayDao.FirstName), Target = nameof(UserInfoWithArray.Name))]
        [InverseMapping(InverseMethodName = "MapToDatabaseModel")]
        UserInfoWithArray MapToDomainModel(UserWithArrayDao userWithArrayDao);

        [InverseMapping(InverseMethodName = "MapFromAddress")]
        Address MapAddress(AddressDao addressDao);

        [InverseMapping(InverseMethodName = "MapFromRegion")]
        Region MapRegion(RegionDao regionDao);
    }

    public class CustomUserDaoArrayMapper : InterfaceUserDaoArrayMapper
    {
        public override UserInfoWithArray MapToDomainModel(UserWithArrayDao userDataDao)
        {
            var result = base.MapToDomainModel(userDataDao);
            result.Addresses = userDataDao.UserAddresses.Select(addressDao => base.MapAddress(addressDao)).ToArray();
            return result;
        }

        public override UserWithArrayDao MapToDatabaseModel(UserInfoWithArray userWithArrayDao)
        {
            var result = base.MapToDatabaseModel(userWithArrayDao);
            result.UserAddresses = userWithArrayDao.Addresses.Select(address => base.MapFromAddress(address)).ToArray();
            return result;
        }
    }
}
