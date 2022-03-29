using CancunHotel.Reservation.Application;
using CancunHotel.Reservation.Application.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CancunHotel.Reservation.Api.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationApplication application;

        public ReservationsController(IReservationApplication application)
        {
            this.application = application;
        }

        [HttpGet("{reservationId:int}", Name = nameof(GetReservation))]
        public async Task<Application.DTOs.Reservation> GetReservation(int reservationId)
        {
            return await application.GetReservation(reservationId);
        }

        [HttpGet]
        public async Task<IList<Application.DTOs.Reservation>> ListUserReservations()
        {
            return await application.ListUserReservations();
        }

        [HttpPost]
        public async Task<ActionResult> CreateReservation([FromBody, Required] CreateReservation reservation)
        {
            var reservationId = await application.PlaceReservation(reservation);

            return CreatedAtRoute(nameof(GetReservation), new { reservationId }, null);
        }

        [HttpDelete("{reservationId:int}")]
        public async Task CancelReservation(int reservationId)
        {
            await application.CancelReservation(reservationId);
        }

        [HttpPut("{reservationId:int}")]
        public async Task EditReservation(int reservationId, [FromBody, Required] CreateReservation reservation)
        {
            await application.EditReservation(reservationId, reservation);
        }
    }
}
