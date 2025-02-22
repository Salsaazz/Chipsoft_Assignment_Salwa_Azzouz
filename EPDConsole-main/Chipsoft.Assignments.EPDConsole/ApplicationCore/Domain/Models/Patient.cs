using Chipsoft.Assignments.EPDConsole.ApplicationCore.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Chipsoft.Assignments.EPDConsole.ApplicationCore.Domain.Models
{
    public class Patient : Person
    {
        [Required]
        private string phoneNumber;
        [Required]
        public string Address { get; set; }
        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {

                if (value.IsPhoneNumberValid())
                    phoneNumber = value;
                else
                    throw new ArgumentException("Foutieve telefoonummer invoer. Gebruik het formaat +landcode nummer.");

            }
        }
        public string? Email { get; set; }
        public int? BtwNumber { get; set; }

        protected Patient()
        {

        }

        public Patient(string firstName, string lastName, string address, string birthdate, string number) : base(firstName, lastName, birthdate)
        {
            Address = address;
            PhoneNumber = number;
        }
    }
}
