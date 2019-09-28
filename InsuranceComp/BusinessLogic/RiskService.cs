using InsuranceComp.BusinessLogic.Exceptions;
using InsuranceComp.DataAccess.Repositories;
using InsuranceComp.Helpers;
using System;

namespace InsuranceComp.BusinessLogic
{
    public class RiskService : IRiskService
    {
        private IPolicyRepository _policyRepository;
        private IRiskRepository _riskRepository;
        public RiskService(IPolicyRepository policyRepository, IRiskRepository riskRepository)
        {
            _policyRepository = policyRepository;
            _riskRepository = riskRepository;
        }

        public void AddRisk(string nameOfInsuredObject, Risk risk, DateTime validFrom, DateTime effectiveDate)
        {
            if (validFrom < effectiveDate) throw new RiskValidityPeriodException();

            var policyId = IdGenerator.ConstructPolicyId(nameOfInsuredObject, effectiveDate);

            var policyModel = _policyRepository.Get(policyId);

            if (validFrom > policyModel.ValidTill) throw new RiskValidityPeriodException();
            if (policyModel.InsuredRisks.Contains(risk)) throw new DuplicateRiskException(risk.Name);

            var riskId = IdGenerator.ConstructRiskId(risk.Name, policyModel.NameOfInsuredObject, effectiveDate);

            var riskModel = new RiskModel
            {
                Name = risk.Name,
                ValidFrom = validFrom,
                PolicyEffectiveDate = effectiveDate,
                PolicyId = policyId,
                Id = riskId,
                ValidTill = policyModel.ValidTill,
                YearlyPrice = risk.YearlyPrice
            };

            _riskRepository.Add(riskModel);
        }

        public void RemoveRisk(string nameOfInsuredObject, Risk risk, DateTime validTill, DateTime effectiveDate)
        {
            var policyId = IdGenerator.ConstructPolicyId(nameOfInsuredObject, effectiveDate);

            var policyModel = _policyRepository.Get(policyId);

            if (validTill.Date > policyModel.ValidTill.Date) throw new RiskRemovalDateException();

            var riskId = IdGenerator.ConstructRiskId(risk.Name, nameOfInsuredObject, effectiveDate);
           
            var riskModel = _riskRepository.Get(riskId);

            var updatedRiskModel = new RiskModel
            {
                Name = riskModel.Name,
                ValidFrom = riskModel.ValidFrom,
                PolicyEffectiveDate = riskModel.PolicyEffectiveDate,
                PolicyId = riskModel.PolicyId,
                Id = riskModel.Id,
                ValidTill = validTill,
                YearlyPrice = riskModel.YearlyPrice
            };

            _riskRepository.Edit(updatedRiskModel);
        }
    }
}
