using Chipsoft.Assignments.EPDConsole.Common.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Chipsoft.Assignments.EPDConsole.ApplicationCore.Domain.Models
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
                    throw new ArgumentException("Foutieve voornaam invoer.");
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
                    throw new ArgumentException("Foutieve achternaam invoer.");
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
                    throw new ArgumentException("Foutieve geboortedatum invoer. Gebruik het formaat dag-maand-jaar.");

                if (birthdate > DateTime.Now)

                    throw new ArgumentException("Geboortedatum kan niet in de toekomst liggen.", nameof(value));
            }
        }

        [NotMapped]
        public string FullName { get => $"{firstName} {lastName}"; }

        public override string ToString() => $"{FullName} ({Birthdate})";
    }
}
