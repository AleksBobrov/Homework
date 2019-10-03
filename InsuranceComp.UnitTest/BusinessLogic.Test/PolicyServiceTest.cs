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
        Mock<IPolicyRepository> PolicyRepositoryMock;
        Mock<IRiskRepository> RiskRepositoryMock;
        Mock<IPremiumCalculator> PremiumCalculatorMock;
        IPolicyService PolicyService;
        string DEFAULT_OBJECT_NAME = "obj";
        DateTime DEFAULT_DATE = new DateTime(2020, 5, 5);

        IList<Risk> DEFAULT_INSURED_RISK_LIST = new List<Risk>()
        {
            new Risk()
            {
                Name = "Test risk 1",
                YearlyPrice = 5.5m
            },
            new Risk()
            {
                Name = "Test risk 2",
                YearlyPrice = 6.0m
            }
        };

        [SetUp]
        public void SetUp()
        {
            PolicyRepositoryMock = new Mock<IPolicyRepository>();
            RiskRepositoryMock = new Mock<IRiskRepository>();
            PremiumCalculatorMock = new Mock<IPremiumCalculator>();

            PolicyService = new PolicyService(PolicyRepositoryMock.Object,
                RiskRepositoryMock.Object, PremiumCalculatorMock.Object);
        }

        [Test]
        public void GetPolicy_PolicyRepositoryGetShouldBeCalledOnce()
        {
            PolicyRepositoryMock.Setup(mock => mock.Get(It.IsAny<string>())).Returns(new PolicyModel());
            RiskRepositoryMock.Setup(mock => mock.GetAll()).Returns(new List<RiskModel>());

            PolicyService.GetPolicy(DEFAULT_OBJECT_NAME, DEFAULT_DATE);

            PolicyRepositoryMock.Verify(mock => mock.Get(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void GetPolicy_ShouldThrowIfPolicyRepositoryGetReturnsNull()
        {
            Assert.That(() => PolicyService.GetPolicy(DEFAULT_OBJECT_NAME, DEFAULT_DATE), 
                Throws.Exception);
        }

        [Test]
        public void GetPolicy_RiskRepositoryGetAllShouldBeCalledOnce()
        {
            PolicyRepositoryMock.Setup(mock => mock.Get(It.IsAny<string>())).Returns(new PolicyModel());
            RiskRepositoryMock.Setup(mock => mock.GetAll()).Returns(new List<RiskModel>());

            PolicyService.GetPolicy(DEFAULT_OBJECT_NAME, DEFAULT_DATE);

            RiskRepositoryMock.Verify(mock => mock.GetAll(), Times.Once);
        }

        [Test]
        public void GetPolicy_PremiumCalculatorCalculateShouldBeCalledOnce()
        {
            PolicyRepositoryMock.Setup(mock => mock.Get(It.IsAny<string>())).Returns(new PolicyModel());
            RiskRepositoryMock.Setup(mock => mock.GetAll()).Returns(new List<RiskModel>());

            PremiumCalculatorMock.Setup(mock =>
                mock.CalculatePremiumOfSoldPolicy(DEFAULT_OBJECT_NAME, DEFAULT_DATE))
                .Returns(It.IsAny<decimal>);

            PolicyService.GetPolicy(DEFAULT_OBJECT_NAME, DEFAULT_DATE);

            PremiumCalculatorMock.Verify(mock => 
                mock.CalculatePremiumOfSoldPolicy(DEFAULT_OBJECT_NAME, DEFAULT_DATE), Times.Once);
        }

        [Test]
        public void SellPolicy_ShouldThrowIfPolicyRepositoryGetReturnsNotNull()
        {
            PolicyRepositoryMock.Setup(mock => mock.Get(It.IsAny<string>()))
                .Returns(new PolicyModel());

            Assert.That(() => PolicyService.SellPolicy(new Policy() {
                NameOfInsuredObject = DEFAULT_OBJECT_NAME,
                ValidFrom = DateTime.Now
            }), Throws.Exception);
        }

        [Test]
        public void SellPolicy_ShouldCallRiskRepositoryAddMethodAccordingToInsuredRiskAmount()
        {
            RiskRepositoryMock.Setup(mock => mock.Add(It.IsAny<RiskModel>())).Verifiable();

            PolicyService.SellPolicy(new Policy {
                NameOfInsuredObject = DEFAULT_OBJECT_NAME,
                ValidFrom = DateTime.Now,
                InsuredRisks = DEFAULT_INSURED_RISK_LIST
            });

            RiskRepositoryMock.Verify(mock => mock.Add(It.IsAny<RiskModel>()), Times.Exactly(2));
        }

        [Test]
        public void SellPolicy_ShouldCallPolicyRepositoryAddMethodOnce()
        {
            PolicyRepositoryMock.Setup(mock => mock.Add(It.IsAny<PolicyModel>())).Verifiable();

            PolicyService.SellPolicy(new Policy
            {
                NameOfInsuredObject = DEFAULT_OBJECT_NAME,
                ValidFrom = DateTime.Now,
                InsuredRisks = DEFAULT_INSURED_RISK_LIST
            });

            PolicyRepositoryMock.Verify(mock => mock.Add(It.IsAny<PolicyModel>()), Times.Once);
        }
    }
}
