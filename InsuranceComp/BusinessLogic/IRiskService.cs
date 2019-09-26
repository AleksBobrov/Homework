using System;

namespace InsuranceComp.BusinessLogic
{
    public interface IRiskService
    {
        void AddRisk(string nameOfInsuredObject, Risk risk, DateTime validFrom, DateTime effectiveDate);
        void RemoveRisk(string nameOfInsuredObject, Risk risk, DateTime validTill, DateTime effectiveDate);
    }
}
