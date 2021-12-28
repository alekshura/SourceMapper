using AutoFixture;
using AutoFixture.AutoMoq;
using Compentio.SourceMapper.Metadata;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Moq;
using System.Collections.Generic;
using System.Collections.Immutable;
using Xunit;

namespace Compentio.SourceMapper.Tests.Metadata
{
    public class ParameterTypeMetadataTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IParameterSymbol> _mockParameterSymbol;
        private readonly Mock<Location> _mockLocation;
        private readonly Mock<ISymbol> _mockSymbol;
        private readonly Mock<IMemberMetadata> _mockMemberMetadata;

        public ParameterTypeMetadataTests()
        {
            _fixture = new Fixture()
                .Customize(new AutoMoqCustomization { ConfigureMembers = true })
                .Customize(new SupportMutableValueTypesCustomization());
            _mockParameterSymbol = _fixture.Create<Mock<IParameterSymbol>>();
            _mockLocation = _fixture.Create<Mock<Location>>();
            _mockSymbol = _fixture.Create<Mock<ISymbol>>();
            _mockMemberMetadata = _fixture.Create<Mock<IMemberMetadata>>();
        }

        [Fact]
        public void Instance_ValidNameField()
        {
            // Arrange
            var fakeName = "FakeName";
            _mockParameterSymbol.Setup(p => p.Name).Returns(fakeName);

            // Act
            var parameterTypeMetadata = new ParameterTypeMetadata(_mockParameterSymbol.Object);

            // Assert
            parameterTypeMetadata.Name.Should().Be(fakeName);
        }

        [Fact]
        public void Instance_ValidFullNameField()
        {
            // Arrange
            var fakeFullName = "FakeFullName";
            _mockParameterSymbol.Setup(t => t.ToDisplayString(It.IsAny<SymbolDisplayFormat>())).Returns(fakeFullName);

            // Act
            var parameterTypeMetadata = new ParameterTypeMetadata(_mockParameterSymbol.Object);

            // Assert
            parameterTypeMetadata.FullName.Should().Be(fakeFullName);
        }

        [Fact]
        public void Instance_ValidLocationField()
        {
            // Arrange
            _mockParameterSymbol.Setup(p => p.Locations).Returns(ImmutableArray.Create(_mockLocation.Object));

            // Act
            var parameterTypeMetadata = new ParameterTypeMetadata(_mockParameterSymbol.Object);

            // Assert
            parameterTypeMetadata.Location.Should().NotBeNull();
        }

        [Fact]
        public void Instance_EmptyProperties()
        {
            // Arrange
            _mockSymbol.Setup(s => s.IsStatic).Returns(true);
            _mockParameterSymbol.Setup(t => t.Type.GetMembers()).Returns(ImmutableArray.Create(_mockSymbol.Object));

            // Act
            var parameterTypeMetadata = new ParameterTypeMetadata(_mockParameterSymbol.Object);

            // Assert
            parameterTypeMetadata.Properties.Should().BeEmpty();
        }

        [Fact]
        public void Instance_EmptyFields()
        {
            // Arrange
            _mockSymbol.Setup(s => s.CanBeReferencedByName).Returns(false);
            _mockParameterSymbol.Setup(t => t.Type.GetMembers()).Returns(ImmutableArray.Create(_mockSymbol.Object));

            // Act
            var parameterTypeMetadata = new ParameterTypeMetadata(_mockParameterSymbol.Object);

            // Assert
            parameterTypeMetadata.Fields.Should().BeEmpty();
        }

        [Fact]
        public void FlattenProperties_ValidFlatten()
        {
            // Arrange
            var limitedPropertyMetadata = _fixture.Create<Mock<IMemberMetadata>>();
            limitedPropertyMetadata.Setup(l => l.Properties).Returns(new List<IMemberMetadata>());

            _mockMemberMetadata.Setup(p => p.Properties).Returns(
                new List<IMemberMetadata>
                {
                    limitedPropertyMetadata.Object,
                    limitedPropertyMetadata.Object,
                    limitedPropertyMetadata.Object
                });

            var objectsCount = 4; //  Primary object plus all objects from properties list

            // Act
            var parameterTypeMetadata = new ParameterTypeMetadata(_mockParameterSymbol.Object);
            var result = parameterTypeMetadata.FlattenProperties(new List<IMemberMetadata> { _mockMemberMetadata.Object });

            // Assert
            result.Should().NotBeEmpty();
            result.Should().HaveCount(objectsCount);
        }
    }
}