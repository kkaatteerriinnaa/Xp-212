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
            foreach (char c in value.Reverse())
            {
                int digit = DigitValue(c);
                res += digit < prevDigit ? -digit : digit;
                prevDigit = digit;
            }
            return new RomanNamber(res);
        }

        public static int DigitValue(char digit)
        {
            if (!"NIVXLCDM".Contains(digit))
            {
                throw new ArgumentException($"The argument '{digit}' is not valid Roman digit.", nameof(digit));
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
                _ => throw new ArgumentException($"The argument '{digit}' is not valid Roman digit.", nameof(digit))
            };
        }
    }

}

