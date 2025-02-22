using Chipsoft.Assignments.EPDConsole.ApplicationCore.Domain.Models;

namespace Chipsoft.Assignments.EPDConsole.Infrastructure.Interfaces
{
    public interface IAppointmentRepository
    {
        public Task<Appointment> CreateAppointment(Appointment appointment);
        public Task<IEnumerable<Appointment>> GetAllAppointments();
    }
}
