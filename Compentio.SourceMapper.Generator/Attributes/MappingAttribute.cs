﻿using System;

namespace Compentio.SourceMapper.Attributes
{
    /// <summary>
    /// Attirbute that defines the mapping rules for method first parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class MappingAttribute : Attribute
    {
        /// <summary>
        /// The name of the source property used for mapping. 
        /// </summary>
        public string Source { get; set; } = string.Empty;
        /// <summary>
        /// Te name for target property for mapping. Use it like: <c>nameof(ReturnType.PropertyName)</c>
        /// </summary>
        /// <example>nameof(ReturnType.PropertyName)</example>
        public string Target { get; set; } = string.Empty;
        /// <summary>
        /// Func name that can be used for additional mapping processing.
        /// Use it in classes. In interface mapping definitions it will be ignored.
        /// </summary>
        public string Expression { get; set; } = string.Empty;
        /// <summary>
        /// Create reverse mapping for the method.
        /// </summary>
        public bool CreateInverse { get; set; } = false;
        /// <summary>
        /// Name of the new method for inverse mapping mechanism
        /// </summary>
        public string InverseMethodName { get; set; } = string.Empty;
        /// <summary>
        /// Default constructor
        /// </summary>
        public MappingAttribute()
        {
        }
    }
}
