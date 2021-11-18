using AutoFixture;
using AutoFixture.AutoMoq;
using Compentio.SourceMapper.Attributes;
using Compentio.SourceMapper.Metadata;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Compentio.SourceMapper.Tests.Attributes
{
    public class InverseAttributeTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IMethodMetadata> _mockMethodMetadata;
        private readonly Mock<MappingAttribute> _mockMappingAttribute;

        public InverseAttributeTests()
        {
            _fixture = new Fixture()
                           .Customize(new AutoMoqCustomization { ConfigureMembers = true })
                           .Customize(new SupportMutableValueTypesCustomization());
            _mockMethodMetadata = _fixture.Create<Mock<IMethodMetadata>>();
            _mockMappingAttribute = _fixture.Create<Mock<MappingAttribute>>();
        }

        [Fact]
        public void AnyInverseMethod_ForInverseMethod_ReturnTrue()
        {
            // Arrange
            _mockMappingAttribute.Setup(m => m.CreateInverse).Returns(true);
            _mockMethodMetadata.Setup(m => m.MappingAttributes).Returns(new List<MappingAttribute> { _mockMappingAttribute.Object });

            // Act
            var result = InverseAttribute.AnyInverseMethod(new List<IMethodMetadata> { _mockMethodMetadata.Object });

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void AnyInverseMethod_NoAttributes_ReturnFalse()
        {
            // Arrange
            _mockMethodMetadata.Setup(m => m.MappingAttributes).Returns((List<MappingAttribute>)null);

            // Act
            var result = InverseAttribute.AnyInverseMethod(new List<IMethodMetadata> { _mockMethodMetadata.Object });

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsInverseMethod_ForInverseMethod_ReturnTrue()
        {
            // Arrange
            _mockMappingAttribute.Setup(m => m.CreateInverse).Returns(true);
            _mockMethodMetadata.Setup(m => m.MappingAttributes).Returns(new List<MappingAttribute> { _mockMappingAttribute.Object });

            // Act
            var result = InverseAttribute.IsInverseMethod(_mockMethodMetadata.Object);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void GetInverseMethodName_ValidInverse_ReturnName()
        {
            // Arrange
            _mockMappingAttribute.Setup(m => m.InverseMethodName).Returns("InverseMethodName");
            _mockMethodMetadata.Setup(m => m.MappingAttributes).Returns(new List<MappingAttribute> { _mockMappingAttribute.Object });

            // Act
            var result = InverseAttribute.GetInverseMethodName(_mockMethodMetadata.Object);

            //Assert
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetInverseMethodName_EmptyName_ReturnEmptyString()
        {
            // Arrange
            _mockMappingAttribute.Setup(m => m.InverseMethodName).Returns(string.Empty);
            _mockMethodMetadata.Setup(m => m.MappingAttributes).Returns(new List<MappingAttribute> { _mockMappingAttribute.Object });

            // Act
            var result = InverseAttribute.GetInverseMethodName(_mockMethodMetadata.Object);

            //Assert
            result.Should().BeNullOrEmpty();
        }

        [Fact]
        public void GetInverseMethodName_AttributeDuplicate_ThrowException()
        {
            // Arrange
            _mockMappingAttribute.Setup(m => m.InverseMethodName).Returns("InverseMethodName");
            _mockMethodMetadata.Setup(m => m.MappingAttributes).Returns(new List<MappingAttribute>
            {
                _mockMappingAttribute.Object,
                _mockMappingAttribute.Object,
                _mockMappingAttribute.Object,
                _mockMappingAttribute.Object
            });

            // Act
            Action result = () => InverseAttribute.GetInverseMethodName(_mockMethodMetadata.Object);

            //Assert
            result.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void GetInverseMethodFullName_ValidData_ReturnFullName()
        {
            // Act
            var result = InverseAttribute.GetInverseMethodFullName(_mockMethodMetadata.Object, "InverseMethodName");

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().NotBeSameAs("InverseMethodName");
        }
    }
}