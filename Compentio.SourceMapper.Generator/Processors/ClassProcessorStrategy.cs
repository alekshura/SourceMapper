using Compentio.SourceMapper.Matchers;
using Compentio.SourceMapper.Metadata;
using System.Linq;
using System.Text;

namespace Compentio.SourceMapper.Processors
{
    /// <summary>
    /// Class that generates mapper code, that was defined in abstract class
    /// </summary>
    internal class ClassProcessorStrategy : AbstractProcessorStrategy
    {
        protected override string Modifier => "override";

        protected override string GenerateMapperCode(IMapperMetadata mapperMetadata)
        {
            var result = @$"// <mapper-source-generated />
                            // <generated-at '{System.DateTime.UtcNow}' />

            using System.Diagnostics.CodeAnalysis;

            { $"namespace {mapperMetadata.Namespace}"}
            {{
               [ExcludeFromCodeCoverage]
               public class {mapperMetadata.TargetClassName} : {mapperMetadata.Name}
               {{
                   { GenerateMethods(mapperMetadata) }
               }}

                { GeneratePartialClass(mapperMetadata) }
            }}
            ";

            return result;
        }

        private string GeneratePartialClass(IMapperMetadata mapperMetadata)
        {
            if (AttributesMatchers.AnyInverseMethod(mapperMetadata.MethodsMetadata))
            {
                return $@"
                public abstract partial class {mapperMetadata?.Name}
                {{
                    { GeneratePartialClassMethods(mapperMetadata) }
                }}
                ";
            }

            return string.Empty;
        }

        private string GeneratePartialClassMethods(IMapperMetadata sourceMetadata)
        {
            var methodsStringBuilder = new StringBuilder();

            foreach (var methodMetadata in sourceMetadata.MethodsMetadata.Where(m => AttributesMatchers.IsInverseMethod(m)))
            {
                methodsStringBuilder.AppendLine(GeneratePartialClassMethod(methodMetadata));
            }

            return methodsStringBuilder.ToString();
        }

        private string GeneratePartialClassMethod(IMethodMetadata methodMetadata)
        {
            return $"public abstract {methodMetadata.InverseMethodFullName};";
        }
    }
}