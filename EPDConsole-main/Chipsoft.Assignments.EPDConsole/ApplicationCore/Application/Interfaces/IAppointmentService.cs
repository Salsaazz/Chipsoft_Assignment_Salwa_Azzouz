using Chipsoft.Assignments.EPDConsole.ApplicationCore.Application.DTO_s;
using Chipsoft.Assignments.EPDConsole.ApplicationCore.Domain.Models;

namespace Chipsoft.Assignments.EPDConsole.ApplicationCore.Application.Interfaces
{
    public interface IAppointmentService
    {
        public Task<Appointment> AddAppointment(Patient patient, Physician physician, CreateAppointment appointmentDTO);
        public Task<IEnumerable<Appointment?>> GetAllAppointments();
    }
}
