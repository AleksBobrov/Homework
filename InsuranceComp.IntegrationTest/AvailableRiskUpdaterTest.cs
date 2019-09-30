using InsuranceComp.BusinessLogic;
using InsuranceComp.BusinessLogic.Exceptions;
using InsuranceComp.InsuranceCompDomain;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InsuranceComp.IntegrationTest.AvailableRiskUpdaterTests
{
    [TestFixture]
    public class AvailableRiskUpdaterTest
    {
        IInsuranceCompany Company;
        IAvailableRiskUpdater AvailableRiskUpdater;
        Risk Risk1;
        Risk Risk2;
        const string InsuranceObjectName = "Test insurance object 1";

        [SetUp]
        public void Setup()
        {
            List<Risk> initialAvailableRisks = new List<Risk>();
            Company = new InsuranceCompany("If", initialAvailableRisks,
                new Mock<IPolicyService>().Object,
                new Mock<IRiskService>().Object,
                new Mock<IPremiumCalculator>().Object);
            AvailableRiskUpdater = new AvailableRiskUpdater(Company);

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
        }

        [Test]
        public void AvailableRisks_AddingRiskToEmptyAvailableRisksList_AvailableRisksListCountShouldBe1()
        {
            AvailableRiskUpdater.AddAvailableRisk(Risk1);
            Assert.AreEqual(1, Company.AvailableRisks.Count);
        }

        [Test]
        public void AvailableRisks_AddingTwoRisksToEmptyAvailableRisksList_AvailableRisksListCountShouldBe2()
        {
            AvailableRiskUpdater
                .AddAvailableRisk(Risk1)
                .AddAvailableRisk(Risk2);

            Assert.AreEqual(2, Company.AvailableRisks.Count);
        }

        [Test]
        public void AvailableRisks_AddingDuplicateRiskToAvailableRisksList_ShouldThrowException()
        {
            Exception ex = Assert.Throws<DuplicateAvailableRiskException>(() =>
               AvailableRiskUpdater
                .AddAvailableRisk(Risk1)
                .AddAvailableRisk(Risk1)
            );

            Assert.AreEqual($"Available risk with '{Risk1.Name}' name already exists.", ex.Message);
        }

        [Test]
        public void AvailableRisks_RemovingNotExistingRiskFromAvailableRisksList_ShouldThrowException()
        {
            var testRiskName = "Test risk 2";

            Exception ex = Assert.Throws<RiskDoesNotExistInAvailableListException>( () =>
                AvailableRiskUpdater
                    .AddAvailableRisk(Risk1)
                    .RemoveAvailableRisk(testRiskName)
            );
            Assert.AreEqual($"Risk with name '{testRiskName}' does not exist in available list.", ex.Message);
        }

        [Test]
        public void AvailableRisks_RemovingRiskFromEmptyAvailableRisksList_ShouldThrowException()
        {
            var testRiskName = "Test risk 2";

            Exception ex = Assert.Throws<RiskDoesNotExistInAvailableListException>(() =>
               AvailableRiskUpdater
                   .AddAvailableRisk(Risk1)
                   .RemoveAvailableRisk(testRiskName)
            );
            Assert.AreEqual($"Risk with name '{testRiskName}' does not exist in available list.", ex.Message);
        }

        [Test]
        public void AvailableRisks_RemovingRiskFromAvailableRisksListWith2Items_AvailableRisksListCountShouldBe1()
        {
            AvailableRiskUpdater
                .AddAvailableRisk(Risk1)
                .AddAvailableRisk(Risk2)
                .RemoveAvailableRisk("Test risk 1");

            
            Assert.AreEqual(1, Company.AvailableRisks.Count);
        }

        [Test]
        public void AvailableRisks_EmptyAvailableRisksList_AvailableRisksListCountShouldBe0()
        {
            AvailableRiskUpdater
                .AddAvailableRisk(Risk1)
                .AddAvailableRisk(Risk2)
                .EmptyAvailableRiskList();

            Assert.AreEqual(0, Company.AvailableRisks.Count);
        }

        [Test]
        public void AvailableRisks_UpdateNameOfExistingRisk_RiskNameShouldThrowIfNewPriceIsNegative()
        {
            const decimal newYearlyPrice = -0.1m;

            Exception ex = Assert.Throws<NegativeRiskYearlyPriceException>(() =>
                AvailableRiskUpdater
                    .AddAvailableRisk(Risk1)
                    .AddAvailableRisk(Risk2)
                    .UpdateAvailableRisk("Test risk 1", newYearlyPrice)
             );

            Assert.AreEqual("Risk yearly price can not be negative.", ex.Message);
        }

        public void AvailableRisks_UpdateYearlyPriceOfExistingRisk_RiskYearlyPriceShouldBeUpdated()
        {
            const decimal newYearlyPrice = 4.0m;

            AvailableRiskUpdater
                .AddAvailableRisk(Risk1)
                .AddAvailableRisk(Risk2)
                .UpdateAvailableRisk("Test risk 1", newYearlyPrice);

            var riskAfterUpdate = Company.AvailableRisks.FirstOrDefault(avRisk => avRisk.Name == "Test risk 1");
            Assert.AreEqual(newYearlyPrice, riskAfterUpdate.YearlyPrice);
            Assert.AreEqual("Test risk 1", riskAfterUpdate.Name);
        }
    }
}