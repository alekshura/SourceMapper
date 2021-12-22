using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Compentio.SourceMapper.Tests.Metadata
{
    public abstract class MethodMetadataTestBase
    {
        protected abstract string MockNamespace { get; }
        protected abstract string MockClassName { get; }
        protected abstract string MockMethodName { get; }
        protected abstract string MockInverseMethodName { get; }

        protected ImmutableArray<AttributeData> GetAttributeDataMock(string sourceCode, string methodName)
        {
            var compilation = GetCompilationMock(sourceCode);

            var mockClass = compilation.GetTypeByMetadataName($"{MockNamespace}.{MockClassName}");
            var mockMethod = mockClass.GetMembers(methodName).First() as IMethodSymbol;

            return mockMethod.GetAttributes();
        }

        protected Compilation GetCompilationMock(string sourceCode)
        {
            return CSharpCompilation.Create("MethodMetadataTestBase",
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
        [Mapping(Source = nameof(MockTypeDao.MockProperty), Target = nameof(MockTypeDto.PropertyMock))]
        public abstract MockTypeDto {MockMethodName}(MockTypeDao mock);
    }}

    {MappingAttributeSourceCode}
}}
";

        protected string MockSourceCodeWithInverseMapping => @$"

namespace {MockNamespace}
{{
    using System;

    public abstract class {MockClassName}
    {{
        [Mapping(Source = nameof(MockTypeDao.MockProperty), Target = nameof(MockTypeDto.PropertyMock))]
        [InverseMapping(InverseMethodName = ""{MockInverseMethodName}""]
        public abstract MockTypeDto {MockMethodName}(MockTypeDao mock);
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