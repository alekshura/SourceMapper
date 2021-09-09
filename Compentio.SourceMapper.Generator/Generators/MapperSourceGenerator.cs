using Microsoft.CodeAnalysis;
using System;
using System.Diagnostics;

namespace Compentio.SourceMapper.Generators
{
    [Generator]
    public class MapperSourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is null)
            {
                throw new ArgumentNullException(nameof(context), "context.SyntaxReceiver could not be null");
            }

            var sourceGenerator = new MappersSyntaxGenerator();
            sourceGenerator.Execute(context);
        }

        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
#endif


            Debug.WriteLine("Initalize code generator");
            context.RegisterForSyntaxNotifications(() => new MappersSyntaxReceiver());
        }             
    }
}
