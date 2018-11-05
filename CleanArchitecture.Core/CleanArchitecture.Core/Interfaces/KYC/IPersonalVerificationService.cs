using CleanArchitecture.Core.ViewModels.KYC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces.KYC
{
   public interface IPersonalVerificationService
    {
        Task<long> AddPersonalVerification(PersonalVerificationViewModel model);
        Task<long> UpdatePersonalVerification(PersonalVerificationViewModel model);
    }
}
