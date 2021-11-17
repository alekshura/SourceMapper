using Compentio.SourceMapper.Attributes;
using Compentio.SourceMapper.Diagnostics;
using Compentio.SourceMapper.Matchers;
using Compentio.SourceMapper.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compentio.SourceMapper.Processors
{
    /// <summary>
    /// Interface for code generation strategies
    /// </summary>
    interface IProcessorStrategy
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

        protected abstract string GenerateMapperCode(IMapperMetadata mapperMetadata);

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
                    return $"target.{matchedTargetMember?.Name} = {InverseAttributeService.GetInverseMethodName(method)}({parameter.Name}.{matchedSourceMember.Name});";
                else
                    return $"target.{matchedTargetMember?.Name} = {method.Name}({parameter.Name}.{matchedSourceMember.Name});";
            }
            else
            {
                PropertyMappingWarning(matchedTargetMember);
            }

            return string.Empty;
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

        protected IMethodMetadata? GetDefinedMethod(IMapperMetadata sourceMetadata, IPropertyMetadata matchedSourceMember, IPropertyMetadata matchedTargetMember, bool inverseMapping)
        {
            if (inverseMapping)
            {
                var originalMethod = sourceMetadata.MatchDefinedMethod(matchedTargetMember, matchedSourceMember);

                if (originalMethod == null) return originalMethod;

                try
                {
                    if (!string.IsNullOrEmpty(InverseAttributeService.GetInverseMethodName(originalMethod)))
                    {
                        return originalMethod;
                    }

                    return null;
                }
                catch (InvalidOperationException)
                {
                    ReportMultipleInternalMethodName(originalMethod);

                    return null;
                }
                catch (Exception exception)
                {
                    ReportInternalMethodNameError(exception, originalMethod);

                    return null;
                }
            }
            else
            {
                return sourceMetadata.MatchDefinedMethod(matchedSourceMember, matchedTargetMember);
            }
        }

        protected string GetInverseMethodName(IMethodMetadata methodMetadata)
        {
            try
            {
                var inverseMethodName = InverseAttributeService.GetInverseMethodName(methodMetadata);

                if (string.IsNullOrEmpty(inverseMethodName))
                {
                    ReportEmptyInverseMethodName(methodMetadata);

                    return inverseMethodName;
                }

                return InverseAttributeService.GetInverseMethodFullName(methodMetadata, inverseMethodName);
            }
            catch (InvalidOperationException)
            {
                ReportMultipleInternalMethodName(methodMetadata);

                return string.Empty;
            }
            catch (Exception exception)
            {
                ReportInternalMethodNameError(exception, methodMetadata);

                return string.Empty;
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

        protected void ReportEmptyInverseMethodName(IMethodMetadata methodMetadata)
        {
            _diagnostics.Add(new DiagnosticsInfo
            {
                DiagnosticDescriptor = SourceMapperDescriptors.ExpectedInverseMethodName,
                Metadata = methodMetadata
            });
        }

        protected void ReportMultipleInternalMethodName(IMethodMetadata methodMetadata)
        {
            _diagnostics.Add(new DiagnosticsInfo
            {
                DiagnosticDescriptor = SourceMapperDescriptors.MultipleInverseMethodName,
                Metadata = methodMetadata
            });
        }

        protected void ReportInternalMethodNameError(Exception exception, IMethodMetadata methodMetadata)
        {
            _diagnostics.Add(new DiagnosticsInfo
            {
                DiagnosticDescriptor = SourceMapperDescriptors.UnexpectedError,
                Metadata = methodMetadata,
                Exception = exception
            });
        }
    }
}
