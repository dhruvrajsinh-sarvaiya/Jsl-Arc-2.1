using System.Threading.Tasks;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.ViewModels.AccountViewModels.SignUp;

namespace CleanArchitecture.Core.Interfaces.User
{
    public interface ITempUserRegisterService 
    {
        bool GetMobileNumber(string MobileNumber);
        Task<TempUserRegisterViewModel> AddTempRegister(TempUserRegisterViewModel model);
        Task<TempUserRegisterViewModel> FindById(long Id);
        void Update(long Id);
        Task<bool> GetEmail(string Email);
        Task<TempUserRegisterViewModel> GetMobileNo(string MobileNo);
        Task<TempUserRegisterViewModel> GetEmailDet(string Email);
    }
}
