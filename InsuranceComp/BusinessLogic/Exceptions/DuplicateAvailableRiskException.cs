using System;

namespace InsuranceComp.BusinessLogic.Exceptions
{
    [Serializable]
    public class DuplicateAvailableRiskException : Exception
    {
        public DuplicateAvailableRiskException(string riskName) 
            : base($"Available risk with '{riskName}' name already exists.")
        {

        }
    }
}
