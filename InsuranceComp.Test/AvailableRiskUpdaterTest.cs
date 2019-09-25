using InsuranceComp;
using InsuranceComp.BusinessLogic.Exceptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AvailableRiskUpdaterTests
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
            Company = new InsuranceCompany("If", initialAvailableRisks);
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
            Exception ex = Assert.Throws<DuplicateRiskException>(() =>
               AvailableRiskUpdater
                .AddAvailableRisk(Risk1)
                .AddAvailableRisk(Risk1)
            );

            Assert.AreEqual("Duplicate risk.", ex.Message);
        }

        [Test]
        public void AvailableRisks_RemovingNotExistingRiskFromAvailableRisksList_ShouldThrowException()
        {
            Exception ex = Assert.Throws<NotExistingRiskException>( () =>
                AvailableRiskUpdater
                    .AddAvailableRisk(Risk1)
                    .RemoveAvailableRisk("Test risk 2")
            );
            Assert.AreEqual("There is no such risk in available risk list.", ex.Message);
        }

        [Test]
        public void AvailableRisks_RemovingRiskFromEmptyAvailableRisksList_ShouldThrowException()
        {
            Exception ex = Assert.Throws<NotExistingRiskException>(() =>
               AvailableRiskUpdater
                   .AddAvailableRisk(Risk1)
                   .RemoveAvailableRisk("Test risk 2")
            );
            Assert.AreEqual("There is no such risk in available risk list.", ex.Message);
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
        public void AvailableRisks_UpdateNameOfExistingRisk_RiskNameShouldBeUpdatedAndYearlyPriceShouldNotBeChanged()
        {
            const string newName = "Updated test risk 1";

            Risk updatedRisk = new Risk
            {
                Name = newName
            };

            AvailableRiskUpdater
                .AddAvailableRisk(Risk1)
                .AddAvailableRisk(Risk2)
                .UpdateAvailableRisk("Test risk 1", updatedRisk);

            Assert.IsTrue(Company.AvailableRisks.Any(avRisk => avRisk.Name == newName));
            Assert.IsFalse(Company.AvailableRisks.Any(avRisk => avRisk.Name == "Test risk 1"));

            var riskAfterUpdate = Company.AvailableRisks.FirstOrDefault(avRisk => avRisk.Name == newName);

            Assert.AreEqual(Risk1.YearlyPrice, riskAfterUpdate.YearlyPrice);
        }

        public void AvailableRisks_UpdateYearlyPriceOfExistingRisk_RiskYearlyPriceShouldBeUpdated()
        {
            const decimal newYearlyPrice = 4.0m;

            Risk updatedRisk = new Risk
            {
                YearlyPrice = newYearlyPrice
            };

            AvailableRiskUpdater
                .AddAvailableRisk(Risk1)
                .AddAvailableRisk(Risk2)
                .UpdateAvailableRisk("Test risk 1", updatedRisk);

            var riskAfterUpdate = Company.AvailableRisks.FirstOrDefault(avRisk => avRisk.Name == "Test risk 1");
            Assert.AreEqual(newYearlyPrice, riskAfterUpdate.YearlyPrice);
            Assert.AreEqual("Test risk 1", riskAfterUpdate.Name);
        }
    }
}