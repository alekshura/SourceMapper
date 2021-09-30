using AutoFixture;
using AutoFixture.AutoMoq;
using Compentio.SourceMapper.Tests.Entities;
using Compentio.SourceMapper.Tests.Mappings;
using FluentAssertions;
using Xunit;

namespace Compentio.SourceMapper.Tests.Processors
{
    public class ClassUserMapperTests
    {
        private readonly IFixture _fixture;
        private readonly UserMapper _userMapperClass;

        public ClassUserMapperTests()
        {
            _fixture = new Fixture()
                .Customize(new AutoMoqCustomization { ConfigureMembers = true })
                .Customize(new SupportMutableValueTypesCustomization());

            _userMapperClass = new ClassUserDaoMapper();
        }


        [Fact]
        public void Mapper_User_Dao_Match_Converters()
        {
            // Arrange 
            var userDao = _fixture.Build<UserDao>()
                .With(p => p.UserGender, UserGender.Female)
                .Create();

            // Act
            var mappingResult = _userMapperClass.MapToDomainMoodel(userDao);

            // Assert
            mappingResult.Name.Should().Be($"{userDao.FirstName} {userDao.LastName}");
            mappingResult.BirthDate.Should().Be(userDao.BirthDate);
            mappingResult.Id.Should().Be((int)userDao.UserId);
            mappingResult.Sex.Should().Be(Sex.W);            
        }
    }
}
