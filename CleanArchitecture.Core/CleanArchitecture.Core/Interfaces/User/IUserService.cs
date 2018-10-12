

using System.Threading.Tasks;
using CleanArchitecture.Core.Entities.User;

namespace CleanArchitecture.Core.Interfaces.User
{
    public interface IUserService
    {
        bool GetMobileNumber(string MobileNumber);
        long GenerateRandomOTP();
        Task<TempUserRegister> FindByMobileNumber(string MobileNumber);
        Task<TempUserRegister> FindByEmail(string Email);
        Task<bool> IsValidPhoneNumber(string Mobilenumber, string CountryCode);
        Task<string> GetCountryByIP(string ipAddress);
    }
}
