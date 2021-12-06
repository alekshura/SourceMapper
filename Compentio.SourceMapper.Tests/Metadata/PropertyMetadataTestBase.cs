using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Compentio.SourceMapper.Tests.Metadata
{
    public abstract class PropertyMetadataTestBase
    {
        protected abstract string FakeNamespace { get; }
        protected abstract string FakeClassName { get; }
        protected abstract string FakeMethodName { get; }

        protected ImmutableArray<AttributeData> GetFakeAttributeData(string sourceCode, string methodName)
        {
            var compilation = GetFakeCompilation(sourceCode);

            INamedTypeSymbol fakeClass = compilation.GetTypeByMetadataName($"{FakeNamespace}.{FakeClassName}");
            IMethodSymbol fakeMethod = fakeClass.GetMembers(methodName).First() as IMethodSymbol;
            IParameterSymbol fakeParameter = fakeMethod.Parameters.First();
            IPropertySymbol fakeProperty = fakeParameter.Type.GetMembers()
                .Where(member => member as IPropertySymbol is not null).First() as IPropertySymbol;

            return fakeProperty.GetAttributes();
        }

        protected Compilation GetFakeCompilation(string sourceCode)
        {
            return CSharpCompilation.Create("PropertyMetadataTestBase",
                new[] { CSharpSyntaxTree.ParseText(sourceCode) },
                new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        }

        protected string FakeSourceCode => @$"

namespace {FakeNamespace}
{{
    using System;

    public abstract class {FakeClassName}
    {{
        public abstract FakeTypeDto {FakeMethodName}(FakeTypeDao fake);
    }}

    public class FakeTypeDao
    {{
        [IgnoreMapping]
        public virtual string PropertyToIgnore {{ get; set; }}
    }}

    {IgnoreMappingAttributeSourceCode}
}}
";

        private static string IgnoreMappingAttributeSourceCode => $@"

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = false)]
    public class IgnoreMappingAttribute : Attribute
    {{
    }}
";
    }
}