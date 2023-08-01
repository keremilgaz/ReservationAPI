using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservation.api.Converter;
using Reservation.api.DatabaseContext;
using Reservation.api.DTO;
using Reservation.api.Entities;

namespace Reservation.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : Controller
    {
        private readonly ReservationDatabaseContext _RezContext;
        public ReservationController(ReservationDatabaseContext rezContext)
        {
            _RezContext = rezContext;
        }

        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] NewReservationDTO request)
        //{
        //    var item = new NewReservationDTO();
        //    if (ReservationCheckOkay(request) )
        //    {
        //        item.Hour = request.Hour;
        //        item.StartDate = request.StartDate;
        //        item.Duration = request.Duration;
        //        _RezContext.Add(item);
        //        await _RezContext.SaveChangesAsync();
        //    }
        //    return NotFound();
        //}

        [HttpPost]
        [Route("Post")]
        public async Task<IActionResult> Post([FromBody] ReservationDTO reservation)
        {
            var converter = new ReservationDTOtoReservationConverter(reservation);
            var item =  converter.Convert();


            if(0 > reservation.Hour || reservation.Hour > 23) //24-0 aralığında girilmeli
            {
                return NotFound();
            }
            if((reservation.StartDate.DayOfWeek == DayOfWeek.Saturday) || (reservation.StartDate.DayOfWeek == DayOfWeek.Sunday))//Haftasonu 
            {
                return NotFound();
            }
            if(reservation.Duration > 4)
            {
                return NotFound();
            }
            if(item.StartDate.Hour > 17 || item.StartDate.Hour < 9 || item.EndDate.Hour > 17 || item.EndDate.Hour < 9) //mesai saatleri dışı
            {
                return NotFound();
            }
            await _RezContext.Reservations.AddAsync(item);
            await _RezContext.SaveChangesAsync();
            return Ok("Rezervasyon başarılı");
        }



    }

}
