using Chipsoft.Assignments.EPDConsole.Application.Interfaces;
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

            IEnumerable<Patient?> findPerson = await GetPerson(person.FullName, person.Birthdate);

            if (findPerson.Any())
                throw new InvalidOperationException("deze patient is al geregistreerd.");

            return await patientRepository.CreatePerson(person);
        }

        public async Task<Patient> DeletePerson(Patient person)
        {
            try
            {
                await patientRepository.DeletePerson(person!);
                return person;
            }
            catch (Exception)
            {
                throw new InvalidOperationException("fout bij het verwijderen van de patient.");
            }
        }

        public async Task<IEnumerable<Patient?>> GetPerson(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("naamveld kan niet leeg zijn.");

            IEnumerable<Patient?> result = await patientRepository.GetPerson(name!.ToLower());

            if (!result.Any())
                throw new InvalidOperationException($"patient {name} is niet geregistreerd in het systeem.");

            return result;
        }

        protected async Task<IEnumerable<Patient?>> GetPerson(string name, string birthdate) => await patientRepository.GetPerson(name, birthdate);

        public void FormatPerson(Patient person)
        {
            person.FirstName = person.FirstName.ToLower();
            person.LastName = person.LastName.ToLower();
            person.FirstName = char.ToUpper(person.FirstName[0]) + person.FirstName.Substring(1).Trim();
            person.LastName = char.ToUpper(person.LastName[0]) + person.LastName.Substring(1).Trim();
            person.Address = person.Address.Trim();
        }
    }
}