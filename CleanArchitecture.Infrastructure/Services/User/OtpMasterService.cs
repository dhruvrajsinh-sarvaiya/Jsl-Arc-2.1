using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.Interfaces.User;
using CleanArchitecture.Core.ViewModels;
using CleanArchitecture.Core.ViewModels.AccountViewModels.SignUp;
using MediatR;

namespace CleanArchitecture.Infrastructure.Services.User
{
    public partial class OtpMasterService : IOtpMasterService
    {
        private readonly CleanArchitectureContext _dbContext;
        private readonly IUserService _userService;
       // private readonly ICustomRepository<OtpMaster> _customRepository;
        private readonly IMessageRepository<OtpMaster> _customRepository;
        private readonly IRegisterTypeService _registerTypeService;
        private readonly IMediator _mediator;
        public OtpMasterService(
            CleanArchitectureContext dbContext, IUserService userService,
            //ICustomRepository<OtpMaster> customRepository,
            IMessageRepository<OtpMaster> customRepository,
        IRegisterTypeService registerTypeService, IMediator mediator)
        {
            _dbContext = dbContext;
            _userService = userService;
            //_customRepository = customRepository;
            _customRepository = customRepository;
            _registerTypeService = registerTypeService;
            _mediator = mediator;
        }

        public async Task<OtpMasterViewModel> AddOtp(int UserId, string Email = null,string Mobile=null)
        {
            string OtpValue = _userService.GenerateRandomOTP().ToString();
            int Regtypeid = 0;
            if (!String.IsNullOrEmpty(Email))
            {
                Regtypeid = await _registerTypeService.GetRegisterId(Core.Enums.enRegisterType.Email);
            }
            else if (!String.IsNullOrEmpty(Mobile))
            {
                Regtypeid = await _registerTypeService.GetRegisterId(Core.Enums.enRegisterType.Mobile);
            }

            var currentotp = new OtpMaster
            {
                UserId = UserId,
                RegTypeId = Regtypeid,
                OTP = OtpValue,
                CreatedTime = DateTime.UtcNow,
                ExpirTime = DateTime.UtcNow.AddHours(2),
                Status = 0,
                CreatedDate = DateTime.Now,
                CreatedBy = UserId

            };
            _customRepository.Add(currentotp);
            //_customRepository.Insert(currentotp);

            if (!String.IsNullOrEmpty(Email))
            {
                //var OtpLink = "<a class='btn-primary' href=\"" + OtpValue + "\"> Login with Email Otp = " + OtpValue + " </a>";
                //_logger.LogInformation(3, "User created a new account with password.");

                SendEmailRequest request = new SendEmailRequest();
                request.Recepient = Email;
                request.Subject = "Login With Email Otp";
                request.Body = "use this code: " + OtpValue +"";

                CommunicationResponse CommResponse = await _mediator.Send(request);
            }
            if(!String.IsNullOrEmpty(Mobile))
            {
                SendSMSRequest request = new SendSMSRequest();
                request.MobileNo = Convert.ToInt64(Mobile);
                request.Message = OtpValue;
                CommunicationResponse CommResponse = await _mediator.Send(request);
            }

            OtpMasterViewModel model = new OtpMasterViewModel();
            if(currentotp !=null)
            {
                model.UserId = model.UserId;
                model.RegTypeId = model.RegTypeId;
                model.OTP = model.OTP;
                model.CreatedTime = model.CreatedTime;
                model.ExpirTime = model.ExpirTime;
                model.Status = model.Status;
                model.Id = model.Id;
                return model;
            }
            else
                return null;
        }



        public async Task<OtpMasterViewModel> GetOtpData(int Id)
        {
            var otpmaster = _dbContext.OtpMaster.Where(i => i.UserId == Id).FirstOrDefault();
            if (otpmaster != null)
            {
                OtpMasterViewModel model = new OtpMasterViewModel();

                model.UserId = otpmaster.UserId;
                model.RegTypeId = otpmaster.RegTypeId;
                model.OTP = otpmaster.OTP;
                model.CreatedTime = otpmaster.CreatedTime;
                model.ExpirTime = otpmaster.ExpirTime;
                model.Status = otpmaster.Status;
                model.Id = otpmaster.Id;
                return model;
            }
            else
                return null;
        }

        public void UpdateOtp(long Id)
        {
            var tempdata = _customRepository.GetById(Convert.ToInt16(Id));
            tempdata.SetAsOTPStatus();
            //tempdata.Status = true;
            _customRepository.Update(tempdata);
        }
    }
}
