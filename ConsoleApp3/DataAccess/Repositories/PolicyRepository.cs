using System.Collections.Generic;

namespace InsuranceComp
{
    public class PolicyRepository : IPolicyRepository
    {
        private IDataProvider<PolicyModel> _dataProvider;
        public PolicyRepository(IDataProvider<PolicyModel> dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public void Add(PolicyModel model)
        {
            var data = _dataProvider.GetAll();
            data.Add(model);
        }

        public void Edit(PolicyModel model)
        {
            Remove(model.Id);
            Add(model);
        }

        public PolicyModel Get(string id)
        {
            return _dataProvider.Get(id);
        }

        public IList<PolicyModel> GetAll()
        {
            return _dataProvider.GetAll();
        }

        public void Remove(string id)
        {
            _dataProvider.Remove(id);
        }
    }
}
