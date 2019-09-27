using InsuranceComp.BusinessLogic.Exceptions;
using System;

namespace InsuranceComp.Helpers
{
    /* 
     In IInsuranceCompany interface all methods provide nameOfInsuranceObject and
     effectiveDate as main params to identify policies, I decided to create serverside
     generated Id's out of those. I believe that helps make communication between 
     business logic layer and data access layer a little bit cleaner.
    */
    public static class IdGenerator
    {
        public static string ConstructPolicyId(string name, DateTime effectiveDate)
        {
            if (string.IsNullOrEmpty(name)) throw new CannotBeNullException(nameof(name));

            return name + effectiveDate.ToString();
        }

        public static string ConstructRiskId(string riskName, string parentName, DateTime effectiveDate)
        {
            if (string.IsNullOrEmpty(riskName)) throw new CannotBeNullException(nameof(riskName));
            if (string.IsNullOrEmpty(parentName)) throw new CannotBeNullException(nameof(parentName));


            return riskName + parentName + effectiveDate.ToString();
        }
    }
}
