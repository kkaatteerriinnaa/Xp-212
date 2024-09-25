using App;
using System.Reflection;

namespace Tests
{
    [TestClass]
    public class RomanNumberValidatorTest
    {
        //private TestCase[] casesCheckSymbols = [
        //        new( "W",   ["W","0"]),
        //        new( "CS",  ["S","1"]),
        //        new( "CX1", ["1","2"]),
        //    ];


        [TestMethod]
        public void ValidateTest()
        {
            String src = "RomanNumber.Parse('%r1') error: illegal";
            String pos = "in position";
            String illegalSymbolTemplate = $"{src} number zero sequence {pos} %r2";
            TestCase[] casesCheckZeroDigitTest = [
                new(["NN",   0, illegalSymbolTemplate]),
                new(["IN",   1, illegalSymbolTemplate]),
                new(["NX",   0, illegalSymbolTemplate]),
                new(["NC",   1, illegalSymbolTemplate]),
                new(["XNC",  1, illegalSymbolTemplate]),
                new(["XVIN", 3, illegalSymbolTemplate]),
                new(["XNNC", 1, illegalSymbolTemplate]),
                new(["NMC",  1, illegalSymbolTemplate]),
                new(["NIX",  1, illegalSymbolTemplate]),
            ];
            foreach (var exCase in casesCheckZeroDigitTest)
            {
                var ex = Assert.ThrowsException<FormatException>(
                    () => RomanNumberValidator.Validate(exCase.Source),
                    $"RomanNumberValidator.Validate(\"{exCase.Source}\") must throw FormatException"
                );
            }
        }


        [TestMethod]
        public void CheckSymbolTest()
        {

        }

