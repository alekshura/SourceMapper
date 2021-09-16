using System;

namespace Compentio.SourceMapper.Attributes
{
    /// <summary>
    /// Attirbute that defines the mapping rules for method first parameter using func expression.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class MappingWithExpressionAttribute : Attribute
    {
        /// <summary>
        /// Te name for target property for mapping. Use it like: nameof(ReturnType.PropertyName)
        /// </summary>
        /// <example>nameof(ReturnType.PropertyName)</example>
        public string Target { get; set; } = string.Empty;
        /// <summary>
        /// Func name that can be used for additional mapping processing.
        /// Use it in classes. In interface mapping definitions it will be ignored.
        /// </summary>
        public string Expression { get; set; } = string.Empty;

        public MappingWithExpressionAttribute()
        {
        }
    }
}
