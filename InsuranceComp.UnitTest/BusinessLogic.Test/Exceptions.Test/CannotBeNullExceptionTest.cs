using InsuranceComp.BusinessLogic.Exceptions;
using NUnit.Framework;

namespace InsuranceComp.UnitTest.BusinessLogic.Test.Exceptions.Test
{
    [TestFixture]
    public class CannotBeNullExceptionTest
    {
        [Test]
        public void ShouldThrowWithCorrectTextMessage()
        {
            string testVariable = "test";

            var ex = Assert.Throws<CannotBeNullException>(() => 
                throw new CannotBeNullException(testVariable));

            Assert.AreEqual("Argument 'test' can not be null.", ex.Message);
        }
    }
}
