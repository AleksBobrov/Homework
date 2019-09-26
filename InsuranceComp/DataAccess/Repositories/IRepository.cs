using System.Collections.Generic;

namespace InsuranceComp
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IList<TEntity> GetAll();
        TEntity Get(string id);
        void Add(TEntity model);
        void Edit(TEntity model);
        void Remove(string id);
    }
}
