using CancunHotel.Reservation.Application;
using CancunHotel.Reservation.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CancunHotel.Reservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomApplication application;

        public RoomController(IRoomApplication application)
        {
            this.application = application;
        }

        [HttpPost("check-availability")]
        public async Task<RoomAvailabilityResponse> CheckAvailabilityByDateRange([FromBody, Required] CreateReservation reservation)
        {
            return await application.CheckAvailabilityByDateRange(reservation.FromDate, reservation.ToDate);
        }
    }
}
