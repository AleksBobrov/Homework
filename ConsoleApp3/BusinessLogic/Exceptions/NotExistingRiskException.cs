using System;

namespace InsuranceComp.BusinessLogic.Exceptions
{
    [Serializable]
    public class NotExistingRiskException : Exception
    {
        public NotExistingRiskException() : base("There is no such risk in available risk list.")
        {

        }
    }
    
}