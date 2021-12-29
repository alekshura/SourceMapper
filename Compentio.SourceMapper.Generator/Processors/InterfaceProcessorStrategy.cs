using Compentio.SourceMapper.Matchers;
using Compentio.SourceMapper.Metadata;
using System.Linq;
using System.Text;

namespace Compentio.SourceMapper.Processors
{
    /// <summary>
    /// Class that generates mapper code, that was defined in interface
    /// </summary>
    internal class InterfaceProcessorStrategy : AbstractProcessorStrategy
    {
        protected override string Modifier => "virtual";

        protected override ProcessorType ProcessorType => ProcessorType.Interface;

        protected override string GenerateMapperCode(IMapperMetadata mapperMetadata)
        {
            var result = @$"// <mapper-source-generated />
                            // <generated-at '{System.DateTime.UtcNow}' />

            using System.Diagnostics.CodeAnalysis;

            {(string.IsNullOrWhiteSpace(mapperMetadata?.Namespace) ? null : $"namespace {mapperMetadata?.Namespace}")}
            {{
               [ExcludeFromCodeCoverage]
               public class {mapperMetadata?.TargetClassName} : {mapperMetadata?.Name}
               {{
                  public static {mapperMetadata?.TargetClassName} Create() => new();

                  { GenerateMethods(mapperMetadata) }
               }}

               { GeneratePartialInterface(mapperMetadata) }
            }}
            ";

            return result;
        }

        private string GeneratePartialInterface(IMapperMetadata mapperMetadata)
        {
            if (AttributesMatchers.AnyInverseMethod(mapperMetadata.MethodsMetadata))
            {
                return $@"
                public partial interface {mapperMetadata?.Name}
                {{
                    { GenerateInterfaceMethods(mapperMetadata) }
                }}
                ";
            }

            return string.Empty;
        }

        private string GenerateInterfaceMethods(IMapperMetadata sourceMetadata)
        {
            var methodsStringBuilder = new StringBuilder();

            foreach (var methodMetadata in sourceMetadata.MethodsMetadata.Where(m => AttributesMatchers.IsInverseMethod(m)))
            {
                methodsStringBuilder.AppendLine(GenerateInterfaceMethod(methodMetadata));
            }

            return methodsStringBuilder.ToString();
        }

        private string GenerateInterfaceMethod(IMethodMetadata methodMetadata)
        {
            return $"{methodMetadata.InverseMethodFullName};";
        }
    }
}