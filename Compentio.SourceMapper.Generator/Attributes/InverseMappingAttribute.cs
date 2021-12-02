using System;

namespace Compentio.SourceMapper.Attributes
{
    /// <summary>
    /// Attribute that allow to create inverse method mapping.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class InverseMappingAttribute : Attribute
    {
        /// <summary>
        /// Name for the inverse method
        /// </summary>
        public virtual string InverseMethodName { get; set; } = string.Empty;
    }
}