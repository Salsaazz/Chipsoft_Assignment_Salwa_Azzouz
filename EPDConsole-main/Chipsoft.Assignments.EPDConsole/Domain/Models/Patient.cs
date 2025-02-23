using Chipsoft.Assignments.EPDConsole.Domain.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Chipsoft.Assignments.EPDConsole.Domain.Models
{
    public class Patient : Person
    {
        [Required]
        private string mobileNumber;
        [Required]
        private string address;
        public string MobileNumber
        {
            get => mobileNumber;
            set
            {

                if (value.IsPhoneNumberValid())
                    mobileNumber = value;
                else
                    throw new ArgumentException("ongeldige gsm nummer invoer. Gebruik het formaat +landcode nummer.");

            }
        }
        public string? Email { get; set; }
        public int? PersonalIdentityNumber { get; set; }

        public string Address
        {
            get => address;
            set
            {
                if (value.IsAddressValid())
                    address = value;

                else throw new ArgumentException("ongeldige adres invoer.");
            }
        }

        protected Patient()
        {

        }

        public Patient(string firstName, string lastName, string address, string birthdate, string number) : base(firstName, lastName, birthdate)
        {
            Address = address;
            MobileNumber = number;
        }
    }
}
