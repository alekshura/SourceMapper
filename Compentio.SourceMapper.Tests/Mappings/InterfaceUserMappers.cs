using Compentio.SourceMapper.Attributes;
using Compentio.SourceMapper.Tests.Entities;
using System.Linq;

namespace Compentio.SourceMapper.Tests.Mappings
{
    [Mapper(ClassName = "InterfaceUserDaoMapper")]
    public interface IUserDaoMapper
    {
        [Mapping(Source = nameof(UserDao.FirstName), Target = nameof(UserInfo.Name))]
        UserInfo MapToDomainModel(UserDao userDao);
        Address MapAddress(UserDao userDao);
        Region MapRegion(UserDao userDao);
    }

    [Mapper(ClassName = "InterfaceUserDataDaoMapper")]
    public interface IUserDataDaoMapper
    {
        [Mapping(Source = nameof(UserDataDao.FirstName), Target = nameof(UserInfo.Name))]
        [Mapping(Source = nameof(UserDataDao.UserAddress), Target = nameof(UserInfo.Address))]
        UserInfo MapToDomainModel(UserDataDao userDataDao);

        Address MapAddress(AddressDao addressDao);

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
    }

    [Mapper(ClassName = "InterfaceUserDaoArrayMapper")]
    public interface IUserDataArrayMapper
    {
        [Mapping(Source = nameof(UserWithArrayDao.FirstName), Target = nameof(UserInfoWithArray.Name))]
        UserInfoWithArray MapToDomainModel(UserWithArrayDao userWithArrayDao);

        Address MapAddress(AddressDao addressDao);

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
    }
}
