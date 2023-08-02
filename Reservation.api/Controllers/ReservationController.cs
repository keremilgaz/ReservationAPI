using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservation.api.Converter;
using Reservation.api.DatabaseContext;
using Reservation.api.DTO;
using Reservation.api.Entities;
using System.Collections;

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
                return Conflict("23-0 aralığında bir saat değeri girilmeli.");
            }
            if((reservation.StartDate.DayOfWeek == DayOfWeek.Saturday) || (reservation.StartDate.DayOfWeek == DayOfWeek.Sunday))//Haftasonu 
            {
                return Conflict("Haftasonu rezervasyon alınamaz.");
            }
            if(reservation.Duration > 4)
            {
                return Conflict("3 Saatten uzun randevu olamaz");
            }
            if(item.StartDate.Hour > 17 || item.StartDate.Hour < 9 || item.EndDate.Hour > 17 || item.EndDate.Hour < 9) //mesai saatleri dışı
            {
                return Conflict("Mesai Saatleri Dışında.");
            }
            foreach (var existingReservation in _RezContext.Reservations)
            {
                if (item.StartDate.Ticks < existingReservation.EndDate.Ticks && existingReservation.StartDate.Ticks < item.EndDate.Ticks)
                    //https://stackoverflow.com/questions/5672862/check-if-datetime-instance-falls-in-between-other-two-datetime-objects If bloğunun içi bu siteden alındı
                {
                    return Conflict("Rezervasyon aralığı başka bir rezervasyon ile çakışıyor.");
                }
            }
            await _RezContext.Reservations.AddAsync(item);
            await _RezContext.SaveChangesAsync();
            return Ok("Rezervasyon başarılı");
        }

        [HttpDelete]
        [Route("Delete")]

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _RezContext.Reservations.SingleOrDefaultAsync(x => x.Id == id);
            if (item != null)
            {
                _RezContext.Remove(item);
                await _RezContext.SaveChangesAsync();
                return NoContent();
            }
            return NotFound();
        }

        [HttpGet]
        [Route("GetAllReservations")]

        public async Task<IActionResult> GetAll()
        {
            var result = await _RezContext.Reservations.
                Include(Reservation => Reservation.Name).
                Include(Reservation => Reservation.StartDate).
                Include(Reservation => Reservation.EndDate).
                ToListAsync();

            if (result == null)
                return NotFound();

            var ReservationlList = new List<ReservationDTO>();
            result.ForEach(Reservation => ReservationlList.Add(new ReservationDTO(Reservation)));
            return Ok(ReservationlList);
        }

    }

}
