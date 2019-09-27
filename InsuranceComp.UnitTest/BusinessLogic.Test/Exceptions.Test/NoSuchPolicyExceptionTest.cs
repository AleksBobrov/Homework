using InsuranceComp.BusinessLogic.Exceptions;
using NUnit.Framework;
using System;

namespace InsuranceComp.UnitTest.BusinessLogic.Test.Exceptions.Test
{
    [TestFixture]
    public class NoSuchPolicyExceptionTest
    {
        [Test]
        public void ShouldThrowWithCorrectTextMessage()
        {
            string testObjectName = "test object";
            DateTime testEffectiveDate = DateTime.Now;

            var ex = Assert.Throws<NoSuchPolicyException>(() =>
                throw new NoSuchPolicyException(testObjectName, testEffectiveDate));

            Assert.AreEqual($"Policy with name '{testObjectName}' and effective date" +
                  $"{testEffectiveDate.ToShortDateString()} does not exist.",
                ex.Message);
        }
    }
}
