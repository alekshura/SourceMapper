using System;

namespace Compentio.SourceMapper.Attributes
{
    /// <summary>
    /// Mark abstract class of interface with this attribute to define mapper
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Struct)]
    public class MapperAttribute : Attribute
    {
        /// <summary>
        /// Name of generated mapper class. If it i empty default name generated:
        /// <list type="bullet">
        /// <item>
        /// <description>for interface name started with 'I', class name will be generated next way <c>INotesMapper</c> -> <c>NotesMapper</c></description>
        /// </item>
        /// <item>
        /// <description>for abstract class or interface without 'I' class name will be generated <c>NotesMapper</c> -> <c>NotesMapperImpl</c></description>
        /// </item>
        /// </list>
        /// </summary>
        public string ClassName { get; set; } = string.Empty;
    }
}
