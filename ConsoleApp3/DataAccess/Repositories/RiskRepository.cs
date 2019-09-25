using System.Collections.Generic;

namespace InsuranceComp.DataAccess.Repositories
{
    public class RiskRepository : IRiskRepository
    {
        private IDataProvider<RiskModel> _dataProvider;
        public RiskRepository(IDataProvider<RiskModel> dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public void Add(RiskModel model)
        {
            var data = _dataProvider.GetAll();
            data.Add(model);
        }

        public void Edit(RiskModel model)
        {
            Remove(model.Id);
            Add(model);
        }

        public RiskModel Get(string id)
        {
            return _dataProvider.Get(id);
        }

        public IList<RiskModel> GetAll()
        {
            return _dataProvider.GetAll();
        }

        public void Remove(string id)
        {
            _dataProvider.Remove(id);
        }
    }
}
