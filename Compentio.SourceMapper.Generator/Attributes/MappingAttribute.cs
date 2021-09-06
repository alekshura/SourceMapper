using System;

namespace Compentio.SourceMapper.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class MappingAttribute : Attribute
    {
        public string Source { get; set; } = string.Empty;
        public string Target { get; set; } = string.Empty;


        public MappingAttribute()
        {
        }
    }
}
