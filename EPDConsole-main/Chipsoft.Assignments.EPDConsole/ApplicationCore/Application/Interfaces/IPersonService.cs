using Chipsoft.Assignments.EPDConsole.ApplicationCore.Domain.Models;

namespace Chipsoft.Assignments.EPDConsole.ApplicationCore.Application.Interfaces
{
    public interface IPersonService<T> where T : Person
    {
        public Task<IEnumerable<T?>> GetPersonByName(string name);
        public Task<T> AddPerson(T person);
        public Task<T> DeletePerson(T person);
        public void FormatPerson(T person);
    }
}
