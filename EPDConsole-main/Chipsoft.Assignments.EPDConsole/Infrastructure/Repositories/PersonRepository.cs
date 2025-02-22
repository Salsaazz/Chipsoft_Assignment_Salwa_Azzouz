using Chipsoft.Assignments.EPDConsole.ApplicationCore.Domain.Models;
using Chipsoft.Assignments.EPDConsole.Infrastructure.Context;
using Chipsoft.Assignments.EPDConsole.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;

namespace Chipsoft.Assignments.EPDConsole.Infrastructure.Repository
{
    public class PersonRepository<T> : IPersonRepository<T> where T : Person
    {
        private readonly EPDDbContext dbContext;
        private readonly DbSet<T> dbSet;

        public PersonRepository(EPDDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<T>();
        }

        public async Task<T> CreatePerson(T person)
        {
            await dbSet.AddAsync(person);
            await dbContext.SaveChangesAsync();
            return person;
        }

        public async Task<T> DeletePerson(T person)
        {
            dbSet.Remove(person);
            await dbContext.SaveChangesAsync();
            return person;
        }

        public async Task<IEnumerable<T?>> GetPerson(string name) => await dbSet
            .Where(p => (p.FirstName + " " + p.LastName).ToLower().Contains(name))
            .ToListAsync();

        public async Task<IEnumerable<T?>> GetPerson(string name, string birthdate) => await dbSet
            .Where(p => (p.FirstName + " " + p.LastName).ToLower() == name.ToLower() && p.Birthdate == birthdate)
            .ToListAsync();
    }
}
