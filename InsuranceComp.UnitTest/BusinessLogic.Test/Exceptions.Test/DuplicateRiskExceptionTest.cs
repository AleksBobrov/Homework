using InsuranceComp.BusinessLogic.Exceptions;
using NUnit.Framework;

namespace InsuranceComp.UnitTest.BusinessLogic.Test.Exceptions.Test
{
    [TestFixture]
    public class DuplicateRiskExceptionTest
    {
        [Test]
        public void ShouldThrowWithCorrectTextMessage()
        {
            string testRiskName = "test risk";

            var ex = Assert.Throws<DuplicateRiskException>(() =>
                throw new DuplicateRiskException(testRiskName));

            Assert.AreEqual($"Risk with '{testRiskName}' name already exists on that policy.",
                ex.Message);
        }
    }
}
