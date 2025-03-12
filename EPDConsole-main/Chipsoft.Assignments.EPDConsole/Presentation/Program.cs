using Chipsoft.Assignments.EPDConsole.Application.Interfaces;
using Chipsoft.Assignments.EPDConsole.Application.Services;
using Chipsoft.Assignments.EPDConsole.ApplicationCore.Application.DTO_s;
using Chipsoft.Assignments.EPDConsole.Domain.Extensions;
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
                Console.WriteLine("Patient registratie.");
                Console.WriteLine("Voer de persoonsgegevens van de patient in.");
                Console.WriteLine("(Voer 'X' in om het proces te annuleren)");

                Patient? patient = CreatePatient();

                if (patient is null) return;

                await patientService.AddPerson(patient);
                ConsoleOutputService.ShowSuccess($"patient {patient.FullName} is geregistreerd.");
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
                Console.WriteLine("Patient verwijderen");
                Console.WriteLine("(Voer 'X' in om het proces te annuleren)");

                Patient? patient = null;

                do
                {
                    var name = GetInput("Voer de naam van de patient in: ");

                    try
                    {
                        if (name is null) return;

                        patient = await GetChosenPatient(name!);

                        if (patient is null) return;
                    }
                    catch (Exception e)
                    {

                        ConsoleOutputService.ShowError($"{e.Message} Probeer opnieuw.");
                    }
                } while (patient is null);


                Patient deletedPatient = await patientService.DeletePerson(patient!);

                ConsoleOutputService.ShowSuccess($"patient {deletedPatient!.FullName} met ID {deletedPatient.Id} is verwijderd.");
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
                Console.WriteLine("Arts verwijderen");
                Console.WriteLine("(Voer 'X' in om het proces te annuleren)");

                Physician? physician = null;

                do
                {
                    var name = GetInput("Voer de naam van de arts in: ");

                    try
                    {
                        if (name is null) return;

                        physician = await GetChosenPhysician(name!);

                        if (physician is null) return;
                    }
                    catch (Exception e)
                    {

                        ConsoleOutputService.ShowError($"{e.Message} Probeer opnieuw.");
                    }
                } while (physician is null);

                Physician? deletedPhysician = await physicianService.DeletePerson(physician!);

                if (deletedPhysician is null)
                    return;

                ConsoleOutputService.ShowSuccess($"{deletedPhysician!} met ID {deletedPhysician.Id} is verwijderd.");
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
                Console.WriteLine("Arts registratie.");
                Console.WriteLine("Voer de persoonsgegevens van de arts in:");
                Console.WriteLine("(Voer 'X' in om het proces te annuleren)");

                Physician? physician = CreatePhysician();

                if (physician is null) return;

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
                    Console.WriteLine($"\t Afspraak ID {appointment!.Id}: Dr. {appointment.Physician.FullName}, Patient: {appointment.Patient.FullName}, Datum: {appointment.Date}");
                }

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
                Console.WriteLine("(Voer 'X' in om het proces te annuleren): ");

                Patient? chosenPatient = null;

                do
                {
                    try
                    {
                        Console.Write("\t Voer de naam van de patient in: ");

                        chosenPatient = await GetChosenPatient(Console.ReadLine());

                        if (chosenPatient is null) return;
                    }
                    catch (Exception e)
                    {

                        ConsoleOutputService.ShowError($"{e.Message} Probeer opnieuw.");
                    }

                } while (chosenPatient is null);

                Console.WriteLine();
                Console.WriteLine();

                Physician? chosenPhysician = null;

                do
                {
                    try
                    {
                        Console.Write("\t Voer de naam van de arts in: ");

                        chosenPhysician = await GetChosenPhysician(Console.ReadLine()!);

                        if (chosenPhysician is null) return;
                    }
                    catch (Exception e)
                    {

                        ConsoleOutputService.ShowError($"{e.Message} Probeer opnieuw.");
                    }

                } while (chosenPhysician is null);

                Console.WriteLine();
                CreateAppointment? createAppointment = CreateAppointment();

                if (createAppointment is null)
                    return;

                Appointment result = await appointmentService.AddAppointment(chosenPatient, chosenPhysician, createAppointment);
                ConsoleOutputService.ShowSuccess($"{result.ToString()} is gemaakt.");
            }
            catch (Exception e)
            {
                ConsoleOutputService.ShowError(e.Message);
            }
        }

        public static async Task ShowFilteredAppointments()
        {
            try
            {
                Console.Clear();
                IEnumerable<Appointment?> appointments = await appointmentService.GetAllAppointments();
                await OptionalAppointmentsFiltering(appointments!);
            }
            catch (Exception e)
            {

                ConsoleOutputService.ShowError(e.Message);
            }

        }

        #region HelperMethods
        private static Patient? CreatePatient()
        {
            var firstName = GetInputWithValidation("Voornaam: ", "voer opnieuw de voornaam in.", InputType.TextOnly);
            if (firstName is null) return null;

            var lastName = GetInputWithValidation("Achternaam: ", "voer opnieuw de achternaam in.", InputType.TextOnly);
            if (lastName is null) return null;

            var birthdate = GetInputWithValidation("Geboortedatum (vb. 10-02-2003): ", "voer opnieuw de geboortedatum in.", InputType.DateOnly);
            if (birthdate is null) return null;

            var address = GetInputWithValidation("Adres (vb. Langestraat 17): ", "voer opnieuw het adres in.", InputType.Address);
            if (address is null) return null;

            var number = GetInputWithValidation("Gsm nummer (vb. +32 456789123): ", "voer opnieuw het GSM nummer in (+landcode nummer).", InputType.PhoneNumber);
            if (number is null) return null;

            return new Patient(firstName!, lastName!, address!, birthdate!, number!);
        }

        private static CreateAppointment? CreateAppointment()
        {
            var date = GetInputWithValidation("Voer de datum en tijdstip in (vb. 10-02-2003 12:30): ", "voer opnieuw de datum in.", InputType.DateTime);
            if (date is null) return null;

            Console.WriteLine();
            Console.WriteLine();

            var reason = GetInput("Reden (optioneel): ");
            if (reason is null) return null;

            return new CreateAppointment(date!, reason);
        }

        private static Physician? CreatePhysician()
        {
            var firstName = GetInputWithValidation("Voornaam: ", "voer opnieuw de voornaam in.", InputType.TextOnly);
            if (firstName is null) return null;

            var lastName = GetInputWithValidation("Achternaam: ", "voer opnieuw de achternaam in.", InputType.TextOnly);
            if (lastName is null) return null;

            var birthdate = GetInputWithValidation("Geboortedatum (vb. 10-02-2003): ", "voer opnieuw de geboortedatum in.", InputType.DateOnly);
            if (birthdate is null) return null;

            return new Physician(firstName!, lastName!, birthdate!);
        }

        private async static Task<Patient?> GetChosenPatient(string? patientName)
        {
            if (patientName?.ToUpper().Trim() == "X")
                return null;

            IEnumerable<Patient?> patients = await patientService.GetPerson(patientName!);
            Console.WriteLine();

            Console.WriteLine("Gevonden patienten:");
            foreach (var patient in patients)
            {
                Console.WriteLine($"\t ID: {patient!.Id}, Naam: {patient}");
            }

            Patient? result = null;
            Console.WriteLine();

            while (result is null)
            {
                string? userInput = GetInput("Voer de ID in: ");

                if (userInput == null) return null;

                if (int.TryParse(userInput, out int patientId))
                {
                    result = patients.FirstOrDefault(p => p!.Id == patientId);
                    if (result is null)
                        ConsoleOutputService.ShowError("ID niet gevonden. Probeer opnieuw.");
                }
                else
                {
                    ConsoleOutputService.ShowError("Ongeldige ID. Probeer opnieuw.");
                }
            }

            return result;
        }

        private async static Task<Physician?> GetChosenPhysician(string physicianName)
        {
            if (physicianName?.ToUpper().Trim() == "X")
                return null;

            IEnumerable<Physician?> physicians = await physicianService.GetPerson(physicianName);

            Console.WriteLine();

            Console.WriteLine("Gevonden artsen:");
            foreach (var physician in physicians)
            {
                Console.WriteLine($"\t ID: {physician!.Id}, Naam: {physician}");
            }

            Physician? result = null;
            Console.WriteLine();

            while (result is null)
            {
                var userInput = GetInput("Voer de ID in: ");

                if (userInput is null) return null;

                if (int.TryParse(userInput, out int physicianId))
                {
                    result = physicians.FirstOrDefault(p => p!.Id == physicianId);
                    if (result is null)
                        ConsoleOutputService.ShowError("ID niet gevonden. Probeer opnieuw.");
                }
                else
                {
                    ConsoleOutputService.ShowError("Ongeldige ID. Probeer opnieuw.");
                }
            }

            return result;
        }

        private async static Task OptionalAppointmentsFiltering(IEnumerable<Appointment> appointments)
        {
            Console.WriteLine("Zoek afspraken op arts of patient.");
            Console.WriteLine();
            Console.WriteLine("Om te filteren op ARTS voer het nummer '1' in, op PATIENT '2'.");

            bool isUserInputCorrect;
            string[] options = { "1", "2", "0", "X" };

            do
            {
                var userInput = GetInput("(Voer 'X' in om het proces te annuleren): ");

                if (userInput == null) return;

                isUserInputCorrect = userInput != null && options.Contains(userInput);

                if (!isUserInputCorrect)
                {
                    Console.WriteLine();
                    ConsoleOutputService.ShowError("Voer 1, of 2, X in.");
                    continue;
                }

                switch (userInput)
                {
                    case "1":
                        await HandlePhysiciantFilter(appointments);
                        break;
                    case "2":
                        await HandlePatientFilter(appointments);
                        break;
                    case "X":
                        return;
                }

            } while (!isUserInputCorrect);
        }

        private static async Task HandlePhysiciantFilter(IEnumerable<Appointment> appointments)
        {
            Physician? chosenPhysician = null;

            do
            {
                var physicianName = GetInput("Voer de naam van de arts in: ");

                if (physicianName is null) return;

                try
                {
                    chosenPhysician = await GetChosenPhysician(physicianName!);

                    if (chosenPhysician is null) return;
                }
                catch (Exception e)
                {
                    ConsoleOutputService.ShowError($"{e.Message} Probeer opnieuw.");
                    continue;
                }
            } while (chosenPhysician is null);

            Console.Clear();

            Console.WriteLine($"Afspraken van {chosenPhysician!}: ");
            appointments = appointments.Where(a => a!.PhysicianId == chosenPhysician.Id);

            Console.WriteLine();
            if (appointments.Any())
            {
                foreach (var appointment in appointments)
                {
                    Console.WriteLine($"\t Afspraak ID {appointment!.Id}: Dr. {appointment.Physician.FullName}, Patient: {appointment.Patient.FullName}, Datum: {appointment.Date}");
                }
            }
            else Console.WriteLine("Er zijn geen afspraken.");
        }

        private static async Task HandlePatientFilter(IEnumerable<Appointment> appointments)
        {
            Patient? chosenPatient = null;

            do
            {
                var patientName = GetInput("Voer de naam van de patient in: ");

                if (patientName is null) return;

                try
                {
                    chosenPatient = await GetChosenPatient(patientName!);

                    if (chosenPatient is null) return;
                }
                catch (Exception e)
                {
                    ConsoleOutputService.ShowError($"{e.Message} Probeer opnieuw.");
                    continue;
                }
            } while (chosenPatient is null);

            Console.Clear();

            Console.WriteLine($"Afspraken van patient {chosenPatient!}: ");
            appointments = appointments.Where(a => a!.PatientId == chosenPatient.Id);

            Console.WriteLine();
            if (appointments.Any())
            {
                foreach (var appointment in appointments)
                {
                    Console.WriteLine($"\t Afspraak ID {appointment!.Id}: Dr. {appointment.Physician.FullName}, Patient: {appointment.Patient.FullName}, Datum: {appointment.Date}");
                }
            }
            else Console.WriteLine("Er zijn geen afspraken.");
        }

        private static string? GetInput(string prompt)
        {
            Console.Write($"\t {prompt}");
            var input = Console.ReadLine()?.Trim();
            return input?.ToUpper() == "X" ? null : input;
        }

        private static string? GetInputWithValidation(string prompt, string errorMessage, InputType inputType)
        {
            while (true)
            {
                var input = GetInput(prompt);
                if (input is null) return null;

                if (input == "")
                {
                    ConsoleOutputService.ShowError("Dit veld kan niet leeg zijn. Probeer opnieuw.");
                    continue;
                }

                switch (inputType)
                {
                    case InputType.TextOnly:
                        if (HasNumbers(input))
                        {
                            ConsoleOutputService.ShowError(errorMessage);
                            continue;
                        }
                        break;
                    case InputType.NumbersOnly:
                        if (!HasNumbers(input) || HasLetters(input))
                        {
                            ConsoleOutputService.ShowError(errorMessage);
                            continue;
                        }
                        break;
                    case InputType.BothRequired:
                        if (!HasNumbers(input) || !HasLetters(input))
                        {
                            ConsoleOutputService.ShowError(errorMessage);
                            continue;
                        }
                        break;
                    case InputType.DateOnly:
                        if (!input.IsDateValid())
                        {
                            ConsoleOutputService.ShowError(errorMessage);
                            continue;
                        }
                        break;
                    case InputType.DateTime:
                        if (!input.IsDateTimeValid())
                        {
                            ConsoleOutputService.ShowError(errorMessage);
                            continue;
                        }
                        break;
                    case InputType.PhoneNumber:
                        var nr = input.IsPhoneNumberValid();
                        if (!nr)
                        {
                            ConsoleOutputService.ShowError(errorMessage);
                            continue;
                        }
                        break;
                    case InputType.Address:
                        if (!input.IsAddressValid())
                        {
                            ConsoleOutputService.ShowError(errorMessage);
                            continue;
                        }
                        break;
                }
                return input;
            }
        }

        private static bool HasNumbers(string input)
        {
            return input.Any(char.IsDigit);
        }

        private static bool HasLetters(string input)
        {
            return input.Any(char.IsLetter);
        }

        private enum InputType
        {
            Any,
            TextOnly,
            NumbersOnly,
            BothRequired,
            DateOnly,
            DateTime,
            PhoneNumber,
            Address
        }

        private static void RestartScreen()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Druk ENTER om terug naar het startscherm te gaan.");
            ConsoleKey choice = Console.ReadKey().Key;

            while (choice != ConsoleKey.Enter)
                choice = Console.ReadKey().Key;
        }
        #endregion

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
            Console.WriteLine("Welkom bij het Huisartsenpraktijk Afspraken Systeem");
            Console.WriteLine("====================================================");
            Console.WriteLine("Wat wilt u doen?");
            Console.WriteLine("1 - Patient toevoegen");
            Console.WriteLine("2 - Patienten verwijderen");
            Console.WriteLine("3 - Arts toevoegen");
            Console.WriteLine("4 - Arts verwijderen");
            Console.WriteLine("5 - Afspraak toevoegen");
            Console.WriteLine("6 - Alle afspraken inzien");
            Console.WriteLine("7 - Afspraken inzien op arts of patient");
            Console.WriteLine("8 - Sluiten");
            Console.WriteLine("9 - Reset db");

            Console.WriteLine();
            if (int.TryParse(Console.ReadLine(), out int option))
            {
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
                        await ShowFilteredAppointments();
                        RestartScreen();
                        return true;
                    case 8:
                        return false;
                    case 9:
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