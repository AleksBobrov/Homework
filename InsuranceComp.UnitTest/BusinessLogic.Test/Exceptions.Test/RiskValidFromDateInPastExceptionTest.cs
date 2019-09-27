using InsuranceComp.BusinessLogic.Exceptions;
using NUnit.Framework;

namespace InsuranceComp.UnitTest.BusinessLogic.Test.Exceptions.Test
{
    [TestFixture]
    public class RiskValidFromDateInPastExceptionTest
    {
        [Test]
        public void ShouldThrowWithCorrectTextMessage()
        {
            var ex = Assert.Throws<RiskValidFromDateInPastException>(() =>
                throw new RiskValidFromDateInPastException());

            Assert.AreEqual("Risk can not be with valid from date in past.", actual: ex.Message);
        }
    }
}
