using InsuranceComp.BusinessLogic.Exceptions;
using InsuranceComp.DataAccess.Repositories;
using InsuranceComp.Helpers;
using System;
using System.Linq;

namespace InsuranceComp.BusinessLogic
{
    public class PolicyService : IPolicyService
    {
        private IPolicyRepository _policyRepository;
        private IRiskRepository _riskRepository;
        private IPremiumCalculator _premiumCalculator;
        public PolicyService(IPolicyRepository policyRepository, IRiskRepository riskRepository,
            IPremiumCalculator premiumCalculator)
        {
            _policyRepository = policyRepository;
            _riskRepository = riskRepository;
            _premiumCalculator = premiumCalculator;
        }

        public IPolicy GetPolicy(string nameOfInsuredObject, DateTime effectiveDate)
        {
            var policyId = IdGenerator.ConstructPolicyId(nameOfInsuredObject, effectiveDate);

            var policyModel = _policyRepository.Get(policyId);

            if (policyModel == null) throw new NoSuchPolicyException(nameOfInsuredObject, 
                effectiveDate);

            var policyRisks = _riskRepository
                .GetAll()
                .Where(riskModel => riskModel.PolicyId == policyId)
                .Select(riskModel => new Risk { Name = riskModel.Name, YearlyPrice = riskModel.YearlyPrice })
                .ToList();

            decimal premium = _premiumCalculator.CalculatePremiumOfSoldPolicy(nameOfInsuredObject, effectiveDate);

            return new Policy() {
                NameOfInsuredObject = policyModel.NameOfInsuredObject,
                ValidFrom = policyModel.ValidFrom,
                ValidTill = policyModel.ValidTill,
                InsuredRisks = policyRisks,
                Premium = premium
            };
        }

        public IPolicy SellPolicy(IPolicy policy)
        {
            var policyId = IdGenerator.ConstructPolicyId(policy.NameOfInsuredObject, policy.ValidFrom);

            if (_policyRepository.Get(policyId) != null)
                throw new DuplicatePolicyException(policy.NameOfInsuredObject, policy.ValidFrom);

            var policyModel = new PolicyModel {
                Id = policyId,
                NameOfInsuredObject = policy.NameOfInsuredObject,
                ValidFrom = policy.ValidFrom,
                ValidTill = policy.ValidTill,
                InsuredRisks = policy.InsuredRisks,
                Premium = policy.Premium
            };

            foreach (var risk in policy.InsuredRisks)
            {
                var riskId = IdGenerator.ConstructRiskId(risk.Name, policy.NameOfInsuredObject, policy.ValidFrom);

                var riskModel = new RiskModel
                {
                    Name = risk.Name,
                    ValidFrom = policy.ValidFrom,
                    ValidTill = policy.ValidTill,
                    PolicyEffectiveDate = policy.ValidFrom,
                    PolicyId = policyId,
                    Id = riskId,
                    YearlyPrice = risk.YearlyPrice
                };

                _riskRepository.Add(riskModel);
            }

            _policyRepository.Add(policyModel);

            return policy;
        }
    }
}