        [TestMethod]
        public void DigitValueTest()
        {
            //Dictionary<char, int> testCases = new()
            //{
            //    {'N', 0},
            //    {'I', 1},
            //    {'V', 5},
            //    {'X', 10},
            //    {'L', 50},
            //    {'C', 100},
            //    {'D', 500},
            //    {'M', 1000},
            //};
            //foreach (var testCase in testCases)
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
            //    () => RomanNumberValidator.DigitValue(testCase),
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
                () => RomanNumberValidator.DigitValue(digit),
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

        [TestMethod]
        public void DigitRatiosTest()
        {
            Object[][] exCases2 = [
                ["VX", "V", "X", 0],  // ---
                ["LC", "L", "C", 0],  // "відстань" між цифрами при відніманні:
                ["DM", "D", "M", 0],  // відніматись можуть I, X, СI причому від
                ["IC", "I", "C", 0],  // двох сусідніх цифр (I - від V та X, ... )
                ["MIM", "I", "M", 1],
                ["MVM", "V", "M", 1],
                ["MXM", "X", "M", 1],
                ["CVC", "V", "C", 1],
                ["MCVC", "V", "C", 2],
                ["DCIC", "I", "C", 2],
                ["IM", "I", "M", 0],
            ];
            foreach (var exCase in exCases2)
            {
                var ex = Assert.ThrowsException<FormatException>(
                    () => RomanNumber.Parse(exCase[0].ToString()!),
                    $"RomanNumber.Parse(\"{exCase[0]}\") must throw FormatException"
                    );
                Assert.IsTrue(
                    ex.Message.Contains($"illegal sequence: '{exCase[1]}' before '{exCase[2]}'"),
                    $"ex.Message must contain symbols which cause error: '{exCase[1]}' and '{exCase[2]}', testCase: '{exCase[0]}', ex.Message: {ex.Message}"
                    );
                Assert.IsTrue(
                    ex.Message.Contains($"in position {exCase[3]}"),
                    $"ex.Message must contain error symbol position, testCase: '{exCase[0]}'"
                    );
                Assert.IsTrue(
                    ex.Message.Contains(nameof(RomanNumber)) &&
                    ex.Message.Contains(nameof(RomanNumber)),
                    $"ex.Message must contain names of class and method, testCase: '{exCase[0]}', ex.Message: {ex.Message}"
                    );
            }
        }

        [TestMethod]
        public void InvalidLessCounterTest()
        {
            Object[][] exCases3 = [
                [ "IIX", 'X', 2 ],   // Перед цифрою є декілька цифр, менших за неї
                [ "VIX", 'X', 2 ],   // !! кожна пара цифр - правильна комбінація,
                [ "XXC", 'C', 2 ],   //    проблема створюється щонайменше трьома цифрами
                [ "IXC", 'C', 2 ],
                [ "IIIV", 'V', 3]
            ];
            foreach (var exCase in exCases3)
            {
                var ex = Assert.ThrowsException<FormatException>(
                    () => RomanNumber.Parse(exCase[0].ToString()!),
                    $"RomanNumber.Parse(\"{exCase[0]}\") must throw FormatException"
                    );
                Assert.IsTrue(
                    ex.Message.Contains($"illegal sequence: more than one smaller digits before '{exCase[1]}'"),
                    $"ex.Message must contain symbol before error: '{exCase[1]}', testCase: '{exCase[0]}', ex.Message: {ex.Message}"
                    );
                Assert.IsTrue(
                    ex.Message.Contains($"in position {exCase[2]}"),
                    $"ex.Message must contain error symbol position, testCase: '{exCase[0]}'"
                    );
                Assert.IsTrue(
                    ex.Message.Contains(nameof(RomanNumber)) &&
                    ex.Message.Contains(nameof(RomanNumber)),
                    $"ex.Message must contain names of class and method, testCase: '{exCase[0]}', ex.Message: {ex.Message}"
                    );
            }
        }

        [TestMethod]
        public void MaxAndLessCountersTest()
        {
            String src = "RomanNumber.Parse('%r1') error: illegal";
            String pos = "in position";
            String maxAndLessCountersTemplate = $"{src} sequence: more than one smaller digits before '%r2' {pos} %r3";

            TestCase[] testCases = [
                new( ["IXX", 'I',  0, maxAndLessCountersTemplate]),
                new( ["IXXX", 'I', 0, maxAndLessCountersTemplate]),
                new( ["XCC", 'X',  0, maxAndLessCountersTemplate]),
                new( ["XCCC", 'X', 0, maxAndLessCountersTemplate]),
                new( ["CXCC", 'X', 1, maxAndLessCountersTemplate]),
                new( ["CMM", 'C',  0, maxAndLessCountersTemplate]),
                new( ["CMMM", 'C', 0, maxAndLessCountersTemplate]),
                new( ["MCMM", 'C', 1, maxAndLessCountersTemplate]),
                new( ["LCC", 'L',  0, maxAndLessCountersTemplate]),
                new( ["ICCC", 'I', 0, maxAndLessCountersTemplate])
            ];
            foreach (var exCase in testCases)
            {
                var ex = Assert.ThrowsException<FormatException>(
                    () => RomanNumber.Parse(exCase.Source.ToString()!),
                    $"RomanNumber.Parse(\"{exCase.Source}\") must throw FormatException"
                    );
            }
        }

        [TestMethod]
        public void CheckZeroDigitTest()
        {
            /* Тестування приватних методів - задача потрібна, але вимагає
             * особливого підходу
             */
            // Об'єктна рефлексія - робота з типами даних
            Type rnvType = typeof(RomanNumberValidator);  // дані про тип
            String methodName = "CheckZeroDigit";
            MethodInfo? method = rnvType                    // шукаємо метод за
                .GetMethod(methodName,                      // назвою, та такий, що
                BindingFlags.Static |                       // є статичним та
                BindingFlags.NonPublic);                    // не public

            Assert.IsNotNull(method, $"Method '{methodName}' must be in type");

            //foreach (var exCase in casesCheckZeroDigitTest)
            //{
            //    // !! Виконання методів з винятками через рефлексію
            //    // призводить до появи окремого винятку:
            //    // TargetInvocationException замість будь-якого винятку,
            //    // що викидається у самому методі. Дані про внутрішній
            //    // виняток збираються у поле eх. InnerException
            //    // !! Також зауважимо, що Assert перевіряє типи суворо, тобто
            //    // зазначити загальний Exception також буде помилкою
            //    var ex = Assert.ThrowsException<TargetInvocationException>(
            //        () => //RomanNumberValidator.CheckZeroDigit(exCase[0].ToString()!),
            //              method.Invoke(                    // Виклик методу через тип
            //                  null,                         // null - немає об'єкту (статичний)
            //                  [exCase.Source.ToString()!]),     // параметри, навіть один іде масивом
            //        $"method.Invoke(\"{exCase.Source}\") must throw TargetInvocationException"
            //        );
            //    Assert.AreEqual(
            //        "FormatException",
            //        ex.InnerException?.GetType().Name,
            //        $"RomanNumberValidator.{methodName}(\"{exCase.Source}\") must throw FormatException"
            //        );
            //}
        }
    }
}