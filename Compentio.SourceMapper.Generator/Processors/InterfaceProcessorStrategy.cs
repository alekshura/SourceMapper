using Compentio.SourceMapper.Attributes;
using Compentio.SourceMapper.Matchers;
using Compentio.SourceMapper.Metadata;
using System;
using System.Linq;
using System.Text;

namespace Compentio.SourceMapper.Processors
{
    /// <summary>
    /// Class that generates mapper code, that was defined in interface
    /// </summary>
    internal class InterfaceProcessorStrategy : AbstractProcessorStrategy
    {
        protected override string GenerateMapperCode(IMapperMetadata mapperMetadata)
        {
            var result = @$"// <mapper-source-generated />
                            // <generated-at '{System.DateTime.UtcNow}' />

            using System;
            using System.Diagnostics.CodeAnalysis;

            {(string.IsNullOrWhiteSpace(mapperMetadata?.Namespace) ? null : $"namespace {mapperMetadata?.Namespace}")}
            {{
               [ExcludeFromCodeCoverage]
               public class {mapperMetadata?.TargetClassName} : {mapperMetadata?.Name}
               {{
                  public static {mapperMetadata?.TargetClassName} Create() => new();

                  { GenerateMethods(mapperMetadata) }
               }}

               { GeneratePartialInterface(mapperMetadata) }
            }}
            ";

            return result;
        }

        private string GeneratePartialInterface(IMapperMetadata mapperMetadata)
        {
            if (InverseAttributeService.AnyInverseMethod(mapperMetadata.MethodsMetadata))
            {
                return $@"
                public partial interface {mapperMetadata?.Name}
                {{
                    { GenerateInterfaceMethods(mapperMetadata) }
                }}
                ";
            }

            return string.Empty;
        }

        private string GenerateInterfaceMethods(IMapperMetadata sourceMetadata)
        {
            var methodsStringBuilder = new StringBuilder();

            foreach (var methodMetadata in sourceMetadata.MethodsMetadata.Where(m => InverseAttributeService.IsInverseMethod(m)))
            {
                methodsStringBuilder.AppendLine(GenerateInterfaceMethod(methodMetadata));
            }

            return methodsStringBuilder.ToString();
        }

        private string GenerateInterfaceMethod(IMethodMetadata methodMetadata)
        {
            var inverseMethodName = GetInverseMethodName(methodMetadata);

            if (!string.IsNullOrEmpty(inverseMethodName)) inverseMethodName += ";";

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
            return @$"public virtual {methodMetadata.FullName}
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
                return @$"public virtual {inverseMethodFullName}
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

                if (inverseMapping)
                    mappingsStringBuilder.Append(GenerateMapping(sourceMetadata, methodMetadata.Parameters.First(), matchedTargetMember, matchedSourceMember));
                else
                    mappingsStringBuilder.Append(GenerateMapping(sourceMetadata, methodMetadata.Parameters.First(), matchedSourceMember, matchedTargetMember));
            }

            return mappingsStringBuilder.ToString();
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

        private string GetInverseMethodName(IMethodMetadata methodMetadata)
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
    }
}