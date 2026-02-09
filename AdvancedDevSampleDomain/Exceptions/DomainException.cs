using System;

namespace AdvancedDevSample.Domain.Exceptions
{
    /// <summary>
    /// Exception métier générique pour le domaine.
    /// </summary>
    public class DomainException : System.Exception
    {
        public DomainException(string message) : base(message) { }

        public DomainException(string message, System.Exception innerException) : base(message, innerException) { }
    }
}
