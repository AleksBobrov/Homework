using FluentAssertions;
using InsuranceComp.BusinessLogic;
using InsuranceComp.BusinessLogic.Exceptions;
using InsuranceComp.DataAccess.Providers;
using InsuranceComp.DataAccess.Repositories;
using InsuranceComp.Helpers;
using InsuranceComp.InsuranceCompDomain;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace InsuranceComp.IntegrationTest.InsuranceCompanyTests
{
    [TestFixture]
    public class InsuranceCompanyTest
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
            List<Risk> initialAvailableRisks = new List<Risk>();
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
                YearlyPrice = 5.5m
            };

            Risk2 = new Risk
            {
                Name = "Test risk 2",
                YearlyPrice = 5.1m
            };

            Risk3 = new Risk
            {
                Name = "Test risk 3",
                YearlyPrice = 5.9m
            };

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
        public void InsuranceCompany_ShouldBeAbleToSellPolicyWithInitialListOfRisks()
        {
            Company.SellPolicy(DEFAULT_OBJECT_NAME, DateTime.Now, 6, new List<Risk>());
            Assert.AreEqual(1, FakeStorage.Instance.PolicyList.Count);
        }

        [Test]
        public void InsuranceCompany_ShouldBeAbleToGetPolicy()
        {
            List<Risk> riskList = new List<Risk>();
            riskList.Add(Risk1);
            riskList.Add(Risk2);

            var effectiveDate = DateTime.Now;
            var validTill = effectiveDate.AddMonths(6);

            Company.SellPolicy(DEFAULT_OBJECT_NAME, effectiveDate, 6, riskList);

            var premium = PremiumCalculator.CalculateInitialPremium(riskList, effectiveDate, validTill);

            var expectedPolicy = new Policy {
                NameOfInsuredObject = DEFAULT_OBJECT_NAME,
                ValidFrom = effectiveDate,
                ValidTill = validTill,
                InsuredRisks = riskList,
                Premium = premium
            };

            var policy = Company.GetPolicy(DEFAULT_OBJECT_NAME, effectiveDate);

            expectedPolicy.Should().BeEquivalentTo(policy);
        }

        [Test]
        public void InsuranceCompany_ShouldBeAbleToGetPolicyWithCorrectAmountOfRisksAfterNewRiskAdded()
        {
            List<Risk> riskList = new List<Risk>();
            riskList.Add(Risk1);
            riskList.Add(Risk2);

            var effectiveDate = DateTime.Now;
            var validTill = effectiveDate.AddMonths(6);

            Company.SellPolicy(DEFAULT_OBJECT_NAME, effectiveDate, 6, riskList);

            Company.AddRisk(DEFAULT_OBJECT_NAME, Risk3, effectiveDate.AddDays(1), effectiveDate);

            var policy = Company.GetPolicy(DEFAULT_OBJECT_NAME, effectiveDate);

            Assert.AreEqual(3, policy.InsuredRisks.Count);
        }

        [Test]
        public void InsuranceCompany_ShouldThrowExceptionIfGetPolicyDoesNotExist()
        {
            var effectiveDate = DateTime.Now;

            Exception ex = Assert.Throws<NoSuchPolicyException>(() =>
                Company.GetPolicy(DEFAULT_OBJECT_NAME, effectiveDate)
            );

            Assert.AreEqual(0, FakeStorage.Instance.PolicyList.Count);
            Assert.AreEqual($"Policy with name '{DEFAULT_OBJECT_NAME}' and effective date" +
                  $"{effectiveDate.ToShortDateString()} does not exist.", ex.Message);
        }

        [Test]
        public void InsuranceCompany_ShouldNotBeAbleToSellPolicyWithEmptyName()
        {
            Exception ex = Assert.Throws<CannotBeNullException>(() =>
                Company.SellPolicy("", DateTime.Now, 6, new List<Risk>())
            );

            Assert.AreEqual(0, FakeStorage.Instance.PolicyList.Count);
            Assert.AreEqual("Argument 'nameOfInsuredObject' can not be null.", ex.Message);
        }

        [Test]
        public void InsuranceCompany_ShouldNotBeAbleToSellPolicyWithNullName()
        {
            Exception ex = Assert.Throws<CannotBeNullException>(() =>
                Company.SellPolicy(null, DateTime.Now, 6, new List<Risk>())
            );

            Assert.AreEqual(0, FakeStorage.Instance.PolicyList.Count);
            Assert.AreEqual("Argument 'nameOfInsuredObject' can not be null.", ex.Message);
        }

        [Test]
        public void InsuranceCompany_ShouldNotBeAbleToSellPolicyWithEffectiveDateInPast()
        {
            Exception ex = Assert.Throws<PolicyEffectiveDateInPastException>(() =>
               Company.SellPolicy(DEFAULT_OBJECT_NAME, DateTime.Now.AddDays(-1), 6, new List<Risk>())
            );

            Assert.AreEqual(0, FakeStorage.Instance.PolicyList.Count);
            Assert.AreEqual("Policy can not be with effective date in past.", ex.Message);
        }

        [Test]
        public void InsuranceCompany_ShouldNotBeAbleToSellPolicyWithValidMonthsEqualToZero()
        {
            Exception ex = Assert.Throws<NegativePolicyDurationException>(() =>
               Company.SellPolicy(DEFAULT_OBJECT_NAME, DateTime.Now, 0, new List<Risk>())
            );

            Assert.AreEqual(0, FakeStorage.Instance.PolicyList.Count);
            Assert.AreEqual("Policy duration period can not be 0 or negative.", ex.Message);
        }

        [Test]
        public void InsuranceCompany_ShouldNotBeAbleToSellPolicyWithNegativeValidMonths()
        {
            Exception ex = Assert.Throws<NegativePolicyDurationException>(() =>
               Company.SellPolicy(DEFAULT_OBJECT_NAME, DateTime.Now, 0, new List<Risk>())
            );

            Assert.AreEqual(0, FakeStorage.Instance.PolicyList.Count);
            Assert.AreEqual("Policy duration period can not be 0 or negative.", ex.Message);
        }

        [Test]
        public void InsuranceCompany_ShouldBeAbleToSellPolicyWithInitialListOfRisks_InitialListOfRisksShouldBeInStorage()
        {
            List<Risk> riskList = new List<Risk>();
            riskList.Add(Risk1);
            riskList.Add(Risk2);

            Company.SellPolicy(DEFAULT_OBJECT_NAME, DateTime.Now, 6, riskList);
            Assert.AreEqual(2, FakeStorage.Instance.RiskList.Count);
        }

        [Test]
        public void InsuranceCompany_ShouldBeAbleToAddRiskToExistingPolicy_RiskShouldBeAdded()
        {
            List<Risk> riskList = new List<Risk>();
            riskList.Add(Risk1);
            riskList.Add(Risk2);

            var effectiveDate = DateTime.Now;

            Company.SellPolicy(DEFAULT_OBJECT_NAME, effectiveDate, 6, riskList);
            Company.AddRisk(DEFAULT_OBJECT_NAME, Risk3, DateTime.Now.AddDays(1), effectiveDate);

            Assert.AreEqual(3, FakeStorage.Instance.RiskList.Count);
        }

        [Test]
        public void InsuranceCompany_ShouldNotBeAbleToAddRiskToPolicyIfRiskStartDateIsEarlierThanPolicyPeriodStart()
        {
            List<Risk> riskList = new List<Risk>();
            riskList.Add(Risk1);
            riskList.Add(Risk2);

            var effectiveDate = DateTime.Now.AddMonths(1);

            Company.SellPolicy(DEFAULT_OBJECT_NAME, effectiveDate, 6, riskList);

            Exception ex = Assert.Throws<RiskValidityPeriodException>(() =>
               Company.AddRisk(DEFAULT_OBJECT_NAME, Risk3, DateTime.Now.AddDays(10), effectiveDate)
            );
            
            Assert.AreEqual(2, FakeStorage.Instance.RiskList.Count);
            Assert.AreEqual("Risk should be within policy validity period.", ex.Message);
        }

        [Test]
        public void InsuranceCompany_ShouldNotBeAbleToAddRiskToPolicyIfRiskStartDateIsLaterThanPolicyPeriodEnd()
        {
            List<Risk> riskList = new List<Risk>();
            riskList.Add(Risk1);
            riskList.Add(Risk2);

            var effectiveDate = DateTime.Now;

            Company.SellPolicy(DEFAULT_OBJECT_NAME, effectiveDate, 6, riskList);

            Exception ex = Assert.Throws<RiskValidityPeriodException>(() =>
               Company.AddRisk(DEFAULT_OBJECT_NAME, Risk3, DateTime.Now.AddMonths(7), effectiveDate)
            );

            Assert.AreEqual(2, FakeStorage.Instance.RiskList.Count);
            Assert.AreEqual("Risk should be within policy validity period.", ex.Message);
        }

        [Test]
        public void InsuranceCompany_ShouldNotBeAbleToAddRiskWithEmptyObjectName()
        {
            Exception ex = Assert.Throws<CannotBeNullException>(() =>
                Company.AddRisk("", Risk3, DateTime.Now, DateTime.Now)
            );

            Assert.AreEqual(0, FakeStorage.Instance.PolicyList.Count);
            Assert.AreEqual("Argument 'nameOfInsuredObject' can not be null.", ex.Message);
        }

        [Test]
        public void InsuranceCompany_ShouldNotBeAbleToAddRiskWithNullObjectName()
        {
            Exception ex = Assert.Throws<CannotBeNullException>(() =>
                Company.AddRisk(null, Risk3, DateTime.Now, DateTime.Now)
            );

            Assert.AreEqual(0, FakeStorage.Instance.PolicyList.Count);
            Assert.AreEqual("Argument 'nameOfInsuredObject' can not be null.", ex.Message);
        }

        [Test]
        public void InsuranceCompany_ShouldBeAbleToRemoveRiskFromExistingPolicy()
        {
            List<Risk> riskList = new List<Risk>();
            riskList.Add(Risk1);
            riskList.Add(Risk2);

            var effectiveDate = DateTime.Now;
            var newValidTillDate = effectiveDate.AddMonths(1);

            Company.SellPolicy(DEFAULT_OBJECT_NAME, effectiveDate, 6, riskList);
            Company.RemoveRisk(DEFAULT_OBJECT_NAME, Risk2, newValidTillDate, effectiveDate);

            var riskId = IdGenerator.ConstructRiskId(Risk2.Name, DEFAULT_OBJECT_NAME, effectiveDate);

            var updatedRisk = RiskRepository.Get(riskId);

            Assert.AreEqual(newValidTillDate, updatedRisk.ValidTill);
        }

        [Test]
        public void InsuranceCompany_ShouldBeAbleToSellPolicyWithSameObjectNameButDifferentEffectiveDate()
        {
            Company.SellPolicy(DEFAULT_OBJECT_NAME, DateTime.Now, 6, new List<Risk>());
            Company.SellPolicy(DEFAULT_OBJECT_NAME, DateTime.Now.AddMonths(1), 6, new List<Risk>());
            Assert.AreEqual(2, FakeStorage.Instance.PolicyList.Count);
        }

        [Test]
        public void InsuranceCompany_ShouldNotBeAbleToSellPolicyWithSameObjectNameAndSameEffectiveDate()
        {
            var effectiveDate = DateTime.Now;
            Company.SellPolicy(DEFAULT_OBJECT_NAME, effectiveDate, 6, new List<Risk>());

            Exception ex = Assert.Throws<DuplicatePolicyException>(() =>
               Company.SellPolicy(DEFAULT_OBJECT_NAME, effectiveDate, 6, new List<Risk>())
            );

            Assert.AreEqual(1, FakeStorage.Instance.PolicyList.Count);
            Assert.AreEqual($"Policy with insured object name '{DEFAULT_OBJECT_NAME}' " +
                  $"and effective date {effectiveDate.ToShortDateString()} already exists.", ex.Message);
        }

        [Test]
        public void InsuranceCompany_ShouldNotBeAbleToAddNotAvailableRisk()
        {
            List<Risk> riskList = new List<Risk>();
            riskList.Add(Risk1);
            riskList.Add(Risk2);

            Risk newRisk = new Risk
            {
                Name = "Test risk 4",
                YearlyPrice = 5.8m
            };

            var effectiveDate = DateTime.Now;

            Company.SellPolicy(DEFAULT_OBJECT_NAME, effectiveDate, 6, riskList);

            Exception ex = Assert.Throws<RiskDoesNotExistInAvailableListException>(() =>
               Company.AddRisk(DEFAULT_OBJECT_NAME, newRisk, DateTime.Now.AddMonths(1), effectiveDate)
            );

            Assert.AreEqual(2, FakeStorage.Instance.RiskList.Count);
            Assert.AreEqual($"Risk with name '{newRisk.Name}' does not exist in available list.", ex.Message);
        }

        [Test]
        public void InsuranceCompany_ShouldNotBeAbleToAddDuplicateRiskToPolicy()
        {
            List<Risk> riskList = new List<Risk>();
            riskList.Add(Risk1);
            riskList.Add(Risk2);

            var effectiveDate = DateTime.Now;

            Company.SellPolicy(DEFAULT_OBJECT_NAME, effectiveDate, 6, riskList);

            Exception ex = Assert.Throws<DuplicateRiskException>(() =>
               Company.AddRisk(DEFAULT_OBJECT_NAME, Risk2, DateTime.Now.AddMonths(1), effectiveDate)
            );

            Assert.AreEqual(2, FakeStorage.Instance.RiskList.Count);
            Assert.AreEqual($"Risk with '{Risk2.Name}' name already exists on that policy.",
                ex.Message);
        }

        [Test]
        public void InsuranceCompany_ShouldNotBeAbleToAddRiskToPolicyWithValidFromInPast()
        {
            List<Risk> riskList = new List<Risk>();
            riskList.Add(Risk1);
            riskList.Add(Risk2);

            var effectiveDate = DateTime.Now;

            Company.SellPolicy(DEFAULT_OBJECT_NAME, effectiveDate, 6, riskList);

            Exception ex = Assert.Throws<RiskValidFromDateInPastException>(() =>
               Company.AddRisk(DEFAULT_OBJECT_NAME, Risk2, DateTime.Now.AddMonths(-1), effectiveDate)
            );

            Assert.AreEqual(2, FakeStorage.Instance.RiskList.Count);
            Assert.AreEqual("Risk can not be with valid from date in past.", ex.Message);
        }

        [Test]
        public void InsuranceCompany_ShouldNotBeAbleToRemoveRiskWithValidTillDateInThePast()
        {
            List<Risk> riskList = new List<Risk>();
            riskList.Add(Risk1);
            riskList.Add(Risk2);

            var effectiveDate = DateTime.Now;

            Company.SellPolicy(DEFAULT_OBJECT_NAME, effectiveDate, 6, riskList);

            Exception ex = Assert.Throws<RiskRemovalDateException>(() =>
               Company.RemoveRisk(DEFAULT_OBJECT_NAME, Risk2, effectiveDate.AddDays(-1), effectiveDate)
            );

            Assert.AreEqual(2, FakeStorage.Instance.RiskList.Count);
            Assert.AreEqual("When removing risk, date should not be in past and should be within policy validity period.", ex.Message);
        }

        [Test]
        public void InsuranceCompany_ShouldNotBeAbleToRemoveRiskWithValidTillDateLaterThanPolicyValidityPeriod()
        {
            List<Risk> riskList = new List<Risk>();
            riskList.Add(Risk1);
            riskList.Add(Risk2);

            var effectiveDate = DateTime.Now;

            Company.SellPolicy(DEFAULT_OBJECT_NAME, effectiveDate, 6, riskList);

            Exception ex = Assert.Throws<RiskRemovalDateException>(() =>
               Company.RemoveRisk(DEFAULT_OBJECT_NAME, Risk2, DateTime.Now.AddMonths(12), effectiveDate)
            );

            Assert.AreEqual(2, FakeStorage.Instance.RiskList.Count);
            Assert.AreEqual("When removing risk, date should not be in past and should be within policy validity period.", ex.Message);
        }

        [Test]
        public void Mock()
        {
            var policyRepositoryMock = new Mock<IPolicyRepository>();
            var riskRepositorMock = new Mock<IRiskRepository>();
            var premiumCalculatorMock = new Mock<IPremiumCalculator>();

            policyRepositoryMock.Setup(pService => pService.Get("Obj05.05.2020 00:00:00"))
                .Returns(new PolicyModel());

            riskRepositorMock.Setup(riskRepo => riskRepo.GetAll()).Returns(new List<RiskModel> {
                new RiskModel()
            });

            PolicyService ps = new PolicyService(policyRepositoryMock.Object, 
                riskRepositorMock.Object, premiumCalculatorMock.Object);

            var mockGetPolicyCall = ps.GetPolicy("Obj", new DateTime(2020, 5, 5));

            policyRepositoryMock.Verify(polRepMock => polRepMock.Get("Obj05.05.2020 00:00:00"), Times.Once);
        }
    }
}
