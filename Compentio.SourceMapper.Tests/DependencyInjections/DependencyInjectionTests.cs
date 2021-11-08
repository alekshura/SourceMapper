using Compentio.SourceMapper.Processors.DependencyInjection;
using FluentAssertions;
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
            var dependencyInjection = new DependencyInjection();

            // Assert
            dependencyInjection.DependencyInjectionClassName.Should().NotBeNullOrEmpty();
            dependencyInjection.DependencyInjectionClassName.Should().BeEquivalentTo(GetDependencyInjectionClassNameString());
        }

        private static string GetDependencyInjectionClassNameString()
        {
            var resourceManager = new ResourceManager(typeof(Compentio.SourceMapper.Resources.AppStrings));
            string message = resourceManager.GetString("DependencyInjectionClassName", CultureInfo.InvariantCulture);

            return message;
        }
    }
}