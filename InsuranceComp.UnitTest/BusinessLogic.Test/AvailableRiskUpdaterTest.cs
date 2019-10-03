using InsuranceComp.InsuranceCompDomain;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace InsuranceComp.UnitTest.BusinessLogic.Test
{
    [TestFixture]
    public class AvailableRiskUpdaterTest
    {
        Mock<IInsuranceCompany> CompanyMock;
        IAvailableRiskUpdater AvailableRiskUpdater;

        string DEFAULT_RISK_NAME = "test risk name";

        Risk DefaultRisk = new Risk
        {
            Name = "Test risk 1",
            YearlyPrice = 5.5m
        };

        [SetUp]
        public void SetUp()
        {
            CompanyMock = new Mock<IInsuranceCompany>();
            AvailableRiskUpdater = new AvailableRiskUpdater(CompanyMock.Object);
        }

        [Test]
        public void AddAvailableRisk_ShouldCallAddMethodOnAvalaibleRiskList()
        {
            CompanyMock.Setup(mock => mock.AvailableRisks.Contains(DefaultRisk)).Returns(false);

            AvailableRiskUpdater.AddAvailableRisk(DefaultRisk);

            CompanyMock.Verify(mock => mock.AvailableRisks.Add(DefaultRisk), Times.Once);
        }

        [Test]
        public void AddAvailableRisk_ShouldThrowWhenAvaialableRisksListAlreadyContainRisk()
        {
            CompanyMock.Setup(mock => mock.AvailableRisks.Contains(DefaultRisk)).Returns(true);

            Assert.That(() => AvailableRiskUpdater.AddAvailableRisk(DefaultRisk), Throws.Exception);
        }

        [Test]
        public void EmptyAvailableRiskList_ShouldCallClearMethodOnAvailableListRisk()
        {
            CompanyMock.Setup(mock => mock.AvailableRisks.Clear()).Verifiable();

            AvailableRiskUpdater.EmptyAvailableRiskList();

            CompanyMock.Verify(mock => mock.AvailableRisks.Clear(), Times.Once);
        }

        [Test]
        public void RemoveAvailableRisk_ShouldThrowIfRiskDoesNotExist()
        {
            var testAvailableRiskList = new List<Risk>();
            testAvailableRiskList.Add(DefaultRisk);

            var testRiskName = DEFAULT_RISK_NAME;

            CompanyMock.Setup(mock => mock.AvailableRisks).Returns(testAvailableRiskList);
            
            Assert.That(() => AvailableRiskUpdater.RemoveAvailableRisk(testRiskName), Throws.Exception);
        }

        [Test]
        public void RemoveAvailableRisk_ShouldThrowIfAvailableRiskListIsEmpty()
        {
            var testAvailableRiskList = new List<Risk>(); 

            var testRiskName = DEFAULT_RISK_NAME;

            CompanyMock.Setup(mock => mock.AvailableRisks).Returns(testAvailableRiskList);

            Assert.That(() => AvailableRiskUpdater.RemoveAvailableRisk(testRiskName), Throws.Exception);
        }

        [Test]
        public void RemoveAvailableRisk_ShouldCallAvailableRisksListToChangeValue()
        {
            var testAvailableRiskList = new List<Risk>();
            testAvailableRiskList.Add(DefaultRisk);

            var testRiskName = DEFAULT_RISK_NAME;

            CompanyMock.Setup(mock => mock.AvailableRisks).Returns(testAvailableRiskList);

            AvailableRiskUpdater.RemoveAvailableRisk(testRiskName);

            CompanyMock.Verify(mock => mock.AvailableRisks, Times.Exactly(2));
        }

        [Test]
        public void UpdateAvailableRisk_ShouldThrowIfPriceIsNegative()
        {
            Assert.That(() => AvailableRiskUpdater.UpdateAvailableRisk(DEFAULT_RISK_NAME, -0.1m), 
                Throws.Exception);
        }

        [Test]
        public void UpdateAvailableRisk_ShouldCallRemove()
        {
            Assert.That(() => AvailableRiskUpdater.UpdateAvailableRisk(DEFAULT_RISK_NAME, -0.1m),
                Throws.Exception);
        }
    }
}
