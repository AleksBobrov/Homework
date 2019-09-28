using System;

namespace InsuranceComp.BusinessLogic.Exceptions
{
    [Serializable]
    public class NegativeRiskYearlyPriceException : Exception
    {
        public NegativeRiskYearlyPriceException() : base("Risk yearly price can not be negative.")
        {

        }
    }
}
