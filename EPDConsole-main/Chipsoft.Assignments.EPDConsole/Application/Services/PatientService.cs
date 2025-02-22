using Chipsoft.Assignments.EPDConsole.Application.Interfaces;
using Chipsoft.Assignments.EPDConsole.Domain.Extensions;
using Chipsoft.Assignments.EPDConsole.Domain.Models;
using Chipsoft.Assignments.EPDConsole.Infrastructure.Interface;

namespace Chipsoft.Assignments.EPDConsole.Application.Services
{
    public class PatientService : IPersonService<Patient>
    {
        private readonly IPersonRepository<Patient> patientRepository;

        public PatientService(IPersonRepository<Patient> patientRepository)
        {
            this.patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        }

        public async Task<Patient> AddPerson(Patient person)
        {
            FormatPerson(person);

            var findPerson = await GetPerson(person.FullName, person.Birthdate);
            if (findPerson?.Count() > 0)
                throw new InvalidOperationException("Deze patient is al geregistreerd.");

            return await patientRepository.CreatePerson(person);
        }
        public async Task<Patient> DeletePerson(Patient person) => await patientRepository.DeletePerson(person!) ?? throw new NullReferenceException("Deze patient bestaat niet in het systeem.");

        public async Task<IEnumerable<Patient?>> GetPersonByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new NullReferenceException("De naam kan niet leeg zijn.");

            name.CapitalizeFirstLetterOfWords();
            IEnumerable<Patient?> result = await patientRepository.GetPerson(name!.ToLower());

            if (!result.Any())
                throw new NullReferenceException($"Patient {name} is nog niet geregistreerd in het systeem. Registreer eerst de patient.");

            return result;
        }

        protected async Task<IEnumerable<Patient?>> GetPerson(string name, string birthdate) => await patientRepository.GetPerson(name, birthdate);

        public void FormatPerson(Patient person)
        {
            person.FirstName = person.FirstName.ToLower();
            person.LastName = person.LastName.ToLower();
            person.FirstName = char.ToUpper(person.FirstName[0]) + person.FirstName.Substring(1).Trim();
            person.LastName = char.ToUpper(person.LastName[0]) + person.LastName.Substring(1).Trim();
        }
    }
}
