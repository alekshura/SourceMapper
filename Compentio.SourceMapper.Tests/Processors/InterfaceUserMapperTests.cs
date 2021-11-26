using AutoFixture;
using AutoFixture.AutoMoq;
using Compentio.SourceMapper.Tests.Entities;
using Compentio.SourceMapper.Tests.Mappings;
using FluentAssertions;
using Xunit;

namespace Compentio.SourceMapper.Tests.Processors
{
    public class InterfaceUserMapperTests
    {
        private readonly IFixture _fixture;

        public InterfaceUserMapperTests()
        {
            _fixture = new Fixture()
                .Customize(new AutoMoqCustomization { ConfigureMembers = true })
                .Customize(new SupportMutableValueTypesCustomization());           
        }

        [Fact]
        public void Mapper_User_Dao_Match_Properties_And_Attributes()
        {
            // Arrange 
            var _userMapperInterface = new InterfaceUserDaoMapper();
            var userDao = _fixture.Create<UserDao>();

            // Act
            var mappingResult = _userMapperInterface.MapToDomainModel(userDao);

            // Assert
            mappingResult.Name.Should().Be(userDao.FirstName);
            mappingResult.BirthDate.Should().Be(userDao.BirthDate);
            // Not mapped
            mappingResult.Id.Should().NotBe((int)userDao.UserId);
            mappingResult.Address.Should().BeNull();
        }

        [Fact]
        public void Mapper_User_Info_Match_Properties_And_Attributes()
        {
            // Arrange 
            var _userMapperInterface = new InterfaceUserDaoMapper();
            var userInfo = _fixture.Create<UserInfo>();

            // Act
            var mappingResult = _userMapperInterface.MapToDatabaseModel(userInfo);

            // Assert
            mappingResult.FirstName.Should().Be(userInfo.Name);
            mappingResult.BirthDate.Should().Be(userInfo.BirthDate);
            //// Not mapped
            mappingResult.UserId.Should().NotBe(userInfo.Id);
            mappingResult.City.Should().BeNull();
            mappingResult.House.Should().BeNull();
            mappingResult.ZipCode.Should().BeNull();
            mappingResult.District.Should().BeNull();
            mappingResult.State.Should().BeNull();
        }

        [Fact]
        public void Mapper_User_Data_Dao_Match_Properties_And_Attributes()
        {
            // Arrange 
            var _userMapperInterface = new InterfaceUserDataDaoMapper();
            var userDataDao = _fixture.Create<UserDataDao>();

            // Act
            var mappingResult = _userMapperInterface.MapToDomainModel(userDataDao);

            // Assert
            mappingResult.Id.Should().NotBe((int)userDataDao.UserId);

            mappingResult.Name.Should().Be(userDataDao.FirstName);
            mappingResult.BirthDate.Should().Be(userDataDao.BirthDate);
            
            mappingResult.Address.Should().NotBeNull();
            mappingResult.Address.City.Should().Be(userDataDao.UserAddress.City);
            mappingResult.Address.House.Should().Be(userDataDao.UserAddress.House);
            mappingResult.Address.Street.Should().Be(userDataDao.UserAddress.Street);

            mappingResult.Address.Region.Should().NotBeNull();
            mappingResult.Address.Region.State.Should().Be(userDataDao.UserAddress.Region.State);
            mappingResult.Address.Region.District.Should().Be(userDataDao.UserAddress.Region.District);
        }

        [Fact]
        public void Mapper_User_Data_Info_Match_Properties_And_Attributes()
        {
            // Arrange 
            var _userMapperInterface = new InterfaceUserDataDaoMapper();
            var userInfo = _fixture.Create<UserInfo>();

            // Act
            var mappingResult = _userMapperInterface.MapToDatabaseModel(userInfo);

            // Assert
            mappingResult.UserId.Should().NotBe(userInfo.Id);

            mappingResult.FirstName.Should().Be(userInfo.Name);
            mappingResult.BirthDate.Should().Be(userInfo.BirthDate);

            mappingResult.UserAddress.Should().NotBeNull();
            mappingResult.UserAddress.City.Should().Be(userInfo.Address.City);
            mappingResult.UserAddress.House.Should().Be(userInfo.Address.House);
            mappingResult.UserAddress.Street.Should().Be(userInfo.Address.Street);

            mappingResult.UserAddress.Region.Should().NotBeNull();
            mappingResult.UserAddress.Region.State.Should().Be(userInfo.Address.Region.State);
            mappingResult.UserAddress.Region.District.Should().Be(userInfo.Address.Region.District);
        }

