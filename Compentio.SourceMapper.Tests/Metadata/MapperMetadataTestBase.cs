using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Immutable;
using System.Reflection;

namespace Compentio.SourceMapper.Tests.Metadata
{
    public abstract class MapperMetadataTestBase
    {
        protected abstract string FakeNamespace { get; }
        protected abstract string FakeClassName { get; }
        protected abstract string FakeInterfaceName { get; }

        protected ImmutableArray<AttributeData> GetFakeClassAttributeData(string sourceCode)
        {
            var compilation = GetFakeCompilation(sourceCode);

            INamedTypeSymbol fakeClass = compilation.GetTypeByMetadataName($"{FakeNamespace}.{FakeClassName}");

            return fakeClass.GetAttributes();
        }

        protected ImmutableArray<AttributeData> GetFakeInterfaceAttributeData(string sourceCode)
        {
            var compilation = GetFakeCompilation(sourceCode);

            INamedTypeSymbol fakeClass = compilation.GetTypeByMetadataName($"{FakeNamespace}.{FakeInterfaceName}");

            return fakeClass.GetAttributes();
        }

        protected Compilation GetFakeCompilation(string sourceCode)
        {
            return CSharpCompilation.Create("MapperMetadataTestBase",
                new[] { CSharpSyntaxTree.ParseText(sourceCode) },
                new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        }

        protected string FakeClassSourceCode => @$"

namespace {FakeNamespace}
{{
    using System;

    [Mapper(ClassName = ""{FakeClassName}"")]
    public abstract partial class {FakeClassName}
    {{
    }}

    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Struct)]
    public class MapperAttribute : Attribute
    {{
        public string ClassName {{ get; set; }} = string.Empty;
        public string UseMapper {{ get; set; }} = string.Empty;
    }}
}}
";

        protected string FakeInterfaceSourceCode => @$"

namespace {FakeNamespace}
{{
    using System;

    [Mapper(ClassName = ""{FakeInterfaceName}"")]
    public partial interface {FakeInterfaceName}
    {{
    }}

    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Struct)]
    public class MapperAttribute : Attribute
    {{
        public string ClassName {{ get; set; }} = string.Empty;
        public string UseMapper {{ get; set; }} = string.Empty;
    }}
}}
";

        protected string FakeSourceCode => @$"

namespace {FakeNamespace}
{{
    using System;

    [Mapper]
    public partial class {FakeClassName}
    {{
    }}

    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Struct)]
    public class MapperAttribute : Attribute
    {{
        public string ClassName {{ get; set; }} = string.Empty;
        public string UseMapper {{ get; set; }} = string.Empty;
    }}
}}
";
    }
}