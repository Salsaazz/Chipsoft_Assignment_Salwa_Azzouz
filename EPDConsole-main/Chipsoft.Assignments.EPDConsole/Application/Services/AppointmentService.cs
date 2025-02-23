using Chipsoft.Assignments.EPDConsole.Application.Interfaces;
using Chipsoft.Assignments.EPDConsole.ApplicationCore.Application.DTO_s;
using Chipsoft.Assignments.EPDConsole.Domain.Models;
using Chipsoft.Assignments.EPDConsole.Infrastructure.Context;
using Chipsoft.Assignments.EPDConsole.Infrastructure.Interfaces;
using Chipsoft.Assignments.EPDConsole.Infrastructure.Repositories;

namespace Chipsoft.Assignments.EPDConsole.Application.Services
{
    internal class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository appointmentRepository;

        public AppointmentService(EPDDbContext dbContext)
        {
            appointmentRepository = new AppointmentRepository(dbContext);
        }

        public async Task<Appointment> AddAppointment(Patient patient, Physician physician, CreateAppointment appointmentDTO)
        {
            Appointment appointment = AppointmentDTOtoModel(appointmentDTO, physician, patient);
            return await appointmentRepository.CreateAppointment(appointment);
        }

        public async Task<IEnumerable<Appointment?>> GetAllAppointments()
        {
            IEnumerable<Appointment?> appointments = await appointmentRepository.GetAllAppointments();
            if (!appointments.Any())
                throw new InvalidOperationException("er zijn geen afspraken.");

            return appointments;
        }

        private static Appointment AppointmentDTOtoModel(CreateAppointment appointmentDTO, Physician physician, Patient patient) => new(physician, patient, appointmentDTO.date, appointmentDTO.reason);
    }
}
