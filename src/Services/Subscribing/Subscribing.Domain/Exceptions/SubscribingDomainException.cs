using System;
using System.Collections.Generic;
using System.Text;

namespace Subscribing.Domain.Exceptions
{
    /// <summary>
    /// Exception type for domain exceptions
    /// </summary>
    public class SubscribingDomainException : Exception
    {
        public SubscribingDomainException()
        { }

        public SubscribingDomainException(string message)
            : base(message)
        { }

        public SubscribingDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
