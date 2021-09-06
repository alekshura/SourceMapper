using Compentio.SourceMapper.Attributes;
using Compentio.SourceMapper.Generators.Strategies;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Linq;
using System.Text;

namespace Compentio.SourceMapper.Generators
{
    internal class MappersSyntaxGenerator
    {
        private readonly CodeGeneratorStrategyFactory _strategyFactory;

        internal MappersSyntaxGenerator()
        {
            _strategyFactory = new();
        }

        internal void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not MappersSyntaxReceiver receiver)
            {
                return;
            }            

            foreach (var typeDeclaration in receiver.Candidates)
            {
                var model = context.Compilation.GetSemanticModel(typeDeclaration.SyntaxTree, true);
                var mapperType = model.GetDeclaredSymbol(typeDeclaration) as ITypeSymbol;

                if (mapperType is null || !IsMapperType(mapperType))
                    continue;

                var codeGenerationStrategy = _strategyFactory.GetStrategy(mapperType);
                var generatedCode = codeGenerationStrategy.GenerateCode(mapperType);

                context.AddSource(codeGenerationStrategy.FileName, SourceText.From(generatedCode, Encoding.UTF8));
            }
        }


        private bool IsMapperType(ITypeSymbol type)
        {
            return type.GetAttributes()
                       .Any(a => a.AttributeClass?.Name == nameof(MapperAttribute));
        }             
    }
}
