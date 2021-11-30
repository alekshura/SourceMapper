using Compentio.SourceMapper.Attributes;
using Compentio.SourceMapper.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.SourceMapper.Matchers
{
    internal static class AttributesMatchers
    {
        /// <summary>
        /// Method searches for <see cref="MappingAttribute"/> that Target property matches target member.
        /// </summary>
        /// <param name="attributes">Attributes collection</param>
        /// <param name="targetProperty">Target property to match</param>
        /// <returns>Matched attribute</returns>
        internal static MappingAttribute MatchTargetAttribute(this IEnumerable<MappingAttribute> attributes, IPropertyMetadata targetProperty)
        {
            return attributes.FirstOrDefault(attribute => attribute?.Target == targetProperty?.Name);
        }
        /// <summary>
        /// Method searches for <see cref="MappingAttribute"/> that Target and Source property matches target member and attribute has not empty <see cref="MappingAttribute.Expression"/> value
        /// If it is <c>null</c> than searches matching <see cref="MappingAttribute"/> only by <see cref="MappingAttribute.Target"/> and <see cref="MappingAttribute.Expression"/> values
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="targetProperty"></param>
        /// <param name="sourceProperty"></param>
        /// <returns></returns>
        internal static MappingAttribute MatchExpressionAttribute(this IEnumerable<MappingAttribute> attributes, IPropertyMetadata targetProperty, IPropertyMetadata sourceProperty)
        {
            var matchedExpressionAttribute = attributes.FirstOrDefault(attribute => attribute?.Target == targetProperty?.Name 
                && attribute?.Source == sourceProperty?.Name 
                && !string.IsNullOrEmpty(attribute?.Expression));

            if (matchedExpressionAttribute is null)
            {
                matchedExpressionAttribute = attributes.FirstOrDefault(attribute => attribute?.Target == targetProperty?.Name && !string.IsNullOrEmpty(attribute?.Expression));
            }
            return matchedExpressionAttribute;
        }

        /// <summary>
        /// Check that exists any method with the inverse attribute set
        /// </summary>
        /// <param name="methodsMetadata">Methods collection</param>
        /// <returns></returns>
        internal static bool AnyInverseMethod(IEnumerable<IMethodMetadata> methodsMetadata)
        {
            return methodsMetadata.Any(m => IsInverseMethod(m));
        }

        /// <summary>
        /// Check method has inverse method attribute
        /// </summary>
        /// <param name="methodMetadata">Method metadata</param>
        /// <returns></returns>
        internal static bool IsInverseMethod(IMethodMetadata methodMetadata)
        {
            return !string.IsNullOrEmpty(methodMetadata.InverseMethodName);
        }

        /// <summary>
        /// Get full inverse method name based on original method and inverse method name
        /// </summary>
        /// <param name="methodMetadata">Method metadata</param>
        /// <returns></returns>
        internal static string GetInverseMethodFullName(IMethodMetadata methodMetadata)
        {
            var inverseMethodFullName =
                $"{methodMetadata.Parameters.First().FullName} {methodMetadata.InverseMethodName} ({methodMetadata.ReturnType.FullName} {methodMetadata.Parameters.First().Name})";

            return inverseMethodFullName;
        }
    }
}
