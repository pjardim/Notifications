using System;
using System.Runtime.Serialization;

namespace Subscribing.Domain
{
    [Serializable]
    internal class NotifyingDomainException : Exception
    {
        public NotifyingDomainException()
        {
        }

        public NotifyingDomainException(string message) : base(message)
        {
        }

        public NotifyingDomainException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotifyingDomainException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}