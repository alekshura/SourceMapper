using Microsoft.CodeAnalysis;

namespace Compentio.SourceMapper.Generators.Strategies
{
    internal abstract class AbstractGeneratorStrategy
    {
        internal string FileName => $"{ClassName}.cs";
        protected abstract string ClassName { get; }
        internal abstract string GenerateCode(ITypeSymbol typeSymbol);           
    }
}
