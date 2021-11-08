using Compentio.SourceMapper.Processors.DependencyInjection;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using Xunit;

namespace Compentio.SourceMapper.Tests.DependencyInjections
{
    public class DependencyInjectionServiceTests
    {
        [Fact]
        public void GetType_DotNetCoreAssembly_ReturnDotNetCoreType()
        {
            // Arrange
            var fakeAssembly = new FakeAssembly("Microsoft.Extensions.DependencyInjection");
            var assemblyCollection = new List<AssemblyIdentity> { AssemblyIdentity.FromAssemblyDefinition(fakeAssembly) };

            // Act
            var result = DependencyInjectionService.GetDependencyInjectionType(assemblyCollection);

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
            var result = DependencyInjectionService.GetDependencyInjectionType(assemblyCollection);

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
            var result = DependencyInjectionService.GetDependencyInjectionType(assemblyCollection);

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
            var result = DependencyInjectionService.GetDependencyInjectionType(assemblyCollection);

            // Assert
            result.Should().Be(DependencyInjectionType.None);
        }
    }
}