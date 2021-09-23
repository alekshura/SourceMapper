using Compentio.SourceMapper.Metadata;
using System.Linq;

namespace Compentio.SourceMapper.Matchers
{
    /// <summary>
    /// Extensions helper class for searching methods in mapper.
    /// </summary>
    internal static class MethodsMatchers
    {
        /// <summary>
        /// Returns method data that defined in mapping corresponds to source and target types.
        /// This method can be used in mappings of nested types.
        /// </summary>
        /// <param name="source">Source property metadata</param>
        /// <param name="target">Target property metadata</param>
        /// <returns>Matched method <see cref="IMethodMetadata"/></returns>
        internal static IMethodMetadata MatchDefinedMethod(this IMapperMetadata mapperMetadata, IPropertyMetadata source, IPropertyMetadata target)
        {
            var method = mapperMetadata.MethodsMetadata.FirstOrDefault(m =>
                m.ReturnType.FullName == target.FullName && m.Parameters.FirstOrDefault().FullName == source.FullName);
            return method;
        }
    }
}