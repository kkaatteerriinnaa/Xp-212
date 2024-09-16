using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class RomanNamber
    {
        private readonly int _value;
        public int Value { get { return _value; } }

        public RomanNamber(int value)
        {
            _value = value;
        }

        public static RomanNamber Parse(string value)
        {
            CheckZeroDigit(value);

            int res = 0;
            int rightDigit = 0;
            int pos = value.Length;
            int maxDigit = 0;
            int lessCounter = 0;
            int maxCounter = 1;
            foreach (char c in value.Reverse())
            {
                pos--;
                int digit;


                //ДЗ
                try
                {
                    digit = DigitValue(c);
                }
                catch
                {
                    throw new FormatException(
                        $"{nameof(RomanNamber)}.{nameof(Parse)}() " +
                        $"found illegal symbol '{c}' in position {pos}");
                }
                //

                //"відстань" між цифрами
                //if (!CheckDigitRatio (digit, rightDigit))  
                //{
                //    throw new FormatException(
                //        $"{nameof(RomanNamber)}.{nameof(Parse)}() " +
                //        $"illegal sequence: '{c}' befor '{value[pos + 1]}' in position {pos}");
                //}
                // віднімання цифр, що є "5"-ками

                // рахуємо цифри, якщо вони менші за максимальну
                if (digit > maxDigit)
                {
                    maxDigit = digit;
                    lessCounter = 0;
                    maxCounter = 1;
                }
                else if (digit == maxDigit)
                {
                    maxCounter += 1;
                    lessCounter = 0;
                }
                else
                {
                    lessCounter += 1;
                }

                if (lessCounter > 1 || lessCounter > 0 && maxCounter > 1)
                {
                    throw new FormatException($"{nameof(RomanNamber)}.{nameof(Parse)}('{value}') " +
                        $"illegal sequence: more than one smaller digits " +
                        $"befor '{value[pos + 2]}' in position {pos + 2}");
                }

                res += digit < rightDigit ? -digit : digit;
                rightDigit = digit;
            }

            return new(res);
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
            for(int i = 0; i < input.Length -1; ++i)
            {
                int leftDigit = DigitValue(input[i]);
                int rightDigit = DigitValue(input[i + 1]);
                if(! (leftDigit >= rightDigit || !(
                leftDigit != 0 && rightDigit / leftDigit > 10 ||
                leftDigit == 5 || leftDigit == 50 || leftDigit == 500
                )))
                {
                    throw new FormatException(
                        $"{nameof(RomanNamber)}.{nameof(Parse)}() " +
                        $"illegal sequence: '{input[i]}' befor '{input[i + 1]}' in position {i}");
                }
            }
        }
        private static int ParseDigit(char c, int pos, string Value)
        {
            int digit;
            try
            {
                digit = DigitValue(c);
            }
            catch (ArgumentException)
            {
                throw new FormatException(
                    $"{nameof(RomanNamber)}.{nameof(Parse)}()" +
                    $" found illegal symbol '{c}' in position {pos}");
            }

            return digit;
        }

        public static int DigitValue(char digit) => digit switch
        {
                'N' => 0,
                'I' => 1,
                'V' => 5,
                'X' => 10,
                'L' => 50,
                'C' => 100,
                'D' => 500,
                'M' => 1000,
                _ => throw new ArgumentException(
                    $"RomanNumber.DigitValue() illegal argument digit: '{digit}' not valid Roman digit"),
            };
        }
    }

