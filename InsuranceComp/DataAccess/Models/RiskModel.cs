using System;

namespace InsuranceComp
{
    public class RiskModel
    {
        public string Name { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTill { get; set; }
        public DateTime PolicyEffectiveDate { get; set; }
        public string PolicyId { get; set; }
        public string Id { get; set; }
        public decimal YearlyPrice { get; set; }
    }
}
