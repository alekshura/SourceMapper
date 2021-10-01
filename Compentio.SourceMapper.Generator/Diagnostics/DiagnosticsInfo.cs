using Compentio.SourceMapper.Metadata;
using Microsoft.CodeAnalysis;
using System;

namespace Compentio.SourceMapper.Diagnostics
{
    /// <summary>
    /// <see cref="DiagnosticInfo"/> stores information about problems or warnings during code generation.
    /// It is used to report it at the end of the process.
    /// </summary>
    internal class DiagnosticsInfo
    {
        /// <summary>
        /// Descriptior of particular diagnostics proble
        /// </summary>
        internal DiagnosticDescriptor? DiagnosticDescriptor { get; set; }
        /// <summary>
        /// Information about object where problem occured
        /// </summary>
        internal IMetadata? Metadata { get; set; }
        /// <summary>
        /// Exception thrown during code generation
        /// </summary>
        internal Exception? Exception { get; set; }
        /// <summary>
        /// Formatted message that is shoun in build console
        /// </summary>
        internal string Message => Exception is not null ? 
            $"Message: {Exception.Message}, StackTrace: {Exception.StackTrace}" : Metadata.Name;
    }
}
