using Compentio.SourceMapper.Attributes;
using Compentio.SourceMapper.Metadata;
using Compentio.SourceMapper.Processors;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Linq;
using System.Text;

namespace Compentio.SourceMapper.Generators
{
    /// <summary>
    /// Class that setups code generation metadata and code processors.
    /// Mappers are generated for interfaces and abstract classes.
    /// </summary>
    internal class MappersSyntaxGenerator
    {
        internal void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not MappersSyntaxReceiver receiver)
                return;         

            foreach (var typeDeclaration in receiver.Candidates)
            {
                var model = context.Compilation.GetSemanticModel(typeDeclaration.SyntaxTree, true);
                var mapperType = model.GetDeclaredSymbol(typeDeclaration) as ITypeSymbol;

                if (mapperType is null || !IsMapperType(mapperType))
                    continue;

                var sourceMetadata = new SourceMetadata(mapperType);
                var processorStrategy = ProcessorStrategyFactory.GetStrategy(sourceMetadata);
                context.AddSource(sourceMetadata.FileName, SourceText.From(processorStrategy.GenerateCode(sourceMetadata), Encoding.UTF8));
            }
        }


        private bool IsMapperType(ITypeSymbol type)
        {
            return type.GetAttributes()
                       .Any(a => a.AttributeClass?.Name == nameof(MapperAttribute)) && type.IsAbstract;
        }             
    }
}
