namespace Chipsoft.Assignments.EPDConsole.ApplicationCore.Domain.Models
{
    public class Physician : Person
    {
        public Physician()
        {

        }

        public Physician(string firstName, string lastName, string birthdate) : base(firstName, lastName, birthdate)
        {
        }

        public override string ToString() => $"Dr. {FullName} ({Birthdate})";
    }
}
