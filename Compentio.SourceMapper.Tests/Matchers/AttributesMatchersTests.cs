using AutoFixture;
using AutoFixture.AutoMoq;
using Compentio.SourceMapper.Attributes;
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
        private readonly Mock<MappingAttribute> _mockMappingAttribute;
        private readonly Mock<IPropertyMetadata> _mockTargetPropertyMetadata;
        private readonly Mock<IPropertyMetadata> _mockSourcePropertyMetadata;

        public AttributesMatchersTests()
        {
            _fixture = new Fixture()
                           .Customize(new AutoMoqCustomization { ConfigureMembers = true })
                           .Customize(new SupportMutableValueTypesCustomization());
            _mockMethodMetadata = _fixture.Create<Mock<IMethodMetadata>>();
            _mockMappingAttribute = _fixture.Create<Mock<MappingAttribute>>();
            _mockTargetPropertyMetadata = _fixture.Create<Mock<IPropertyMetadata>>();
            _mockSourcePropertyMetadata = _fixture.Create<Mock<IPropertyMetadata>>();
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

        [Fact]
        public void MatchTargetAttribute_Match()
        {
            // Arrange
            IEnumerable<MappingAttribute> mappingAttributes = new List<MappingAttribute> { _mockMappingAttribute.Object };
            _mockMappingAttribute.Setup(m => m.Target).Returns(_mockTargetPropertyMetadata.Object.Name);

            // Act
            var result = mappingAttributes.MatchTargetAttribute(_mockTargetPropertyMetadata.Object);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void MatchTargetAttribute_NotMatch()
        {
            // Arrange
            IEnumerable<MappingAttribute> mappingAttributes = new List<MappingAttribute> { _mockMappingAttribute.Object };
            _mockMappingAttribute.Setup(m => m.Target).Returns(string.Empty);

            // Act
            var result = mappingAttributes.MatchTargetAttribute(_mockTargetPropertyMetadata.Object);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void MatchExpressionAttribute_MatchSourceAndTarget()
        {
            // Arrange
            IEnumerable<MappingAttribute> mappingAttributes = new List<MappingAttribute> { _mockMappingAttribute.Object };
            _mockMappingAttribute.Setup(m => m.Target).Returns(_mockTargetPropertyMetadata.Object.Name);
            _mockMappingAttribute.Setup(m => m.Source).Returns(_mockSourcePropertyMetadata.Object.Name);

            // Act
            var result = mappingAttributes.MatchExpressionAttribute(_mockTargetPropertyMetadata.Object, _mockSourcePropertyMetadata.Object);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void MatchExpressionAttribute_MatchTarget()
        {
            // Arrange
            IEnumerable<MappingAttribute> mappingAttributes = new List<MappingAttribute> { _mockMappingAttribute.Object };
            _mockMappingAttribute.Setup(m => m.Target).Returns(_mockTargetPropertyMetadata.Object.Name);

            // Act
            var result = mappingAttributes.MatchExpressionAttribute(_mockTargetPropertyMetadata.Object, _mockSourcePropertyMetadata.Object);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void MatchExpressionAttribute_NotMatchSourceAndTarget()
        {
            // Arrange
            IEnumerable<MappingAttribute> mappingAttributes = new List<MappingAttribute> { _mockMappingAttribute.Object };
            _mockMappingAttribute.Setup(m => m.Target).Returns(string.Empty);
            _mockMappingAttribute.Setup(m => m.Source).Returns(string.Empty);

            // Act
            var result = mappingAttributes.MatchExpressionAttribute(_mockTargetPropertyMetadata.Object, _mockSourcePropertyMetadata.Object);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void MatchExpressionAttribute_NotMatchTarget()
        {
            // Arrange
            IEnumerable<MappingAttribute> mappingAttributes = new List<MappingAttribute> { _mockMappingAttribute.Object };
            _mockMappingAttribute.Setup(m => m.Target).Returns(string.Empty);
            _mockMappingAttribute.Setup(m => m.Source).Returns(_mockSourcePropertyMetadata.Object.Name);

            // Act
            var result = mappingAttributes.MatchExpressionAttribute(_mockTargetPropertyMetadata.Object, _mockSourcePropertyMetadata.Object);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void IgnorePropertyMapping_IgnoreDueToSourceMetadata()
        {
            // Arrange
            _mockSourcePropertyMetadata.Setup(s => s.IgnoreInMapping).Returns(true);
            _mockTargetPropertyMetadata.Setup(t => t.IgnoreInMapping).Returns(false);

            // Act
            var result = AttributesMatchers.IgnorePropertyMapping(_mockSourcePropertyMetadata.Object, _mockTargetPropertyMetadata.Object);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IgnorePropertyMapping_IgnoreDueToTargetMetadata()
        {
            // Arrange
            _mockSourcePropertyMetadata.Setup(s => s.IgnoreInMapping).Returns(false);
            _mockTargetPropertyMetadata.Setup(t => t.IgnoreInMapping).Returns(true);

            // Act
            var result = AttributesMatchers.IgnorePropertyMapping(_mockSourcePropertyMetadata.Object, _mockTargetPropertyMetadata.Object);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IgnorePropertyMapping_NullProperties_ReturnFalse()
        {
            // Act
            var result = AttributesMatchers.IgnorePropertyMapping(null, null);

            // Assert
            result.Should().BeFalse();
        }
    }
}