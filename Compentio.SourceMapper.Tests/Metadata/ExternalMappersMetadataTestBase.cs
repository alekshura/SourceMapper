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

        protected abstract string FakeAssemblyName { get; }
        protected abstract string FakeNamespace { get; }
        protected abstract string FakeClassName { get; }
        protected abstract string FakeInterfaceName { get; }

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
            _mockAssembly.Setup(a => a.Identity).Returns(GetFakeAssemblyIdentity(FakeAssemblyName));
            _mockAssembly.Setup(a => a.GlobalNamespace).Returns(_mockGlobalNamespace.Object);
            _mockGlobalNamespace.Setup(n => n.GetNamespaceMembers()).Returns(new List<INamespaceSymbol> { _mockNamespace.Object });
            _mockNamespace.Setup(n => n.GetTypeMembers()).Returns(ImmutableArray.Create(_mockNamedType.Object));
        }

        protected static AssemblyIdentity GetFakeAssemblyIdentity(string name)
        {
            return new AssemblyIdentity(name: name);
        }

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
            return CSharpCompilation.Create("ExternalMappersMetadataTestBase",
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
        public abstract FakeTypeDto FakeMethodName(FakeTypeDao fake);
    }}
    {MapperAttributeClassCode}
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
    {MapperAttributeClassCode}
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