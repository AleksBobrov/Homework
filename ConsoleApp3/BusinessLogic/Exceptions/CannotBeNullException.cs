using System;

namespace InsuranceComp.BusinessLogic.Exceptions
{
    [Serializable]
    public class CannotBeNullException : Exception
    {
        public CannotBeNullException() : base("Parameter can not be null.") 
        {

        }
    }
}
