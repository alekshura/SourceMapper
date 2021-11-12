using Compentio.SourceMapper.Extensions;
using FluentAssertions;
using Xunit;

namespace Compentio.SourceMapper.Tests.Extensions
{
    public class ExtExceptionTests
    {
        private const string ExceptionMessage = "Message";
        private const string ExceptionStackTrace = "StackTrace";

        [Fact]
        public void SetCtorMessage_HasMessageFieldSet()
        {
            // Act
            var extException = new ExtException(ExceptionMessage, string.Empty);
            var exceptionMessage = extException.Message;

            // Assert
            exceptionMessage.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void SetCtorStackTrace_HasStackTraceFieldSet()
        {
            // Act
            var extException = new ExtException(string.Empty, ExceptionStackTrace);
            var exceptionStackTrace = extException.StackTrace;

            //Assert
            exceptionStackTrace.Should().NotBeNullOrEmpty();
        }
    }
}
