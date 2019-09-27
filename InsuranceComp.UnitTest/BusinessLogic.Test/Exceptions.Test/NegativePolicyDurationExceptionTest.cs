using InsuranceComp.BusinessLogic.Exceptions;
using NUnit.Framework;

namespace InsuranceComp.UnitTest.BusinessLogic.Test.Exceptions.Test
{
    [TestFixture]
    public class NegativePolicyDurationExceptionTest
    {
        [Test]
        public void ShouldThrowWithCorrectTextMessage()
        {
            var ex = Assert.Throws<NegativePolicyDurationException>(() =>
                throw new NegativePolicyDurationException());

            Assert.AreEqual("Policy duration period can not be 0 or negative.",
                ex.Message);
        }
    }
}
