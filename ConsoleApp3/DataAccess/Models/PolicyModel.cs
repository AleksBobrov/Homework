using System;
using System.Collections.Generic;

namespace InsuranceComp
{
    public class PolicyModel
    {
        public string NameOfInsuredObject { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTill { get; set; }
        public decimal Premium { get; set; }
        public IList<Risk> InsuredRisks { get; set; }
        public string Id { get; set; }
    }
}
