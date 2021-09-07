using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Compentio.SourceMapper.Processors
{
    interface ISourceMetadata
    {
        string FileName { get; }
        string Namespace { get; }
        string MapperName { get; }
        string TargetClassName { get; }
        IDictionary<string, IMethodSymbol> MethodsMap { get; }
    }
}
