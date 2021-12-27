using Compentio.SourceMapper.Attributes;
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
        IResult GenerateCode(IMapperMetadata mapperMetadata);
    }

    /// <summary>
    /// Base abstract class for code generation strategies.
    /// It containt common functionality for code processors
    /// </summary>
    internal abstract class AbstractProcessorStrategy : IProcessorStrategy
    {
        private readonly List<DiagnosticsInfo> _diagnostics = new();

        /// <summary>
        /// Method generates source code for <see cref="MapperMetadata">
        /// </summary>
        /// <param name="mapperMetadata"></param>
        /// <returns>>Generated code and diagnostics information. See also: <seealso cref="Result"/></returns>
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

        /// <summary>
        /// Methods additional keyword modifier (virtual/override), related to interface or class methods implementation mechanism.
        /// For interfaces, methods should to be virtual for further override possibility.
        /// In case of classes, methods should override methods from mappings source class.
        /// </summary>
        protected abstract string Modifier { get; }

        protected abstract string GenerateMapperCode(IMapperMetadata mapperMetadata);

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

                {GenerateMappings(sourceMetadata, methodMetadata, MemberType.Field)}
                {GenerateMappings(sourceMetadata, methodMetadata, MemberType.Property)}

                return target;
            }}";
        }

        protected string GenerateInverseMethod(IMapperMetadata sourceMetadata, IMethodMetadata methodMetadata)
        {
            return @$"public {Modifier} {methodMetadata.InverseMethodFullName}
            {{
                if ({methodMetadata.Parameters.First().Name} == null)
                    return null;

                var target = new {methodMetadata.Parameters.First().FullName}();

                {GenerateMappings(sourceMetadata, methodMetadata, MemberType.Field, true)}
                {GenerateMappings(sourceMetadata, methodMetadata, MemberType.Property, true)}

                return target;
            }}";
        }

        protected string GenerateMappings(IMapperMetadata sourceMetadata, IMethodMetadata methodMetadata, MemberType memberType, bool inverseMapping = false)
        {
            var mappingsStringBuilder = new StringBuilder();
            var sourceMembers = GetSourceMembers(methodMetadata, memberType);
            var targetMemebers = GetTargetMembers(methodMetadata, memberType);

            foreach (var targetMember in targetMemebers)
            {
                var matchedSourceMember = sourceMembers.MatchSourceMember(methodMetadata.MappingAttributes, targetMember);
                var matchedTargetMember = targetMemebers.MatchTargetMember(methodMetadata.MappingAttributes, targetMember);

                if (IgnoreMapping(matchedSourceMember, matchedTargetMember)) continue;

                if (memberType == MemberType.Property)
                {
                    var expressionAttribute = methodMetadata.MappingAttributes.MatchExpressionAttribute(targetMember, matchedSourceMember);
                    var expressionMapping = MapExpression(expressionAttribute, methodMetadata.Parameters.First(), matchedSourceMember, matchedTargetMember);

                    if (!string.IsNullOrEmpty(expressionMapping))
                    {
                        if (inverseMapping) continue;

                        mappingsStringBuilder.Append(expressionMapping);
                        continue;
                    }
                }

                mappingsStringBuilder.Append(GenerateMapping(sourceMetadata, methodMetadata, matchedSourceMember, matchedTargetMember, memberType, inverseMapping));
            }

            return mappingsStringBuilder.ToString();
        }

        protected string GenerateMapping(IMapperMetadata sourceMetadata, IMethodMetadata methodMetadata, IMemberMetadata matchedSourceMember, IMemberMetadata matchedTargetMember, MemberType memberType, bool inverseMapping = false)
        {
            if (inverseMapping) ObjectHelper.Swap(ref matchedSourceMember, ref matchedTargetMember);

            if (matchedTargetMember is null && matchedSourceMember is not null)
            {
                MemberMappingWarning(matchedSourceMember);
            }

            if (matchedSourceMember is null || matchedTargetMember is null)
            {
                MemberMappingWarning(matchedSourceMember ?? matchedTargetMember);
                return string.Empty;
            }

            var mapping = new StringBuilder();

            if (matchedSourceMember.IsStatic && matchedTargetMember.IsStatic && memberType == MemberType.Field)
            {
                mapping.AppendLine(MapStaticProperty(matchedSourceMember, matchedTargetMember, methodMetadata, inverseMapping));
                return mapping.ToString();
            }

            if (!matchedSourceMember.IsClass && !matchedTargetMember.IsClass)
            {
                mapping.AppendLine(MapProperty(matchedSourceMember, matchedTargetMember, methodMetadata.Parameters.First()));
            }

            if (matchedSourceMember.IsClass && matchedTargetMember.IsClass)
            {
                mapping.AppendLine(MapClass(sourceMetadata, matchedSourceMember, matchedTargetMember, methodMetadata.Parameters.First(), inverseMapping));
            }

            return mapping.ToString();
        }

        protected string MapProperty(IMetadata matchedSourceMember, IMetadata matchedTargetMember, ITypeMetadata parameter)
        {
            return $"target.{matchedTargetMember?.Name} = {parameter.Name}.{matchedSourceMember?.Name};";
        }

        protected string MapClass(IMapperMetadata sourceMetadata, IMemberMetadata matchedSourceMember, IMemberMetadata matchedTargetMember, ITypeMetadata parameter, bool inverseMapping)
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
                MemberMappingWarning(matchedTargetMember);
            }

            return string.Empty;
        }

        protected string MapStaticProperty(IMemberMetadata matchedSourceMember, IMemberMetadata matchedTargetMember, IMethodMetadata methodMetadata, bool inverseMapping)
        {
            if (inverseMapping)
                return $"{methodMetadata.Parameters.First().FullName}.{matchedTargetMember?.Name} = {methodMetadata.ReturnType.FullName}.{matchedSourceMember?.Name};";
            else
                return $"{methodMetadata.ReturnType.FullName}.{matchedTargetMember?.Name} = {methodMetadata.Parameters.First().FullName}.{matchedSourceMember?.Name};";
        }

        protected string MapExpression(MappingAttribute expressionAttribute, ITypeMetadata parameter, IMemberMetadata matchedSourceMember, IMemberMetadata matchedTargetMember)
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

        protected IMethodMetadata? GetDefinedMethod(IMapperMetadata sourceMetadata, IMemberMetadata matchedSourceMember, IMemberMetadata matchedTargetMember, bool inverseMapping)
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

        /// <summary>
        /// Metchod checks that member metadata should be ignored during mapping due to <see cref="IgnoreMappingAttribute"/>
        /// </summary>
        /// <param name="sourceFieldMetadata"></param>
        /// <param name="targetFieldMetadata"></param>
        /// <returns></returns>
        protected bool IgnoreMapping(IMemberMetadata? sourceFieldMetadata, IMemberMetadata? targetFieldMetadata)
        {
            return (sourceFieldMetadata?.IgnoreInMapping is true || targetFieldMetadata?.IgnoreInMapping is true);
        }

        protected void MemberMappingWarning(IMemberMetadata metadata)
        {
            _diagnostics.Add(new DiagnosticsInfo
            {
                DiagnosticDescriptor = metadata.MemberType == MemberType.Field ? SourceMapperDescriptors.FieldIsNotMapped : SourceMapperDescriptors.PropertyIsNotMapped,
                Metadata = metadata
            });
        }

        protected IEnumerable<IMemberMetadata> GetSourceMembers(IMethodMetadata methodMetadata, MemberType memberType)
        {
            var typeMetadata = methodMetadata.Parameters.First();

            switch (memberType)
            {
                case MemberType.Field:
                    return typeMetadata.Fields;

                case MemberType.Property:
                    return typeMetadata.Properties;

                default:
                    return Enumerable.Empty<IMemberMetadata>();
            }
        }

        protected IEnumerable<IMemberMetadata> GetTargetMembers(IMethodMetadata methodMetadata, MemberType memberType)
        {
            var typeMetadata = methodMetadata.ReturnType;

            switch (memberType)
            {
                case MemberType.Field:
                    return typeMetadata.Fields;

                case MemberType.Property:
                    return typeMetadata.Properties;

                default:
                    return Enumerable.Empty<IMemberMetadata>();
            }
        }
    }
}