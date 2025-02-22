using System.Text.RegularExpressions;

namespace Chipsoft.Assignments.EPDConsole.Common.Extensions
{
    public static class UserInput
    {
        public static bool IsNameValid(this string name) => Regex.Match(name, "^[A-Z][a-zA-Z]*$", RegexOptions.IgnoreCase).Success;
        public static bool IsPhoneNumberValid(this string number) => Regex.Match(number, "^\\+?[1-9][0-9]{7,14}$").Success;

        public static string CapitalizeFirstLetterOfWords(this string text) => Regex.Replace(text, @"\b([a-z])", m => m.Value.ToUpper());
    }
}
