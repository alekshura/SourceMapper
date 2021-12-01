using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Compentio.SourceMapper.Tests.Metadata
{
    public abstract class MethodMetadataTestBase
    {
        protected abstract string FakeNamespace { get; }
        protected abstract string FakeClassName { get; }
        protected abstract string FakeMethodName { get; }
        protected abstract string FakeInverseMethodName { get; }

        protected ImmutableArray<AttributeData> GetFakeAttributeData(string sourceCode, string methodName)
        {
            var compilation = GetFakeCompilation(sourceCode);

            INamedTypeSymbol fakeClass = compilation.GetTypeByMetadataName($"{FakeNamespace}.{FakeClassName}");
            IMethodSymbol fakeMethod = fakeClass.GetMembers(methodName).First() as IMethodSymbol;

            return fakeMethod.GetAttributes();
        }

        protected Compilation GetFakeCompilation(string sourceCode)
        {
            return CSharpCompilation.Create("MethodMetadataTestBase",
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
        [Mapping(Source = nameof(FakeTypeDao.FakeProperty), Target = nameof(FakeTypeDto.PropertyFake))]
        public abstract FakeTypeDto {FakeMethodName}(FakeTypeDao fake);
    }}

    {MappingAttributeSourceCode}
}}
";

        protected string FakeSourceCodeWithInverseMapping => @$"

namespace {FakeNamespace}
{{
    using System;

    public abstract class {FakeClassName}
    {{
        [Mapping(Source = nameof(FakeTypeDao.FakeProperty), Target = nameof(FakeTypeDto.PropertyFake))]
        [InverseMapping(InverseMethodName = ""{FakeInverseMethodName}""]
        public abstract FakeTypeDto {FakeMethodName}(FakeTypeDao fake);
    }}

    {MappingAttributeSourceCode}

    {InverseMappingAttributeSourceCode}
}}
";

        private static string MappingAttributeSourceCode => $@"

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class MappingAttribute : Attribute
    {{
        public virtual string Source {{ get; set; }} = string.Empty;
        public virtual string Target {{ get; set; }} = string.Empty;
        public virtual string Expression {{ get; set; }} = string.Empty;
    }}
";

        private static string InverseMappingAttributeSourceCode => $@"

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class InverseMappingAttribute : Attribute
    {{
        public virtual string InverseMethodName {{ get; set; }} = string.Empty;
    }}
";
    }
}