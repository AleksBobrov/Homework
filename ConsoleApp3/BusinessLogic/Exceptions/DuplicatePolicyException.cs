using System;

namespace InsuranceComp.BusinessLogic.Exceptions
{
    [Serializable]
    public class DuplicatePolicyException : Exception
    {
        public DuplicatePolicyException () : base("Duplicate policy!")
        {

        }
    }
}
