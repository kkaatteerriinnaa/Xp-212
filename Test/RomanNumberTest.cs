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