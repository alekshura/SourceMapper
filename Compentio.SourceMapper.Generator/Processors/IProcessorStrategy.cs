using Compentio.SourceMapper.Diagnostics;
using Compentio.SourceMapper.Matchers;
using Compentio.SourceMapper.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compentio.SourceMapper.Processors
{
    /// <summary>
    /// Interface for code generation strategies
    /// </summary>
    internal interface IProcessorStrategy
    {
        /// <summary>
        /// Generates code for mappings
        /// </summary>
        /// <param name="mapperMetadata"></param>
        /// <returns>Generated code and diagnostics information. See also: <seealso cref="Result"/></returns>
        IResult GenerateCode(IMapperMetadata mapperMetadata);
    }

    /// <summary>
    /// Base abstract class for code generation strategies.
    /// It containt common functionality for code processors
    /// </summary>
    internal abstract class AbstractProcessorStrategy : IProcessorStrategy
    {
        private readonly List<DiagnosticsInfo> _diagnostics = new();

        public IResult GenerateCode(IMapperMetadata mapperMetadata)
        {
            try
            {
                string code = GenerateMapperCode(mapperMetadata);
                return Result.Ok(code, _diagnostics);
            }
            catch (Exception ex)
            {
                return Result.Error(ex);
            }
        }

        protected abstract string Modifier { get; }

        protected abstract string GenerateMapperCode(IMapperMetadata mapperMetadata);

        protected abstract string GenerateMappings(IMapperMetadata sourceMetadata, IMethodMetadata methodMetadata, bool inverseMapping = false);

        protected string GenerateMethods(IMapperMetadata sourceMetadata)
        {
            var methodsStringBuilder = new StringBuilder();

            foreach (var methodMetadata in sourceMetadata.MethodsMetadata)
            {
                methodsStringBuilder.Append(GenerateMethod(sourceMetadata, methodMetadata));

                if (AttributesMatchers.IsInverseMethod(methodMetadata))
                {
                    methodsStringBuilder.AppendLine(GenerateInverseMethod(sourceMetadata, methodMetadata));
                }
            }

            return methodsStringBuilder.ToString();
        }

        protected string GenerateMethod(IMapperMetadata sourceMetadata, IMethodMetadata methodMetadata)
        {
            return @$"public {Modifier} {methodMetadata.FullName}
            {{
                if ({methodMetadata.Parameters.First().Name} == null)
                    return null;

                var target = new {methodMetadata.ReturnType.FullName}();

                {GenerateMappings(sourceMetadata, methodMetadata)}

                return target;
            }}";
        }

        protected string GenerateInverseMethod(IMapperMetadata sourceMetadata, IMethodMetadata methodMetadata)
        {
            return @$"public {Modifier} {AttributesMatchers.GetInverseMethodFullName(methodMetadata)}
            {{
                if ({methodMetadata.Parameters.First().Name} == null)
                    return null;

                var target = new {methodMetadata.Parameters.First().FullName}();

                {GenerateMappings(sourceMetadata, methodMetadata, true)}

                return target;
            }}";
        }

        protected string GenerateMapping(IMapperMetadata sourceMetadata, ITypeMetadata parameter, IPropertyMetadata matchedSourceMember, IPropertyMetadata matchedTargetMember, bool inverseMapping = false)
        {
            if (inverseMapping) PropertyMetadata.Swap(ref matchedSourceMember, ref matchedTargetMember);

            if (matchedTargetMember is null && matchedSourceMember is not null)
            {
                PropertyMappingWarning(matchedSourceMember);
            }

            if (matchedSourceMember is null || matchedTargetMember is null)
            {
                PropertyMappingWarning(matchedSourceMember != null ? matchedSourceMember : matchedTargetMember);
                return string.Empty;
            }

            var mapping = new StringBuilder();

            if (!matchedSourceMember.IsClass && !matchedTargetMember.IsClass)
            {
                mapping.AppendLine(MapProperty(matchedSourceMember, matchedTargetMember, parameter));
            }

            if (matchedSourceMember.IsClass && matchedTargetMember.IsClass)
            {
                mapping.AppendLine(MapClass(sourceMetadata, matchedSourceMember, matchedTargetMember, parameter, inverseMapping));
            }
            return mapping.ToString();
        }

        protected string MapProperty(IPropertyMetadata matchedSourceMember, IPropertyMetadata matchedTargetMember, ITypeMetadata parameter)
        {
            return $"target.{matchedTargetMember?.Name} = {parameter.Name}.{matchedSourceMember?.Name};";
        }

        protected string MapClass(IMapperMetadata sourceMetadata, IPropertyMetadata matchedSourceMember, IPropertyMetadata matchedTargetMember, ITypeMetadata parameter, bool inverseMapping)
        {
            var method = GetDefinedMethod(sourceMetadata, matchedSourceMember, matchedTargetMember, inverseMapping);

            if (method is not null)
            {
                if (inverseMapping)
                    return $"target.{matchedTargetMember?.Name} = {method.InverseMethodName}({parameter.Name}.{matchedSourceMember.Name});";
                else
                    return $"target.{matchedTargetMember?.Name} = {method.Name}({parameter.Name}.{matchedSourceMember.Name});";
            }
            else
            {
                PropertyMappingWarning(matchedTargetMember);
            }

            return string.Empty;
        }

        protected IMethodMetadata? GetDefinedMethod(IMapperMetadata sourceMetadata, IPropertyMetadata matchedSourceMember, IPropertyMetadata matchedTargetMember, bool inverseMapping)
        {
            if (inverseMapping)
            {
                var originalMethod = sourceMetadata.MatchDefinedMethod(matchedTargetMember, matchedSourceMember);

                if (originalMethod == null) return originalMethod;

                if (!string.IsNullOrEmpty(originalMethod.InverseMethodName))
                {
                    return originalMethod;
                }

                return null;
            }
            else
            {
                return sourceMetadata.MatchDefinedMethod(matchedSourceMember, matchedTargetMember);
            }
        }

        protected void PropertyMappingWarning(IPropertyMetadata metadata)
        {
            _diagnostics.Add(new DiagnosticsInfo
            {
                DiagnosticDescriptor = SourceMapperDescriptors.PropertyIsNotMapped,
                Metadata = metadata
            });
        }
    }
}