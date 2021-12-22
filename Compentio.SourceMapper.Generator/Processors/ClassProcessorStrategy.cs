using Compentio.SourceMapper.Attributes;
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

        protected override string GenerateMappings(IMapperMetadata sourceMetadata, IMethodMetadata methodMetadata, bool inverseMapping = false)
        {
            var mappingsStringBuilder = new StringBuilder();
            var sourceMembers = methodMetadata.Parameters.First().Properties;
            var targetMemebers = methodMetadata.ReturnType.Properties;

            foreach (var targetMember in targetMemebers)
            {
                var matchedSourceMember = sourceMembers.MatchSourceMember(methodMetadata.MappingAttributes, targetMember);
                var matchedTargetMember = targetMemebers.MatchTargetMember(methodMetadata.MappingAttributes, targetMember);
                var expressionAttribute = methodMetadata.MappingAttributes.MatchExpressionAttribute(targetMember, matchedSourceMember);

                var expressionMapping = MapExpression(expressionAttribute, methodMetadata.Parameters.First(), matchedSourceMember, matchedTargetMember);

                if (!string.IsNullOrEmpty(expressionMapping))
                {
                    if (inverseMapping) continue;

                    mappingsStringBuilder.Append(expressionMapping);
                    continue;
                }

                mappingsStringBuilder.Append(GenerateMapping(sourceMetadata, methodMetadata.Parameters.First(), matchedSourceMember, matchedTargetMember, inverseMapping));
            }

            return mappingsStringBuilder.ToString();
        }

        private string MapExpression(MappingAttribute expressionAttribute, ITypeMetadata parameter, IPropertyMetadata matchedSourceMember, IPropertyMetadata matchedTargetMember)
        {
            var mapping = new StringBuilder();

            if (expressionAttribute is not null && matchedSourceMember is not null)
            {
                mapping.AppendLine($"target.{expressionAttribute?.Target} = {expressionAttribute?.Expression}({parameter.Name}.{matchedSourceMember.Name});");
                return mapping.ToString();
            }

            if (expressionAttribute is not null && matchedTargetMember is not null && matchedSourceMember is null)
            {
                mapping.AppendLine($"target.{expressionAttribute?.Target} = {expressionAttribute?.Expression}({parameter.Name});");
                return mapping.ToString();
            }

            return mapping.ToString();
        }
    }
}