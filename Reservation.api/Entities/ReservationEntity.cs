using Reservation.api.Entities.ValidationAttributes;

namespace Reservation.api.Entities
{
 
    public class ReservationEntity : BaseEntity
    {
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public int Hour { get; set; }
        public double Duration { get; set; }
        public DateTime EndDate { get; set; }

        //public ReservationEntity(string name,DateTime startDate, int Hour, double duration)
        //{
        //    Name = name;
        //    StartDate = startDate.Date.AddHours(Hour);
        //    EndDate = startDate.AddHours(duration);
        //}

        //public ReservationEntity() 
        //{
        //    Name = string.Empty;
        //    Hour = 0;
        //    StartDate = DateTime.MinValue;
        //    EndDate = DateTime.MinValue;

        //}
    }
}
