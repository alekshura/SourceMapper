using Compentio.SourceMapper.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;

namespace Compentio.SourceMapper.Processors
{
    interface IProcessingResult
    {
        string GeneratedCode { get; }

        IEnumerable<DiagnosticsInfo>? Diagnostics { get; }
    }
    /// <summary>
    /// 
    /// </summary>
    internal class ProcessingResult : IProcessingResult
    {
        private readonly string _code = string.Empty;
        private readonly IEnumerable<DiagnosticsInfo>? _diagnostics;

        private ProcessingResult(string code, IEnumerable<DiagnosticsInfo>? diagnostics)
        {
            _code = code;
            _diagnostics = diagnostics;
        }

        internal static IProcessingResult Return(string code, IEnumerable<DiagnosticsInfo> diagnostics)
        {
            return new ProcessingResult(code, diagnostics);
        }

        public string GeneratedCode => FormatCode(_code);

        public IEnumerable<DiagnosticsInfo>? Diagnostics => _diagnostics;

        private string FormatCode(string code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = tree.GetRoot().NormalizeWhitespace();
            return root.ToFullString();
        }
    }
}
