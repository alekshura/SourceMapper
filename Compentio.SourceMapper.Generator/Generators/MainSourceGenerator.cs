using Compentio.SourceMapper.Attributes;
using Compentio.SourceMapper.Metadata;
using Compentio.SourceMapper.Processors.DependencyInjection;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
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
        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is null)
            {
                throw new ArgumentNullException(nameof(context), $"{nameof(context.SyntaxReceiver)} could not be null");
            }

            if (context.SyntaxReceiver is not MappersSyntaxReceiver receiver)
                return;

            var sourcesMetadata = SourcesMetadata.Create(GetDependencyInjectionType(context.Compilation.ReferencedAssemblyNames));

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

        private DependencyInjectionType GetDependencyInjectionType(IEnumerable<AssemblyIdentity> assemblies)
        {
            if (assemblies.Any(ai => ai.Name.Equals("Microsoft.Extensions.DependencyInjection", StringComparison.OrdinalIgnoreCase)))
            {
                return DependencyInjectionType.DotNetCore;
            }

            if (assemblies.Any(ai => ai.Name.Equals("Autofac.Extensions.DependencyInjection", StringComparison.OrdinalIgnoreCase)))
            {
                return DependencyInjectionType.Autofac;
            }

            if (assemblies.Any(ai => ai.Name.Equals("StructureMap.Microsoft.DependencyInjection", StringComparison.OrdinalIgnoreCase)))
            {
                return DependencyInjectionType.Autofac;
            }
            
            return DependencyInjectionType.None;            
        }
    }
}
