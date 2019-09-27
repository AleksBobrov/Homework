using InsuranceComp.BusinessLogic;
using InsuranceComp.BusinessLogic.Exceptions;
using System;
using System.Collections.Generic;

namespace InsuranceComp
{
    public class InsuranceCompany : IInsuranceCompany
    {
        public string Name { get; }

        public IList<Risk> AvailableRisks { get; set; }

        private IPolicyService _policyService;

        private IRiskService _riskService;

        private IPremiumCalculator _premiumCalculator;
        public InsuranceCompany(string name, IList<Risk> availableRisks)
        {
            Name = name;
            AvailableRisks = availableRisks;
        }

        public InsuranceCompany(string name, IList<Risk> availableRisks, 
            IPolicyService policyService, IRiskService riskService, IPremiumCalculator premiumCalculator)
        {
            Name = name;
            AvailableRisks = availableRisks;
            _policyService = policyService;
            _riskService = riskService;
            _premiumCalculator = premiumCalculator;
        }

        public void AddRisk(string nameOfInsuredObject, Risk risk, DateTime validFrom, DateTime effectiveDate)
        {
            if (string.IsNullOrEmpty(nameOfInsuredObject))
                throw new CannotBeNullException(nameof(nameOfInsuredObject));

            if (validFrom.Date < DateTime.Now.Date) throw new RiskValidFromDateInPastException();

            if (!AvailableRisks.Contains(risk)) throw new RiskDoesNotExistInAvailableListException(risk.Name);

            _riskService.AddRisk(nameOfInsuredObject, risk, validFrom, effectiveDate);
        }

        public IPolicy GetPolicy(string nameOfInsuredObject, DateTime effectiveDate)
        {
            return _policyService.GetPolicy(nameOfInsuredObject, effectiveDate);
        }

        public void RemoveRisk(string nameOfInsuredObject, Risk risk, DateTime validTill, DateTime effectiveDate)
        {
            if (validTill.Date < DateTime.Now.Date) throw new RiskRemovalDateException();

            _riskService.RemoveRisk(nameOfInsuredObject, risk, validTill, effectiveDate);
        }

        public IPolicy SellPolicy(string nameOfInsuredObject, DateTime validFrom, short validMonths, IList<Risk> selectedRisks)
        {
            if (string.IsNullOrEmpty(nameOfInsuredObject))
                throw new CannotBeNullException(nameof(nameOfInsuredObject));

            if (validFrom.Date < DateTime.Now.Date) throw new PolicyEffectiveDateInPastException();

            if (validMonths < 1) throw new NegativePolicyDurationException();

            var validTill = validFrom.AddMonths(validMonths);

            var policy = new Policy
            {
                NameOfInsuredObject = nameOfInsuredObject,
                ValidFrom = validFrom,
                ValidTill = validTill,
                InsuredRisks = selectedRisks,
                Premium = _premiumCalculator.CalculateInitialPremium(selectedRisks, validFrom, validTill)
            };

            _policyService.SellPolicy(policy);

            return policy;
        }
    }
}
