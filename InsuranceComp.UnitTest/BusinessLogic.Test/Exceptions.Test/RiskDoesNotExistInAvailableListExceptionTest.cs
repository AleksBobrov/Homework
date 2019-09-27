using InsuranceComp.BusinessLogic.Exceptions;
using NUnit.Framework;

namespace InsuranceComp.UnitTest.BusinessLogic.Test.Exceptions.Test
{
    [TestFixture]
    public class RiskDoesNotExistInAvailableListExceptionTest
    {
        [Test]
        public void ShouldThrowWithCorrectTextMessage()
        {
            string testRiskName = "test risk";

            var ex = Assert.Throws<RiskDoesNotExistInAvailableListException>(() =>
                throw new RiskDoesNotExistInAvailableListException(testRiskName));

            Assert.AreEqual($"Risk with name '{testRiskName}' does not exist in available list.",
                ex.Message);
        }
    }
}
