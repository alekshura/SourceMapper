using Compentio.SourceMapper.Matchers;
using Compentio.SourceMapper.Metadata;
using System.Linq;

namespace Compentio.SourceMapper.Processors
{
    /// <summary>
    /// Class that generates mapper code, that was defined in abstract class
    /// </summary>
    internal class ClassProcessorStrategy : AbstractProcessorStrategy
    {
        protected override string GenerateMapperCode(IMapperMetadata mapperMetadata)
        {
            var result = @$"// <mapper-source-generated />
                            // <generated-at '{System.DateTime.UtcNow}' />

            using System;

            { $"namespace {mapperMetadata.Namespace}"}
            {{
               public class {mapperMetadata.TargetClassName} : {mapperMetadata.Name}
               {{                  
                   { GenerateMethods(mapperMetadata) }                  
               }}
            }}
            ";

            return result;
        }

        private string GenerateMethods(IMapperMetadata sourceMetadata)
        {
            var methods = string.Empty;

            foreach (var methodMetadata in sourceMetadata.MethodsMetadata)
            {
                methods += @$"public override {methodMetadata.FullName}
                {{
                    var target = new {methodMetadata.ReturnType.FullName}();
                    
                    {GenerateMappings(sourceMetadata, methodMetadata)}
                    
                    return target;
                }}";
            }

            return methods;
        }

        private string GenerateMappings(IMapperMetadata sourceMetadata, IMethodMetadata methodMetadata)
        {
            var mappings = string.Empty;
            var sourceMembers = methodMetadata.Parameters.First().Properties;
            var targetMemebers = methodMetadata.ReturnType.Properties;

            foreach (var targetMember in targetMemebers)
            {               
                var matchedSourceMember = sourceMembers.MatchSourceMember(methodMetadata.MappingAttributes, targetMember);
                var matchedTargetMember = targetMemebers.MatchTargetMember(methodMetadata.MappingAttributes, targetMember);
                var expressionAttribute = methodMetadata.MappingAttributes.MatchExpressionAttribute(targetMember, matchedSourceMember);

                if (expressionAttribute is not null && matchedSourceMember is not null)
                {
                    mappings += "\n";
                    mappings += $"target.{expressionAttribute?.Target} = {expressionAttribute?.Expression}({methodMetadata.Parameters.First().Name}.{matchedSourceMember.Name});";
                    continue;
                }

                if (expressionAttribute is not null && matchedTargetMember is not null && matchedSourceMember is null)
                {
                    mappings += "\n";
                    mappings += $"target.{expressionAttribute?.Target} = {expressionAttribute?.Expression}({methodMetadata.Parameters.First().Name});";
                    continue;
                }

                mappings += GenerateMapping(sourceMetadata, methodMetadata.Parameters.First(), matchedSourceMember, matchedTargetMember);
            }

            return mappings;
        }

        private string GenerateMapping(IMapperMetadata sourceMetadata, ITypeMetadata parameter, IPropertyMetadata matchedSourceMember, IPropertyMetadata matchedTargetMember)
        {
            if (matchedTargetMember is null && matchedSourceMember is not null)
            {
                PropertyMappingWarning(matchedSourceMember);
            }

            if (matchedSourceMember is null || matchedTargetMember is null)
            {
                PropertyMappingWarning(matchedTargetMember);
                return string.Empty;
            }                

            var mapping = "\n";
            if (!matchedSourceMember.IsClass && !matchedTargetMember.IsClass)
            {
                mapping += $"target.{matchedTargetMember?.Name} = {parameter.Name}.{matchedSourceMember?.Name};";
            }

            if (matchedSourceMember.IsClass && matchedTargetMember.IsClass)
            {
                var method = sourceMetadata.MatchDefinedMethod(matchedSourceMember, matchedTargetMember);
                if (method is not null)
                {
                    mapping += $"target.{matchedTargetMember?.Name} = {method.Name}({parameter.Name}.{matchedSourceMember.Name});";
                }
                else
                {
                    PropertyMappingWarning(matchedTargetMember);
                }
            }
            return mapping;
        }
    }
}
