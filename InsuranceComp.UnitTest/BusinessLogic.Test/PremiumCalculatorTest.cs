using InsuranceComp.BusinessLogic;
using InsuranceComp.DataAccess.Repositories;
using InsuranceComp.InsuranceCompDomain;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace InsuranceComp.UnitTest.BusinessLogic.Test
{
    [TestFixture]
    public class PremiumCalculatorTest
    {
        string DEFAULT_OBJECT_NAME = "obj";
        Mock<IRiskRepository> RiskRepositoryMock;
        PremiumCalculator PremiumCalculator;

        [SetUp]
        public void SetUp()
        {
            RiskRepositoryMock = new Mock<IRiskRepository>();
            PremiumCalculator = new PremiumCalculator(RiskRepositoryMock.Object);
        }

        [Test]
        public void CalculateInitialPremium_ShouldCorrectlyCalculatePremium()
        {
            var risk1 = new Risk
            {
                Name = "Test risk 1",
                YearlyPrice = 500.0m
            };

            var risk2 = new Risk
            {
                Name = "Test risk 2",
                YearlyPrice = 800.0m
            };

            List<Risk> riskList = new List<Risk>();
            riskList.Add(risk1);
            riskList.Add(risk2);

            var effectiveDate = DateTime.Now;

            var premium = PremiumCalculator
                .CalculateInitialPremium(riskList, effectiveDate, effectiveDate.AddMonths(6));

            Assert.AreEqual(651.48m, premium);
        }

        [Test]
        public void CalculatePremiumOfSoldPolicy_ShouldCallRiskRepositoryGetAllOnce()
        {
            RiskRepositoryMock.Setup(mock => mock.GetAll()).Returns(new List<RiskModel>());

            var premium = new PremiumCalculator(RiskRepositoryMock.Object)
                .CalculatePremiumOfSoldPolicy(DEFAULT_OBJECT_NAME, DateTime.Now);

            RiskRepositoryMock.Verify(mock => mock.GetAll(), Times.Once);
        }

        [Test]
        public void CalculatePremiumOfSoldPolicy_ShouldCorrectlyCalculatePremiumWhenRiskValidityPeriodsAreSameWithPolicy()
        {
            var validFrom = DateTime.Now;
            var validTill = validFrom.AddMonths(6);

            var policyId = DEFAULT_OBJECT_NAME + validFrom.ToString();

            RiskRepositoryMock.Setup(mock => mock.GetAll()).Returns(new List<RiskModel>() {
                new RiskModel()
                {
                    YearlyPrice = 500.0m,
                    ValidFrom = validFrom,
                    ValidTill = validTill,
                    PolicyId = policyId
                },
                new RiskModel()
                {
                    YearlyPrice = 800.0m,
                    ValidFrom = validFrom,
                    ValidTill = validTill,
                    PolicyId = policyId
                }
            });

            var premium = PremiumCalculator
                .CalculatePremiumOfSoldPolicy(DEFAULT_OBJECT_NAME, validFrom);

            Assert.AreEqual(651.48m, premium);
        }

        [Test]
        public void CalculatePremiumOfSoldPolicy_ShouldCorrectlyCalculatePremiumWhenRiskValidityPeriodsAreDifferentFromPolicy()
        {
            var riskRepositoryMock = new Mock<IRiskRepository>();

            var validFrom = DateTime.Now;
            var validTill = validFrom.AddMonths(6);

            var policyId = DEFAULT_OBJECT_NAME + validFrom.ToString();

            riskRepositoryMock.Setup(mock => mock.GetAll()).Returns(new List<RiskModel>() {
                new RiskModel()
                {
                    YearlyPrice = 500.0m,
                    ValidFrom = validFrom.AddMonths(2),
                    ValidTill = validTill,
                    PolicyId = policyId
                },
                new RiskModel()
                {
                    YearlyPrice = 800.0m,
                    ValidFrom = validFrom.AddMonths(1),
                    ValidTill = validTill.AddMonths(-1),
                    PolicyId = policyId
                }
            });

            var premium = PremiumCalculator
                .CalculatePremiumOfSoldPolicy(DEFAULT_OBJECT_NAME, validFrom);

            Assert.AreEqual(432.13m, premium);
        }
    }
}
