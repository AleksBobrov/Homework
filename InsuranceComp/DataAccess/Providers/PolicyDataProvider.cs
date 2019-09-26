using System.Collections.Generic;

namespace InsuranceComp.DataAccess.Providers
{
    public class PolicyDataProvider : IPolicyDataProvider
    {
        public PolicyModel Get(string id)
        {
            return FakeStorage.Instance.PolicyList.Find(policy => policy.Id == id);
        }

        public IList<PolicyModel> GetAll()
        {
            return FakeStorage.Instance.PolicyList;
        }

        public void Remove(string id)
        {
            FakeStorage.Instance.PolicyList.RemoveAll(policy => policy.Id == id);
        }
    }
}
