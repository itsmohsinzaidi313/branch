using System;

namespace Branch.Exceptions.OrderExceptions
{
    internal class OrderUpdateException : Exception
    {
        internal OrderUpdateException()
        {

        }
        internal OrderUpdateException(string message) : base(message)
        {

        }
        internal OrderUpdateException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