        [Fact]
        public void Mapper_User_Data_Dao_With_Extension_In_Inherited_Class()
        {
            // Arrange 
            var _userMapperInterface = new CustomUserDataDaoMapper();
            var userDataDao = _fixture.Create<UserDataDao>();

            // Act
            var mappingResult = _userMapperInterface.MapToDomainModel(userDataDao);

            // Assert
            mappingResult.Id.Should().Be((int)userDataDao.UserId);
            mappingResult.Name.Should().Be($"{userDataDao.FirstName} {userDataDao.LastName}");
            mappingResult.BirthDate.Should().Be(userDataDao.BirthDate);

            mappingResult.Address.Should().NotBeNull();
            mappingResult.Address.City.Should().Be(userDataDao.UserAddress.City);
            mappingResult.Address.House.Should().Be(userDataDao.UserAddress.House);
            mappingResult.Address.Street.Should().Be(userDataDao.UserAddress.Street);

            mappingResult.Address.Region.Should().NotBeNull();
            mappingResult.Address.Region.State.Should().Be(userDataDao.UserAddress.Region.State);
            mappingResult.Address.Region.District.Should().Be(userDataDao.UserAddress.Region.District);
        }

        [Fact]
        public void Mapper_User_Info_With_Extension_In_Inherited_Class()
        {
            // Arrange 
            var _userMapperInterface = new CustomUserDataDaoMapper();
            var userInfo = _fixture.Create<UserInfo>();

            // Act
            var mappingResult = _userMapperInterface.MapToDatabaseModel(userInfo);

            // Assert
            mappingResult.UserId.Should().Be(userInfo.Id);
            mappingResult.FirstName.Should().Be(userInfo.Name);
            mappingResult.LastName.Should().Be(userInfo.Name);
            mappingResult.BirthDate.Should().Be(userInfo.BirthDate);

            mappingResult.UserAddress.Should().NotBeNull();
            mappingResult.UserAddress.City.Should().Be(userInfo.Address.City);
            mappingResult.UserAddress.House.Should().Be(userInfo.Address.House);
            mappingResult.UserAddress.Street.Should().Be(userInfo.Address.Street);

            mappingResult.UserAddress.Region.Should().NotBeNull();
            mappingResult.UserAddress.Region.State.Should().Be(userInfo.Address.Region.State);
            mappingResult.UserAddress.Region.District.Should().Be(userInfo.Address.Region.District);
        }

        [Fact]
        public void Mapper_User_With_Array_Dao_Match_Properties_And_Attributes()
        {
            // Arrange 
            var _userMapperInterface = new InterfaceUserDaoArrayMapper();
            var userWithArrayDao = _fixture.Create<UserWithArrayDao>();

            // Act
            var mappingResult = _userMapperInterface.MapToDomainModel(userWithArrayDao);

            // Assert
            mappingResult.Id.Should().NotBe((int)userWithArrayDao.UserId);

            mappingResult.Name.Should().Be(userWithArrayDao.FirstName);
            mappingResult.BirthDate.Should().Be(userWithArrayDao.BirthDate);

            mappingResult.Addresses.Should().BeNull();
        }

        [Fact]
        public void Mapper_User_Info_With_Array_Match_Properties_And_Attributes()
        {
            // Arrange 
            var _userMapperInterface = new InterfaceUserDaoArrayMapper();
            var userInfoWithArray = _fixture.Create<UserInfoWithArray>();

            // Act
            var mappingResult = _userMapperInterface.MapToDatabaseModel(userInfoWithArray);

            // Assert
            mappingResult.UserId.Should().NotBe(userInfoWithArray.Id);

            mappingResult.FirstName.Should().Be(userInfoWithArray.Name);
            mappingResult.BirthDate.Should().Be(userInfoWithArray.BirthDate);

            mappingResult.UserAddresses.Should().BeNull();
        }

