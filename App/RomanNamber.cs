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
            int res = 0;
            int prevDigit = 0;
            int pos = value.Length;
            int maxDigit = 0;
            int lessCounter = 0;
            int maxCounter = 1;
            foreach (char c in value.Reverse())
            {
                pos--;
                int digit;
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
                //"відстань" між цифрами
                if (digit != 0 && prevDigit / digit > 10)  //цифра занадто "далекі" для віднімання
                {
                    throw new FormatException(
                        $"{nameof(RomanNamber)}.{nameof(Parse)}() " +
                        $"illegal sequence: '{c}' befor '{value[pos + 1]}' in position {pos}");
                }
                // віднімання цифр, що є "5"-ками
                if(digit < prevDigit && (digit == 5 || digit == 50 || digit == 500))
                {
                    throw new FormatException(
                        $"{nameof(RomanNamber)}.{nameof(Parse)}() " +
                        $"illegal sequence: '{c}' befor '{value[pos + 1]}' in position {pos}");

                }

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
                    throw new FormatException(value);
                }

                if (digit == 0 && value.Length > 1)
                {
                    throw new FormatException();
                }

                res += digit < prevDigit ? -digit : digit;
                prevDigit = digit;
            }

            return new(res);
        }

        public static int DigitValue(char digit)
        {
            if (!"NIVXLCDM".Contains(digit))
            {
                throw new ArgumentException($"The argument " +
                    $"'{digit}' is not valid Roman digit.", 
                    nameof(digit));
            }

            return digit switch
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
                    $"RomanNumber.DigitValue() illegal digit: '{digit}'"),
            };
        }
    }

}

