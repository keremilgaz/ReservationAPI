using System.ComponentModel.DataAnnotations;

namespace Reservation.api.Entities.ValidationAttributes
{
    public class ReservationHoursIsValidAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid
                  (object value, ValidationContext validationContext)
        {
            ReservationEntity reservation = value as ReservationEntity;
            if (0 > reservation.Hour || reservation.Hour > 23) //24-0 aralığında girilmeli
            {
                return new ValidationResult("Gün içi saatler değiş");
            }
            if ((reservation.StartDate.DayOfWeek == DayOfWeek.Saturday) || (reservation.StartDate.DayOfWeek == DayOfWeek.Sunday))//Haftasonu 
            {
                return new  ValidationResult("Haftasonu"); 
            }
            if (reservation.Duration > 4)
            {
                return new  ValidationResult("4 saat");
            }
            if (reservation.StartDate.Hour > 17 || reservation.StartDate.Hour < 9 || reservation.EndDate.Hour > 17 || reservation.EndDate.Hour < 9) //mesai saatleri dışı
            {
                return new ValidationResult("Gün içi saatler değiş");
            }
            return ValidationResult.Success; 
        }

    }
}
