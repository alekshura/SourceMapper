using Compentio.SourceMapper.Attributes;
using Compentio.SourceMapper.Metadata;
using Microsoft.CodeAnalysis;
using System;
using System.Diagnostics;
using System.Linq;

namespace Compentio.SourceMapper.Generators
{
    /// <summary>
    /// Mappers source generator main class. 
    /// </summary>
    [Generator]
    public class MapperSourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is null)
            {
                throw new ArgumentNullException(nameof(context), $"{nameof(context.SyntaxReceiver)} could not be null");
            }

            if (context.SyntaxReceiver is not MappersSyntaxReceiver receiver)
                return;

            var sourcesMetadata = SourcesMetadata.Create();

            foreach (var typeDeclaration in receiver.Candidates)
            {
                var model = context.Compilation.GetSemanticModel(typeDeclaration.SyntaxTree, true);
                var mapperType = model.GetDeclaredSymbol(typeDeclaration) as ITypeSymbol;

                if (mapperType is null || !IsMapperType(mapperType))
                    continue;

                sourcesMetadata.AddOrUpdate(new MapperMetadata(mapperType));
            }

            var sourceGenerator = new MappersSyntaxGenerator(sourcesMetadata);
            sourceGenerator.Execute(context);
        }

        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            //if (!Debugger.IsAttached)
            //{
            //    Debugger.Launch();
            //}
#endif


            Debug.WriteLine("Initalize code generator");
            context.RegisterForSyntaxNotifications(() => new MappersSyntaxReceiver());
        }

        private bool IsMapperType(ITypeSymbol type)
        {
            return type.GetAttributes()
                       .Any(a => a.AttributeClass?.Name == nameof(MapperAttribute)) && type.IsAbstract;
        }
    }
}
