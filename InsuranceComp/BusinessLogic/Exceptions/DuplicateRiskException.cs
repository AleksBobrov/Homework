using System;

namespace InsuranceComp.BusinessLogic.Exceptions
{
    [Serializable]
    public class DuplicateRiskException : Exception
    {
        public DuplicateRiskException(string riskName)
            : base($"Risk with '{riskName}' name already exists on that policy.")
        {

        }
    }
}
