using InsuranceComp.BusinessLogic.Exceptions;
using NUnit.Framework;

namespace InsuranceComp.UnitTest.BusinessLogic.Test.Exceptions.Test
{
    [TestFixture]
    public class RiskValidityPeriodExceptionTest
    {
        [Test]
        public void ShouldThrowWithCorrectTextMessage()
        {
            var ex = Assert.Throws<RiskValidityPeriodException>(() =>
                throw new RiskValidityPeriodException());

            Assert.AreEqual("Risk should be within policy validity period.", actual: ex.Message);
        }
    }
}
