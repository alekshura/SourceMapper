﻿using Compentio.SourceMapper.Attributes;
using Compentio.SourceMapper.Matchers;
using Compentio.SourceMapper.Metadata;
using System;
using System.Linq;
using System.Text;

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
            if (InverseAttributeService.AnyInverseMethod(mapperMetadata.MethodsMetadata))
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

            foreach (var methodMetadata in sourceMetadata.MethodsMetadata.Where(m => InverseAttributeService.IsInverseMethod(m)))
            {
                methodsStringBuilder.AppendLine(GeneratePartialClassMethod(methodMetadata));
            }

            return methodsStringBuilder.ToString();
        }

        private string GeneratePartialClassMethod(IMethodMetadata methodMetadata)
        {
            var inverseMethodName = GetInverseMethodName(methodMetadata);

            if (!string.IsNullOrEmpty(inverseMethodName)) inverseMethodName = $"public abstract {inverseMethodName};";

            return inverseMethodName;
        }

        private string GenerateMethods(IMapperMetadata sourceMetadata)
        {
            var methodsStringBuilder = new StringBuilder();

            foreach (var methodMetadata in sourceMetadata.MethodsMetadata)
            {
                methodsStringBuilder.Append(GenerateRegularMethod(sourceMetadata, methodMetadata));

                if (InverseAttributeService.IsInverseMethod(methodMetadata))
                {
                    methodsStringBuilder.AppendLine(GenerateInverseMethod(sourceMetadata, methodMetadata));
                }
            }

            return methodsStringBuilder.ToString();
        }

        private string GenerateRegularMethod(IMapperMetadata sourceMetadata, IMethodMetadata methodMetadata)
        {
            return @$"public override {methodMetadata.FullName}
            {{
                if ({methodMetadata.Parameters.First().Name} == null)
                    return null;

                var target = new {methodMetadata.ReturnType.FullName}();

                {GenerateMappings(sourceMetadata, methodMetadata)}

                return target;
            }}";
        }

        private string GenerateInverseMethod(IMapperMetadata sourceMetadata, IMethodMetadata methodMetadata)
        {
            var inverseMethodFullName = GetInverseMethodName(methodMetadata);

            if (string.IsNullOrEmpty(inverseMethodFullName))
                return inverseMethodFullName;
            else
                return @$"public override {inverseMethodFullName}
                {{
                    if ({methodMetadata.Parameters.First().Name} == null)
                        return null;

                    var target = new {methodMetadata.Parameters.First().FullName}();

                    {GenerateMappings(sourceMetadata, methodMetadata, true)}

                    return target;
                }}";
        }

        private string GenerateMappings(IMapperMetadata sourceMetadata, IMethodMetadata methodMetadata, bool inverseMapping = false)
        {
            var mappingsStringBuilder = new StringBuilder();
            var sourceMembers = methodMetadata.Parameters.First().Properties;
            var targetMemebers = methodMetadata.ReturnType.Properties;

            foreach (var targetMember in targetMemebers)
            {               
                var matchedSourceMember = sourceMembers.MatchSourceMember(methodMetadata.MappingAttributes, targetMember);
                var matchedTargetMember = targetMemebers.MatchTargetMember(methodMetadata.MappingAttributes, targetMember);
                var expressionAttribute = methodMetadata.MappingAttributes.MatchExpressionAttribute(targetMember, matchedSourceMember);

                var expressionMapping = MapExpression(expressionAttribute, methodMetadata.Parameters.First(), matchedSourceMember, matchedTargetMember, inverseMapping);

                if(!string.IsNullOrEmpty(expressionMapping))
                {
                    mappingsStringBuilder.Append(expressionMapping);
                    continue;
                }                

                mappingsStringBuilder.Append(GenerateMapping(sourceMetadata, methodMetadata.Parameters.First(), matchedSourceMember, matchedTargetMember, inverseMapping));
            }

            return mappingsStringBuilder.ToString();
        }

        private string MapExpression(MappingAttribute expressionAttribute, ITypeMetadata parameter, IPropertyMetadata matchedSourceMember, IPropertyMetadata matchedTargetMember, bool inverseMapping = false)
        {
            if (inverseMapping)
                return MapInverseExpression(expressionAttribute, parameter, matchedSourceMember, matchedTargetMember);
            else
                return MapRegularExpression(expressionAttribute, parameter, matchedSourceMember, matchedTargetMember);                    
        }

        private string MapInverseExpression(MappingAttribute expressionAttribute, ITypeMetadata parameter, IPropertyMetadata matchedSourceMember, IPropertyMetadata matchedTargetMember)
        {
            var mapping = new StringBuilder();

            if (string.IsNullOrEmpty(expressionAttribute?.InverseExpression) || string.IsNullOrEmpty(expressionAttribute?.InverseTarget)) return mapping.ToString();

            if (expressionAttribute is not null && matchedSourceMember is not null)
            {
                mapping.AppendLine($"target.{expressionAttribute?.InverseTarget} = {expressionAttribute?.InverseExpression}({parameter.Name}.{matchedSourceMember.Name});");
                return mapping.ToString();
            }

            if (expressionAttribute is not null && matchedTargetMember is not null && matchedSourceMember is null)
            {
                mapping.AppendLine($"target.{expressionAttribute?.InverseTarget} = {expressionAttribute?.InverseExpression}({parameter.Name});");
                return mapping.ToString();
            }

            return mapping.ToString();
        }

        private string MapRegularExpression(MappingAttribute expressionAttribute, ITypeMetadata parameter, IPropertyMetadata matchedSourceMember, IPropertyMetadata matchedTargetMember)
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
