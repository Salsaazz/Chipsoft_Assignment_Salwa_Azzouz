namespace Chipsoft.Assignments.EPDConsole.Domain.Models
{
    public class Physician : Person
    {
        protected Physician()
        {

        }

        public Physician(string firstName, string lastName, string birthdate) : base(firstName, lastName, birthdate)
        {
        }

        public override string ToString() => $"Dr. {FullName} ({Birthdate})";
    }
}
