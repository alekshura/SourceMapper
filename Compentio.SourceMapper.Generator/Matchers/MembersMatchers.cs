using Compentio.SourceMapper.Attributes;
using Compentio.SourceMapper.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.SourceMapper.Matchers
{
    internal static class MembersMatchers
    {
        /// <summary>
        /// Method searches for source member that matches <see cref="MappingAttribute.Source"/> value. This <see cref="MappingAttribute"/> also should match the target property.
        /// </summary>
        /// <param name="sourceMembers">Collection of source properties</param>
        /// <param name="mappingAttributes">Collection of all defined mapping attributes</param>
        /// <param name="targetMember">Target member</param>
        /// <returns>Matched source member</returns>
        internal static IMetadata MatchSourceMember(this IEnumerable<IMetadata> sourceMembers, IEnumerable<MappingAttribute> mappingAttributes, IMetadata targetMember)
        {
            var matchedAttribute = mappingAttributes.MatchTargetAttribute(targetMember);
            var matchedSourceMember = sourceMembers.FirstOrDefault(member => member?.Name == matchedAttribute?.Source);
            if (matchedSourceMember is null)
            {
                matchedSourceMember = sourceMembers.MatchSourceMember(targetMember);
            }
            return matchedSourceMember;
        }
        /// <summary>
        /// Method searches for source member that name matches target member.
        /// </summary>
        /// <param name="members">Collection of source properties</param>
        /// <param name="targetMember">Target member</param>
        /// <returns>Matched source member</returns>
        internal static IMetadata MatchSourceMember(this IEnumerable<IMetadata> members, IMetadata targetMember)
        {
            return members.FirstOrDefault(member => member?.Name == targetMember?.Name);
        }
        /// <summary>
        /// Method searches for target member that matches <see cref="MappingAttribute.Target"/> value. This <see cref="MappingAttribute"/> also should match the target property.
        /// </summary>
        /// <param name="targetMembers">Collection of target properties</param>
        /// <param name="mappingAttributes">Collection of all defined mapping attributes</param>
        /// <param name="targetMember">Target member</param>
        /// <returns>Matched target member</returns>
        internal static IMetadata MatchTargetMember(this IEnumerable<IMetadata> targetMembers, IEnumerable<MappingAttribute> mappingAttributes, IMetadata targetMember)
        {
            var matchedAttribute = mappingAttributes.MatchTargetAttribute(targetMember);
            var matchedTargetMember = targetMembers.FirstOrDefault(member => member?.Name == matchedAttribute?.Target);
            return matchedTargetMember is not null ? matchedTargetMember : targetMember;
        }
    }
}
