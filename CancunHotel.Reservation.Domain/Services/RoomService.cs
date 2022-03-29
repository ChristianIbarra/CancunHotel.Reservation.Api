using CancunHotel.Reservation.Domain.Utilities;
using System;
using System.Threading.Tasks;

namespace CancunHotel.Reservation.Domain.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository repository;
        private readonly IValidationService validationService;

        public RoomService(IRoomRepository repository, IValidationService validationService)
        {
            this.repository = repository;
            this.validationService = validationService;
        }

        public async Task<bool> CheckAvailabilityByDateRange(DateTime fromDate, DateTime toDate)
        {
            validationService.ValidateDateRange(fromDate, toDate);
            return await repository.IsRoomAvailableOnDateRange(fromDate, toDate);
        }
    }
}
