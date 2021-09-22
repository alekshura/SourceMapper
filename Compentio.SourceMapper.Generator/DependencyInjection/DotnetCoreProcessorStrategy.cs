using Compentio.SourceMapper.Metadata;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Compentio.SourceMapper.DependencyInjection
{
    internal class DotnetCoreProcessorStrategy : IDependencyInjectionStrategy
    {
        public string GenerateCode(ISourcesMetadata sourcesMetadata)
        {
            var result = @$"// <mapper-source-generated />
                            // <generated-at '{System.DateTime.UtcNow}' />

            using System;
            using Microsoft.Extensions.DependencyInjection;

            { $"namespace {typeof(DependencyInjection).Namespace}"}
            {{
               public static class {sourcesMetadata.DependencyInjection.DependencyInjectionClassName}
               {{                  
                   public static IServiceCollection AddMappers(this IServiceCollection services)
                   {{
                        { GenerateServices(sourcesMetadata) }
                        return services;
                   }}                                  
               }}
            }}
            ";

            var tree = CSharpSyntaxTree.ParseText(result);
            var root = tree.GetRoot().NormalizeWhitespace();
            return root.ToFullString();
        }

        private string GenerateServices(ISourcesMetadata sourcesMetadata)
        {
            var services = string.Empty;

            foreach (var mapper in sourcesMetadata.Mappers)
            {
                services += $"services.AddSingleton<{mapper.Namespace}.{mapper.MapperName}, {mapper.Namespace}.{mapper.TargetClassName}>();";
            }

            return services;
        }
    }
}
