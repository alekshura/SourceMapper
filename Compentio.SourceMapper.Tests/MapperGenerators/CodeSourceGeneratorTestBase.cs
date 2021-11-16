using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace Compentio.SourceMapper.Tests.MapperGenerators
{
    public class CodeSourceGeneratorTestBase
    {
        protected readonly IFixture _fixture;
        protected readonly GeneratorExecutionContext _generatorExecutionContext;

        protected CodeSourceGeneratorTestBase()
        {
            _fixture = new Fixture()
                .Customize(new AutoMoqCustomization { ConfigureMembers = true })
                .Customize(new SupportMutableValueTypesCustomization());

            _generatorExecutionContext = GetFakeContext(TestCode);
        }

        protected string TestCode =>
        @"namespace Compentio.SourceMapper.Tests
                {
                    public class Program
                    {
                        public static void Main(string[] args)
                        {
                        }
                    }
                }
                ";

        protected GeneratorExecutionContext GetFakeContext(string sourceCode)
        {
            var compilation = CSharpCompilation.Create("CodeSourceGeneratorTests",
                                                       new[] { CSharpSyntaxTree.ParseText(sourceCode) },
                                                       new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                                                       new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            var generator = new FakeSourceGenerator();
            CSharpGeneratorDriver.Create(generator).RunGeneratorsAndUpdateCompilation(compilation, out _, out _);

            return generator.GeneratorExecutionContext;
        }
    }
}