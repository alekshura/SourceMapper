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
    public class MainSourceGenerator : ISourceGenerator
    {
        /// <summary>
        /// Main entrypoint for code generation process start. Here all mappers metadata and dependency injection configuration are set up
        /// </summary>
        /// <param name="context"></param>
        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is null)
            {
                throw new ArgumentNullException(nameof(context), $"{nameof(context.SyntaxReceiver)} could not be null");
            }

            if (context.SyntaxReceiver is not MappersSyntaxReceiver receiver)
                return;

            var sourcesMetadata = SourcesMetadata.Create(context.Compilation.ReferencedAssemblyNames);

            foreach (var typeDeclaration in receiver.Candidates)
            {
                var model = context.Compilation.GetSemanticModel(typeDeclaration.SyntaxTree, true);
                var mapperType = model.GetDeclaredSymbol(typeDeclaration) as ITypeSymbol;

                if (mapperType is null || !IsMapperType(mapperType))
                    continue;

                sourcesMetadata.AddOrUpdate(new MapperMetadata(mapperType));
            }

            var sourceGenerator = new CodeSourceGenerator(sourcesMetadata);
            sourceGenerator.GenerateMappings(context);
            sourceGenerator.GenerateDependencyInjectionExtensions(context);
        }

        /// <summary>
        /// <see cref="MainSourceGenerator"/> inirialization entrypoint. Here you can attach <see cref="Debugger"/> for debug code that is generated during build process 
        /// </summary>
        /// <param name="context"></param>
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
