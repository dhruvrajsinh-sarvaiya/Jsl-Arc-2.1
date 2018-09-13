using System.Collections.Generic;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Core.Interfaces
{
    public interface ISMSService<T> where T : BaseEntity
    {
        T GetBySMSId(int SMSID);
        T Add(T entity);
    }
}