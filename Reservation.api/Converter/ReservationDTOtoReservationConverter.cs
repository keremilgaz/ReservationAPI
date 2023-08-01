using Reservation.api.DTO;
using Reservation.api.Entities;

namespace Reservation.api.Converter
{
    public class ReservationDTOtoReservationConverter
    {
        private ReservationDTO reservation;
        public ReservationDTOtoReservationConverter(ReservationDTO request)
        {
            this.reservation = request;
        }
        public ReservationEntity Convert() 
        {
            var item = new ReservationEntity();
            double dayHour = reservation.Hour + reservation.Duration;
            item.Name = reservation.Name;
            item.Hour = reservation.Hour;
            item.StartDate = reservation.StartDate;
            item.Duration = reservation.Duration;
            item.EndDate = reservation.StartDate.AddHours(item.Duration);
            return item;
        }
    }
}
