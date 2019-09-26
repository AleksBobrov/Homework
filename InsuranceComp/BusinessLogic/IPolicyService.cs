using System;

namespace InsuranceComp.BusinessLogic
{
    public interface IPolicyService
    {
        IPolicy SellPolicy(IPolicy policy);

        IPolicy GetPolicy(string nameOfInsuredObject, DateTime effectiveDate);
    }
}
