using System.ComponentModel.DataAnnotations;

namespace Chipsoft.Assignments.EPDConsole.ApplicationCore.Application.DTO_s
{
    public class CreateAppointment
    {
        [Required]
        public readonly string date;
        public readonly string? reason;

        protected CreateAppointment() { }

        public CreateAppointment(string date, string? reason)
        {
            this.date = date;
            this.reason = reason;
        }
    }
}
