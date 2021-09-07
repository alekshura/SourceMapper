using Compentio.SourceMapper.Attributes;
using Compentio.SourceMapper.Processors;
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

                var metadata = new InterfaceSourceMetadata(mapperType);
                var sourceProcessor = new InterfaceSourceProcessor(metadata);
                var generatedCode = sourceProcessor.GenerateCode();

                context.AddSource(sourceProcessor.FileName, SourceText.From(generatedCode, Encoding.UTF8));
            }
        }


        private bool IsMapperType(ITypeSymbol type)
        {
            return type.GetAttributes()
                       .Any(a => a.AttributeClass?.Name == nameof(MapperAttribute));
        }             
    }
}
