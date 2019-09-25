using System.Collections.Generic;

namespace InsuranceComp.DataAccess.Providers
{
    public class RiskDataProvider : IRiskDataProvider
    {
        public RiskModel Get(string id)
        {
            return FakeStorage.Instance.RiskList.Find(risk => risk.Id == id);
        }

        public IList<RiskModel> GetAll()
        {
            return FakeStorage.Instance.RiskList;
        }

        public void Remove(string id)
        {
            FakeStorage.Instance.RiskList.RemoveAll(risk => risk.Id == id);
        }
}
}
