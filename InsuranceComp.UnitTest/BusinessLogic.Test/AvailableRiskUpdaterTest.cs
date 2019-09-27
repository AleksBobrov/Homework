using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace InsuranceComp.UnitTest.BusinessLogic.Test
{
    [TestFixture]
    public class AvailableRiskUpdaterTest
    {
        [Test]
        public void AddAvailableRisk_ShouldCallAddMethodOnAvalaibleRiskList()
        {
            var testRisk = new Risk {
                Name = "Test risk 1",
                YearlyPrice = 5.5m
            };

            var mockCompany = new Mock<IInsuranceCompany>();

            mockCompany.Setup(mock => mock.AvailableRisks.Contains(testRisk)).Returns(false);

            var availableRiskUpdater = new AvailableRiskUpdater(mockCompany.Object);
            availableRiskUpdater.AddAvailableRisk(testRisk);

            mockCompany.Verify(mock => mock.AvailableRisks.Add(testRisk), Times.Once);
        }

        [Test]
        public void AddAvailableRisk_ShouldThrowWhenAvaialableRisksListAlreadyContainRisk()
        {
            var testRisk = new Risk
            {
                Name = "Test risk 1",
                YearlyPrice = 5.5m
            };

            var mockCompany = new Mock<IInsuranceCompany>();

            mockCompany.Setup(mock => mock.AvailableRisks.Contains(testRisk)).Returns(true);

            var availableRiskUpdater = new AvailableRiskUpdater(mockCompany.Object);
            Assert.That(() => availableRiskUpdater.AddAvailableRisk(testRisk), Throws.Exception);
        }

        [Test]
        public void EmptyAvailableRiskList_ShouldCallClearMethodOnAvailableListRisk()
        {
            var mockCompany = new Mock<IInsuranceCompany>();

            mockCompany.Setup(mock => mock.AvailableRisks.Clear()).Verifiable();

            var availableRiskUpdater = new AvailableRiskUpdater(mockCompany.Object);
            availableRiskUpdater.EmptyAvailableRiskList();

            mockCompany.Verify(mock => mock.AvailableRisks.Clear(), Times.Once);
        }

        [Test]
        public void RemoveAvailableRisk_ShouldThrowIfRiskDoesNotExist()
        {
            var testRisk = new Risk
            {
                Name = "Test risk 1",
                YearlyPrice = 5.5m
            };

            var testAvailableRiskList = new List<Risk>();
            testAvailableRiskList.Add(testRisk);

            var mockCompany = new Mock<IInsuranceCompany>();

            var testRiskName = "test risk";

            mockCompany.Setup(mock => mock.AvailableRisks).Returns(testAvailableRiskList);

            var availableRiskUpdater = new AvailableRiskUpdater(mockCompany.Object);
            
            Assert.That(() => availableRiskUpdater.RemoveAvailableRisk(testRiskName), Throws.Exception);
        }

        [Test]
        public void RemoveAvailableRisk_ShouldThrowIfAvailableRiskListIsEmpty()
        {
            var testAvailableRiskList = new List<Risk>(); 

            var mockCompany = new Mock<IInsuranceCompany>();

            var testRiskName = "test risk";

            mockCompany.Setup(mock => mock.AvailableRisks).Returns(testAvailableRiskList);

            var availableRiskUpdater = new AvailableRiskUpdater(mockCompany.Object);

            Assert.That(() => availableRiskUpdater.RemoveAvailableRisk(testRiskName), Throws.Exception);
        }

        [Test]
        public void RemoveAvailableRisk_ShouldCallAvailableRisksListToChangeValue()
        {
            var testRisk = new Risk
            {
                Name = "test risk",
                YearlyPrice = 5.5m
            };

            var testAvailableRiskList = new List<Risk>();
            testAvailableRiskList.Add(testRisk);

            var mockCompany = new Mock<IInsuranceCompany>();

            var testRiskName = "test risk";

            mockCompany.Setup(mock => mock.AvailableRisks).Returns(testAvailableRiskList);

            var availableRiskUpdater = new AvailableRiskUpdater(mockCompany.Object);
            availableRiskUpdater.RemoveAvailableRisk(testRiskName);

            mockCompany.Verify(mock => mock.AvailableRisks, Times.Exactly(2));
        }
    }
}
