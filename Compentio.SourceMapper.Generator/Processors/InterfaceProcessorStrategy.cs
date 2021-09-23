using Compentio.SourceMapper.Attributes;
using Compentio.SourceMapper.Matchers;
using Compentio.SourceMapper.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.SourceMapper.Processors
{
    /// <summary>
    /// Class that generates mapper code, that was defined in interface
    /// </summary>
    internal class InterfaceProcessorStrategy : AbstractProcessorStrategy
    {
        public override string GenerateCode(IMapperMetadata mapperMetadata)
        {
            var result = @$"// <mapper-source-generated />
                            // <generated-at '{System.DateTime.UtcNow}' />

            using System;

            {(string.IsNullOrWhiteSpace(mapperMetadata?.Namespace) ? null : $"namespace {mapperMetadata?.Namespace}")}
            {{
               public class {mapperMetadata?.TargetClassName} : {mapperMetadata?.Name}
               {{
                  public static {mapperMetadata?.TargetClassName} Create() => new();
                  
                   { GenerateMethods(mapperMetadata) }                  
               }}
            }}
            ";

            return FormatCode(result);
        }

        private string GenerateMethods(IMapperMetadata sourceMetadata)
        {
            var methods = string.Empty;

            foreach (var methodMetadata in sourceMetadata.MethodsMetadata)
            {
                methods += @$"public virtual {methodMetadata.FullName}
                {{
                    var target = new {methodMetadata.ReturnType.FullName}();
                    
                    {GenerateMappings(sourceMetadata, methodMetadata.Parameters.First().Properties, methodMetadata.ReturnType.Properties, methodMetadata.MappingAttributes)}
                    
                    return target;
                }}";
            }

            return methods;
        }

        private string GenerateMappings(IMapperMetadata sourceMetadata, IEnumerable<IPropertyMetadata> sourceMembers, 
            IEnumerable<IPropertyMetadata> targetMemebers, IEnumerable<MappingAttribute> mappingAttributes)
        {
            var mappings = string.Empty;

            foreach (var targetMember in targetMemebers)
            {
                var matchedSourceMember = sourceMembers.MatchSourceMember(mappingAttributes, targetMember);
                var matchedTargetMember = targetMemebers.MatchTargetMember(mappingAttributes, targetMember);

                if (matchedSourceMember is null || matchedTargetMember is null)
                {
                    matchedSourceMember = sourceMembers.MatchSourceMember(targetMember);
                    if (matchedSourceMember is null)
                    {
                        PropertyMappingWarning(targetMember);
                        continue;
                    }

                    matchedTargetMember = targetMember;
                }

                mappings += GenerateMapping(sourceMetadata, matchedSourceMember, matchedTargetMember);
            }

            return mappings;
        }

        private string GenerateMapping(IMapperMetadata sourceMetadata, IPropertyMetadata matchedSourceMember, IPropertyMetadata matchedTargetMember)
        {
            if (matchedSourceMember is null || matchedTargetMember is null)
                return string.Empty;

            var mapping = "\n";

            if (!matchedSourceMember.IsClass && !matchedTargetMember.IsClass)
            {
                mapping += $"target.{matchedTargetMember?.Name} = source.{matchedSourceMember?.Name};";
            }

            if (matchedSourceMember.IsClass && matchedTargetMember.IsClass)
            {
                var method = sourceMetadata.MatchDefinedMethod(matchedSourceMember, matchedTargetMember);
                if (method is not null)
                {
                    mapping += $"target.{matchedTargetMember?.Name} = {method.Name}(source.{matchedSourceMember.Name});";
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
