using InsuranceComp.BusinessLogic.Exceptions;
using InsuranceComp.Helpers;
using NUnit.Framework;
using System;

namespace InsuranceComp.IntegrationTest.IdGeneratorTests
{
    [TestFixture]
    public class IdGeneratorTests
    { 
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void IdGenerator_ShouldNotAllowGeneratePolicyIdWithNullObjectName()
        {
            Exception ex = Assert.Throws<CannotBeNullException>(() =>
                IdGenerator.ConstructPolicyId(null, new DateTime(2020, 5, 5)));

            Assert.AreEqual("Argument 'name' can not be null.", ex.Message);
        }

        [Test]
        public void IdGenerator_ShouldNotAllowGenerateRiskIdWithNullRiskName()
        {
            Exception ex = Assert.Throws<CannotBeNullException>(() =>
                IdGenerator.ConstructRiskId(null, "Obj", new DateTime(2020, 5, 5)));

            Assert.AreEqual("Argument 'riskName' can not be null.", ex.Message);
        }

        [Test]
        public void IdGenerator_ShouldNotAllowGenerateRiskIdWithNullParentName()
        {
            Exception ex = Assert.Throws<CannotBeNullException>(() =>
                IdGenerator.ConstructRiskId("Test risk", null, new DateTime(2020, 5, 5)));

            Assert.AreEqual("Argument 'parentName' can not be null.", ex.Message);
        }
    }
}
