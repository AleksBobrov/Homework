using InsuranceComp.Helpers;
using NUnit.Framework;
using System;

namespace InsuranceComp.UnitTest.Helpers.Test
{
    [TestFixture]
    public class IdGeneratorTest
    {
        string DEFAULT_OBJECT_NAME = "Obj";
        string DEFAULT_RISK_NAME = "Test risk";
        DateTime DEFAULT_DATE = new DateTime(2020, 5, 5);

        [Test]
        public void IdGenerator_ShouldCorrectlyGeneratePolicyId()
        {
            var expectedPolicyId = $"{DEFAULT_OBJECT_NAME}{DEFAULT_DATE.ToString()}";
            var policyId = IdGenerator.ConstructPolicyId(DEFAULT_OBJECT_NAME, DEFAULT_DATE);

            Assert.AreEqual(expectedPolicyId, policyId);
        }

        [Test]
        public void IdGenerator_ShouldThrowWithNullObjectName()
        {
            Assert.That(() => IdGenerator.ConstructPolicyId(null, DEFAULT_DATE),
                Throws.Exception);
        }

        [Test]
        public void IdGenerator_ShouldCorrectlyGenerateRiskId()
        {
            var expectedRiskId = $"{DEFAULT_RISK_NAME}{DEFAULT_OBJECT_NAME}{DEFAULT_DATE.ToString()}";
            var riskId = IdGenerator.ConstructRiskId(DEFAULT_RISK_NAME, DEFAULT_OBJECT_NAME, DEFAULT_DATE);

            Assert.AreEqual(expectedRiskId, riskId);
        }

        [Test]
        public void IdGenerator_ShouldThrowWithNullRiskName()
        {
            Assert.That(() =>
                IdGenerator.ConstructRiskId(null, DEFAULT_OBJECT_NAME, DEFAULT_DATE),
                Throws.Exception);
        }

        [Test]
        public void IdGenerator_ShouldThrowWithNullParentName()
        {

            Assert.That(() =>
                IdGenerator.ConstructRiskId(DEFAULT_RISK_NAME, null, DEFAULT_DATE),
                Throws.Exception);
        }
    }
}
