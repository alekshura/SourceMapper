using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Compentio.SourceMapper.Tests.Metadata
{
    public abstract class MemberMetadataTestBase
    {
        protected abstract string MockNamespace { get; }
        protected abstract string MockClassName { get; }
        protected abstract string MockMethodName { get; }

        protected ImmutableArray<AttributeData> GetAttributeDataMock(string sourceCode, string methodName)
        {
            var compilation = GetCompilationMock(sourceCode);

            var fakeClass = compilation.GetTypeByMetadataName($"{MockNamespace}.{MockClassName}");
            var fakeMethod = fakeClass.GetMembers(methodName).First() as IMethodSymbol;
            var fakeParameter = fakeMethod.Parameters.First();
            var fakeProperty = fakeParameter.Type.GetMembers()
                .Where(member => member as IPropertySymbol is not null).First() as IPropertySymbol;

            return fakeProperty.GetAttributes();
        }

        protected Compilation GetCompilationMock(string sourceCode)
        {
            return CSharpCompilation.Create("MemberMetadataTestBase",
                new[] { CSharpSyntaxTree.ParseText(sourceCode) },
                new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        }

        protected string MockSourceCode => @$"

namespace {MockNamespace}
{{
    using System;

    public abstract class {MockClassName}
    {{
        public abstract MockTypeDto {MockMethodName}(MockTypeDao mock);
    }}

    public class MockTypeDao
    {{
        [IgnoreMapping]
        public virtual string PropertyToIgnore {{ get; set; }}
        public static string StaticFieldProperty;
        public string FieldProperty;
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