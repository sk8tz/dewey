﻿using System.Text;

namespace Dewey.Net.Types
{
    public static class DecimalExtensions
    {
        public static string NumberToCurrencyText(this decimal number)
        {
            // Round the value just in case the decimal value is longer than two digits
            number = decimal.Round(number, 2);

            // Divide the number into the whole and fractional part strings
            var arrNumber = number.ToString().Split('.');

            // Get the whole number text
            var wholePart = long.Parse(arrNumber[0]);
            var strWholePart = NumberToText(wholePart);

            // For amounts of zero dollars show 'No Dollars...' instead of 'Zero Dollars...'
            var wordNumber = (wholePart == 0 ? "No" : strWholePart) + (wholePart == 1 ? " Dollar and " : " Dollars and ");

            // If the array has more than one element then there is a fractional part otherwise there isn't
            // just add 'No Cents' to the end
            if (arrNumber.Length > 1) {
                // If the length of the fractional element is only 1, add a 0 so that the text returned isn't,
                // 'One', 'Two', etc but 'Ten', 'Twenty', etc.
                var fractionPart = long.Parse((arrNumber[1].Length == 1 ? arrNumber[1] + "0" : arrNumber[1]));
                var strFarctionPart = NumberToText(fractionPart);

                wordNumber += (fractionPart == 0 ? " No" : strFarctionPart) + (fractionPart == 1 ? " Cent" : " Cents");
            } else
                wordNumber += "No Cents";

            return wordNumber;
        }

        private static string NumberToText(long number)
        {
            StringBuilder wordNumber = new StringBuilder();

            var powers = new[] { "Thousand ", "Million ", "Billion " };
            var tens = new[] { "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
            var ones = new[] { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };

            if (number == 0) { return "Zero"; }
            if (number < 0) {
                wordNumber.Append("Negative ");
                number = -number;
            }

            var groupedNumber = new long[] { 0, 0, 0, 0 };
            var groupIndex = 0;

            while (number > 0) {
                groupedNumber[groupIndex++] = number % 1000;
                number /= 1000;
            }

            for (int i = 3; i >= 0; i--) {
                var group = groupedNumber[i];

                if (group >= 100) {
                    wordNumber.Append(ones[group / 100 - 1] + " Hundred ");
                    group %= 100;

                    if (group == 0 && i > 0)
                        wordNumber.Append(powers[i - 1]);
                }

                if (group >= 20) {
                    if ((group % 10) != 0)
                        wordNumber.Append(tens[group / 10 - 2] + " " + ones[group % 10 - 1] + " ");
                    else
                        wordNumber.Append(tens[group / 10 - 2] + " ");
                } else if (group > 0)
                    wordNumber.Append(ones[group - 1] + " ");

                if (group != 0 && i > 0)
                    wordNumber.Append(powers[i - 1]);
            }

            return wordNumber.ToString().Trim();
        }

        public static decimal Add(this decimal value, decimal arg)
        {
            return (value + arg);
        }

        public static decimal Subtract(this decimal value, decimal arg)
        {
            return (value - arg);
        }

        public static decimal Multiply(this decimal value, decimal arg)
        {
            return (value * arg);
        }

        public static decimal Divide(this decimal value, decimal arg)
        {
            return (value / arg);
        }

        public static decimal Round(this decimal value, int decimals = 2)
        {
            return decimal.Round(value, decimals);
        }

        public static bool IsZero(this decimal value)
        {
            return (value == decimal.Zero);
        }
    }
}
