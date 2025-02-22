using Chipsoft.Assignments.EPDConsole.Domain.Models;

namespace Chipsoft.Assignments.EPDConsole.Infrastructure.Interface
{
    public interface IPersonRepository<T> where T : Person
    {
        public Task<T> CreatePerson(T person);
        public Task DeletePerson(T person);
        public Task<IEnumerable<T?>> GetPerson(string name);
        public Task<IEnumerable<T?>> GetPerson(string name, string birthdate);
    }
}
