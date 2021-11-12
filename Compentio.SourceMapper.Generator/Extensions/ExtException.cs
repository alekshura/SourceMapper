using System;
using System.Runtime.Serialization;

namespace Compentio.SourceMapper.Extensions
{
    public class ExtException : Exception, ISerializable
    {
        private readonly string _stackTrace;

        public ExtException(string message, string stackTrace) : base(message)
        {
            _stackTrace = stackTrace;
        }

        public override string StackTrace
        {
            get
            {
                return _stackTrace;
            }
        }
    }
}