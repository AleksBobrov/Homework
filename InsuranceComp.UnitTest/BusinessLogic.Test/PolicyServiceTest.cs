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
    public class PolicyServiceTest
    {
        [Test]
        public void GetPolicy_PolicyRepositoryGetShouldBeCalledOnce()
        {
            var policyRepositoryMock = new Mock<IPolicyRepository>();
            var riskRepositoryMock = new Mock<IRiskRepository>();
            var premiumCalculatorMock = new Mock<IPremiumCalculator>();

            var policyService = new PolicyService(policyRepositoryMock.Object, 
                riskRepositoryMock.Object, premiumCalculatorMock.Object);

            policyRepositoryMock.Setup(mock => mock.Get(It.IsAny<string>())).Returns(new PolicyModel());
            riskRepositoryMock.Setup(mock => mock.GetAll()).Returns(new List<RiskModel>());

            policyService.GetPolicy("obj", new DateTime(2020, 5, 5));

            policyRepositoryMock.Verify(mock => mock.Get(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void GetPolicy_ShouldThrowIfPolicyRepositoryGetReturnsNull()
        {
            var policyRepositoryMock = new Mock<IPolicyRepository>();
            var riskRepositoryMock = new Mock<IRiskRepository>();
            var premiumCalculatorMock = new Mock<IPremiumCalculator>();

            var policyService = new PolicyService(policyRepositoryMock.Object,
                riskRepositoryMock.Object, premiumCalculatorMock.Object);

            Assert.That(() => policyService.GetPolicy("obj", new DateTime(2020, 5, 5)), 
                Throws.Exception);
        }

        [Test]
        public void GetPolicy_RiskRepositoryGetAllShouldBeCalledOnce()
        {
            var policyRepositoryMock = new Mock<IPolicyRepository>();
            var riskRepositoryMock = new Mock<IRiskRepository>();
            var premiumCalculatorMock = new Mock<IPremiumCalculator>();

            var policyService = new PolicyService(policyRepositoryMock.Object,
                riskRepositoryMock.Object, premiumCalculatorMock.Object);

            policyRepositoryMock.Setup(mock => mock.Get(It.IsAny<string>())).Returns(new PolicyModel());
            riskRepositoryMock.Setup(mock => mock.GetAll()).Returns(new List<RiskModel>());

            policyService.GetPolicy("obj", new DateTime(2020, 5, 5));

            riskRepositoryMock.Verify(mock => mock.GetAll(), Times.Once);
        }

        [Test]
        public void GetPolicy_PremiumCalculatorCalculateShouldBeCalledOnce()
        {
            var policyRepositoryMock = new Mock<IPolicyRepository>();
            var riskRepositoryMock = new Mock<IRiskRepository>();
            var premiumCalculatorMock = new Mock<IPremiumCalculator>();

            var policyService = new PolicyService(policyRepositoryMock.Object,
                riskRepositoryMock.Object, premiumCalculatorMock.Object);

            policyRepositoryMock.Setup(mock => mock.Get(It.IsAny<string>())).Returns(new PolicyModel());
            riskRepositoryMock.Setup(mock => mock.GetAll()).Returns(new List<RiskModel>());

            premiumCalculatorMock.Setup(mock =>
                mock.CalculatePremiumOfSoldPolicy("obj", new DateTime(2020, 5, 5)))
                .Returns(It.IsAny<decimal>);

            policyService.GetPolicy("obj", new DateTime(2020, 5, 5));

            premiumCalculatorMock.Verify(mock => 
                mock.CalculatePremiumOfSoldPolicy("obj", new DateTime(2020, 5, 5)), Times.Once);
        }

        [Test]
        public void SellPolicy_ShouldThrowIfPolicyRepositoryGetReturnsNotNull()
        {
            var policyRepositoryMock = new Mock<IPolicyRepository>();
            var riskRepositoryMock = new Mock<IRiskRepository>();
            var premiumCalculatorMock = new Mock<IPremiumCalculator>();

            var policyService = new PolicyService(policyRepositoryMock.Object,
                riskRepositoryMock.Object, premiumCalculatorMock.Object);

            policyRepositoryMock.Setup(mock => mock.Get(It.IsAny<string>()))
                .Returns(new PolicyModel());

            Assert.That(() => policyService.SellPolicy(new Policy() {
                NameOfInsuredObject = "Obj",
                ValidFrom = DateTime.Now
            }), Throws.Exception);
        }

        [Test]
        public void SellPolicy_ShouldCallRiskRepositoryAddMethodAccordingToInsuredRiskAmount()
        {
            var policyRepositoryMock = new Mock<IPolicyRepository>();
            var riskRepositoryMock = new Mock<IRiskRepository>();
            var premiumCalculatorMock = new Mock<IPremiumCalculator>();

            var policyService = new PolicyService(policyRepositoryMock.Object,
                riskRepositoryMock.Object, premiumCalculatorMock.Object);

            riskRepositoryMock.Setup(mock => mock.Add(It.IsAny<RiskModel>())).Verifiable();

            policyService.SellPolicy(new Policy {
                NameOfInsuredObject = "Obj",
                ValidFrom = DateTime.Now,
                InsuredRisks = new List<Risk>()
                {
                    new Risk() {
                        Name = "Test risk 1",
                        YearlyPrice = 5.5m
                    },
                    new Risk() {
                        Name = "Test risk 2",
                        YearlyPrice = 6.0m
                    }
                }
            });

            riskRepositoryMock.Verify(mock => mock.Add(It.IsAny<RiskModel>()), Times.Exactly(2));
        }

        [Test]
        public void SellPolicy_ShouldCallPolicyRepositoryAddMethodOnce()
        {
            var policyRepositoryMock = new Mock<IPolicyRepository>();
            var riskRepositoryMock = new Mock<IRiskRepository>();
            var premiumCalculatorMock = new Mock<IPremiumCalculator>();

            var policyService = new PolicyService(policyRepositoryMock.Object,
                riskRepositoryMock.Object, premiumCalculatorMock.Object);

            policyRepositoryMock.Setup(mock => mock.Add(It.IsAny<PolicyModel>())).Verifiable();

            policyService.SellPolicy(new Policy
            {
                NameOfInsuredObject = "Obj",
                ValidFrom = DateTime.Now,
                InsuredRisks = new List<Risk>()
                {
                    new Risk() {
                        Name = "Test risk 1",
                        YearlyPrice = 5.5m
                    },
                    new Risk() {
                        Name = "Test risk 2",
                        YearlyPrice = 6.0m
                    }
                }
            });

            policyRepositoryMock.Verify(mock => mock.Add(It.IsAny<PolicyModel>()), Times.Once);
        }
    }
}
