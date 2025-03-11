namespace Chipsoft.Assignments.EPDConsole.Application.Services
{
    public static class ConsoleOutputService
    {
        public static void ShowSuccess(string message)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"SUCCESS");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($": {message}");
        }

        public static void ShowError(string message)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"FOUTIEF");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($": {message}");
        }
    }
}
