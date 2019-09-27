using InsuranceComp.BusinessLogic.Exceptions;
using NUnit.Framework;

namespace InsuranceComp.UnitTest.BusinessLogic.Test.Exceptions.Test
{
    [TestFixture]
    public class DuplicateAvailableRiskExceptionTest
    {
        [Test]
        public void ShouldThrowWithCorrectTextMessage()
        {
            string testRiskName = "test risk";

            var ex = Assert.Throws<DuplicateAvailableRiskException>(() =>
                throw new DuplicateAvailableRiskException(testRiskName));

            Assert.AreEqual($"Available risk with '{testRiskName}' name already exists.", 
                ex.Message);
        }
    }
}
