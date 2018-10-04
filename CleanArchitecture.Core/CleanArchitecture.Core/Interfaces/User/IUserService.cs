

using System.Threading.Tasks;
using CleanArchitecture.Core.Entities.User;

namespace CleanArchitecture.Core.Interfaces.User
{
    public interface IUserService
    {
        bool GetMobileNumber(string MobileNumber);
        long GenerateRandomOTP();
        Task<ApplicationUser> FindByMobileNumber(string MobileNumber);
        Task<ApplicationUser> FindByEmail(string Email);
    }
}
