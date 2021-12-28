using System;

namespace Branch.Exceptions.OrderExceptions
{
    internal class OrderSaveException : Exception
    {
        internal OrderSaveException()
        {

        }
        internal OrderSaveException(string message) : base(message)
        {

        }
        internal OrderSaveException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
