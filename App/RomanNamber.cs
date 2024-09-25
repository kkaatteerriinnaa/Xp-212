using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public record RomanNumber(int Value)
    {
        public static RomanNumber Parse(String input) =>
            RomanNumberParser.FromString(input);

        public static explicit operator short(RomanNumber rn) => (short)rn.Value;
        public static explicit operator byte(RomanNumber rn) => (byte)rn.Value;
        public static explicit operator long(RomanNumber rn) => (long)rn.Value;
        public static explicit operator int(RomanNumber rn) => rn.Value;
        public static explicit operator float(RomanNumber rn) => (float)rn.Value;
        public static explicit operator double(RomanNumber rn) => (double)rn.Value;

        public override string? ToString()
        {
            Dictionary<int, String> ranges = new()
            {
                { 1, "I" },
                { 4, "IV" },
                { 5, "V" },
                { 9, "IX" },
                { 10, "X" },
                { 40, "XL" },
                { 50, "L" },
                { 90, "XC" },
                { 100, "C" },
                { 400, "CD" },
                { 500, "D" },
                { 900, "CM" },
                { 1000, "M" },
            };
            int value = this.Value;
            if (value == 0) return "N";
            var result = new StringBuilder();
            foreach (var pair in ranges.OrderByDescending(kv => kv.Key))
            {
                while (value >= pair.Key)
                {
                    result.Append(pair.Value);
                    value -= pair.Key;
                }
            }
            return result.ToString();
        }
    }
}