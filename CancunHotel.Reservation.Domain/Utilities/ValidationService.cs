using CancunHotel.Reservation.Domain.Exceptions;
using System;

namespace CancunHotel.Reservation.Domain.Utilities
{
    public class ValidationService : IValidationService
    {
        private readonly IClock clock;

        public ValidationService(IClock clock)
        {
            this.clock = clock;
        }

        public void ValidateDateRange(DateTime fromDate, DateTime toDate)
        {
            if (fromDate >= toDate)
            {
                throw new ValidationException("Invalid date range.");
            }

            var utcToday = clock.UtcNow.Date;
            if (utcToday.AddDays(1) > fromDate)
            {
                throw new ValidationException("Reservations start at least the next day of booking.");
            }

            if (utcToday.AddDays(30) < fromDate)
            {
                throw new ValidationException("The room cannot be reserved more than 30 days in advance.");
            }

            if ((toDate - fromDate).TotalDays > 3)
            {
                throw new ValidationException("The stay cannot be longer than 3 days.");
            }
        }
    }
}
