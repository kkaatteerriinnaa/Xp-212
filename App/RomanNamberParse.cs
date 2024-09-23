﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class RomanNamberParse
    {
        public static RomanNamber FormString(string value)
        {
            RomanNumberValidator.Validate(value);
            int res = 0;
            int rightDigit = 0;

            foreach (char c in value.Reverse())
            {
                int digit = DigitValue(c);

                res += digit < rightDigit ? -digit : digit;
                rightDigit = digit;
            }

            return new(res);
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
