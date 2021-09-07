using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compentio.SourceMapper.Processors
{
    interface ISourceMetadata
    {
        string FileName { get; }
        string InterfaceName { get; }
        string Namespace { get; }
        string TargetClassName { get; }
        IDictionary<string, IMethodSymbol> MethodsMap { get; }
    }
}
