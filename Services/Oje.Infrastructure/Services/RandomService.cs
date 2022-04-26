using System;
using System.Collections.Generic;

namespace Oje.Infrastructure.Services
{
    public class RandomService
    {
        static List<string> strChars = new List<string>()
        {
            "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "A", "S", "D", "F", "G", "H", "J", "K", "L", "Z", "X", "C", "V", "B", "N", "M", "q", "w", "e", "r",
            "t", "y", "u", "i", "o", "p", "a", "s", "d", "f", "g", "h", "j", "k", "l", "z", "x", "c", "v", "b", "n", "m"
        };
        static List<string> strNumbers = new List<string>
        {
            "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"
        };
        static List<string> strNonAlpha = new List<string>
        {
            "!", "@", "#","$", "%", "^","&", "*", "(",")", "|", "\\","/", ",", ".","~"
        };
        static List<List<string>> validChars = new List<List<string>>() { strChars, strNumbers, strNonAlpha };

        public static string GeneratePassword(int count)
        {
            string result = "";
            Random random = new Random();

            for (int i = 0; i < count; i++)
            {
                List<string> currCategroy = null;
                if (i == 0)
                    currCategroy = validChars[2];
                else
                    currCategroy = validChars[random.Next(0, 3)];
                result += currCategroy[random.Next(0, currCategroy.Count - 1)];
            }

            return result;
        }

        public static int GenerateRandomNumber(int count)
        {
            string result = "";
            Random random = new Random();

            for (int i = 0; i < count; i++)
            {
                var currCategroy = validChars[1];
                result += currCategroy[random.Next(0, currCategroy.Count - 1)];
            }

            return result.ToIntReturnZiro();
        }
    }
}
