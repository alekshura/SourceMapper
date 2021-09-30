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
    }
}
