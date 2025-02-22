using Chipsoft.Assignments.EPDConsole.Application.Interfaces;
using Chipsoft.Assignments.EPDConsole.Application.Services;
using Chipsoft.Assignments.EPDConsole.ApplicationCore.Application.DTO_s;
using Chipsoft.Assignments.EPDConsole.Domain.Models;
using Chipsoft.Assignments.EPDConsole.Infrastructure.Context;
using Chipsoft.Assignments.EPDConsole.Infrastructure.Repository;

namespace Chipsoft.Assignments.EPDConsole.Presentation
{
    public class Program
    {

        private readonly static EPDDbContext dbContext;
        private readonly static IPersonService<Patient> patientService;
        private readonly static IPersonService<Physician> physicianService;
        private readonly static IAppointmentService appointmentService;

        static Program()
        {
            dbContext = new EPDDbContext();
            patientService = new PatientService(new PersonRepository<Patient>(dbContext));
            physicianService = new PhysicianService(new PersonRepository<Physician>(dbContext));
            appointmentService = new AppointmentService(dbContext);
        }

        private async static Task AddPatient()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Registratie patient");

                Patient patient = CreatePatient();
                await patientService.AddPerson(patient);
                ConsoleOutputService.ShowSuccess($"Patient {patient.FullName} is geregistreerd.");
            }
            catch (Exception e)
            {
                ConsoleOutputService.ShowError(e.Message);
            }
        }

        private async static Task DeletePatient()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Verwijder patient");
                Console.Write("\t Naam van patient: ");
                var name = Console.ReadLine();

                Patient patient = await GetChosenPatient(name);

                Patient deletedPatient = await patientService.DeletePerson(patient!);

                ConsoleOutputService.ShowSuccess($"Patient: {deletedPatient!.FullName} met id {deletedPatient.Id} is verwijderd.");
            }
            catch (Exception e)
            {
                ConsoleOutputService.ShowError(e.Message);
            }
        }

        private async static Task DeletePhysician()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Verwijderen van arts");
                Console.Write("\t Naam van arts: ");
                var name = Console.ReadLine();

                Physician physician = await GetChosenPhysician(name!);

                var deletedPhysician = await physicianService.DeletePerson(physician!);

                ConsoleOutputService.ShowSuccess($"Arts: {deletedPhysician!.FullName} met id {deletedPhysician.Id} is verwijderd.");
            }
            catch (Exception e)
            {
                ConsoleOutputService.ShowError(e.Message);
            }
        }

        private async static Task AddPhysician()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Registratie arts");

                var physician = CreatePhysician();
                Physician result = await physicianService.AddPerson(physician);

                ConsoleOutputService.ShowSuccess($"Dr. {physician.FullName} is geregistreerd.");
            }
            catch (Exception e)
            {
                ConsoleOutputService.ShowError(e.Message);
            }
        }

        private async static Task ShowAppointment()
        {
            try
            {
                Console.Clear();

                IEnumerable<Appointment?> appointments = await appointmentService.GetAllAppointments();

                Console.WriteLine();
                Console.WriteLine("Lijst van alle afspraken: ");

                foreach (var appointment in appointments)
                {
                    Console.WriteLine($"\t {appointment!.Id}. {appointment}");
                }

                Console.WriteLine();

                await OptionalAppointmentsFiltering(appointments!);

            }
            catch (Exception e)
            {

                ConsoleOutputService.ShowError(e.Message);
            }
        }

        private async static Task AddAppointment()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Afspraak aanmaken");

                Console.Write("\t Zoek naam van patient: ");
                Patient? chosenPatient = await GetChosenPatient(Console.ReadLine()) ?? throw new ArgumentException("Foutieve patient id. Probeer opnieuw.");

                Console.WriteLine();
                Console.WriteLine();

                Console.Write("\t Zoek naam van arts: ");
                Physician? chosenPhysician = await GetChosenPhysician(Console.ReadLine()!) ?? throw new ArgumentException("Foutieve arts id. Probeer opnieuw.");

                Console.WriteLine();

                Appointment result = await appointmentService.AddAppointment(chosenPatient, chosenPhysician, CreateAppointment());
                ConsoleOutputService.ShowSuccess($"Afspraak is gemaakt voor patient {result} met {result.Physician}");
            }
            catch (Exception e)
            {
                ConsoleOutputService.ShowError(e.Message);
            }
        }

        private static Patient CreatePatient()
        {
            Console.Write("\t Voornaam: ");
            string firstName = Console.ReadLine();

            Console.Write(" \t Achternaam: ");
            string lastName = Console.ReadLine();

            Console.Write(" \t Adres (optioneel): ");
            string address = Console.ReadLine();

            Console.Write(" \t Geboortedatum (vb. 10-02-2003): ");
            string birthdate = Console.ReadLine();

            Console.Write(" \t Telefoonnummer (vb. +32456789123): ");
            string number = Console.ReadLine();

            return new Patient(firstName!, lastName!, address!, birthdate!, number!);
        }

        public static CreateAppointment CreateAppointment()
        {
            Console.Write(" \t Voer de datum en tijdstip in (vb. 10-02-2003 12:30): ");
            string date = Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine();
            Console.Write("\t Reden (optioneel): ");
            string? reason = Console.ReadLine();

            return new CreateAppointment(date!, reason);
        }

        private static Physician CreatePhysician()
        {
            Console.Write("\t Voornaam: ");
            string firstName = Console.ReadLine();

            Console.Write(" \t Achternaam: ");
            string lastName = Console.ReadLine();

            Console.Write(" \t Geboortedatum (vb. 10-02-2003): ");
            string birthdate = Console.ReadLine();

            return new Physician(firstName!, lastName!, birthdate!);
        }

        public async static Task<Patient> GetChosenPatient(string? patientName)
        {

            IEnumerable<Patient?> patients = await patientService.GetPersonByName(patientName!);

            foreach (var patient in patients)
            {
                Console.WriteLine($"\t {patient!.Id}. {patient}");
            }

            Console.WriteLine();

            Console.Write("\t Voer de id van de patient in: ");
            var isUserInputCorrect = int.TryParse(Console.ReadLine(), out int patientId);


            return patients.FirstOrDefault(p => p!.Id == patientId) ?? throw new ArgumentException("foutieve id invoer.");
        }

        public async static Task<Physician> GetChosenPhysician(string physicianName)
        {

            IEnumerable<Physician?> physicians = await physicianService.GetPersonByName(physicianName);

            Console.WriteLine();

            foreach (var physician in physicians)
            {
                Console.WriteLine($"\t {physician!.Id}. {physician}");
            }

            Console.WriteLine();

            Console.Write("\t Voer de id van de arts in: ");

            var isUserInputCorrect = int.TryParse(Console.ReadLine(), out int physicianId);

            return physicians.FirstOrDefault(p => p!.Id == physicianId) ?? throw new ArgumentException("foutieve id invoer.");
        }

        public async static Task OptionalAppointmentsFiltering(IEnumerable<Appointment> appointments)
        {
            Console.WriteLine("Voer cijfer 1 in om te filteren op naam van arts of 2 op naam van patient.");
            Console.Write("Druk op een willekeurige toets om verder te gaan: ");

            bool isUserInputCorrect;

            do
            {
                isUserInputCorrect = int.TryParse(Console.ReadLine(), out int option);
                Console.WriteLine();

                if (option == 1)
                {
                    await HandlePhysiciantFilter(appointments);
                }
                else if (option == 2)
                {
                    await HandlePatientFilter(appointments);
                }
                else
                    break;

            } while (!isUserInputCorrect);
        }

        private static async Task HandlePhysiciantFilter(IEnumerable<Appointment> appointments)
        {
            Console.Write("\t Zoek naam van arts: ");
            string physicianName = Console.ReadLine();
            var chosenPhysician = await GetChosenPhysician(physicianName!);

            Console.Clear();

            Console.WriteLine($"Afspraken van {chosenPhysician!}: ");
            appointments = appointments.Where(a => a!.PhysicianId == chosenPhysician.Id);

            Console.WriteLine();
            if (appointments.Any())
            {
                foreach (var appointment in appointments)
                {
                    Console.WriteLine($"\t {appointment!.Id}. {appointment}");
                }
            }
            else Console.WriteLine("Er zijn geen afspraken.");
        }

        private static async Task HandlePatientFilter(IEnumerable<Appointment> appointments)
        {
            Console.Write("\t Zoek naam van patient: ");
            string patientName = Console.ReadLine();
            var chosenPatient = await GetChosenPatient(patientName);

            Console.Clear();

            Console.WriteLine($"Afspraken van patient {chosenPatient!}: ");
            appointments = appointments.Where(a => a!.PatientId == chosenPatient.Id);

            Console.WriteLine();
            if (appointments.Any())
            {
                foreach (var appointment in appointments)
                {
                    Console.WriteLine($"\t {appointment!.Id}. {appointment}");
                }
            }
            else Console.WriteLine("Er zijn geen afspraken.");
        }

        public static void RestartScreen()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Druk ENTER om terug naar het startscherm te gaan.");
            ConsoleKey choice = Console.ReadKey().Key;

            while (choice != ConsoleKey.Enter)
                choice = Console.ReadKey().Key;
        }

        #region FreeCodeForAssignment
        static async Task Main(string[] args)
        {
            while (await ShowMenu())
            {
                //Continue
            }
        }

        public async static Task<bool> ShowMenu()
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Clear();

            foreach (var line in File.ReadAllLines(@"Presentation\logo.txt"))
            {
                Console.WriteLine(line);
            }
            Console.WriteLine("");
            Console.WriteLine("1 - Patient toevoegen");
            Console.WriteLine("2 - Patienten verwijderen");
            Console.WriteLine("3 - Arts toevoegen");
            Console.WriteLine("4 - Arts verwijderen");
            Console.WriteLine("5 - Afspraak toevoegen");
            Console.WriteLine("6 - Afspraken inzien");
            Console.WriteLine("7 - Sluiten");
            Console.WriteLine("8 - Reset db");

            if (int.TryParse(Console.ReadLine(), out int option))
            {
                Console.WriteLine();
                switch (option)
                {
                    case 1:
                        await AddPatient();
                        RestartScreen();
                        return true;
                    case 2:
                        await DeletePatient();
                        RestartScreen();
                        return true;
                    case 3:
                        await AddPhysician();
                        RestartScreen();
                        return true;
                    case 4:
                        await DeletePhysician();
                        RestartScreen();
                        return true;
                    case 5:
                        await AddAppointment();
                        RestartScreen();
                        return true;
                    case 6:
                        await ShowAppointment();
                        RestartScreen();
                        return true;
                    case 7:
                        return false;
                    case 8:
                        var dbContext = new EPDDbContext();
                        dbContext.Database.EnsureDeleted();
                        dbContext.Database.EnsureCreated();
                        return true;
                    default:
                        return true;
                }
            }
            return true;
        }

        #endregion
    }
}