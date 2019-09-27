using InsuranceComp.BusinessLogic.Exceptions;
using NUnit.Framework;


namespace InsuranceComp.UnitTest.BusinessLogic.Test.Exceptions.Test
{
    [TestFixture]
    public class RiskRemovalDateExceptionTest
    {
        [Test]
        public void ShouldThrowWithCorrectTextMessage()
        {
            var ex = Assert.Throws<RiskRemovalDateException>(() =>
                throw new RiskRemovalDateException());

            Assert.AreEqual("When removing risk, date should not be in past and should be " +
                "within policy validity period.",ex.Message);
        }
    }
}
