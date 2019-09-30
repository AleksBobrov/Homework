using InsuranceComp.BusinessLogic;
using InsuranceComp.DataAccess.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace InsuranceComp.UnitTest.BusinessLogic.Test
{
    [TestFixture]
    public class PremiumCalculatorTest
    {
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

            var premium = new PremiumCalculator(new Mock<IRiskRepository>().Object)
                .CalculateInitialPremium(riskList, effectiveDate, effectiveDate.AddMonths(6));

            Assert.AreEqual(648.22m, premium);
        }

        [Test]
        public void CalculatePremiumOfSoldPolicy_ShouldCallRiskRepositoryGetAllOnce()
        {
            var riskRepositoryMock = new Mock<IRiskRepository>();

            riskRepositoryMock.Setup(mock => mock.GetAll()).Returns(new List<RiskModel>());

            var premium = new PremiumCalculator(riskRepositoryMock.Object)
                .CalculatePremiumOfSoldPolicy("obj", DateTime.Now);

            riskRepositoryMock.Verify(mock => mock.GetAll(), Times.Once);
        }

        [Test]
        public void CalculatePremiumOfSoldPolicy_ShouldCorrectlyCalculatePremiumWhenRiskValidityPeriodsAreSameWithPolicy()
        {
            var riskRepositoryMock = new Mock<IRiskRepository>();

            var validFrom = DateTime.Now;
            var validTill = validFrom.AddMonths(6);

            var policyId = "obj" + validFrom.ToString();

            riskRepositoryMock.Setup(mock => mock.GetAll()).Returns(new List<RiskModel>() {
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

            var premium = new PremiumCalculator(riskRepositoryMock.Object)
                .CalculatePremiumOfSoldPolicy("obj", validFrom);

            Assert.AreEqual(648.22m, premium);
        }

        [Test]
        public void CalculatePremiumOfSoldPolicy_ShouldCorrectlyCalculatePremiumWhenRiskValidityPeriodsAreDifferentFromPolicy()
        {
            var riskRepositoryMock = new Mock<IRiskRepository>();

            var validFrom = DateTime.Now;
            var validTill = validFrom.AddMonths(6);

            var policyId = "obj" + validFrom.ToString();

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

            var premium = new PremiumCalculator(riskRepositoryMock.Object)
                .CalculatePremiumOfSoldPolicy("obj", validFrom);

            Assert.AreEqual(433.15m, premium);
        }
    }
}
