using Microsoft.CodeAnalysis;

namespace Compentio.SourceMapper.Metadata
{
    /// <summary>
    /// Main interface for all metadata types
    /// </summary>
    interface IMetadata
    {
        /// <summary>
        /// Location of the object in the code
        /// </summary>
        Location? Location { get; }
        /// <summary>
        /// Name of the object that is mapped
        /// </summary>
        string Name { get; }
    }  
}
