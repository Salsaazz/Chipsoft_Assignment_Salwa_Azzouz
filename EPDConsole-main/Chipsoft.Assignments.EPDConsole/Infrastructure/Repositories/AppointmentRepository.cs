using Chipsoft.Assignments.EPDConsole.Domain.Models;
using Chipsoft.Assignments.EPDConsole.Infrastructure.Context;
using Chipsoft.Assignments.EPDConsole.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chipsoft.Assignments.EPDConsole.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly EPDDbContext dbContext;

        public AppointmentRepository(EPDDbContext dbContext) => this.dbContext = dbContext;


        public async Task<Appointment> CreateAppointment(Appointment appointment)
        {
            await dbContext.Appointments.AddAsync(appointment);
            await dbContext.SaveChangesAsync();
            return appointment;
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointments()
        {
            return await dbContext.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Physician)
                .ToListAsync();
        }
    }
}
