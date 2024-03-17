using CP_KM_JN_2024;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Calculator kalkulator = new Calculator();
            Assert.AreEqual(kalkulator.add(1, 3), 4);
        }
    }
}