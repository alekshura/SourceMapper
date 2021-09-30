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
            var mappingResult = userMapperClass.MapToDomainMoodel(userDao);

            // Assert
            mappingResult.Name.Should().Be($"{userDao.FirstName} {userDao.LastName}");
            mappingResult.BirthDate.Should().Be(userDao.BirthDate);
            mappingResult.Id.Should().Be((int)userDao.UserId);
            mappingResult.Sex.Should().Be(Sex.W);            
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
    }
}
