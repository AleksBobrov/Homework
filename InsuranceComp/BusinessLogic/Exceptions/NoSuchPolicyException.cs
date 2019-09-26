using System;

namespace InsuranceComp.BusinessLogic.Exceptions
{
    [Serializable]
    public class NoSuchPolicyException : Exception
    {
        public NoSuchPolicyException() : base("Such policy does not exist.")
        {

        }
    }
}
