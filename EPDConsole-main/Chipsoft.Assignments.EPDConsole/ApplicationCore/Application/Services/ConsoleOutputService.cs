﻿namespace Chipsoft.Assignments.EPDConsole.ApplicationCore.Application.Services
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
            Console.Write($"FAILED");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($": {message}");
        }
    }
}
