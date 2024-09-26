using App;

namespace Tests
{
    [TestClass]
    public class RomanNumberTest
    {
        private Dictionary<char, int> digits = new()
        {
            {'N', 0},
            {'I', 1},
            {'V', 5},
            {'X', 10},
            {'L', 50},
            {'C', 100},
            {'D', 500},
            {'M', 1000},
        };

        [TestMethod]
        public void PlusTest()
        {
            RomanNumber rn1 = new(1),rn2 = new(2);
            Assert.IsInstanceOfType<RomanNumber>(
                rn1.Plus(rn2)
            );
            Assert.AreNotSame( rn1, rn1.Plus(rn2)); 
            Assert.AreNotSame( rn2, rn1.Plus(rn2));

            for (var i = 0; i < 100; i++)
            {
                Assert.AreEqual(
                    i + rn1.Value,
                    rn1.Plus(new(i)).Value,
                    $"1 + {i} --> {1 + i}"
                );
            }
            Assert.ThrowsException<ArgumentNullException>(() => rn1.Plus(null!));

    }

    [TestMethod]
        public void TestToShort()
        {
            RomanNumber rn = new(123);
            short result = (short)rn;
            Assert.AreEqual(123, result);
        }

        [TestMethod]
        public void TestToByte()
        {
            RomanNumber rn = new(100);
            byte result = (byte)rn;
            Assert.AreEqual(100, result);
        }

        [TestMethod]
        public void TestToLong()
        {
            RomanNumber rn = new(1234);
            long result = (long)rn;
            Assert.AreEqual(1234, result);
        }

        [TestMethod]
        public void TestToInt()
        {
            RomanNumber rn = new(567);
            int result = (int)rn;
            Assert.AreEqual(567, result);
        }

        [TestMethod]
        public void TestToFloat()
        {
            RomanNumber rn = new(345);
            float result = (float)rn;
            Assert.AreEqual(345f, result);
        }

        [TestMethod]
        public void TestToDouble()
        {
            RomanNumber rn = new(678);
            double result = (double)rn;
            Assert.AreEqual(678d, result);
        }

        [TestMethod]
        public void TestParseToInt()
        {
            RomanNumber rn = RomanNumber.Parse("CXXIII"); // 123
            int result = (int)rn;
            Assert.AreEqual(123, result);
        }

        [TestMethod]
        public void ToStringTest()
        {
            var testCases = new Dictionary<int, String>()
            {
                {4 , "IV"},
                {6 , "VI"},
                {19 , "XIX"},
                {49 , "XLIX"},
                {95 , "XCV"},
                {444 , "CDXLIV"},
                {946 , "CMXLVI"},
                {3333 , "MMMCCCXXXIII"},
            }
            .Concat(digits.Select(d => new
                KeyValuePair<int, string>(d.Value, d.Key.ToString()))
            )
            .ToDictionary();
            foreach (var testCase in testCases)
            {
                RomanNumber rn = new(testCase.Key);
                var value = rn.ToString();
                Assert.IsNotNull(value);
                Assert.AreEqual(testCase.Value, value);
            }
        }

        [TestMethod]
        public void CrossTest_Parse_ToString()
        {
            for (int i = 0; i <= 1000; i++)
            {
                int c = RomanNumberParser.FromString(new RomanNumber(i).ToString()!).Value;
                Assert.AreEqual(
                    i,
                    c,
                    $"Cross test for {i}: {new RomanNumber(i)} -> {c}"
                );
            }
        }

