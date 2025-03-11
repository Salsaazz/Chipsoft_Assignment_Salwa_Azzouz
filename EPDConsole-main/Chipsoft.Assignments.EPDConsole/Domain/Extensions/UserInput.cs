using System.Globalization;
using System.Text.RegularExpressions;

namespace Chipsoft.Assignments.EPDConsole.Domain.Extensions
{
    public static class UserInput
    {
        public static bool IsNameValid(this string name) => Regex.Match(name, "^[A-Z][a-zA-Z]*$", RegexOptions.IgnoreCase).Success;

        public static bool IsPhoneNumberValid(this string number) => Regex.Match(number, "^\\+[1-9][0-9]{7,14}$").Success;

        public static bool IsAddressValid(this string address) => Regex.Match(address, @"^(?=.*[a-zA-Z])(?=.*[0-9])[a-zA-Z0-9\s]+$").Success;

        public static string CapitalizeFirstLetterOfWords(this string text) => Regex.Replace(text, @"\b([a-z])", m => m.Value.ToUpper());

        public static bool IsDateValid(this string input)
        {
            var format = "dd-MM-yyyy";
            bool IsDateInputCorrect = DateTime.TryParseExact(input, format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None, out _);

            if (!IsDateInputCorrect) return false;

            return true;
        }

        public static bool IsDateTimeValid(this string input)
        {
            var format = "dd-MM-yyyy HH:mm";
            bool isDateInputCorrect = DateTime.TryParseExact(input, format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None, out _);

            if (!isDateInputCorrect) return false;

            return true;
        }
    }
}