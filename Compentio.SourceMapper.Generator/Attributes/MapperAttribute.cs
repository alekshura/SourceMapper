using System;

namespace Compentio.SourceMapper.Attributes
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Struct)]
    public class MapperAttribute : Attribute
    {
        public string ClassName { get; set; } = string.Empty;
    }
}
