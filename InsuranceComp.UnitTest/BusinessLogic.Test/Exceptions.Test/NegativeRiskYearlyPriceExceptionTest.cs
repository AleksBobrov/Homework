using InsuranceComp.BusinessLogic.Exceptions;
using NUnit.Framework;

namespace InsuranceComp.UnitTest.BusinessLogic.Test.Exceptions.Test
{
    [TestFixture]
    public class NegativeRiskYearlyPriceExceptionTest
    {
        [Test]
        public void ShouldThrowWithCorrectTextMessage()
        {
            var ex = Assert.Throws<NegativeRiskYearlyPriceException>(() =>
                throw new NegativeRiskYearlyPriceException());

            Assert.AreEqual("Risk yearly price can not be negative.", ex.Message);
        }
    }
}
