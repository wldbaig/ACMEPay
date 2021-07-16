using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace API.Common
{
    public class InvalidModelException : System.Exception
    {
        public InvalidModelException() : base() { }
        public InvalidModelException(string message) : base(message) { }
        public InvalidModelException(string message, System.Exception innerException) : base(message, innerException) { }
        protected InvalidModelException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }

    public class InvalidFormatException : System.Exception
    {
        public InvalidFormatException() : base() { }
        public InvalidFormatException(string message) : base(message) { }
        public InvalidFormatException(string message, System.Exception innerException) : base(message, innerException) { }
        protected InvalidFormatException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
