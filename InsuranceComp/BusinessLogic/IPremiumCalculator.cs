using InsuranceComp.InsuranceCompDomain;
using System;
using System.Collections.Generic;

namespace InsuranceComp.BusinessLogic
{
    public interface IPremiumCalculator
    {
        decimal CalculateInitialPremium(IList<Risk> riskList, DateTime validFrom, DateTime ValidTill);
        decimal CalculatePremiumOfSoldPolicy(string nameOfInsuredObject, DateTime effectiveDate);
    }
}
