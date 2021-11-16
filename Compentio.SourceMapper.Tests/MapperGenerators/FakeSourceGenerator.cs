using Compentio.SourceMapper.Generators;
using Microsoft.CodeAnalysis;

namespace Compentio.SourceMapper.Tests.MapperGenerators
{
    [Generator]
    public class FakeSourceGenerator : ISourceGenerator
    {
        private GeneratorExecutionContext _generatorExecutionContext;

        public void Execute(GeneratorExecutionContext context)
        {
            _generatorExecutionContext = context;
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new MappersSyntaxReceiver());
        }

        public GeneratorExecutionContext GeneratorExecutionContext => _generatorExecutionContext;
    }
}