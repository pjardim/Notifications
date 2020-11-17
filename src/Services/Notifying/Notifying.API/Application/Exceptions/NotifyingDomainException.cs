using System;

namespace Notifying.API.Application.Exceptions
{
    /// <summary>
    /// Exception type for domain exceptions
    /// </summary>
    public class NotifyingDomainException : Exception
    {
        public NotifyingDomainException()
        { }

        public NotifyingDomainException(string message)
            : base(message)
        { }

        public NotifyingDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}