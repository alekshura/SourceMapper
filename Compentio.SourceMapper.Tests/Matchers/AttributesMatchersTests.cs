using AutoFixture;
using AutoFixture.AutoMoq;
using Compentio.SourceMapper.Matchers;
using Compentio.SourceMapper.Metadata;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Compentio.SourceMapper.Tests.Matchers
{
    public class AttributesMatchersTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IMethodMetadata> _mockMethodMetadata;

        public AttributesMatchersTests()
        {
            _fixture = new Fixture()
                           .Customize(new AutoMoqCustomization { ConfigureMembers = true })
                           .Customize(new SupportMutableValueTypesCustomization());
            _mockMethodMetadata = _fixture.Create<Mock<IMethodMetadata>>();
        }

        [Fact]
        public void AnyInverseMethod_ForInverseMethod_ReturnTrue()
        {
            // Act
            var result = AttributesMatchers.AnyInverseMethod(new List<IMethodMetadata> { _mockMethodMetadata.Object });

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void AnyInverseMethod_NoAttributes_ReturnFalse()
        {
            // Arrange
            _mockMethodMetadata.Setup(m => m.InverseMethodName).Returns(string.Empty);

            // Act
            var result = AttributesMatchers.AnyInverseMethod(new List<IMethodMetadata> { _mockMethodMetadata.Object });

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsInverseMethod_ForInverseMethod_ReturnTrue()
        {
            // Act
            var result = AttributesMatchers.IsInverseMethod(_mockMethodMetadata.Object);

            // Assert
            result.Should().BeTrue();
        }
    }
}