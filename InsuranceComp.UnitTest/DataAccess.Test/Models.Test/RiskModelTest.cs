using NUnit.Framework;
using System;

namespace InsuranceComp.UnitTest.DataAccess.Test.Models.Test
{
    [TestFixture]
    public class RiskModelTest
    {
        [Test]
        public void RiskModel_GettersReturnCorrectValues()
        {
            const string name = "risk name";
            DateTime validFrom = new DateTime(2020, 5, 5);
            DateTime validTill = new DateTime(2020, 6, 6);
            DateTime policyEffectiveDate = new DateTime(2020, 4, 4);
            const string policyId = "policyId";
            const string id = "id";
            const decimal yearlyPrice = 10.0m;

            var testRiskModel = new RiskModel()
            {
                Name = name,
                ValidFrom = validFrom,
                ValidTill = validTill,
                PolicyEffectiveDate = policyEffectiveDate,
                PolicyId = policyId,
                Id = id,
                YearlyPrice = yearlyPrice
            };

            Assert.AreEqual(name, testRiskModel.Name);
            Assert.AreEqual(validFrom, testRiskModel.ValidFrom);
            Assert.AreEqual(validTill, testRiskModel.ValidTill);
            Assert.AreEqual(policyEffectiveDate, testRiskModel.PolicyEffectiveDate);
            Assert.AreEqual(policyId, testRiskModel.PolicyId);
            Assert.AreEqual(id, testRiskModel.Id);
            Assert.AreEqual(yearlyPrice, testRiskModel.YearlyPrice);
        }
    }
}
