using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Immutable;
using System.Reflection;

namespace Compentio.SourceMapper.Tests.Metadata
{
    public abstract class MapperMetadataTestBase
    {
        protected abstract string MockNamespace { get; }
        protected abstract string MockClassName { get; }
        protected abstract string MockInterfaceName { get; }

        protected ImmutableArray<ISymbol> GetClassMethodsMock(string sourceCode)
        {
            var compilation = GetCompilationMock(sourceCode);

            var mockClass = compilation.GetTypeByMetadataName($"{MockNamespace}.{MockClassName}");

            return mockClass.GetMembers();
        }

        protected ImmutableArray<AttributeData> GetClassAttributeDataMock(string sourceCode)
        {
            var compilation = GetCompilationMock(sourceCode);

            var mockClass = compilation.GetTypeByMetadataName($"{MockNamespace}.{MockClassName}");

            return mockClass.GetAttributes();
        }

        protected ImmutableArray<AttributeData> GetInterfaceAttributeDataMock(string sourceCode)
        {
            var compilation = GetCompilationMock(sourceCode);

            var mockInterface = compilation.GetTypeByMetadataName($"{MockNamespace}.{MockInterfaceName}");

            return mockInterface.GetAttributes();
        }

        protected Compilation GetCompilationMock(string sourceCode)
        {
            return CSharpCompilation.Create("MapperMetadataTestBase",
                new[] { CSharpSyntaxTree.ParseText(sourceCode) },
                new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        }

        protected string MockClassSourceCode => @$"
namespace {MockNamespace}
{{
    using System;
    [Mapper(ClassName = ""{MockClassName}"")]
    public abstract partial class {MockClassName}
    {{
        public abstract MockTypeDto MockMethodName(MockTypeDao mock);
    }}
    {MapperAttributeClassCode}
}}
";

        protected string MockInterfaceSourceCode => @$"
namespace {MockNamespace}
{{
    using System;
    [Mapper(ClassName = ""{MockInterfaceName}"")]
    public partial interface {MockInterfaceName}
    {{
    }}
    {MapperAttributeClassCode}
}}
";

        protected string MockSourceCode => @$"
namespace {MockNamespace}
{{
    using System;
    [Mapper]
    public partial class {MockClassName}
    {{
    }}
    {MapperAttributeClassCode}
}}
";

        private static string MapperAttributeClassCode => $@"
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Struct)]
    public class MapperAttribute : Attribute
    {{
        public string ClassName {{ get; set; }} = string.Empty;
    }}
";
    }
}