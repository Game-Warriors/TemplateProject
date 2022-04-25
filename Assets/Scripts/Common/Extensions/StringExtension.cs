using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Common.Extensions
{
    public static class StringExtension
    {
        private const int LENGTH = 101;
        private static readonly char[] EN_NUMBERS = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private static readonly char[] FA_NUMBERS = { '٠', '١', '٢', '٣', '٤', '٥', '٦', '٧', '٨', '٩' };

        private static readonly char[] INVALID_USERNAME_CHAR = { '"', '\'', ':', '&', '(', ')', '=', '_', ';', '[', ']', '}', '{', '\\', '/' };
        private static readonly char[] INVALID_TEXT_CHAR = { '"', '[', ']', '}', '{' };

        private static readonly string[] EN_NUMBER_TEXT = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22",
        "23","24","25","26","27","28","29","30","31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "52","53",
        "54","55","56","57","58","59","60","61","62","63","64","65","66","67","68","69","70","71","72","73","74","75","76","77","78","79","80","81","82","83","84","85","86","87","88","89"
        ,"90","91","92","93","94","95","96","97","98","99","100"};

        private static readonly string[] FA_NUMBER_TEXT = { "٠", "١", "٢", "٣", "٤", "٥", "٦", "٧", "٨", "٩", "۱۰", "۱۱", "۱۲", "۱۳", "۱۴", "۱۵", "١٦", "١٧", "١٨", "١٩", "٢٠", "٢١", "٢٢",
        "٢٣","٢٤","٢٥","26","27","28","29","30","31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "52","53",
        "54","55","56","57","58","59","60","61","62","63","64","65","66","67","68","69","70","71","72","73","74","75","76","77","78","79","80","81","82","83","84","85","86","87","88","89"
        ,"90","91","92","93","94","95","96","97","98","99","100"};

        public static string ToCachNumberString(this int input, bool isPersion = false)
        {
            if (input < 0 || input >= 26)
                return isPersion ? Convert.ToString(input).ToPersianNumber() : Convert.ToString(input);

            if (isPersion)
            {
                return FA_NUMBER_TEXT[input];
            }
            else
            {
                return EN_NUMBER_TEXT[input];
            }
        }

        public static string ToPersianNumber(this string input)
        {
            int length = 10;
            for (int i = 0; i < length; i++)
                input = input.Replace(EN_NUMBERS[i], FA_NUMBERS[i]);
            return input;
        }


        public static bool IsValidName(this string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                return false;
            int result = name.IndexOfAny(INVALID_USERNAME_CHAR);
            return result < 0;
        }


        public static bool IsValidText(this string text)
        {
            int result = text.IndexOfAny(INVALID_TEXT_CHAR);
            return result < 0;
        }

        public static bool IsValidEmail(this string email)
        {
            //reference: https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
            return Regex.IsMatch(email, @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", RegexOptions.IgnoreCase);
        }

        public static bool IsPhoneNumber(this string number)
        {
            return Regex.Match(number, @"^(\+[0-9]{9})$").Success;
        }

        /// <summary>
        /// RegEx refrence: https://barnamenevisan.org/Articles/Article1872.html
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsMobilePhoneNumber(string number)
        {
            //old regex ^(09|9)(0[0-9]|1[0-9]|3[1-9]|2[1-9])\d{7}$
            return Regex.Match(number, @"^(09|9)\d{9}$").Success;
        }

        public static string RemoveWhiteSpaces(this string str)
        {
            return Regex.Replace(str, @"\s+", "");
            ;
        }

        /// <summary>
        /// is iranian phone number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsMyMobilePhoneNumber(string number)
        {
            return Regex.Match(number, @"(0|\+98)?([ ]|,|-|[()]){0,2}9[1|2|3|4]([ ]|,|-|[()]){0,2}(?:[0-9]([ ]|,|-|[()]){0,2}){8}").Success;
        }

        /// <summary>
        /// Remove any redundant characters(e.g Country Code, first zero character,...) from mobile phone number.
        /// It will return 10 digit mobile phone number or null when it is not in right format.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetFormatedMobilePhoneNumber(string number)
        {
            if (StringExtension.IsMyMobilePhoneNumber(number))
            {
                string result = number.RemoveWhiteSpaces();
                result = result.Replace("+", "");
                if (result[0] == '0')
                    result = result.Remove(0, 1);
                if (result[0] == '9' && result[1] == '8' && result.Length > 10)
                    result = result.Remove(0, 2);
                return result;
            }

            return null;
        }

        public static bool IsInvalidPassword(this string inputString)
        {
            string MatchNumberPattern = @"[-&'/=?{}:]";
            if (!string.IsNullOrEmpty(inputString))
                return Regex.IsMatch(inputString, MatchNumberPattern, RegexOptions.IgnoreCase);
            return false;

        }
    }
}
