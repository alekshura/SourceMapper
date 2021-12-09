//using Compentio.SourceMapper.Attributes;
//using Compentio.SourceMapper.Tests.Entities;

//namespace Compentio.SourceMapper.Tests.Mappings
//{
//    [Mapper(ClassName = "InterfaceUserDaoMapper")]
//    public partial interface IUserDaoMapper
//    {
//        [Mapping(Source = nameof(UserDao.FirstName), Target = nameof(UserInfo.Name))]
//        [InverseMapping(InverseMethodName = "MapToDatabaseModel")]
//        UserInfo MapToDomainModel(UserDao source);

//        [InverseMapping(InverseMethodName = "MapFromAddress")]
//        Address MapAddress(UserDao source);

//        [InverseMapping(InverseMethodName = "MapFromRegion")]
//        Region MapRegion(UserDao source);
//    }

//    [Mapper(ClassName = "InterfaceUserDataDaoMapper")]
//    public partial interface IUserDataDaoMapper
//    {
//        [Mapping(Source = nameof(UserDataDao.FirstName), Target = nameof(UserInfo.Name))]
//        [Mapping(Source = nameof(UserDataDao.UserAddress), Target = nameof(UserInfo.Address))]
//        [InverseMapping(InverseMethodName = "MapToDatabaseModel")]
//        UserInfo MapToDomainModel(UserDataDao source);

//        [InverseMapping(InverseMethodName = "MapFromAddress")]
//        Address MapAddress(AddressDao addressDao);

//        [InverseMapping(InverseMethodName = "MapFromRegion")]
//        Region MapRegion(RegionDao regionDao);
//    }

//    [Mapper(ClassName = "InterfaceUserDaoArrayMapper")]
//    public partial interface IUserDataArrayMapper
//    {
//        [Mapping(Source = nameof(UserWithArrayDao.FirstName), Target = nameof(UserInfoWithArray.Name))]
//        [InverseMapping(InverseMethodName = "MapToDatabaseModel")]
//        UserInfoWithArray MapToDomainModel(UserWithArrayDao userWithArrayDao);

//        [InverseMapping(InverseMethodName = "MapFromAddress")]
//        Address MapAddress(AddressDao addressDao);

//        [InverseMapping(InverseMethodName = "MapFromRegion")]
//        Region MapRegion(RegionDao regionDao);
//    }
//}