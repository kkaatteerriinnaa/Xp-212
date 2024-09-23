using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public record RomanNamber(int Value)
    {
        public static RomanNamber Parse(String input) =>
        RomanNamberParse.FormString(input);

        public override string? ToString()
        {
            Dictionary<int, String> ranges = new()
    {
                { 1, "I"    },
                { 4, "IV"   },
                { 5, "V"    },
                { 9, "IX"   },
                { 10, "X"   },
                { 40, "XL"  },
                { 50, "L"   },
                { 90, "XC"  },
                { 100, "C"  },
                { 400, "CD" },
                { 500, "D"  },
                { 900, "CM" },
                { 1000, "M" },
            };

            if (Value == 0) { return "N"; }

            int number = Value;
            StringBuilder result = new();
             
            foreach (var range in ranges.Reverse())
            {
               while (number >= range.Key)
               {
                   result.Append(range.Value);
                   number -= range.Key;
               }
            }

            return result.ToString();
        }
    
        
    }
}

