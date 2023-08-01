using Microsoft.EntityFrameworkCore;
using Reservation.api.DTO;
using Reservation.api.Entities;
using System.Collections.Generic;

namespace Reservation.api.DatabaseContext
{
    public class ReservationDatabaseContext : DbContext
    {
        public DbSet<ReservationEntity> Reservations { get; set; }

        public ReservationDatabaseContext(DbContextOptions<ReservationDatabaseContext> opt) : base(opt)
        {

        }
    }
}
