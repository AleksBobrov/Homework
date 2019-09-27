using System;

namespace InsuranceComp.BusinessLogic.Exceptions
{
    [Serializable]
    public class DuplicatePolicyException : Exception
    {
        public DuplicatePolicyException (string nameOfInsuredObject, DateTime effectiveDate)
            : base($"Policy with insured object name '{nameOfInsuredObject}' " +
                  $"and effective date {effectiveDate.ToShortDateString()} already exists.")
        {

        }
    }
}
