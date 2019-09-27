using InsuranceComp.BusinessLogic.Exceptions;
using NUnit.Framework;

namespace InsuranceComp.UnitTest.BusinessLogic.Test.Exceptions.Test
{
    [TestFixture]
    public class PolicyEffectiveDateInPastExceptionTest
    {
        [Test]
        public void ShouldThrowWithCorrectTextMessage()
        {
            var ex = Assert.Throws<PolicyEffectiveDateInPastException>(() =>
                throw new PolicyEffectiveDateInPastException());

            Assert.AreEqual("Policy can not be with effective date in past.",
                ex.Message);
        }
    }
}
