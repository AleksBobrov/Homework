using System;

namespace InsuranceComp.BusinessLogic.Exceptions
{
    [Serializable]
    public class RiskValidFromDateInPastException : Exception
    {
        public RiskValidFromDateInPastException() : base("Risk can not be with valid from date in past.")
        {

        }
    }
}
