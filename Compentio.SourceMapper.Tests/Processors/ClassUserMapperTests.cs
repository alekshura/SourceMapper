using AutoFixture;
using AutoFixture.AutoMoq;
using Compentio.SourceMapper.Tests.Entities;
using Compentio.SourceMapper.Tests.Mappings;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Compentio.SourceMapper.Tests.Processors
{
    public class ClassUserMapperTests
    {
        private readonly IFixture _fixture;

        public ClassUserMapperTests()
        {
            _fixture = new Fixture()
                .Customize(new AutoMoqCustomization { ConfigureMembers = true })
                .Customize(new SupportMutableValueTypesCustomization());         
        }


        [Fact]
        public void Mapper_User_Dao_Match_Converters()
        {
            // Arrange 
            var userMapperClass = new ClassUserDaoMapper();
            var userDao = _fixture.Build<UserDao>()
                .With(p => p.UserGender, UserGender.Female)
                .Create();

            // Act
            var mappingResult = userMapperClass.MapToDomainModel(userDao);

            // Assert
            mappingResult.Name.Should().Be($"{userDao.FirstName} {userDao.LastName}");
            mappingResult.BirthDate.Should().Be(userDao.BirthDate);
            mappingResult.Id.Should().Be((int)userDao.UserId);
            mappingResult.Sex.Should().Be(Sex.W);            
        }

        [Fact]
        public void Mapper_User_Info_Match_Converters()
        {
            // Arrange 
            var userMapperClass = new ClassUserDaoMapper();
            var userInfo = _fixture.Build<UserInfo>()
                .With(p => p.Sex, Sex.W)
                .Create();

            // Act
            var mappingResult = userMapperClass.MapToDatabaseModel(userInfo);

            // Assert
            mappingResult.BirthDate.Should().Be(userInfo.BirthDate);
            mappingResult.UserGender.Should().Be(UserGender.Female);
            mappingResult.UserId.Should().Be(userInfo.Id);

            // Not mapped
            mappingResult.FirstName.Should().BeNullOrEmpty();
            mappingResult.LastName.Should().BeNullOrEmpty();
        }

        [Fact]
        public void Mapper_User_Data_Dao_With_Array()
        {
            // Arrange 
            var userMapperClass = new ClassUserDaoArrayMapper();
            var userDao = _fixture.Build<UserWithArrayDao>()
                .Create();

            // Act
            var mappingResult = userMapperClass.MapToDomainModel(userDao);
            var addresses = userDao.UserAddresses.Select(a => userMapperClass.MapAddress(a)).ToArray();

            // Assert
            mappingResult.Addresses.Should().NotBeEmpty();
            mappingResult.Addresses.Should().BeEquivalentTo(addresses);
        }

        [Fact]
        public void Mapper_User_Info_With_Array()
        {
            // Arrange 
            var userMapperClass = new ClassUserDaoArrayMapper();
            var userInfo = _fixture.Build<UserInfoWithArray>()
                .Create();

            // Act
            var mappingResult = userMapperClass.MapToDatabaseModel(userInfo);
            var addresses = userInfo.Addresses.Select(a => userMapperClass.MapFromAddress(a)).ToArray();

            // Assert
            mappingResult.UserAddresses.Should().NotBeEmpty();
            mappingResult.UserAddresses.Should().BeEquivalentTo(addresses);
        }

        [Fact]
        public void Mapper_User_Data_Dao_With_List()
        {
            // Arrange 
            var userMapperClass = new ClassUserDaoListMapper();
            var userDao = _fixture.Build<UserWithListDao>()
                .Create();

            // Act
            var mappingResult = userMapperClass.MapToDomainModel(userDao);
            var addresses = userDao.UserAddresses.Select(a => userMapperClass.MapAddress(a)).ToList();

            // Assert
            mappingResult.Addresses.Should().NotBeEmpty();
            mappingResult.Addresses.Should().BeEquivalentTo(addresses);
        }

        [Fact]
        public void Mapper_User_Info_With_List()
        {
            // Arrange 
            var userMapperClass = new ClassUserDaoListMapper();
            var userInfo = _fixture.Build<UserInfoWithList>()
                .Create();

            // Act
            var mappingResult = userMapperClass.MapToDatabaseModel(userInfo);
            var addresses = userInfo.Addresses.Select(a => userMapperClass.MapFromAddress(a)).ToList();

            // Assert
            mappingResult.UserAddresses.Should().NotBeEmpty();
            mappingResult.UserAddresses.Should().BeEquivalentTo(addresses);
        }

        [Fact]
        public void Mapper_User_Data_Dao_With_Base_Mapper()
        {
            // Arrange 
            var userMapperClass = new ClassUserDaoWithBaseMapper();
            var userDataDao = _fixture.Build<UserDataDao>()
                .With(p => p.UserGender, UserGender.Female)
                .Create();

            // Act
            var mappingResult = userMapperClass.MapToDomainModel(userDataDao);

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
        public void Mapper_User_Info_With_Base_Mapper()
        {
            // Arrange 
            var userMapperClass = new ClassUserDaoWithBaseMapper();
            var userInfo = _fixture.Build<UserInfo>()
                .With(p => p.Sex, Sex.W)
                .Create();

            // Act
            var mappingResult = userMapperClass.MapToDatabaseModel(userInfo);

            // Assert
            mappingResult.UserId.Should().Be(userInfo.Id);
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
        public void Mapper_User_Dao_List_With_Base_Mapper()
        {
            // Arrange 
            var userMapperClass = new ClassUserListWithBaseMapper();
            var userDao = _fixture.Build<UserDataWithListDao>()
                .Create();

            // Act
            var mappingResult = userMapperClass.MapToDto(userDao);
            var addresses = userDao.UserAddresses.Select(a => userMapperClass.MapAddress(a)).ToList();

            // Assert
            mappingResult.Addresses.Should().NotBeEmpty();
            mappingResult.Addresses.Should().BeEquivalentTo(addresses);
        }
    }
}
