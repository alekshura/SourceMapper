using System;
using System.Runtime.Serialization;

namespace Compentio.SourceMapper.Extensions
{
    [Serializable]
    public class DependencyInjectionException : Exception
    {
        private readonly string _stackTrace = string.Empty;

        public DependencyInjectionException(string message, string stackTrace) : base(message)
        {
            _stackTrace = stackTrace;
        }

        protected DependencyInjectionException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
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