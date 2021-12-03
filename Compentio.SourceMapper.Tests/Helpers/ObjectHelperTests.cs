using AutoFixture;
using AutoFixture.AutoMoq;
using Compentio.SourceMapper.Helpers;
using FluentAssertions;
using Moq;
using Xunit;

namespace Compentio.SourceMapper.Tests.Helpers
{
    public class ObjectHelperTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<object> _mockSourceObject;
        private readonly Mock<object> _mockTargetObject;

        public ObjectHelperTests()
        {
            _fixture = new Fixture()
               .Customize(new AutoMoqCustomization { ConfigureMembers = true })
               .Customize(new SupportMutableValueTypesCustomization());
            _mockSourceObject = _fixture.Create<Mock<object>>();
            _mockTargetObject = _fixture.Create<Mock<object>>();
        }

        [Fact]
        public void Swap_SourceAndTarget_ValidResult()
        {
            // Arrange
            var sourceObject = _mockSourceObject.Object;
            var targetObject = _mockTargetObject.Object;

            // Act
            ObjectHelper.Swap(ref sourceObject, ref targetObject);

            // Assert
            _mockSourceObject.Object.Should().BeSameAs(targetObject);
            _mockTargetObject.Object.Should().BeSameAs(sourceObject);
            _mockSourceObject.Object.Should().NotBeSameAs(sourceObject);
            _mockTargetObject.Object.Should().NotBeSameAs(targetObject);
        }
    }
}