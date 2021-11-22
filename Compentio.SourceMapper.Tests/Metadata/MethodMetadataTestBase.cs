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

        protected ImmutableArray<AttributeData> GetFakeAttributeData(string sourceCode)
        {
            var compilation = GetFakeCompilation(sourceCode);

            INamedTypeSymbol fakeClass = compilation.GetTypeByMetadataName($"{FakeNamespace}.{FakeClassName}");
            IMethodSymbol fakeMethod = fakeClass.GetMembers(FakeMethodName).First() as IMethodSymbol;

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

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class MappingAttribute : Attribute
    {{
        public virtual string Source {{ get; set; }} = string.Empty;
        public virtual string Target {{ get; set; }} = string.Empty;
        public virtual string Expression {{ get; set; }} = string.Empty;
        public virtual bool CreateInverse {{ get; set; }} = false;
        public virtual string InverseMethodName {{ get; set; }} = string.Empty;
        public virtual string InverseExpression {{ get; set; }} = string.Empty;
        public virtual string InverseTarget {{ get; set; }} = string.Empty;
        public virtual string InverseSource {{ get; set; }} = string.Empty;
    }}
}}
";
    }
}