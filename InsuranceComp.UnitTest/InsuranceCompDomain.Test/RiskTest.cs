using NUnit.Framework;

namespace InsuranceComp.UnitTest.InsuranceCompDomain.Test
{
    [TestFixture]
    public class RiskTest
    {
        [Test]
        public void Risk_GettersReturnCorrectValues()
        {
            const string name = "risk name";
            const decimal yearlyPrice = 20.0m;

            var risk = new RiskModel()
            {
                Name = name,
                YearlyPrice = yearlyPrice
            };

            Assert.AreEqual(name, risk.Name);
            Assert.AreEqual(yearlyPrice, risk.YearlyPrice);
        }
    }
}
