using System;

namespace InsuranceComp.BusinessLogic.Exceptions
{
    [Serializable]
    public class RiskDoesNotExistInAvailableListException : Exception
    {
        public RiskDoesNotExistInAvailableListException(string riskName) 
            : base($"Risk with name '{riskName}' does not exist in available list.")
        {

        }
    }
    
}