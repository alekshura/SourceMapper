using Compentio.SourceMapper.Attributes;
using Compentio.SourceMapper.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.SourceMapper.Matchers
{
    internal static class MembersMatchers
    {
        /// <summary>
        /// Method searches for source member that matches <see cref="MappingAttribute.Source"/> value. This <see cref="MappingAttribute"/> also should match the target member.
        /// </summary>
        /// <param name="sourceMembers">Collection of source members</param>
        /// <param name="mappingAttributes">Collection of all defined mapping attributes</param>
        /// <param name="targetMember">Target member</param>
        /// <returns>Matched source member</returns>
        internal static IMemberMetadata MatchSourceMember(this IEnumerable<IMemberMetadata> sourceMembers, IEnumerable<MappingAttribute> mappingAttributes, IMemberMetadata targetMember)
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
        /// <param name="members">Collection of source members</param>
        /// <param name="targetMember">Target member</param>
        /// <returns>Matched source member</returns>
        internal static IMemberMetadata MatchSourceMember(this IEnumerable<IMemberMetadata> members, IMemberMetadata targetMember)
        {
            return members.FirstOrDefault(member => member?.Name == targetMember?.Name);
        }
        /// <summary>
        /// Method searches for target member that matches <see cref="MappingAttribute.Target"/> value. This <see cref="MappingAttribute"/> also should match the target member.
        /// </summary>
        /// <param name="targetMembers">Collection of target members</param>
        /// <param name="mappingAttributes">Collection of all defined mapping attributes</param>
        /// <param name="targetMember">Target member</param>
        /// <returns>Matched target member</returns>
        internal static IMemberMetadata MatchTargetMember(this IEnumerable<IMemberMetadata> targetMembers, IEnumerable<MappingAttribute> mappingAttributes, IMemberMetadata targetMember)
        {
            var matchedAttribute = mappingAttributes.MatchTargetAttribute(targetMember);
            var matchedTargetMember = targetMembers.FirstOrDefault(member => member?.Name == matchedAttribute?.Target);
            return matchedTargetMember is not null ? matchedTargetMember : targetMember;
        }
    }
}
