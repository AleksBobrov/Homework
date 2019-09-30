using InsuranceComp.InsuranceCompDomain;

namespace InsuranceComp
{
    public interface IAvailableRiskUpdater
    {
        IAvailableRiskUpdater AddAvailableRisk(Risk risk);

        IAvailableRiskUpdater RemoveAvailableRisk(string nameOfRisktoRemove);

        IAvailableRiskUpdater UpdateAvailableRisk(string nameOfRisktoUpdate, decimal newYearlyPrice);

        IAvailableRiskUpdater EmptyAvailableRiskList();
    }
}
