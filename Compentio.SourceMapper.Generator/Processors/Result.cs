using Compentio.SourceMapper.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Compentio.SourceMapper.Processors
{
    /// <summary>
    /// Result of mapper code generation
    /// </summary>
    interface IResult
    {
        string GeneratedCode { get; }
        bool IsSuccess { get; }
        IEnumerable<DiagnosticsInfo>? Diagnostics { get; }
    }

    /// <summary>
    /// Result of mapper code generation
    /// </summary>
    internal class Result : IResult
    {
        private readonly string _code = string.Empty;
        private readonly IList<DiagnosticsInfo>? _diagnostics;

        private Result(string code, IList<DiagnosticsInfo>? diagnostics)
        {
            _code = code;
            _diagnostics = diagnostics;
        }

        private Result(Exception exception)
        {
            _diagnostics = new List<DiagnosticsInfo>
            {
                new DiagnosticsInfo
                {
                    DiagnosticDescriptor = SourceMapperDescriptors.UnexpectedError,
                    Exception = exception
                }
            };        
        }

        internal static IResult Ok(string code, IList<DiagnosticsInfo> diagnostics)
        {
            return new Result(code, diagnostics);
        }

        internal static IResult Error(Exception exception)
        {           
            return new Result(exception);
        }

        public string GeneratedCode => FormatCode(_code);

        public IEnumerable<DiagnosticsInfo>? Diagnostics => _diagnostics;

        public bool IsSuccess => !_diagnostics.Any(info => info.Exception is not null);

        private string FormatCode(string code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = tree.GetRoot().NormalizeWhitespace();
            return root.ToFullString();
        }
    }
}
