using System;

namespace InsuranceComp.BusinessLogic.Exceptions
{
    [Serializable]
    public class RiskValidityPeriodException : Exception
    {
        public RiskValidityPeriodException() : base("Risk should be within policy validity period.")
        {

        }
    }
}
