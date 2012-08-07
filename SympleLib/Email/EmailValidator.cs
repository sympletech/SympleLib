using System.Text.RegularExpressions;

namespace SympleLib.Email
{
    public class EmailValidator
    {
        public static bool IsValidEmail(string strIn)
        {
            return IsValidEmail(strIn, true);
        }
        /// <summary>
        /// Checks to ensure a valid e-mail format
        /// </summary>
        public static bool IsValidEmail(string strIn, bool allowBlank)
        {
            if (!string.IsNullOrEmpty(strIn))
            {
                // Return true if strIn is in valid e-mail format.
                return Regex.IsMatch(strIn,
                       @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
                       @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
            }
            return allowBlank;
        }
    }
}
