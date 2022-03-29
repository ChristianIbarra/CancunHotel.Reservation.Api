using System;

namespace CancunHotel.Reservation.Domain.Exceptions
{
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException(object resourceId, string domainName, Exception exception = null)
            : base($"{domainName} not found for id {resourceId}", exception)
        {

        }
    }
}
