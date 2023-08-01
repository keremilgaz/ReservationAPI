using Reservation.api.Entities;
using Reservation.api.Entities.ValidationAttributes;
using System;

namespace Reservation.api.DTO
{
    public class ReservationDTO : BaseDTO
    {
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public int Hour { get; set; }
        public double Duration { get; set; }
        public DateTime EndDate { get; set; }

        public ReservationDTO()
        {
            // Default constructor
        }

        public ReservationEntity ToReservationEntity()
        {
            return new ReservationEntity
            {
                Name = this.Name,
                StartDate = this.StartDate,
                Hour = this.Hour,
                Duration = this.Duration
                
            };
        }
    }
}
