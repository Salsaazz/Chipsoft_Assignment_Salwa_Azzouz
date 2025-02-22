using Chipsoft.Assignments.EPDConsole.ApplicationCore.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Chipsoft.Assignments.EPDConsole.Infrastructure.Context
{
    public class EPDDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source=epd.db");

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Physician> Physicians { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

    }
}
