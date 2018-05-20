using System;
using System.Runtime.Serialization;

namespace MiniMaster.RessourceScheduling
{
    [Serializable]
    public class InsufficientAcolytesException : Exception
    {
        public InsufficientAcolytesException()
        {
        }

        public InsufficientAcolytesException(string message) : base(message)
        {
        }

        public InsufficientAcolytesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InsufficientAcolytesException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}