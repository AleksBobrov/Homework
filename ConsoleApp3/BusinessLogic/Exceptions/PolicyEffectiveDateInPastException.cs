using System;

namespace InsuranceComp.BusinessLogic.Exceptions
{
    [Serializable]
    public class PolicyEffectiveDateInPastException : Exception
    {
        public PolicyEffectiveDateInPastException() : base("Policy can not be with effective date in past.")
        {

        }
    }
}
