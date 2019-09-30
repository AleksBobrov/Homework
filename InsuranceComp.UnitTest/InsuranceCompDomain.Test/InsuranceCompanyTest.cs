using InsuranceComp.BusinessLogic;
using InsuranceComp.InsuranceCompDomain;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace InsuranceComp.UnitTest.InsuranceCompDomain.Test
{
    [TestFixture]
    public class InsuranceCompanyTest
    {
        IInsuranceCompany Company;
        Mock<IRiskService> RiskServiceMock;
        Mock<IPolicyService> PolicyServiceMock;
        Mock<IPremiumCalculator> PremiumCalculatorMock;

        [SetUp]
        public void SetUp()
        {
            RiskServiceMock = new Mock<IRiskService>();
            PolicyServiceMock = new Mock<IPolicyService>();
            PremiumCalculatorMock = new Mock<IPremiumCalculator>();

            Company = new InsuranceCompany("If", new List<Risk>(), 
                PolicyServiceMock.Object, 
                RiskServiceMock.Object, 
                PremiumCalculatorMock.Object);
        }

        [Test]
        public void AddRisk_ShouldThrowIfNameIsNull()
        {
            Assert.That(() => Company.AddRisk(null, It.IsAny<Risk>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()),
                Throws.Exception);
        }


        [Test]
        public void AddRisk_ShouldThrowIfNameIsEmpty()
        {
            Assert.That(() => Company.AddRisk("", It.IsAny<Risk>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()),
                Throws.Exception);
        }

        [Test]
        public void AddRisk_ShouldThrowIfValidFromDateIsInThePast()
        {
            Assert.That(() => Company.AddRisk("Obj", It.IsAny<Risk>(), DateTime.Now.AddDays(-5),
                It.IsAny<DateTime>()), Throws.Exception);
        }

        [Test]
        public void AddRisk_ShouldThrowIfRiskIsNotAvailable()
        {
            Assert.That(() => Company.AddRisk("Obj", new Risk() { Name = "name" }, DateTime.Now,
                It.IsAny<DateTime>()), Throws.Exception);
        }

        [Test]
        public void AddRisk_ShouldCallRiskServiceAddOnce()
        {
            RiskServiceMock.Setup(mock => mock.AddRisk(It.IsAny<string>(), It.IsAny<Risk>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>())).Verifiable();

            Company.AvailableRisks.Add(new Risk() { Name = "name" });

            Company.AddRisk("Obj", new Risk() { Name = "name" }, DateTime.Now,
                It.IsAny<DateTime>());

            RiskServiceMock.Verify(mock => mock.AddRisk(It.IsAny<string>(), It.IsAny<Risk>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Test]
        public void GetPolicy_ShouldCallPolicyRepositoryGetOnce()
        {
            Company.GetPolicy(It.IsAny<string>(), It.IsAny<DateTime>());

            PolicyServiceMock.Verify(mock => mock.GetPolicy(It.IsAny<string>(), It.IsAny<DateTime>()), 
                Times.Once);
        }

        [Test]
        public void RemoveRisk_ShouldThrowIfValidTillDateIsInPast()
        {
            Assert.That(() => Company.RemoveRisk(It.IsAny<string>(), It.IsAny<Risk>(), DateTime.Now.AddDays(-2), 
                It.IsAny<DateTime>()), Throws.Exception);
        }

        [Test]
        public void RemoveRisk_ShouldCallRiskServiceRemoveRiskOnce()
        {
            Company.RemoveRisk(It.IsAny<string>(), It.IsAny<Risk>(), DateTime.Now,
                It.IsAny<DateTime>());

            RiskServiceMock.Verify(mock => mock.RemoveRisk(It.IsAny<string>(), It.IsAny<Risk>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>()), Times.Once);
        }

        [Test]
        public void SellPolicy_ShouldThrowIfNameIsNull()
        {
            Assert.That(() => Company.SellPolicy(null, It.IsAny<DateTime>(), It.IsAny<short>(), 
                It.IsAny<IList<Risk>>()), Throws.Exception);
        }

        [Test]
        public void SellPolicy_ShouldThrowIfNameIsEmpty()
        {
            Assert.That(() => Company.SellPolicy("", It.IsAny<DateTime>(), It.IsAny<short>(),
                It.IsAny<IList<Risk>>()), Throws.Exception);
        }

        [Test]
        public void SellPolicy_ShouldThrowIfValidFromDateIsInPast()
        {
            Assert.That(() => Company.SellPolicy("Obj", DateTime.Now.AddDays(-2), It.IsAny<short>(),
                It.IsAny<IList<Risk>>()), Throws.Exception);
        }

        [Test]
        public void SellPolicy_ShouldThrowIfValidMonthsAmountIsLessThanOne()
        {
            Assert.That(() => Company.SellPolicy(null, DateTime.Now, 0,
                It.IsAny<IList<Risk>>()), Throws.Exception);
        }

        [Test]
        public void SellPolicy_ShouldCallPolicyServiceSellPolicyOnce()
        {
            Company.SellPolicy("Obj", DateTime.Now, 2,
                It.IsAny<IList<Risk>>());

            PolicyServiceMock.Verify(mock => mock.SellPolicy(It.IsAny<Policy>()), Times.Once);
        }
    }
}
