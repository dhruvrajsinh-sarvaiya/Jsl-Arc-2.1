using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.ViewModels.AccountViewModels.SignUp;

namespace CleanArchitecture.Core.Interfaces.User
{
    public partial interface IOtpMasterService
    {
        Task<OtpMaster> AddOtp(int UserId,string Email=null,string Mobile =null);
        Task<OtpMasterViewModel> GetOtpData(int Id);
        void UpdateOtp(long Id);
    }
}
