using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface ITransaction<T>
    {
         Task SendAPIRequestAsync(string Url, string Request, string MethodType="POST");
         Task<T> TransactionParseResponse(string TransactionResponse);
    }
}
