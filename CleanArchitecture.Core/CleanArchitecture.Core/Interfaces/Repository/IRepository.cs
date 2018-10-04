using System.Collections.Generic;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Core.Interfaces.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        T GetById(int id);
        List<T> List();
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        T AddProduct(T entity); // for testing
    }
}