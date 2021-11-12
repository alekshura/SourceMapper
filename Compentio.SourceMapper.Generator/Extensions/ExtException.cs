using System;
using System.Runtime.Serialization;

namespace Compentio.SourceMapper.Extensions
{
    [Serializable]
    public class ExtException : Exception
    {
        private readonly string _stackTrace = string.Empty;

        public ExtException(string message, string stackTrace) : base(message)
        {
            _stackTrace = stackTrace;
        }

        protected ExtException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {

        }

        public override string StackTrace
        {
            get
            {
                if (!string.IsNullOrEmpty(_stackTrace))
                    return _stackTrace;

                return StackTrace;
            }
        }
    }
}