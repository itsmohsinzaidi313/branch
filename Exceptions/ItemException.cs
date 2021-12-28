using System;

namespace Branch.Exceptions
{
    internal class ItemException : Exception
    {
        internal ItemException()
        {

        }
        internal ItemException(string message) : base(message)
        {

        }
        internal ItemException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
