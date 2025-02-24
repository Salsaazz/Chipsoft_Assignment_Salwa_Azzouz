using Chipsoft.Assignments.EPDConsole.Domain.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Chipsoft.Assignments.EPDConsole.Domain.Models
{
    public abstract class Person
    {
        [Required]
        public int Id { get; init; }
        [Required]
        private string firstName;
        [Required]
        private string lastName;
        [Required]
        private DateTime birthdate;

        // Ef Core one-to-many relation with appointment
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();

        protected Person() { }

        public Person(string firstName, string lastName, string birthdate)
        {
            FirstName = firstName;
            LastName = lastName;
            Birthdate = birthdate;
        }

        public string FirstName
        {
            get => firstName;
            set
            {
                if (value.IsNameValid())
                    firstName = value;
                else
                    throw new ArgumentException("ongeldige voornaam invoer.");
            }
        }

        public string LastName
        {
            get => lastName;
            set
            {
                if (value.IsNameValid())
                    lastName = value;
                else
                    throw new ArgumentException("ongeldige achternaam invoer.");
            }
        }

        public string Birthdate
        {
            get => birthdate.ToString("dd-MM-yyyy");
            set
            {
                var format = "dd-MM-yyyy";
                bool IsDateInputCorrect = DateTime.TryParseExact(value, format,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out birthdate);

                if (!IsDateInputCorrect)
                    throw new ArgumentException("ongeldige geboortedatum invoer. Gebruik het formaat dag-maand-jaar.");

                if (birthdate > DateTime.Now)
                    throw new ArgumentException("ongeldige geboortedatum invoer. Geboortedatum kan niet in de toekomst liggen.");
            }
        }

        [NotMapped]
        public string FullName { get => $"{firstName} {lastName}"; }

        public override string ToString() => $"{FullName} ({Birthdate})";
    }
}
