using Compentio.SourceMapper.Extensions;
using FluentAssertions;
using Xunit;

namespace Compentio.SourceMapper.Tests.Extensions
{
    public class DependencyInjectionExceptionTests
    {
        private const string ExceptionMessage = "Message";
        private const string ExceptionStackTrace = "StackTrace";

        [Fact]
        public void SetCtorMessage_HasMessageFieldSet()
        {
            // Act
            var dependencyInjectionException = new DependencyInjectionException(ExceptionMessage, string.Empty);
            var exceptionMessage = dependencyInjectionException.Message;

            // Assert
            exceptionMessage.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void SetCtorStackTrace_HasStackTraceFieldSet()
        {
            // Act
            var dependencyInjectionException = new DependencyInjectionException(string.Empty, ExceptionStackTrace);
            var exceptionStackTrace = dependencyInjectionException.StackTrace;

            //Assert
            exceptionStackTrace.Should().NotBeNullOrEmpty();
        }
    }
}
