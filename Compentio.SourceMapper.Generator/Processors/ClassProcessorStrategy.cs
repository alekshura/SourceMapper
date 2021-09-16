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
        public string GenerateCode(ISourceMetadata sourceMetadata)
        {
            var result = @$"// <mapper-source-generated />

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

        private string GenerateMethods(ISourceMetadata sourceMetadata)
        {
            var methods = string.Empty;

            foreach (var methodMetadata in sourceMetadata.MethodsMetadata)
            {
                methods += @$"{GenerateMethod(sourceMetadata, methodMetadata)}";
            }

            return methods;
        }

        private string GenerateMethod(ISourceMetadata sourceMetadata, IMethodMetadata methodMetadata)
        {
            return @$"public override {methodMetadata.MethodFullName}
                {{
                    var target = new {methodMetadata.ReturnType.FullName}();
                    
                    {GenerateMappings(sourceMetadata, methodMetadata.Parameters.First().Properties, methodMetadata.ReturnType.Properties, methodMetadata.MappingAttributes)}
                    
                    return target;
                }}";

        }

        private string GenerateMappings(ISourceMetadata sourceMetadata, IEnumerable<IPropertyMetadata> sourceMembers, 
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
                    if (matchedSourceMember is null)
                    {
                        continue;
                    }

                    matchedTargetMember = targetMember;
                }

                if (expressionAttribute is not null) 
                {
                    mappings += "\n";
                    //TODO
                    mappings += $"target.{expressionAttribute?.Target} = ConvertAutor(source);";
                }

                if (!matchedSourceMember.IsClass && !matchedTargetMember.IsClass)
                {
                    mappings += "\n";
                    mappings += $"target.{matchedTargetMember?.Name} = source.{matchedSourceMember?.Name};";
                }

                if (matchedSourceMember.IsClass && matchedTargetMember.IsClass)
                {
                    var method = sourceMetadata.FindDefinedMethod(matchedSourceMember, matchedTargetMember);
                    if (method is not null)
                    {
                        mappings += "\n";
                        mappings += $"target.{matchedTargetMember?.Name} = {method.MethodName}(source.{matchedSourceMember.Name});";
                    }
                }
            }

            return mappings;
        }
    }
}
