using System.Collections.Generic;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IMessageRepository<T> where T : BizBase
    {
        T GetById(int id);
        List<T> List();
        T Add(T entity);
        void Update(T entity);
        //void Delete(T entity);
    }
}