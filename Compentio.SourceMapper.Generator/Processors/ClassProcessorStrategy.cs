using Compentio.SourceMapper.Attributes;
using Compentio.SourceMapper.Metadata;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.SourceMapper.Processors
{
    internal class ClassProcessorStrategy : IProcessorStrategy
    {
        public string GenerateCode(IMapperMetadata sourceMetadata)
        {
            var result = @$"// <mapper-source-generated />
                            // <generated-at '{System.DateTime.UtcNow}' />

            using System;

            { $"namespace {sourceMetadata.Namespace}"}
            {{
               public class {sourceMetadata.TargetClassName} : {sourceMetadata.MapperName}
               {{                  
                   { GenerateMethods(sourceMetadata) }                  
               }}
            }}
            ";

            var tree = CSharpSyntaxTree.ParseText(result);
            var root = tree.GetRoot().NormalizeWhitespace();
            return root.ToFullString();
        }

        private string GenerateMethods(IMapperMetadata sourceMetadata)
        {
            var methods = string.Empty;

            foreach (var methodMetadata in sourceMetadata.MethodsMetadata)
            {
                methods += @$"public override {methodMetadata.MethodFullName}
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
                var matchedAttribute = mappingAttributes.FirstOrDefault(attribute => attribute?.Target == targetMember?.Name);
                var expressionAttribute = mappingAttributes.FirstOrDefault(attribute => attribute?.Target == targetMember?.Name && !string.IsNullOrEmpty(attribute?.Expression));
                var matchedSourceMember = sourceMembers.FirstOrDefault(symbol => symbol?.Name == matchedAttribute?.Source);
                var matchedTargetMember = targetMemebers.FirstOrDefault(symbol => symbol?.Name == matchedAttribute?.Target);

                if (matchedSourceMember is null || matchedTargetMember is null)
                {
                    matchedSourceMember = sourceMembers.FirstOrDefault(p => p?.Name == targetMember?.Name);
                    if (matchedSourceMember is null && expressionAttribute is not null)
                    {
                        mappings += "\n";
                        mappings += $"target.{expressionAttribute?.Target} = {expressionAttribute?.Expression}(source);";
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
                var method = sourceMetadata.FindDefinedMethod(matchedSourceMember, matchedTargetMember);
                if (method is not null)
                {
                    mapping += $"target.{matchedTargetMember?.Name} = {method.MethodName}(source.{matchedSourceMember.Name});";
                }
            }
            return mapping;
        }
    }
}
