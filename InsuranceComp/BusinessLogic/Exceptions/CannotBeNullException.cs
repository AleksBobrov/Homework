using System;

namespace InsuranceComp.BusinessLogic.Exceptions
{
    [Serializable]
    public class CannotBeNullException : Exception
    {
        public CannotBeNullException(string argName) 
            : base("Argument '" + argName + "' can not be null.")
        {

        }
    }
}
