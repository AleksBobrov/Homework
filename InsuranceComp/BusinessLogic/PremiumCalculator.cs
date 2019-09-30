using InsuranceComp.DataAccess.Repositories;
using InsuranceComp.Helpers;
using InsuranceComp.InsuranceCompDomain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InsuranceComp.BusinessLogic
{
    public class PremiumCalculator : IPremiumCalculator
    {
        private IRiskRepository _riskRepository;
        public PremiumCalculator(IRiskRepository riskRepository)
        {
            _riskRepository = riskRepository;
        }
        //Used to calculate premium before adding to storage
        public decimal CalculateInitialPremium(IList<Risk> riskList, DateTime validFrom, DateTime validTill)
        {
            decimal premium = 0.0m;

            foreach (var insuredRisk in riskList)
            {
                premium += insuredRisk.YearlyPrice / 365.0m * (decimal)(validTill - validFrom).TotalDays;
            }

            return Math.Round(premium, 2);
        }
        //Used to calculate premium for Risk already in storage
        public decimal CalculatePremiumOfSoldPolicy(string nameOfInsuredObject, DateTime effectiveDate)
        {
            var policyId = IdGenerator.ConstructPolicyId(nameOfInsuredObject, effectiveDate);
            decimal premium = 0.0m;

            var insuredRisks = _riskRepository
                .GetAll()
                .Where(riskModel => riskModel.PolicyId == policyId)
                .ToList();

            foreach (var insuredRisk in insuredRisks)
            {
                premium += insuredRisk.YearlyPrice / 365.0m * 
                    (decimal)(insuredRisk.ValidTill - insuredRisk.ValidFrom).TotalDays;
            }

            return Math.Round(premium, 2);
        }
    }
}
