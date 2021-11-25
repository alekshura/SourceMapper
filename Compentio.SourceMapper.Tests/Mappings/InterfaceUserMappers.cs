using Compentio.SourceMapper.Attributes;
using Compentio.SourceMapper.Tests.Entities;
using System.Linq;

namespace Compentio.SourceMapper.Tests.Mappings
{
    [Mapper(ClassName = "InterfaceUserDaoMapper")]
    public partial interface IUserDaoMapper
    {
        [Mapping(Source = nameof(UserDao.FirstName), Target = nameof(UserInfo.Name))]
        [Mapping(CreateInverse = true, InverseMethodName = "MapToDatabaseModel")]
        UserInfo MapToDomainModel(UserDao source);
        [Mapping(CreateInverse = true, InverseMethodName = "MapFromAddress")]
        Address MapAddress(UserDao source);
        [Mapping(CreateInverse = true, InverseMethodName = "MapFromRegion")]
        Region MapRegion(UserDao source);
    }

    [Mapper(ClassName = "InterfaceUserDataDaoMapper")]
    public partial interface IUserDataDaoMapper
    {
        [Mapping(Source = nameof(UserDataDao.FirstName), Target = nameof(UserInfo.Name))]
        [Mapping(Source = nameof(UserDataDao.UserAddress), Target = nameof(UserInfo.Address))]
        [Mapping(CreateInverse = true, InverseMethodName = "MapToDatabaseModel")]
        UserInfo MapToDomainModel(UserDataDao source);

        [Mapping(CreateInverse = true, InverseMethodName = "MapFromAddress")]
        Address MapAddress(AddressDao addressDao);

        [Mapping(CreateInverse = true, InverseMethodName = "MapFromRegion")]
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

    [Mapper(ClassName = "InterfaceWithBaseMapper", UseMapper = nameof(IUserDataArrayMapper))]
    public partial interface IUserDataDaoWithBaseMapper
    {
        [Mapping(Source = nameof(UserDataDao.FirstName), Target = nameof(UserInfo.Name))]
        [Mapping(Source = nameof(UserDataDao.UserAddress), Target = nameof(UserInfo.Address))]
        [Mapping(CreateInverse = true, InverseMethodName = "MapToDatabaseModel")]
        UserInfo MapToDomainModel(UserDataDao source);
    }

    [Mapper(ClassName = "InterfaceUserDaoArrayMapper")]
    public partial interface IUserDataArrayMapper
    {
        [Mapping(Source = nameof(UserWithArrayDao.FirstName), Target = nameof(UserInfoWithArray.Name))]
        [Mapping(CreateInverse = true, InverseMethodName = "MapToDatabaseModel")]
        UserInfoWithArray MapToDomainModel(UserWithArrayDao userWithArrayDao);

        [Mapping(CreateInverse = true, InverseMethodName = "MapFromAddress")]
        Address MapAddress(AddressDao addressDao);

        [Mapping(CreateInverse = true, InverseMethodName = "MapFromRegion")]
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
