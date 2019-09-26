using System.Collections.Generic;

namespace InsuranceComp
{
    public sealed class FakeStorage
    { 
        public List<PolicyModel> PolicyList { get; set; }
        public List<RiskModel> RiskList { get; set; }
        private FakeStorage()
        {
        }
        private static FakeStorage instance = null;
        public static FakeStorage Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FakeStorage {
                        PolicyList = new List<PolicyModel>(),
                        RiskList = new List<RiskModel>()
                    };
                }
                return instance;
            }
        }
    }
}
