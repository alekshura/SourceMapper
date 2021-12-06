using System;

namespace Compentio.SourceMapper.Attributes
{
    /// <summary>
    /// Attribute that ignore field/property/method during mapping creating
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = false)]
    public class IgnoreMapping : Attribute
    {
    }
}