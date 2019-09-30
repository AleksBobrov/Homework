using InsuranceComp.BusinessLogic.Exceptions;
using InsuranceComp.InsuranceCompDomain;
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
            var risk = _insuranceCompany
                .AvailableRisks
                .FirstOrDefault(avRisk => avRisk.Name == nameOfRisktoRemove);

            if (string.IsNullOrEmpty(risk.Name))
            {
                throw new RiskDoesNotExistInAvailableListException(nameOfRisktoRemove);
            } else
            {
                _insuranceCompany.AvailableRisks.Remove(risk);
            }
            return this;
        }

        public IAvailableRiskUpdater UpdateAvailableRisk(string nameOfRisktoUpdate, decimal newYearlyPrice)
        {
            if (newYearlyPrice < 0) throw new NegativeRiskYearlyPriceException();

            RemoveAvailableRisk(nameOfRisktoUpdate);

            AddAvailableRisk(new Risk {
                Name = nameOfRisktoUpdate,
                YearlyPrice = newYearlyPrice
            });

            return this;
        }
    }
}
