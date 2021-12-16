using AutoFixture;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Moq;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace Compentio.SourceMapper.Tests.Metadata
{
    public abstract class ExternalMappersMetadataTestBase
    {
        protected readonly IFixture _fixture;
        protected readonly Mock<IAssemblySymbol> _mockAssembly;
        protected readonly Mock<INamespaceSymbol> _mockGlobalNamespace;
        protected readonly Mock<INamespaceSymbol> _mockNamespace;
        protected readonly Mock<INamedTypeSymbol> _mockNamedType;

        protected abstract string MockAssemblyName { get; }
        protected abstract string MockNamespace { get; }
        protected abstract string MockClassName { get; }
        protected abstract string MockInterfaceName { get; }

        protected ExternalMappersMetadataTestBase()
        {
            _fixture = new Fixture();
            _mockAssembly = _fixture.Create<Mock<IAssemblySymbol>>();
            _mockGlobalNamespace = _fixture.Build<Mock<INamespaceSymbol>>()
                .WithAutoProperties()
                .Without(n => n.DefaultValue)
                .Create();
            _mockNamespace = _fixture.Build<Mock<INamespaceSymbol>>()
                .WithAutoProperties()
                .Without(n => n.DefaultValue)
                .Create();
            _mockNamedType = _fixture.Build<Mock<INamedTypeSymbol>>()
                .WithAutoProperties()
                .Without(n => n.DefaultValue)
                .Create();

            // Arrange
            _mockAssembly.Setup(a => a.Identity).Returns(GetAssemblyIdentityMock(MockAssemblyName));
            _mockAssembly.Setup(a => a.GlobalNamespace).Returns(_mockGlobalNamespace.Object);
            _mockGlobalNamespace.Setup(n => n.GetNamespaceMembers()).Returns(new List<INamespaceSymbol> { _mockNamespace.Object });
            _mockNamespace.Setup(n => n.GetTypeMembers()).Returns(ImmutableArray.Create(_mockNamedType.Object));
        }

        protected static AssemblyIdentity GetAssemblyIdentityMock(string name)
        {
            return new AssemblyIdentity(name: name);
        }

        protected ImmutableArray<AttributeData> GetClassAttributeDataMock(string sourceCode)
        {
            var mockCompilation = GetCompilationMock(sourceCode);

            var mockClass = mockCompilation.GetTypeByMetadataName($"{MockNamespace}.{MockClassName}");

            return mockClass.GetAttributes();
        }

        protected ImmutableArray<AttributeData> GetInterfaceAttributeDataMock(string sourceCode)
        {
            var mockCompilation = GetCompilationMock(sourceCode);

            var mockInterface = mockCompilation.GetTypeByMetadataName($"{MockNamespace}.{MockInterfaceName}");

            return mockInterface.GetAttributes();
        }

        protected Compilation GetCompilationMock(string sourceCode)
        {
            return CSharpCompilation.Create("ExternalMappersMetadataTestBase",
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
        public abstract FakeTypeDto FakeMethodName(FakeTypeDao fake);
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