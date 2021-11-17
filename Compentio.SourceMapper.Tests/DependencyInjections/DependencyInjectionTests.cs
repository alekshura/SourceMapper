using Compentio.SourceMapper.Processors.DependencyInjection;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using Xunit;

namespace Compentio.SourceMapper.Tests.DependencyInjections
{
    public class DependencyInjectionTests
    {
        [Fact]
        public void DependencyInjectionClassName_ReturnProperString()
        {
            // Act
            var dependencyInjection = new SourceMapper.Processors.DependencyInjection.DependencyInjection();

            // Assert
            dependencyInjection.DependencyInjectionClassName.Should().NotBeNullOrEmpty();
            dependencyInjection.DependencyInjectionClassName.Should().BeEquivalentTo(GetDependencyInjectionClassNameString());
        }

        [Fact]
        public void GetType_DotNetCoreAssembly_ReturnDotNetCoreType()
        {
            // Arrange
            var fakeAssembly = new FakeAssembly("Microsoft.Extensions.DependencyInjection");
            var assemblyCollection = new List<AssemblyIdentity> { AssemblyIdentity.FromAssemblyDefinition(fakeAssembly) };

            // Act
            var dependencyInjection = new SourceMapper.Processors.DependencyInjection.DependencyInjection(assemblyCollection);
            var result = dependencyInjection.DependencyInjectionType;

            // Assert
            result.Should().Be(DependencyInjectionType.DotNetCore);
        }

        [Fact]
        public void GetType_AutofacAssembly_ReturnAutofacType()
        {
            // Arrange
            var fakeAssembly = new FakeAssembly("Autofac.Extensions.DependencyInjection");
            var assemblyCollection = new List<AssemblyIdentity> { AssemblyIdentity.FromAssemblyDefinition(fakeAssembly) };

            // Act
            var dependencyInjection = new SourceMapper.Processors.DependencyInjection.DependencyInjection(assemblyCollection);
            var result = dependencyInjection.DependencyInjectionType;

            // Assert
            result.Should().Be(DependencyInjectionType.Autofac);
        }

        [Fact]
        public void GetType_StructureMapAssembly_ReturnStructureMapType()
        {
            // Arrange
            var fakeAssembly = new FakeAssembly("StructureMap.Microsoft.DependencyInjection");
            var assemblyCollection = new List<AssemblyIdentity> { AssemblyIdentity.FromAssemblyDefinition(fakeAssembly) };

            // Act
            var dependencyInjection = new SourceMapper.Processors.DependencyInjection.DependencyInjection(assemblyCollection);
            var result = dependencyInjection.DependencyInjectionType;

            // Assert
            result.Should().Be(DependencyInjectionType.StructureMap);
        }

        [Fact]
        public void GetType_NoDependencyInjection_ReturnNoneType()
        {
            // Arrange
            var fakeAssembly = new FakeAssembly("NoDependencyInjection");
            var assemblyCollection = new List<AssemblyIdentity> { AssemblyIdentity.FromAssemblyDefinition(fakeAssembly) };

            // Act
            var dependencyInjection = new SourceMapper.Processors.DependencyInjection.DependencyInjection(assemblyCollection);
            var result = dependencyInjection.DependencyInjectionType;

            // Assert
            result.Should().Be(DependencyInjectionType.None);
        }

        private static string GetDependencyInjectionClassNameString()
        {
            var resourceManager = new ResourceManager(typeof(Resources.AppStrings));
            string message = resourceManager.GetString("DependencyInjectionClassName", CultureInfo.InvariantCulture);

            return message;
        }
    }
}