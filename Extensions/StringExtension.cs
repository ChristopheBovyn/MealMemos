using System;
using System.Text.RegularExpressions;

namespace MealMemos.Extensions
{
    public static class StringExtension
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return String.IsNullOrEmpty(value);
        }

        public static bool IsAValidEmail(this string email)
        {
            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
        }


    }
}
