using App;

namespace Test
{
    [TestClass]
    public class RomanNumberTest
    {
        [TestMethod]
        public void ParseTest()
        {
            RomanNamber rn = RomanNamber.Parse("");
            Assert.IsNotNull(rn, "Parse result is not null");
            Assert.AreEqual(0, rn.Value, "Zero testing");
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