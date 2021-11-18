using Compentio.SourceMapper.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.SourceMapper.Attributes
{
    internal static class InverseAttribute
    {
        /// <summary>
        /// Check that exists any method with the inverse attribute set
        /// </summary>
        /// <param name="methodsMetadata">Methods collection</param>
        /// <returns></returns>
        internal static bool AnyInverseMethod(IEnumerable<IMethodMetadata> methodsMetadata)
        {
            return methodsMetadata.Any(m => m.MappingAttributes != null && IsInverseMethod(m));
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
        /// Get inverse method name from method attribute
        /// </summary>
        /// <param name="methodMetadata">Method metadata</param>
        /// <returns></returns>
        internal static string GetInverseMethodName(IMethodMetadata methodMetadata)
        {
            var methodNameAttribute = methodMetadata.MappingAttributes.SingleOrDefault(a => !string.IsNullOrEmpty(a.InverseMethodName));

            if (methodNameAttribute == null)
                return string.Empty;

            return methodNameAttribute.InverseMethodName;
        }

        /// <summary>
        /// Get full inverse method name based on original method and inverse method name
        /// </summary>
        /// <param name="methodMetadata">Method metadata</param>
        /// <returns></returns>
        internal static string GetInverseMethodFullName(IMethodMetadata methodMetadata, string inverseMethodName)
        {
            var inverseMethodFullName =
                $"{methodMetadata.Parameters.First().FullName} {inverseMethodName} ({methodMetadata.ReturnType.FullName} {methodMetadata.Parameters.First().Name})";

            return inverseMethodFullName;
        }
    }
}