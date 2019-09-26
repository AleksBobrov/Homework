using InsuranceComp.BusinessLogic.Exceptions;
using System;

namespace InsuranceComp.Helpers
{
    public static class IdGenerator
    {
        public static string ConstructPolicyId(string name, DateTime effectiveDate)
        {
            if (string.IsNullOrEmpty(name)) throw new CannotBeNullException();

            return name + effectiveDate.ToString();
        }

        public static string ConstructRiskId(string riskName, string parentName, DateTime effectiveDate)
        {
            if (string.IsNullOrEmpty(riskName) || string.IsNullOrEmpty(parentName))
                throw new CannotBeNullException();

            return riskName + parentName + effectiveDate.ToString();
        }
    }
}
