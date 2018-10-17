using System;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels.SignUp
{
   public class OtpMasterViewModel : BizBase
    {
        public int UserId { get; set; }
        public int RegTypeId { get; set; }
        public string OTP { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ExpirTime { get; set; }
        public bool EnableStatus { get; set; }
        public string Password { get; set; }
        public string appkey { get; set; }
    }
}
