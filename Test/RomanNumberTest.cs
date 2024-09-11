using App;

namespace Test
{
    [TestClass]
    public class RomanNumberTest
    {
        [TestMethod]
        public void ParseTest()
        {
            Dictionary<string, int> testCases = new()
        {
            { "N",      0      },
            { "I",      1      },
            { "II",     2      },
            { "III",    3      },
            { "IIII",   4      },
            { "V",      5      },
            { "X",      10     },
            { "D",      500    },
            { "IV",     4      },
            { "VI",     6      },
            { "XI",     11     },
            { "IX",     9      },
            { "MM",     2000   },
            { "MCM",    1900   },
        };
            foreach (var testCase in testCases)
            {
                RomanNamber rn = RomanNamber.Parse(testCase.Key);
                Assert.IsNotNull(rn, $"Parse result of '{testCase.Key}' is not null");
                Assert.AreEqual(
                    testCase.Value,
                    rn.Value,
                    $"Parse '{testCase.Key}' => {testCase.Value}"
                );
            }

            /* виняток парсера - окрім причини винятку містить відомості
             * про місце виникнення помилки (позиція у рядку)
             */

            Object[][] exCases = [
                ["W",   'W', 0],
                    ["CS",  'S', 1],
                    ["CX1", '1', 2]
            ];
            foreach (var exCase in exCases)
            {
                var ex = Assert.ThrowsException<FormatException>(
                    () => RomanNamber.Parse(exCase[0].ToString()!),
                    $"RomanNumber.Parse(\"{exCase[0]}\") must throw FormatException"
                );


                //Накладаємо вимоги на повідомлення:
                //Має містити сам символ, що призводить до винятку.
                //Має містити позицію символу в рядку.
                //Має містити назву методу та класу.

                Assert.IsTrue(
                    ex.Message.Contains($"illegal symbol '{exCase[1]}'"),
                    $"ex.Message must contain symbol which cause error:" +
                    $" '{exCase[1]}', ex.Message: {ex.Message}"
);
                Assert.IsTrue(
                    ex.Message.Contains($"in position {exCase[2]}"),
                    $"ex.Message must contain error symbol position, ex.Message: {ex.Message}"
                );
                Assert.IsTrue(
                    ex.Message.Contains(nameof(RomanNamber)) &&
                    ex.Message.Contains(nameof(RomanNamber.Parse)),
                    $"ex.Message must contain names of class and method, ex.Message: {ex.Message}"
                );
            }

            Object[][] exCases2 = [
                    ["VX",   'V', 'X', 0],     //---------
                    ["LC",   'L', 'C', 0],     // "віднять" між цифрами при відніманні:
                    ["DM",   'D', 'M', 0],     // відніматись можуть І, Х, С причому від
                    ["IC",   'I', 'C', 0],     // двох сусідніх цифр (І - від V та X, ...)
                    ["MIM",  'I', 'M', 1],     //---------
                    ["MVM",  'V', 'M', 1],
                    ["MXM",  'X', 'M', 1],
                    ["CVC",  'V', 'C', 1],
                    ["MCVC", 'V', 'C', 2],
                    ["DCIC", 'I', 'C', 2],
                    ["IM",   'I', 'M', 0]
            ];
            foreach (var exCase in exCases2)
            {
                var ex = Assert.ThrowsException<FormatException>(
                    () => RomanNamber.Parse(exCase[0].ToString()!),
                    $"RomanNumber.Parse(\"{exCase[0]}\") must throw FormatException"
                );

                Assert.IsTrue(
                    ex.Message.Contains($"illegal sequence: '{exCase[1]}' befor '{exCase[2]}'"),
                    $"ex.Message must contain symbol which cause error: '{exCase[1]}' and '{exCase[2]}'" +
                    $" '{exCase[0]}', ex.Message: {ex.Message}"
);
                Assert.IsTrue(
                    ex.Message.Contains($"in position {exCase[3]}"),
                    $"ex.Message must contain error symbol position, ex.Message: {ex.Message}"
                );
                Assert.IsTrue(
                    ex.Message.Contains(nameof(RomanNamber)) &&
                    ex.Message.Contains(nameof(RomanNamber.Parse)),
                    $"ex.Message must contain names of class and method, ex.Message: {ex.Message}"
                );
            }


            Object[][] exCases3 = [
                ["IIX", 'X', 2],    // Перед цифрою є декілька цифр, менших за неї
                ["VIX", 'X', 2],    // !! кожна пара цифр - правильна комбінація,
                ["XXC", 'C', 2],    //    проблема створюється щонайменше трьома цифрами
                ["IXC", 'C', 2],    //
            ];
            foreach (var exCase in exCases3)
            {
                var ex = Assert.ThrowsException<FormatException>(
                    () => RomanNamber.Parse(exCase[0].ToString()!),
                    $"RomanNumber.Parse(\"{exCase[0]}\") must throw FormatException"
                );
                Assert.IsTrue(
                    ex.Message.Contains($"illegal sequence: more than one smaller digits befor '{exCase[1]}'"),
                    $"ex.Message must contain symbol which cause error: '{exCase[1]}'. " +
                    $"testCase '{exCase[0]}', testCase: {ex.Message}"
);
                Assert.IsTrue(
                    ex.Message.Contains($"in position {exCase[2]}"),
                    $"ex.Message must contain error symbol position, testCase: {exCase[0]}"
                );
                Assert.IsTrue(
                    ex.Message.Contains(nameof(RomanNamber)) &&
                    ex.Message.Contains(nameof(RomanNamber.Parse)),
                    $"ex.Message must contain names of class and method, testCase: {exCase[0]}"
                );
            }

            Object[][] exCases4 = [
                ["IIX",  'X'],    // Менша цифра після двох однакових
                ["XCC",  'X'],    // 
                ["XCC",  'C'],    // 
                ["XCCC", 'C'],    //
                ["CXCC", 'C'],    //
                ["CMM",  'C'],    //
                ["CMMM", 'C'],    //
                ["MCMM", 'C'],    //

            ];
            foreach (var exCase in exCases4)
            {
                var ex = Assert.ThrowsException<FormatException>(
                    () => RomanNamber.Parse(exCase[0].ToString()!),
                    $"RomanNumber.Parse(\"{exCase[0]}\") must throw FormatException"
                );
            }
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
                    RomanNamber.DigitValue(testCase.Key),
                    $"{testCase.Key} => {testCase.Value}"
                );
            }

            char[] excCases = { '0', '1', 'x', 'i', '&' };

            foreach (var testCase in excCases)
            {
                var ex = Assert.ThrowsException<ArgumentException>(
                    () => RomanNamber.DigitValue(testCase),
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
    }

}
/* Тестовий проєкт за структурою відтворює осноний проєкт:
 * - його папки відповідають папкам основного проєкту
 * - його класи називають як і проєктн, з дописом Test
 * - методи класів також відтворюють методи випробуваних класів
 *      і також дописам Test
 * Основу тестів складають вислови (Assert)
 * 
 */