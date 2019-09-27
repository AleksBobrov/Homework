using InsuranceComp.BusinessLogic.Exceptions;
using NUnit.Framework;
using System;

namespace InsuranceComp.UnitTest.BusinessLogic.Test.Exceptions.Test
{
    [TestFixture]
    public class DuplicatePolicyExceptionTest
    {
        [Test]
        public void ShouldThrowWithCorrectTextMessage()
        {
            string testObjectName = "test object";
            DateTime testDate = DateTime.Now;

            var ex = Assert.Throws<DuplicatePolicyException>(() =>
                throw new DuplicatePolicyException(testObjectName, testDate));

            Assert.AreEqual($"Policy with insured object name '{testObjectName}' " +
                  $"and effective date {testDate.ToShortDateString()} already exists.", ex.Message);
        }
    }
}