        [Fact]
        public void Mapper_User_With_Array_Dao_With_Extension_In_Inherited_Class()
        {
            // Arrange 
            var _userMapperInterface = new CustomUserDaoArrayMapper();
            var userWithArrayDao = _fixture.Create<UserWithArrayDao>();

            // Act
            var mappingResult = _userMapperInterface.MapToDomainModel(userWithArrayDao);

            // Assert
            mappingResult.Id.Should().NotBe((int)userWithArrayDao.UserId);

            mappingResult.Name.Should().Be(userWithArrayDao.FirstName);
            mappingResult.BirthDate.Should().Be(userWithArrayDao.BirthDate);

            mappingResult.Addresses.Should().NotBeNull();
        }

        [Fact]
        public void Mapper_User_Info_With_Array_With_Extension_In_Inherited_Class()
        {
            // Arrange 
            var _userMapperInterface = new CustomUserDaoArrayMapper();
            var userInfoWithArray = _fixture.Create<UserInfoWithArray>();

            // Act
            var mappingResult = _userMapperInterface.MapToDatabaseModel(userInfoWithArray);

            // Assert
            mappingResult.UserId.Should().NotBe(userInfoWithArray.Id);

            mappingResult.FirstName.Should().Be(userInfoWithArray.Name);
            mappingResult.BirthDate.Should().Be(userInfoWithArray.BirthDate);

            mappingResult.UserAddresses.Should().NotBeNull();
        }

        [Fact]
        public void Mapper_Dao_With_Other_Mapper_Converts()
        {
            // Arrange 
            var _userMapperInterface = new InterfaceWithBaseMapper();
            var userDataDao = _fixture.Create<UserDataDao>();

            // Act
            var mappingResult = _userMapperInterface.MapToDomainModel(userDataDao);

            // Assert
            mappingResult.Id.Should().NotBe((int)userDataDao.UserId);

            mappingResult.Name.Should().Be(userDataDao.FirstName);
            mappingResult.BirthDate.Should().Be(userDataDao.BirthDate);

            mappingResult.Address.Should().NotBeNull();
            mappingResult.Address.City.Should().Be(userDataDao.UserAddress.City);
            mappingResult.Address.House.Should().Be(userDataDao.UserAddress.House);
            mappingResult.Address.Street.Should().Be(userDataDao.UserAddress.Street);

            mappingResult.Address.Region.Should().NotBeNull();
            mappingResult.Address.Region.State.Should().Be(userDataDao.UserAddress.Region.State);
            mappingResult.Address.Region.District.Should().Be(userDataDao.UserAddress.Region.District);
        }

        [Fact]
        public void Mapper_Info_With_Other_Mapper_Converts()
        {
            // Arrange
            var userMapperInterface = new InterfaceWithBaseMapper();
            var userInfo = _fixture.Create<UserInfo>();

            // Act
            var mappingResult = userMapperInterface.MapToDatabaseModel(userInfo);

            // Assert
            mappingResult.UserId.Should().NotBe(userInfo.Id);

            mappingResult.FirstName.Should().Be(userInfo.Name);
            mappingResult.BirthDate.Should().Be(userInfo.BirthDate);

            mappingResult.UserAddress.Should().NotBeNull();
            mappingResult.UserAddress.City.Should().Be(userInfo.Address.City);
            mappingResult.UserAddress.House.Should().Be(userInfo.Address.House);
            mappingResult.UserAddress.Street.Should().Be(userInfo.Address.Street);

            mappingResult.UserAddress.Region.Should().NotBeNull();
            mappingResult.UserAddress.Region.State.Should().Be(userInfo.Address.Region.State);
            mappingResult.UserAddress.Region.District.Should().Be(userInfo.Address.Region.District);
        }
    }
}
