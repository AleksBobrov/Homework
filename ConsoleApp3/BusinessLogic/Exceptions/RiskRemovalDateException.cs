using System;

namespace InsuranceComp.BusinessLogic.Exceptions
{
    [Serializable]
    public class RiskRemovalDateException : Exception
    {
        public RiskRemovalDateException() : base("When removing risk, date should not be in past and should be within policy validity period.")
        {

        }
    }
}
