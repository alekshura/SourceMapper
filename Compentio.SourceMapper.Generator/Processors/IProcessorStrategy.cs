using Compentio.SourceMapper.Diagnostics;
using Compentio.SourceMapper.Helpers;
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
        IResult GenerateCode(IMapperMetadata mapperMetadata, IMapperMetadata? baseMapper);
    }

    /// <summary>
    /// Base abstract class for code generation strategies.
    /// It containt common functionality for code processors
    /// </summary>
    internal abstract class AbstractProcessorStrategy : IProcessorStrategy
    {
        private readonly List<DiagnosticsInfo> _diagnostics = new();
        protected IMapperMetadata? _baseMapperMetadata { get; set; }

        /// <summary>
        /// Method generates source code for <see cref="MapperMetadata">
        /// </summary>
        /// <param name="mapperMetadata"></param>
        /// <returns>>Generated code and diagnostics information. See also: <seealso cref="Result"/></returns>
        public IResult GenerateCode(IMapperMetadata mapperMetadata, IMapperMetadata? baseMapper)
        {
            _baseMapperMetadata = baseMapper;

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

        /// <summary>
        /// Methods additional keyword modifier (virtual/override), related to interface or class methods implementation mechanism.
        /// For interfaces, methods should to be virtual for further override possibility.
        /// In case of classes, methods should override methods from mappings source class.
        /// </summary>
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

		protected string GenerateMethodsFromBaseMapper()
        {
            if (_baseMapperMetadata is null || _baseMapperMetadata.MethodsMetadata is null) return string.Empty;

            return GenerateMethods(_baseMapperMetadata);
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
            if (inverseMapping) ObjectHelper.Swap(ref matchedSourceMember, ref matchedTargetMember);

            if (matchedTargetMember is null && matchedSourceMember is not null)
            {
                PropertyMappingWarning(matchedSourceMember);
            }

            if (matchedSourceMember is null || matchedTargetMember is null)
            {
                PropertyMappingWarning(matchedSourceMember ?? matchedTargetMember);
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

            // Check that method can be used from other/base mapper
            if (method is null && _baseMapperMetadata is not null)
            {
                method = GetDefinedMethodFromBaseMapper(matchedSourceMember, matchedTargetMember, inverseMapping);
            }

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

        protected IMethodMetadata? GetDefinedMethodFromBaseMapper(IPropertyMetadata matchedSourceMember, IPropertyMetadata matchedTargetMember, bool inverseMapping)
        {
            return GetDefinedMethod(_baseMapperMetadata, matchedSourceMember, matchedTargetMember, inverseMapping);
        }


        protected IMethodMetadata? GetDefinedMethod(IMapperMetadata sourceMetadata, IPropertyMetadata matchedSourceMember, IPropertyMetadata matchedTargetMember, bool inverseMapping)
        {
            if (inverseMapping)
            {
                return sourceMetadata.MatchDefinedMethod(matchedTargetMember, matchedSourceMember);
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