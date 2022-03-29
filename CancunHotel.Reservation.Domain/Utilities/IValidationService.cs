using System;

namespace CancunHotel.Reservation.Domain.Utilities
{
    public interface IValidationService
    {
        void ValidateDateRange(DateTime fromDate, DateTime toDate);
    }
}
