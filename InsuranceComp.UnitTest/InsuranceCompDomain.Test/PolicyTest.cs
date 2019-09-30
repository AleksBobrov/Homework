using FluentAssertions;
using InsuranceComp.InsuranceCompDomain;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace InsuranceComp.UnitTest.InsuranceCompDomain.Test
{
    [TestFixture]
    public class PolicyTest
    {
        [Test]
        public void Policy_GettersReturnCorrectValues()
        {
            const string nameOfInsuredObject = "obj";
            DateTime validFrom = new DateTime(2020, 4, 4);
            DateTime validTill = new DateTime(2020, 5, 5);
            const decimal premium = 1.0m;
            IList<Risk> insuredRisks = new List<Risk>()
            {
                new Risk()
                {
                    Name = "risk 1",
                    YearlyPrice = 2.0m
                },
                new Risk()
                {
                    Name = "risk 2",
                    YearlyPrice = 4.0m
                }
            };

            var policy = new Policy()
            {
                NameOfInsuredObject = nameOfInsuredObject,
                ValidFrom = validFrom,
                ValidTill = validTill,
                Premium = premium,
                InsuredRisks = insuredRisks
            };

            Assert.AreEqual(nameOfInsuredObject, policy.NameOfInsuredObject);
            Assert.AreEqual(validFrom, policy.ValidFrom);
            Assert.AreEqual(validTill, policy.ValidTill);
            Assert.AreEqual(premium, policy.Premium);

            policy.NameOfInsuredObject.Should().BeEquivalentTo(nameOfInsuredObject);
        }
    }
}
