using AutoFixture;
using Compentio.SourceMapper.Diagnostics;
using Compentio.SourceMapper.Processors;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Compentio.SourceMapper.Tests.Processors
{
    public class ResultTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<DiagnosticsInfo> _mockDiagnosticsInfo;
        private readonly IList<DiagnosticsInfo> _diagnosticsInfoList;
        private readonly Mock<Exception> _mockException;
        private const string GeneratedCodeTest = "TestCode";

        public ResultTests()
        {
            _fixture = new Fixture();
            _mockDiagnosticsInfo = _fixture.Create<Mock<DiagnosticsInfo>>();
            _mockException = _fixture.Create<Mock<Exception>>();

            _diagnosticsInfoList = new List<DiagnosticsInfo> { _mockDiagnosticsInfo.Object };
        }

        [Fact]
        public void ResultOk_NoParms_ReturnSuccess()
        {
            // Act
            var result = Result.Ok(null);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Diagnostics.Should().BeNullOrEmpty();
        }

        [Fact]
        public void ResultOk_NoExceptions_ReturnSuccess()
        {
            // Arrange
            _mockDiagnosticsInfo.Setup(d => d.Exception).Returns((Exception)null);

            // Act
            var result = Result.Ok(string.Empty, _diagnosticsInfoList);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Diagnostics.Any(d => d.Exception is not null).Should().BeFalse();
        }

        [Fact]
        public void ResultOk_WithException_ReturnSuccessFalse()
        {
            // Arrange
            _mockDiagnosticsInfo.Setup(d => d.Exception).Returns(new Exception());

            // Act
            var result = Result.Ok(string.Empty, _diagnosticsInfoList);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Diagnostics.Any(d => d.Exception is not null).Should().BeTrue();
        }

        [Fact]
        public void ResultError_ReportException_ReturnSuccessFalse()
        {
            // Arrange
            _mockDiagnosticsInfo.Setup(d => d.Exception).Returns(new Exception());

            // Act
            var result = Result.Error(_mockException.Object);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Diagnostics.Any(d => d.Exception is not null).Should().BeTrue();
        }

        [Fact]
        public void GeneratedCore_FromResultOk_ReturnCode()
        {
            //Act
            var result = Result.Ok(GeneratedCodeTest);
            var code = result.GeneratedCode;

            // Assert
            code.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GeneratedCode_FromResultError_EmptyCode()
        {
            // Arrange
            _mockDiagnosticsInfo.Setup(d => d.Exception).Returns(new Exception());

            // Act
            var result = Result.Error(_mockException.Object);
            var code = result.GeneratedCode;

            // Assert
            code.Should().BeNullOrEmpty();
        }
    }
}