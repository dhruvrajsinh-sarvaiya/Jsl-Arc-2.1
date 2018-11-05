using CleanArchitecture.Core.Entities.KYC;
using CleanArchitecture.Core.Interfaces.KYC;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.ViewModels.KYC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services.KYC
{
    public class PersonalVerificationService : IPersonalVerificationService
    {
        private readonly ICustomRepository<PersonalVerification> _personalVerificationRepository;

        public PersonalVerificationService(ICustomRepository<PersonalVerification> personalVerificationRepository)
        {
            _personalVerificationRepository = personalVerificationRepository;
        }

        public long AddPersonalVerification(PersonalVerificationViewModel model)
        {
            try
            {
                var GetVerify = _personalVerificationRepository.Table.Where(i => i.UserID == model.UserId && !i.EnableStatus).FirstOrDefault(); ;
                if (GetVerify != null)
                {
                    var personalVerificationdataupdate = new PersonalVerification
                    {
                        Id= GetVerify.Id,
                        UserID = GetVerify.UserID,
                        Surname = model.Surname,
                        GivenName = model.GivenName,
                        ValidIdentityCard = model.ValidIdentityCard,
                        FrontImage = model.FrontImage,
                        BackImage = model.BackImage,
                        SelfieImage = model.SelfieImage,
                        EnableStatus = model.EnableStatus,
                        VerifyStatus = model.VerifyStatus,
                        //CreatedDate = DateTime.UtcNow,
                        // CreatedBy = model.UserId,
                        UpdatedDate = DateTime.UtcNow,
                        UpdatedBy = model.UserId,
                        Status = 0,

                    };
                    //return GetVerify;
                    _personalVerificationRepository.Update(personalVerificationdataupdate);
                    return personalVerificationdataupdate.Id;
                }
                else
                {
                    var personalVerificationdata = new PersonalVerification
                    {
                        UserID = model.UserId,
                        Surname = model.Surname,
                        GivenName = model.GivenName,
                        ValidIdentityCard = model.ValidIdentityCard,
                        FrontImage = model.FrontImage,
                        BackImage = model.BackImage,
                        SelfieImage = model.SelfieImage,
                        EnableStatus = model.EnableStatus,
                        VerifyStatus = model.VerifyStatus,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = model.UserId,
                        //Status = 0,

                    };
                    _personalVerificationRepository.Insert(personalVerificationdata);
                    //_dbContext.SaveChanges();
                    return personalVerificationdata.Id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

    }
}
