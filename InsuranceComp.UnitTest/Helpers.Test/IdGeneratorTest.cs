using InsuranceComp.Helpers;
using NUnit.Framework;
using System;

namespace InsuranceComp.UnitTest.Helpers.Test
{
    [TestFixture]
    public class IdGeneratorTest
    {
        [Test]
        public void IdGenerator_ShouldCorrectlyGeneratePolicyId()
        {
            var policyId = IdGenerator.ConstructPolicyId("Obj", new DateTime(2020, 5, 5));

            Assert.AreEqual("Obj05.05.2020 00:00:00", policyId);
        }

        [Test]
        public void IdGenerator_ShouldThrowWithNullObjectName()
        {
            Assert.That(() => IdGenerator.ConstructPolicyId(null, new DateTime(2020, 5, 5)),
                Throws.Exception);
        }

        [Test]
        public void IdGenerator_ShouldCorrectlyGenerateRiskId()
        {
            var policyId = IdGenerator.ConstructRiskId("Test risk", "Obj", new DateTime(2020, 5, 5));

            Assert.AreEqual("Test riskObj05.05.2020 00:00:00", policyId);
        }

        [Test]
        public void IdGenerator_ShouldThrowWithNullRiskName()
        {
            Assert.That(() =>
                IdGenerator.ConstructRiskId(null, "Obj", new DateTime(2020, 5, 5)),
                Throws.Exception);
        }

        [Test]
        public void IdGenerator_ShouldThrowWithNullParentName()
        {

            Assert.That(() =>
                IdGenerator.ConstructRiskId("Test risk", null, new DateTime(2020, 5, 5)),
                Throws.Exception);
        }
    }
}
