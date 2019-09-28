using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace InsuranceComp.UnitTest.DataAccess.Test.Models.Test
{
    [TestFixture]
    public class PolicyModelTest
    {
        [Test]
        public void PolicyModel_GettersReturnCorrectValues()
        {
            const string nameOfInsuredObject = "obj";
            DateTime validFrom = new DateTime(2020, 5, 5);
            DateTime validTill = new DateTime(2020, 6, 6);
            const decimal premium = 20.0m;
            IList<Risk> insuredRisks = new List<Risk>()
            {
                new Risk()
                {
                    Name = "risk 1",
                    YearlyPrice = 1.0m
                },
                new Risk()
                {
                    Name = "risk 2",
                    YearlyPrice = 2.0m
                }
            };
            const string id = "testid";

            var testPolicyModel = new PolicyModel()
            {
                NameOfInsuredObject = nameOfInsuredObject,
                ValidFrom = validFrom,
                ValidTill = validTill,
                Premium = premium,
                InsuredRisks = insuredRisks,
                Id = id
            };

            Assert.AreEqual(nameOfInsuredObject, testPolicyModel.NameOfInsuredObject);
            Assert.AreEqual(validFrom, testPolicyModel.ValidFrom);
            Assert.AreEqual(validTill, testPolicyModel.ValidTill);
            Assert.AreEqual(premium, testPolicyModel.Premium);
            Assert.AreEqual(id, testPolicyModel.Id);

            testPolicyModel.InsuredRisks.Should().BeEquivalentTo(insuredRisks);
        }
    }
}
