using InsuranceComp.BusinessLogic;
using InsuranceComp.DataAccess.Providers;
using InsuranceComp.DataAccess.Repositories;
using InsuranceComp.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace InsuranceComp.IntegrationTest.PremiumCalculatorTests
{
    [TestFixture]
    public class PremiumCalculatorTest
    {
        IInsuranceCompany Company;
        IPolicyDataProvider PolicyDataProvder;
        IRiskDataProvider RiskDataProvider;
        IPolicyRepository PolicyRepository;
        IRiskRepository RiskRepository;
        IPolicyService PolicySellService;
        IRiskService RiskService;
        IAvailableRiskUpdater AvailableRiskUpdater;
        IPremiumCalculator PremiumCalculator;
        Risk Risk1;
        Risk Risk2;
        Risk Risk3;
        string DEFAULT_OBJECT_NAME;

        [SetUp]
        public void Setup()
        {
            PolicyDataProvder = new PolicyDataProvider();
            RiskDataProvider = new RiskDataProvider();
            PolicyRepository = new PolicyRepository(PolicyDataProvder);
            RiskRepository = new RiskRepository(RiskDataProvider);
            PolicySellService = new PolicyService(PolicyRepository, RiskRepository, PremiumCalculator);
            RiskService = new RiskService(PolicyRepository, RiskRepository);
            PremiumCalculator = new PremiumCalculator(RiskRepository);
            AvailableRiskUpdater = new AvailableRiskUpdater(Company);
            DEFAULT_OBJECT_NAME = "obj";

            Risk1 = new Risk
            {
                Name = "Test risk 1",
                YearlyPrice = 500.0m
            };

            Risk2 = new Risk
            {
                Name = "Test risk 2",
                YearlyPrice = 800.0m
            };

            Risk3 = new Risk
            {
                Name = "Test risk 3",
                YearlyPrice = 400.0m
            };

            List<Risk> initialAvailableRisks = new List<Risk>();
            initialAvailableRisks.Add(Risk1);
            initialAvailableRisks.Add(Risk2);
            initialAvailableRisks.Add(Risk3);

            Company = new InsuranceCompany("If", initialAvailableRisks,
                PolicySellService, RiskService, PremiumCalculator);
        }

        [TearDown]
        public void StorageCleanUp()
        {
            FakeStorage.Instance.PolicyList = new List<PolicyModel>();
            FakeStorage.Instance.RiskList = new List<RiskModel>();
        }

        [Test]
        public void PremiumCalculator_ShouldCalculatePremiumForSoldPolicyWithOnlyInitialRisks()
        {
            List<Risk> riskList = new List<Risk>();
            riskList.Add(Risk1);
            riskList.Add(Risk2);

            var effectiveDate = DateTime.Now;

            var soldPolicy = Company.SellPolicy(DEFAULT_OBJECT_NAME, effectiveDate, 6, riskList);

            var policyID = IdGenerator.ConstructPolicyId(DEFAULT_OBJECT_NAME, effectiveDate);

            var premium = PremiumCalculator.CalculatePremiumOfSoldPolicy(DEFAULT_OBJECT_NAME, effectiveDate);

            Assert.AreEqual(648.22m, premium);
        }

        [Test]
        public void PremiumCalculator_ShouldCalculatePremiumForSoldPolicyWithRiskAdded()
        {
            List<Risk> riskList = new List<Risk>();
            riskList.Add(Risk1);
            riskList.Add(Risk2);

            var effectiveDate = DateTime.Now;

            var soldPolicy = Company.SellPolicy(DEFAULT_OBJECT_NAME, effectiveDate, 6, riskList);

            var policyID = IdGenerator.ConstructPolicyId(DEFAULT_OBJECT_NAME, effectiveDate);

            Company.AddRisk(DEFAULT_OBJECT_NAME, Risk3, effectiveDate.AddDays(1), effectiveDate);

            var premium = PremiumCalculator.CalculatePremiumOfSoldPolicy(DEFAULT_OBJECT_NAME, effectiveDate);

            Assert.AreEqual(846.58m, premium);
        }

        [Test]
        public void PremiumCalculator_ShouldCalculatePremiumForSoldPolicyWithRiskRemoved()
        {
            List<Risk> riskList = new List<Risk>();
            riskList.Add(Risk1);
            riskList.Add(Risk2);

            var effectiveDate = DateTime.Now;

            var soldPolicy = Company.SellPolicy(DEFAULT_OBJECT_NAME, effectiveDate, 6, riskList);

            var policyID = IdGenerator.ConstructPolicyId(DEFAULT_OBJECT_NAME, effectiveDate);

            Company.RemoveRisk(DEFAULT_OBJECT_NAME, Risk2, effectiveDate.AddDays(10), effectiveDate);

            var premium = PremiumCalculator.CalculatePremiumOfSoldPolicy(DEFAULT_OBJECT_NAME, effectiveDate);

            Assert.AreEqual(271.23m, premium);
        }
    }
}
