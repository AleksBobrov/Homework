using System.Collections.Generic;

namespace InsuranceComp
{
    public interface IDataProvider<T> where T : class
    {
        IList<T> GetAll();

        T Get(string id);

        void Remove(string id);

        void Add(T model);
    }
}
