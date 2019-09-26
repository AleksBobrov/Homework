using System;

namespace InsuranceComp.BusinessLogic.Exceptions
{
    [Serializable]
    public class DuplicateRiskException : Exception
    {
        public DuplicateRiskException() : base("Duplicate risk.")
        {

        }
    }
}
