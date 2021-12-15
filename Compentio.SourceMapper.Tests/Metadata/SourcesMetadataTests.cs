using Compentio.SourceMapper.Metadata;
using Compentio.SourceMapper.Processors.DependencyInjection;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using Xunit;

namespace Compentio.SourceMapper.Tests.Metadata
{
    public class SourcesMetadataTests
    {
        private const string AutofacAssemblyName = "Autofac.Extensions.DependencyInjection";
        private const string DotNetCoreAssemblyName = "Microsoft.Extensions.DependencyInjection";
        private const string StructureMapAssemblyName = "StructureMap.Microsoft.DependencyInjection";
        private const string FakeAssemblyName = "FakeAssemblyName";

        [Fact]
        public void DependencyInjection_DotNetCoreDependencyInjection()
        {
            // Arrange
            var assemblies = new List<AssemblyIdentity> { GetFakeAssemblyIdentity(DotNetCoreAssemblyName) };

            // Act
            var result = SourcesMetadata.Create(assemblies);

            // Assert
            result.DependencyInjection.Should().NotBeNull();
            result.DependencyInjection.DependencyInjectionType.Should().Be(DependencyInjectionType.DotNetCore);
        }

        [Fact]
        public void DependencyInjection_AutofacDependencyInjection()
        {
            // Arrange
            var assemblies = new List<AssemblyIdentity> { GetFakeAssemblyIdentity(AutofacAssemblyName) };

            // Act
            var result = SourcesMetadata.Create(assemblies);

            // Assert
            result.DependencyInjection.Should().NotBeNull();
            result.DependencyInjection.DependencyInjectionType.Should().Be(DependencyInjectionType.Autofac);
        }

        [Fact]
        public void DependencyInjection_StructureMapDependencyInjection()
        {
            // Arrange
            var assemblies = new List<AssemblyIdentity> { GetFakeAssemblyIdentity(StructureMapAssemblyName) };

            // Act
            var result = SourcesMetadata.Create(assemblies);

            // Assert
            result.DependencyInjection.Should().NotBeNull();
            result.DependencyInjection.DependencyInjectionType.Should().Be(DependencyInjectionType.StructureMap);
        }

        [Fact]
        public void DependencyInjection_NoDependencyInjection()
        {
            // Arrange
            var assemblies = new List<AssemblyIdentity> { GetFakeAssemblyIdentity(FakeAssemblyName) };

            // Act
            var result = SourcesMetadata.Create(assemblies);

            // Assert
            result.DependencyInjection.Should().NotBeNull();
            result.DependencyInjection.DependencyInjectionType.Should().Be(DependencyInjectionType.None);
        }

        private static AssemblyIdentity GetFakeAssemblyIdentity(string name)
        {
            return new AssemblyIdentity(name: name);
        }
    }
}