using System;

namespace CancunHotel.Reservation.Domain.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string message, Exception exception = null)
            : base(message, exception)
        {

        }
    }
}
