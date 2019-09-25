using System;

namespace InsuranceComp.BusinessLogic.Exceptions
{
    [Serializable]
    public class NegativePolicyDurationException : Exception
    {
        public NegativePolicyDurationException() : base("Policy duration period can not be 0 or negative.")
        {

        }
    }
}
