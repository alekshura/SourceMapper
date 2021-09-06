using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compentio.SourceMapper.Generators.Strategies
{
    internal class CodeGeneratorStrategyFactory
    {
        private readonly Dictionary<TypeKind, AbstractGeneratorStrategy> _codeProcessors;

        internal CodeGeneratorStrategyFactory()
        {
            _codeProcessors = new()
            {
                { TypeKind.Interface, new InterfaceGeneratorStrategy() },
                { TypeKind.Class, new ClassGeneratorStrategy() },
            };
        }

        internal AbstractGeneratorStrategy GetStrategy(ITypeSymbol type)
        {

            if (_codeProcessors.TryGetValue(type.TypeKind, out AbstractGeneratorStrategy processor))
                return processor;

            return new InterfaceGeneratorStrategy();
        }
    }
}
