using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IApplicationDataService
    {
        Task<object> GetApplicationData(HttpContext context);
    }
}