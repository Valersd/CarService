using System;
using System.Text;

namespace CarService.Common
{
    public static class RandomGenerator
    {
        private static Random _rand;
        private const string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const string LettersWithSpaces = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz       ";

        static RandomGenerator()
        {
            _rand = new Random();
        }

        public static string GetRandomString(int min = 0, int max = 30)
        {
            StringBuilder result = new StringBuilder();
            int length = _rand.Next(min, max + 1);
            for (int i = 0; i < length; i++)
            {
                result.Append(Letters[_rand.Next(Letters.Length - 1)]);
            }
            return result.ToString();
        }

        public static int GetRandomInteger(int min = 0, int max = 100)
        {
            return _rand.Next(min, max + 1);
        }

        public static string GetRandomText(int min = 0, int max = 300)
        {
            StringBuilder result = new StringBuilder();
            int length = _rand.Next(min, max + 1);
            for (int i = 0; i < length; i++)
            {
                result.Append(LettersWithSpaces[_rand.Next(LettersWithSpaces.Length - 1)]);
            }
            return result.ToString().Trim();
        }

        public static DateTime GetRandomDateTime(DateTime start, int maxHoursAfterStart)
        {
            double hoursAfterStart = (double)GetRandomInteger(0, maxHoursAfterStart - 1) + (double)GetRandomInteger(1, 100) * 0.01;
            DateTime result = start.AddHours(hoursAfterStart);
            return result;
        }
    }
}

