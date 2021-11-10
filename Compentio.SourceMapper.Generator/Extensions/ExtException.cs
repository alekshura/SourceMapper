using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Compentio.SourceMapper.Extensions
{
    public class ExtException : Exception
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
