using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    internal class RomanNumberValidator
    {
        public static void Validate(string value)
        {
            CheckSymbols(value);
            CheckZeroDigit(value);
            CheckDigitRatio(value);
            CheckSequence(value);
        }

        public static void CheckSequence(String input)
        {
            int maxDigit = 0; // найбільша цифра, що зустрічається
            int lessCounter = 0; // кількість цифр, менших за ней
            int maxCounter = 1; // кількість однакових найбільших цифр
            int pos = input.Length;

            for (int i = input.Length - 1; i >= 0; --i)
            {
                int digit = RomanNamberParse.DigitValue(input[i]);
                if (digit > maxDigit)
                {
                    maxDigit = digit;
                    lessCounter = 0;
                    maxCounter = 1;
                }
                else if (digit == maxDigit)
                {
                    lessCounter = 0;
                    maxCounter += 1;
                }
                else
                {
                    lessCounter += 1;
                }
                if (lessCounter > 1 || lessCounter > 0 && maxCounter > 1)
                    throw new FormatException(
                        $"{nameof(RomanNamber)}.Parse.('{input}') " +
                        $"illegal sequence: more than one smaller digits " +
                        $"before '{input[i + 2]}' in position {i + 2}");

            }
        }

        public static void CheckZeroDigit(String input)
        {
            if (input.Contains('N') && input.Length > 1)
            {
                throw new FormatException();
            }

        }

        //цифра занадто "далекі" для віднімання
        public static void CheckDigitRatio(String input)
        {
            for (int i = 0; i < input.Length - 1; ++i)
            {
                int leftDigit = RomanNamberParse.DigitValue(input[i]);
                int rightDigit = RomanNamberParse.DigitValue(input[i + 1]);
                if (!(leftDigit >= rightDigit || !(
                leftDigit != 0 && rightDigit / leftDigit > 10 ||
                leftDigit == 5 || leftDigit == 50 || leftDigit == 500
                )))
                {
                    throw new FormatException(
                        $"{nameof(RomanNamber)}.Parse() " +
                        $"illegal sequence: '{input[i]}' before '{input[i + 1]}' in position {i}");
                }
            }
        }
        private static void CheckSymbols(String input)
        {
            for (int i = 0; i < input.Length; ++i)
            {
                try
                {
                    RomanNamberParse.DigitValue(input[i]);
                }
                catch
                {
                    throw new FormatException(
                    $"{nameof(RomanNamber)}.Parse()" +
                    $" found illegal symbol '{input[i]}' in position {i}");
                }
            }
        }

    }
}
