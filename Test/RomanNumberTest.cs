using App;
using System.Runtime.CompilerServices;

namespace Test
{
    [TestClass]
    public class RomanNumberTest
    {

        private Dictionary<char, int> digits = new()
             {
                { 'I', 1 },
                { 'V', 5 },
                { 'X', 10 },
                { 'L', 50 },
                { 'C', 100 },
                { 'D', 500 },
                { 'M', 1000 }
             };


        [TestMethod]
        public void ToString_TestCases()
        {
            var testCases = new Dictionary<int, string>()
            {
                { 4, "IV" },
                { 6, "VI" },
                { 19, "XIX" },
                { 49, "XLIX" },
                { 95, "XCV" },
                { 444, "CDXLIV" },
                { 3333, "MMMCCCXXXIII" }
            }.Concat(digits.Select(d => new KeyValuePair<int, string>(d.Value, d.Key.ToString())))
             .ToDictionary();


            foreach (var testCase in digits)
            {
                Assert.AreEqual(
                    RomanNamberParse.DigitValue(testCase.Key),
                    testCase.Value,
                    $"{testCase.Key} => {testCase.Value}"
                );
            }

        }

        public void CrossTest_Parse_ToString()
        {
            // Наявність двох методів прототипної роботи дозволяє
            // використовувати крос-тести, які послідовно застосовують
            // два методи одержувати початковий результат
            // "XVII" -> Parse -> 17 -> ToString -> "XVII"
            // "III" -> Parse -> 3 -> ToString -> "III"
            // 4 -> ToString -> "IV" -> Parse -> 4
            for (int i = 0; i <= 1000; ++i)
            {
                int c = RomanNamberParse.FormString(new RomanNamber(i).ToString()!).Value;

                Assert.AreEqual(
                    i,
                    c,
                    $"Cross test for {i}: {new RomanNamber(i)} -> {c}"
                );
            }

        }

        [TestMethod]
        public void ParseTest()
        {
            TestCase[] testCases = [
              new ( "N",     0    ),
              new ( "I",     1    ),
              new ( "II",    2    ),
              new ( "III",   3    ),
              new ( "IIII",  4    ),
              new ( "V",     5    ),
              new ( "IX",    9    ),
              new ( "XL",    40   ),
              new ( "L",     50   ),
              new ( "XC",    90   ),
              new ( "CD",    400  ),
              new ( "D",     500  ),
              new ( "M",     1000 ),
              new ( "MM",    2000 ),
              new ( "MCM",   1900 ),
            ];

            foreach (var testCase in testCases)
            {
                RomanNamber rn = RomanNamber.Parse(testCase.Source);
                Assert.IsNotNull(rn, $"Parse result of '{testCase.Source}' is not null");
                Assert.AreEqual(
                    testCase.Value,
                    rn.Value,
                    $"Parse '{testCase.Source}' => {testCase.Value}"
                );
            }

            /* виняток парсера - окрім причини винятку містить відомості
             * про місце виникнення помилки (позиція у рядку)
             */
            String tpl1 = "illegal symbol '%r1'";
            String tpl2 = "in position %r1";
            String tpl3 = "RomanNamber.Parse";
            String tpl4 = "illegal sequence: more than one smaller digits before '%c'";
            String tpl5 = "illegal sequence: '%r1' before '%r2'";
            String[] all = [tpl3];
            testCases = [
                new( "W",    [ tpl1.R(["W"]), tpl2.R(["0"]), tpl3 ] ),
                new( "CS",   [ tpl1.R(["S"]), tpl2.R(["1"]), tpl3 ] ),
                new( "CX1",  [ tpl1.R(["1"]), tpl2.R(["2"]), tpl3 ] ),
               
                new( "IIX",  [ tpl4.Replace("%c", "X"), tpl2.R(["2"]), tpl3 ] ),
                new( "VIX",  [ tpl4.Replace("%c", "X"), tpl2.R(["2"]), tpl3 ] ),
                new( "XXC",  [ tpl4.Replace("%c", "C"), tpl2.R(["2"]), tpl3 ] ),
                new( "IXC",  [ tpl4.Replace("%c", "C"), tpl2.R(["2"]), tpl3 ] ),


                new( "VX",   [ tpl5.R(["V", "X"]), tpl2.R(["0"]), tpl3 ] ),
                new( "LC",   [ tpl5.R(["L", "C"]), tpl2.R(["0"]), tpl3 ] ),
                new( "DM",   [ tpl5.R(["D", "M"]), tpl2.R(["0"]), tpl3 ] ),
                new( "IC",   [ tpl5.R(["I", "C"]), tpl2.R(["0"]), tpl3 ] ),
                new( "MIM",  [ tpl5.R(["I", "M"]), tpl2.R(["1"]), tpl3 ] ),
                new( "MVM",  [ tpl5.R(["V", "M"]), tpl2.R(["1"]), tpl3 ] ),
                new( "MXM",  [ tpl5.R(["X", "M"]), tpl2.R(["1"]), tpl3 ] ),
                new( "CVC",  [ tpl5.R(["V", "C"]), tpl2.R(["1"]), tpl3 ] ),
                new( "MCVC", [ tpl5.R(["V", "C"]), tpl2.R(["2"]), tpl3 ] ),
                new( "DCIC", [ tpl5.R(["I", "C"]), tpl2.R(["2"]), tpl3 ] ),
                new( "IM",   [ tpl5.R(["I", "M"]), tpl2.R(["0"]), tpl3 ] ),


];
            foreach (var exCase in testCases)
            {
                var ex = Assert.ThrowsException<FormatException>(
                    () => RomanNamber.Parse(exCase.Source),
                    $"RomanNumber.Parse(\"{exCase.Source}\") must throw FormatException"
                );
                foreach (String part in exCase.ExMassageParts!)
                {
                    Assert.IsTrue(
                       ex.Message.Contains(part),
                       $"ex.Message must contain '{part}'; ex.Message: {ex.Message}" 
                    );
                }
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
    }

    record TestCase(String Source, int? Value, IEnumerable<String>? ExMassageParts = null )
    {
        public TestCase(String Sourse, IEnumerable<String> parts)
            : this(Sourse, null, parts) { }
    }

    public static class StringExtension
    {
        public static String F(this String Source, IEnumerable<String> olds, IEnumerable<String> news)
        {
            String res = Source;
            foreach (var item in olds.Zip(news))
            {
               res = res.Replace(item.First, item.Second);
            }
            return res;
        }

        public static String R(
            this String Source, 
            IEnumerable<String> replaces)
        {
            String res = Source;
            int i = 0;
            foreach(String r in replaces)
            {
                i++;
                res = res.Replace($"%r{i}", r);
            }
            return res;
        }
    }
}