        [TestMethod]
        public void ParseTest()
        {
            TestCase[] testCases =
            [
                new( "N", 0),
                new( "I",  1),
                new( "II",  2),
                new( "III",  3),
                new( "IIII",  4),
                new( "V",  5),
                new( "X",  10),
                new( "D",  500),
                new( "IV",  4),
                new( "VI",  6),
                new( "XI",  11),
                new( "IX",  9),
                new( "MM",  2000),
                new( "MCM", 1900),
                #region HW
                new( "XL", 40),
                new( "XC", 90),
                new( "CD", 400),
                new( "CMII", 902),
                new( "DCCCC", 900),
                new( "CCCC", 400),
                new( "XXXXX", 50),
                #endregion

            ];
            foreach (var testCase in testCases)
            {
                RomanNumber rn = RomanNumber.Parse(testCase.Source);
                Assert.IsNotNull(rn, $"Parse result of '{testCase.Source}' is not null");
                Assert.AreEqual(
                    testCase.Value,
                    rn.Value,
                    $"Parse '{testCase.Source}' => {testCase.Value}"
                    );
            }
           
            String src = "RomanNumber.Parse('%r1') error: illegal";
            String pos = "in position";
            String illegalSymbolTemplate = $"{src} symbol '%r2' {pos} %r3";
            String tpl2 = $"{src} sequence: more than one smaller digits before '%r2' {pos} %r3";
            String tpl3 = $"{src} sequence: '%r2' before '%r3' {pos} %r4";
            testCases = [
                new( ["W", 'W', 0, illegalSymbolTemplate ]),
                new( ["CS",  'S', 1, illegalSymbolTemplate ]),
                new( ["CX1", '1', 2, illegalSymbolTemplate ]),
           
                new(["IIX", 'X', 2, tpl2]),
                new(["VIX", 'X', 2, tpl2]),
                new(["XXC", 'C', 2, tpl2]),
                new(["IXC", 'C', 2, tpl2]),
                
                new(["VX",  'V', 'X', 0, tpl3]),
                new(["LC",  'L', 'C', 0, tpl3]),
                new(["DM",  'D', 'M', 0, tpl3]),
                new(["IC",  'I', 'C', 0, tpl3]),
                new(["MIM", 'I', 'M', 1, tpl3]),
                new(["MVM", 'V', 'M', 1, tpl3]),
                new(["MXM", 'X', 'M', 1, tpl3]),
                new(["CVC", 'V', 'C', 1, tpl3]),
                new(["MCVC",'V', 'C', 2, tpl3]),
                new(["DCIC",'I', 'C', 2, tpl3]),
                new(["IM",  'I', 'M', 0, tpl3]),
                ];
            foreach (var exCase in testCases)
            {
                var ex = Assert.ThrowsException<FormatException>(
                    () => RomanNumber.Parse(exCase.Source),
                    $"RomanNumber.Parse(\"{exCase.Source}\") must throw FormatException"
                    );
                Assert.AreEqual(
                    exCase.ExMessage,
                    ex.Message,
                    $"ex.Message must contain '{exCase.ExMessage}'; ex.Message: {ex.Message}");
     
                //foreach (String part in exCase.ExMessageParts!)
                //{
                //    Assert.IsTrue(
                //    ex.Message.Contains(part),
                //    $"ex.Message must contain '{part}'; ex.Message: {ex.Message}"
                //    );
                //}
            }

        }

        [TestMethod]
        public void DigitValueChar()
        {
            //foreach (var testCase in digits)
            //{
            //    Assert.AreEqual(testCase.Value,
            //        RomanNumber.DigitValue(testCase.Key),
            //        $"{testCase.Key} => {testCase.Value}"
            //        );
            //}

            //char[] excCases = { '1', 'x', 'i', '&' };

            //foreach (var testCase in excCases)
            //{
            //    var ex = Assert.ThrowsException<ArgumentException>(
            //    () => RomanNumber.DigitValue(testCase),
            //    $"DigitValue({testCase}) must throw ArgumentException"
            //    );
            //    Assert.IsTrue(
            //    ex.Message.Contains($"'{testCase}'"),
            //        "DigitValue ex.Message should contain a symbol which cause exception:" +
            //        $" symbol: '{testCase}', ex.Message: '{ex.Message}'"
            //        );
            //    Assert.IsTrue(
            //        ex.Message.Contains($"{nameof(RomanNumber)}") &&
            //        ex.Message.Contains($"{nameof(RomanNumber.DigitValue)}"),
            //        "DigitValue ex.Message should contain a symbol which cause exception:" +
            //        $" symbol: '{testCase}', ex.Message: '{ex.Message}'"
            //        );
            //}

            #region HW2

            char[] excCases = { '0', '1', 'x', 'i', '&' };

            foreach (var digit in excCases)
            {
                var ex = Assert.ThrowsException<ArgumentException>(
                () => RomanNumberParser.DigitValue(digit),
                $"DigitValue({digit}) must throw ArgumentException"
                );
                Assert.IsTrue(
                    ex.Message.Contains($"'{digit}'"),
                    "Not valid Roman digit:" +
                    $" argument: ('{digit}'), ex.Message: {ex.Message}"
                    );
                Assert.IsTrue(
                    ex.Message.Contains($"{nameof(RomanNumber)}") &&
                    ex.Message.Contains($"DigitValue"),
                    "Not valid Roman digit:" +
                    $" argument: ('{digit}'), ex.Message: {ex.Message}"
                    );
            }

            #endregion
        }
    }

    record TestCase(String Source, int? Value, String? ExMessage = null)
    {
        public TestCase(String Source, String? ExMessagets)
            : this(Source, null, ExMessagets) { }

        public TestCase(List<Object> data) :
            this(
                data.First().ToString()!,
                null,
                data.Last().ToString()!.R(data[..^1])
                )
        { }
    }

    public static class StringExtension
    {
        public static String R(
            this String Source,
            IEnumerable<Object> replaces)
        {
            String res = Source;
            int i = 0;
            foreach (var r in replaces)
            {
                ++i;
                res = res.Replace($"%r{i}", r.ToString());
            }
            return res;
        }
    }

}