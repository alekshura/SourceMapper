using Compentio.SourceMapper.Diagnostics;
using Compentio.SourceMapper.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.SourceMapper.Attributes
{
    internal static class InverseAttributeService
    {
        private static readonly List<DiagnosticsInfo> _diagnostics = new();

        /// <summary>
        /// Check that exists any method with the inverse attribute set
        /// </summary>
        /// <param name="methodsMetadata">Methods collection</param>
        /// <returns></returns>
        internal static bool AnyInverseMethod(IEnumerable<IMethodMetadata> methodsMetadata)
        {
            return methodsMetadata.Any(m => m.MappingAttributes != null && m.MappingAttributes.Any(a => a.CreateInverse));
        }

        /// <summary>
        /// Check method has inverse method attribute
        /// </summary>
        /// <param name="methodMetadata">Method metadata</param>
        /// <returns></returns>
        internal static bool IsInverseMethod(IMethodMetadata methodMetadata)
        {
            return methodMetadata.MappingAttributes.Any(a => a.CreateInverse);
        }

        /// <summary>
        /// Get full inverse method name based on original method and inverse method name attribute
        /// </summary>
        /// <param name="methodMetadata">Method metadata</param>
        /// <returns></returns>
        internal static string GetInverseMethodName(IMethodMetadata methodMetadata)
        {
            try
            {
                var methodNameAttribute = methodMetadata.MappingAttributes.SingleOrDefault(a => !string.IsNullOrEmpty(a.InverseMethodName));

                if (methodNameAttribute == null)
                {
                    ReportEmptyInverseMethodName(methodMetadata);

                    return string.Empty;
                }

                var inverseMethodFullName =
                    $"{methodMetadata.Parameters.First().FullName} {methodNameAttribute.InverseMethodName} ({methodMetadata.ReturnType.FullName} {methodMetadata.Parameters.First().Name})";

                return inverseMethodFullName;
            }
            catch (InvalidOperationException invalidOperationException)
            {
                ReportMultipleInternalMethodName(invalidOperationException, methodMetadata);

                return string.Empty;
            }
            catch (Exception exception)
            {
                ReportInternalMethodNameError(exception, methodMetadata);

                return string.Empty;
            }
        }

        internal static void ReportEmptyInverseMethodName(IMethodMetadata methodMetadata)
        {
            _diagnostics.Add(new DiagnosticsInfo
            {
                DiagnosticDescriptor = SourceMapperDescriptors.ExpectedInverseMethodName,
                Metadata = methodMetadata
            });
        }

        internal static void ReportMultipleInternalMethodName(Exception exception, IMethodMetadata methodMetadata)
        {
            _diagnostics.Add(new DiagnosticsInfo
            {
                DiagnosticDescriptor = SourceMapperDescriptors.MultipleInverseMethodName,
                Metadata = methodMetadata,
                Exception = exception
            });
        }

        internal static void ReportInternalMethodNameError(Exception exception, IMethodMetadata methodMetadata)
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