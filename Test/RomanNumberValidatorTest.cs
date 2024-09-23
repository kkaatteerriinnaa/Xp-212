﻿using App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    [TestClass]
    public class RomanNumberValidatorTest
    {
        [TestMethod]
        public void DigitValueTest()
        {
            Dictionary<char, int> testCases = new()
        {
            {'N', 0    },
            {'I', 1    },
            {'V', 5    },
            {'X', 10   },
            {'L', 50   },
            {'C', 100  },
            {'D', 500  },
            {'M', 1000 },
        };
            foreach (var testCase in testCases)
            {
                Assert.AreEqual(
                    testCase.Value,
                    RomanNamberParse.DigitValue(testCase.Key),
                    $"{testCase.Key} => {testCase.Value}"
                );
            }

            char[] excCases = { '0', '1', 'x', 'i', '&' };

            foreach (var testCase in excCases)
            {
                var ex = Assert.ThrowsException<ArgumentException>(
                    () => RomanNamberParse.DigitValue(testCase),
                    $"DigitValue('{testCase}') must throw Exception"
                );
                Assert.IsTrue(
                    ex.Message.Contains($"{testCase}"),
                    "DigitValue ex.Message should contain a symbol which caused the exception! " +
                    $"Symbol: '{testCase}', ex.Message: '{ex.Message}'"
                );
                Assert.IsTrue(
                    ex.Message.Contains("digit") &&
                    ex.Message.Contains("argument") &&
                    ex.Message.Contains("not valid Roman digit"),
                    "DigitValue ex.Message should contain the argument name 'digit', the word 'argument', and the text 'not valid Roman digit'. " +
                    $"Symbol: '{testCase}', ex.Message: '{ex.Message}'"
                );
            }
        }

        public void CheckZeroDigitTest()
        {
            Object[][] exCases5 = [
                ["NN",    '0'],    // Цифра не може бути у числі, тільки
                ["IN",    '1'],    // сама по собі
                ["NX",    '0'],
                ["NC",    '0'],
                ["XNC",   '1'],
                ["IVIN",  '3'],
                ["XNNN",  '1'],

            ];
            foreach (var exCase in exCases5)
            {
                var ex = Assert.ThrowsException<FormatException>(
                    () => RomanNamber.Parse(exCase[0].ToString()!),
                    $"RomanNumber.Parse(\"{exCase[0]}\") must throw FormatException"
                );
            }
        }
    }
}
