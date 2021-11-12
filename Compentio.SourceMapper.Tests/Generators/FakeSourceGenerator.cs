using Microsoft.CodeAnalysis;

namespace Compentio.SourceMapper.Tests.Generators
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
        }

        public GeneratorExecutionContext GeneratorExecutionContext
        {
            get
            {
                return _generatorExecutionContext;
            }
        }
    }
}