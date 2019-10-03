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
    public class RiskServiceTest
    {
        string DEFAULT_OBJECT_NAME = "obj";
        string DEFAULT_RISK_NAME = "risk";
        Mock<IRiskRepository> RiskRepositoryMock;
        Mock<IPolicyRepository> PolicyRepositoryMock;
        IRiskService RiskService;
        DateTime EffectiveDate;

        [SetUp]
        public void SetUp()
        {
            EffectiveDate = DateTime.Now;
            RiskRepositoryMock = new Mock<IRiskRepository>();
            PolicyRepositoryMock = new Mock<IPolicyRepository>();

            RiskService = new RiskService(PolicyRepositoryMock.Object, RiskRepositoryMock.Object);
        }

        [Test]
        public void AddRisk_ShouldThrowIfValidTillIsEarlierThanEffectiveDate()
        {
            Assert.That(() => RiskService.AddRisk(It.IsAny<string>(), It.IsAny<Risk>(), 
                DateTime.Now.AddMonths(-1), DateTime.Now), Throws.Exception);
        }

        [Test]
        public void AddRisk_ShouldCallPolicyRepositoryGetOnce()
        {
            PolicyRepositoryMock.Setup(mock => mock.Get(It.IsAny<string>()))
                .Returns(new PolicyModel() {
                    NameOfInsuredObject = DEFAULT_OBJECT_NAME,
                    ValidFrom = EffectiveDate,
                    ValidTill = EffectiveDate.AddMonths(6),
                    InsuredRisks = new List<Risk>()
                });

            RiskService.AddRisk(DEFAULT_OBJECT_NAME, new Risk() { Name = DEFAULT_RISK_NAME},
                EffectiveDate.AddDays(1), EffectiveDate);

            PolicyRepositoryMock.Verify(mock => mock.Get(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void AddRisk_ShouldThrowIfValidFromIsLaterThanPolicyValidTill()
        {
            PolicyRepositoryMock.Setup(mock => mock.Get(It.IsAny<string>()))
                .Returns(new PolicyModel()
                {
                    NameOfInsuredObject = DEFAULT_OBJECT_NAME,
                    ValidFrom = EffectiveDate,
                    ValidTill = EffectiveDate.AddMonths(6),
                    InsuredRisks = new List<Risk>()
                });

            Assert.That(() => RiskService.AddRisk(DEFAULT_OBJECT_NAME, It.IsAny<Risk>(),
                 EffectiveDate.AddMonths(7), EffectiveDate), Throws.Exception);
        }

        [Test]
        public void AddRisk_ShouldThrowIfPolicyAlreadyHaveRiskWithSameName()
        {
            PolicyRepositoryMock.Setup(mock => mock.Get(It.IsAny<string>()))
                .Returns(new PolicyModel()
                {
                    NameOfInsuredObject = DEFAULT_OBJECT_NAME,
                    ValidFrom = EffectiveDate,
                    ValidTill = EffectiveDate.AddMonths(6),
                    InsuredRisks = new List<Risk>()
                    {
                        new Risk() { Name = DEFAULT_RISK_NAME }
                    }
                });

            Assert.That(() => RiskService.AddRisk(DEFAULT_OBJECT_NAME, new Risk() { Name = DEFAULT_RISK_NAME },
                 EffectiveDate, EffectiveDate), Throws.Exception);
        }

        [Test]
        public void AddRisk_ShouldCallRiskRepositoryGetOnce()
        {
            PolicyRepositoryMock.Setup(mock => mock.Get(It.IsAny<string>()))
                .Returns(new PolicyModel()
                {
                    NameOfInsuredObject = DEFAULT_OBJECT_NAME,
                    ValidFrom = EffectiveDate,
                    ValidTill = EffectiveDate.AddMonths(6),
                    InsuredRisks = new List<Risk>()
                });

            RiskRepositoryMock.Setup(mock => mock.Add(It.IsAny<RiskModel>())).Verifiable();

            RiskService.AddRisk(DEFAULT_OBJECT_NAME, new Risk() { Name = DEFAULT_RISK_NAME },
                EffectiveDate.AddDays(1), EffectiveDate);

            RiskRepositoryMock.Verify(mock => mock.Add(It.IsAny<RiskModel>()), Times.Once);
        }

        [Test]
        public void RemoveRisk_ShouldCallPolicyRepositoryGet()
        {
            PolicyRepositoryMock.Setup(mock => mock.Get(It.IsAny<string>()))
                .Returns(new PolicyModel() {
                    ValidTill = EffectiveDate.AddMonths(6)
                });

            RiskRepositoryMock.Setup(mock => mock.Get(It.IsAny<string>()))
                .Returns(new RiskModel()
                {

                });

            RiskService.RemoveRisk(DEFAULT_OBJECT_NAME, new Risk() { Name = DEFAULT_RISK_NAME}, 
                EffectiveDate.AddMonths(2), EffectiveDate);

            PolicyRepositoryMock.Verify(mock => mock.Get(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void RemoveRisk_ShouldThrowIfValidTillIsLaterThanPolicyValidTill()
        {
            PolicyRepositoryMock.Setup(mock => mock.Get(It.IsAny<string>()))
                .Returns(new PolicyModel()
                {
                    ValidTill = EffectiveDate.AddMonths(6)
                });
            
            Assert.That(() => RiskService.RemoveRisk(DEFAULT_OBJECT_NAME, new Risk() { Name = DEFAULT_RISK_NAME },
                EffectiveDate.AddMonths(7), EffectiveDate), Throws.Exception);
        }

        [Test]
        public void RemoveRisk_ShouldCallRiskRepositoryGet()
        {
            PolicyRepositoryMock.Setup(mock => mock.Get(It.IsAny<string>()))
                .Returns(new PolicyModel()
                {
                    ValidTill = EffectiveDate.AddMonths(6)
                });

            RiskRepositoryMock.Setup(mock => mock.Get(It.IsAny<string>()))
                .Returns(new RiskModel()
                {

                });

            RiskService.RemoveRisk(DEFAULT_OBJECT_NAME, new Risk() { Name = DEFAULT_RISK_NAME },
                EffectiveDate.AddMonths(2), EffectiveDate);

            RiskRepositoryMock.Verify(mock => mock.Get(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void RemoveRisk_ShouldCallRiskRepositoryEdit()
        {
            var riskRepositoryMock = new Mock<IRiskRepository>();
            var policyRepositoryMock = new Mock<IPolicyRepository>();

            var riskService = new RiskService(policyRepositoryMock.Object,
                riskRepositoryMock.Object);

            var EffectiveDate = DateTime.Now;

            policyRepositoryMock.Setup(mock => mock.Get(It.IsAny<string>()))
                .Returns(new PolicyModel()
                {
                    ValidTill = EffectiveDate.AddMonths(6)
                });

            riskRepositoryMock.Setup(mock => mock.Get(It.IsAny<string>()))
                .Returns(new RiskModel()
                {

                });

            riskService.RemoveRisk(DEFAULT_OBJECT_NAME, new Risk() { Name = DEFAULT_RISK_NAME },
                EffectiveDate.AddMonths(2), EffectiveDate);

            riskRepositoryMock.Verify(mock => mock.Edit(It.IsAny<RiskModel>()), Times.Once);
        }
    }
}
