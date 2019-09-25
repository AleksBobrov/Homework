using InsuranceComp.BusinessLogic;
using InsuranceComp.BusinessLogic.Exceptions;
using InsuranceComp.DataAccess.Providers;
using InsuranceComp.DataAccess.Repositories;
using InsuranceComp.Helpers;
using NUnit.Framework;
using System;

namespace IdGeneratorTests
{
    [TestFixture]
    public class IdGeneratorTests
    {
        IRiskRepository RiskRepository;
        IPremiumCalculator PremiumCalculator;

        [SetUp]
        public void Setup()
        {
            IRiskDataProvider riskDataProvider = new RiskDataProvider();
            RiskRepository = new RiskRepository(riskDataProvider);
            PremiumCalculator = new PremiumCalculator(RiskRepository);
        }

        [Test]
        public void IdGenerator_ShouldCorrectlyGeneratePolicyId()
        {
            var policyId = IdGenerator.ConstructPolicyId("Obj", new DateTime(2020, 5, 5));

            Assert.AreEqual("Obj05.05.2020 00:00:00", policyId);
        }

        [Test]
        public void IdGenerator_ShouldNotAllowGeneratePolicyIdWithNullObjectName()
        {
            Exception ex = Assert.Throws<CannotBeNullException>(() =>
                IdGenerator.ConstructPolicyId(null, new DateTime(2020, 5, 5)));

            Assert.AreEqual("Parameter can not be null.", ex.Message);
        }

        [Test]
        public void IdGenerator_ShouldCorrectlyGenerateRiskId()
        {
            var policyId = IdGenerator.ConstructRiskId("Test risk", "Obj", new DateTime(2020, 5, 5));
            
            Assert.AreEqual("Test riskObj05.05.2020 00:00:00", policyId);
        }

        [Test]
        public void IdGenerator_ShouldNotAllowGenerateRiskIdWithNullRiskName()
        {
            Exception ex = Assert.Throws<CannotBeNullException>(() =>
                IdGenerator.ConstructRiskId(null, "Obj", new DateTime(2020, 5, 5)));

            Assert.AreEqual("Parameter can not be null.", ex.Message);
        }

        [Test]
        public void IdGenerator_ShouldNotAllowGenerateRiskIdWithNullParentName()
        {
            Exception ex = Assert.Throws<CannotBeNullException>(() =>
                IdGenerator.ConstructRiskId("Test risk", null, new DateTime(2020, 5, 5)));

            Assert.AreEqual("Parameter can not be null.", ex.Message);
        }
    }
}
