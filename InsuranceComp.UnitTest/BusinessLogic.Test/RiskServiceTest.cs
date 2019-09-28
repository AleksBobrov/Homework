using InsuranceComp.BusinessLogic;
using InsuranceComp.DataAccess.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace InsuranceComp.UnitTest.BusinessLogic.Test
{
    [TestFixture]
    public class RiskServiceTest
    {
        [Test]
        public void AddRisk_ShouldThrowIfValidTillIsEarlierThanEffectiveDate()
        {
            var riskRepositoryMock = new Mock<IRiskRepository>();
            var policyRepositoryMock = new Mock<IPolicyRepository>();

            var riskService = new RiskService(policyRepositoryMock.Object, 
                riskRepositoryMock.Object);

            Assert.That(() => riskService.AddRisk(It.IsAny<string>(), It.IsAny<Risk>(), 
                DateTime.Now.AddMonths(-1), DateTime.Now), Throws.Exception);
        }

        [Test]
        public void AddRisk_ShouldCallPolicyRepositoryGetOnce()
        {
            var riskRepositoryMock = new Mock<IRiskRepository>();
            var policyRepositoryMock = new Mock<IPolicyRepository>();

            var riskService = new RiskService(policyRepositoryMock.Object,
                riskRepositoryMock.Object);

            var effectiveDate = DateTime.Now;

            policyRepositoryMock.Setup(mock => mock.Get(It.IsAny<string>()))
                .Returns(new PolicyModel() {
                    NameOfInsuredObject = "obj",
                    ValidFrom = effectiveDate,
                    ValidTill = effectiveDate.AddMonths(6),
                    InsuredRisks = new List<Risk>()
                });

            riskService.AddRisk("obj", new Risk() { Name = "risk name"},
                effectiveDate.AddDays(1), effectiveDate);

            policyRepositoryMock.Verify(mock => mock.Get(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void AddRisk_ShouldThrowIfValidFromIsLaterThanPolicyValidTill()
        {
            var riskRepositoryMock = new Mock<IRiskRepository>();
            var policyRepositoryMock = new Mock<IPolicyRepository>();

            var riskService = new RiskService(policyRepositoryMock.Object,
                riskRepositoryMock.Object);

            var effectiveDate = DateTime.Now;

            policyRepositoryMock.Setup(mock => mock.Get(It.IsAny<string>()))
                .Returns(new PolicyModel()
                {
                    NameOfInsuredObject = "obj",
                    ValidFrom = effectiveDate,
                    ValidTill = effectiveDate.AddMonths(6),
                    InsuredRisks = new List<Risk>()
                });

            Assert.That(() => riskService.AddRisk("Obj", It.IsAny<Risk>(),
                 effectiveDate.AddMonths(7), effectiveDate), Throws.Exception);
        }

        [Test]
        public void AddRisk_ShouldThrowIfPolicyAlreadyHaveRiskWithSameName()
        {
            var riskRepositoryMock = new Mock<IRiskRepository>();
            var policyRepositoryMock = new Mock<IPolicyRepository>();

            var riskService = new RiskService(policyRepositoryMock.Object,
                riskRepositoryMock.Object);

            var effectiveDate = DateTime.Now;

            policyRepositoryMock.Setup(mock => mock.Get(It.IsAny<string>()))
                .Returns(new PolicyModel()
                {
                    NameOfInsuredObject = "obj",
                    ValidFrom = effectiveDate,
                    ValidTill = effectiveDate.AddMonths(6),
                    InsuredRisks = new List<Risk>()
                    {
                        new Risk() { Name = "risk name" }
                    }
                });

            Assert.That(() => riskService.AddRisk("Obj", new Risk() { Name = "risk name" },
                 effectiveDate, effectiveDate), Throws.Exception);
        }

        [Test]
        public void AddRisk_ShouldCallRiskRepositoryGetOnce()
        {
            var riskRepositoryMock = new Mock<IRiskRepository>();
            var policyRepositoryMock = new Mock<IPolicyRepository>();

            var riskService = new RiskService(policyRepositoryMock.Object,
                riskRepositoryMock.Object);

            var effectiveDate = DateTime.Now;

            policyRepositoryMock.Setup(mock => mock.Get(It.IsAny<string>()))
                .Returns(new PolicyModel()
                {
                    NameOfInsuredObject = "obj",
                    ValidFrom = effectiveDate,
                    ValidTill = effectiveDate.AddMonths(6),
                    InsuredRisks = new List<Risk>()
                });

            riskRepositoryMock.Setup(mock => mock.Add(It.IsAny<RiskModel>())).Verifiable();

            riskService.AddRisk("obj", new Risk() { Name = "risk name" },
                effectiveDate.AddDays(1), effectiveDate);

            riskRepositoryMock.Verify(mock => mock.Add(It.IsAny<RiskModel>()), Times.Once);
        }
    }
}
