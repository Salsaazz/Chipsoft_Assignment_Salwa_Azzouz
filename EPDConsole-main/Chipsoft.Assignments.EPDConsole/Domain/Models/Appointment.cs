using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Chipsoft.Assignments.EPDConsole.Domain.Models
{
    public class Appointment
    {
        [Required]
        public int Id { get; init; }
        [Required]
        private DateTime date;
        public readonly string? reason;

        // Ef Core one-to-many relations with physician and patient
        [ForeignKey(nameof(Physician))]
        public int PhysicianId { get; set; }

        [ForeignKey(nameof(Patient))]
        public int PatientId { get; set; }

        [InverseProperty(nameof(Person.Appointments))]
        public Physician Physician { get; set; } = null;

        [InverseProperty(nameof(Person.Appointments))]
        public Patient Patient { get; set; } = null;


        protected Appointment() { }

        public Appointment(Physician physician, Patient patient, string date, string? reason)
        {
            Physician = physician;
            Patient = patient;
            Date = date;
            this.reason = reason;
        }

        public string Date
        {

            get => date.ToString("dd-MM-yyyy HH:mm");
            set
            {
                var format = "dd-MM-yyyy HH:mm";
                bool isDateInputCorrect = DateTime.TryParseExact(value, format,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out date);

                if (!isDateInputCorrect)
                    throw new ArgumentException("ongeldige datum invoer. Gebruik het formaat dag-maand-jaar uur:minuut.");

                if (date <= DateTime.Now)

                    throw new ArgumentException("datum kan niet in het verleden liggen.");
            }
        }

        public override string ToString()
        {
            return $"afspraak {Physician} met patient {Patient} - {date:dd-MM-yyyy} om {date:HH:mm}";
        }
    }
}
