using System;

namespace InsuranceComp.BusinessLogic.Exceptions
{
    [Serializable]
    public class RiskNotAvailableException : Exception
    {
        public RiskNotAvailableException() : base("Risk you are trying to add is not available.")
        {

        }
    }
}
