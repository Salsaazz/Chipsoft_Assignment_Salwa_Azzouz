using Chipsoft.Assignments.EPDConsole.Application.Interfaces;
using Chipsoft.Assignments.EPDConsole.Domain.Models;
using Chipsoft.Assignments.EPDConsole.Infrastructure.Interface;

namespace Chipsoft.Assignments.EPDConsole.Application.Services
{
    public class PhysicianService : IPersonService<Physician>
    {
        private readonly IPersonRepository<Physician> physicianRepository;

        public PhysicianService(IPersonRepository<Physician> physicianRepository)
        {
            this.physicianRepository = physicianRepository ?? throw new ArgumentNullException(nameof(physicianRepository));
        }

        public async Task<Physician> AddPerson(Physician person)
        {
            FormatPerson(person);

            var findPhysician = await GetPerson(person.FullName, person.Birthdate);

            if (!findPhysician.Any())
                throw new InvalidOperationException("Deze arts is al geregistreerd.");

            return await physicianRepository.CreatePerson(person);
        }

        public async Task<Physician> DeletePerson(Physician person)
        {
            try
            {
                await physicianRepository.DeletePerson(person!);
                return person;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Fout bij het verwijderen van de arts.", ex);
            }
        }

        public async Task<IEnumerable<Physician?>> GetPerson(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new NullReferenceException("De naam kan niet leeg zijn.");

            IEnumerable<Physician?> result = await physicianRepository.GetPerson(name.ToLower());

            if (!result.Any())
                throw new NullReferenceException($"Dr. {name} is nog niet geregistreerd in het systeem. Registreer eerst de arts.");

            return result;
        }

        protected async Task<IEnumerable<Physician?>> GetPerson(string name, string birthdate) => await physicianRepository.GetPerson(name, birthdate);

        public void FormatPerson(Physician person)
        {
            person.FirstName = person.FirstName.ToLower();
            person.LastName = person.LastName.ToLower();
            person.FirstName = char.ToUpper(person.FirstName[0]) + person.FirstName.Substring(1).Trim();
            person.LastName = char.ToUpper(person.LastName[0]) + person.LastName.Substring(1).Trim();
        }
    }
}
