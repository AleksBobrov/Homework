using InsuranceComp.BusinessLogic.Exceptions;
using System.Linq;

namespace InsuranceComp
{
    public class AvailableRiskUpdater : IAvailableRiskUpdater
    {
        private IInsuranceCompany _insuranceCompany;
        public AvailableRiskUpdater(IInsuranceCompany insuranceCompany)
        {
            _insuranceCompany = insuranceCompany;
        }

        public IAvailableRiskUpdater AddAvailableRisk(Risk risk)
        {
            if (!_insuranceCompany.AvailableRisks.Contains(risk))
            {
                _insuranceCompany.AvailableRisks.Add(risk);
                return this;
            }
            else
            {
                throw new DuplicateAvailableRiskException(risk.Name);
            }
        }

        public IAvailableRiskUpdater EmptyAvailableRiskList()
        {
            _insuranceCompany.AvailableRisks.Clear();
            return this;
        }

        public IAvailableRiskUpdater RemoveAvailableRisk(string nameOfRisktoRemove)
        {
            var riskIndex = _insuranceCompany
                .AvailableRisks
                .ToList()
                .FindIndex(risk => risk.Name == nameOfRisktoRemove);

            if (riskIndex == -1)
            {
                throw new RiskDoesNotExistInAvailableListException(nameOfRisktoRemove);
            } else
            {
                _insuranceCompany.AvailableRisks.RemoveAt(riskIndex);
            }
            return this;
        }

        public IAvailableRiskUpdater UpdateAvailableRisk(string nameOfRisktoUpdate, Risk riskUpdate)
        {
            var riskToUpdate = _insuranceCompany.AvailableRisks
                .FirstOrDefault(avRisk => avRisk.Name == nameOfRisktoUpdate);

            if (riskUpdate.Name == null)
            {
                riskUpdate.Name = riskToUpdate.Name;
            }

            if (riskUpdate.YearlyPrice == 0.0m)
            {
                riskUpdate.YearlyPrice = riskToUpdate.YearlyPrice;
            }

            _insuranceCompany.AvailableRisks = _insuranceCompany
                .AvailableRisks
                .Where(avRisk => avRisk.Name != nameOfRisktoUpdate)
                .ToList();

            _insuranceCompany.AvailableRisks.Add(riskUpdate);

            return this;
        }
    }
}
