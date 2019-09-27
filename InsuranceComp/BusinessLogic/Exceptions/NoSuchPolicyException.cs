using System;

namespace InsuranceComp.BusinessLogic.Exceptions
{
    [Serializable]
    public class NoSuchPolicyException : Exception
    {
        public NoSuchPolicyException(string nameOfInsuredObject, DateTime effectiveDate)
            : base($"Policy with name '{nameOfInsuredObject}' and effective date" +
                  $"{effectiveDate.ToShortDateString()} does not exist.")
        {

        }
    }
}
