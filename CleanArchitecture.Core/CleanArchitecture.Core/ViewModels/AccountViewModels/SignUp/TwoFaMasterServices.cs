using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.Interfaces.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels.SignUp
{
    public partial class TwoFaMasterServices : ITwoFaMaster
    {
        //private readonly CleanArchitectureContext _dbContext;
        private readonly IUserService _userService;
        private readonly ICustomRepository<TwoFAmaster> _customRepository;
        //private readonly IMessageRepository<TwoFAmaster> _customRepository;
        private readonly IRegisterTypeService _registerTypeService;
        private readonly IMediator _mediator;


        public TwoFaMasterServices(IUserService userService,
            ICustomRepository<TwoFAmaster> customRepository,

        IRegisterTypeService registerTypeService, IMediator mediator)
        {
            _userService = userService;
            _customRepository = customRepository;
            _registerTypeService = registerTypeService;
            _mediator = mediator;
        }
        public async Task<TwoFaMasterViewModel> AddOtpAsync(int UserId, string Email = null, string Mobile = null)
        {
            var checkotp = await GetOtpData(UserId);
            string OtpValue = string.Empty;

            if (checkotp != null)
                UpdateOtp(checkotp.Id);
            OtpValue = _userService.GenerateRandomOTP().ToString();
            int Regtypeid = 0;
            if (!String.IsNullOrEmpty(Email))
            {
                Regtypeid = await _registerTypeService.GetRegisterId(Core.Enums.enRegisterType.Email);
            }
            else if (!String.IsNullOrEmpty(Mobile))
            {
                Regtypeid = await _registerTypeService.GetRegisterId(Core.Enums.enRegisterType.Mobile);
            }

            var currentotp = new TwoFAmaster
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
            _customRepository.Insert(currentotp);
            if (!String.IsNullOrEmpty(Email))
            {
                //var OtpLink = "<a class='btn-primary' href=\"" + OtpValue + "\"> Login with Email Otp = " + OtpValue + " </a>";
                //_logger.LogInformation(3, "User created a new account with password.");

                SendEmailRequest request = new SendEmailRequest();
                request.Recepient = Email;
                request.Subject = EnResponseMessage.LoginEmailSubject;
                request.Body = EnResponseMessage.SendMailBody + OtpValue;
                await _mediator.Send(request);
            }
            if (!String.IsNullOrEmpty(Mobile))
            {
                SendSMSRequest request = new SendSMSRequest();
                request.MobileNo = Convert.ToInt64(Mobile);
                request.Message = EnResponseMessage.SendMailBody + OtpValue;
                await _mediator.Send(request);
            }
            TwoFaMasterViewModel model = new TwoFaMasterViewModel();
            if (currentotp != null)
            {
                model.UserId = currentotp.UserId;
                model.RegTypeId = currentotp.RegTypeId;
                model.OTP = currentotp.OTP;
                model.CreatedTime = currentotp.CreatedTime;
                model.ExpirTime = currentotp.ExpirTime;
                model.Status = currentotp.Status;
                model.Id = currentotp.Id;
                return model;
            }
            else
                return null;
        }



        public void UpdateOtp(long Id)
        {
            var tempdata = _customRepository.GetById(Convert.ToInt16(Id));
            tempdata.SetAsOTPStatus();
            tempdata.SetAsUpdateDate(tempdata.UserId);
            //tempdata.Status = true;
            _customRepository.Update(tempdata);
        }



        public async Task<TwoFaMasterViewModel> GetOtpData(int Id)
        {
            var otpmaster = _customRepository.Table.Where(i => i.UserId == Id).LastOrDefault();
            if (otpmaster != null)
            {
                TwoFaMasterViewModel model = new TwoFaMasterViewModel();

                model.UserId = otpmaster.UserId;
                model.RegTypeId = otpmaster.RegTypeId;
                model.OTP = otpmaster.OTP;
                model.CreatedTime = otpmaster.CreatedTime;
                model.ExpirTime = otpmaster.ExpirTime;
                model.EnableStatus = otpmaster.EnableStatus;
                model.Id = otpmaster.Id;
                return model;
            }
            else
                return null;
        }
    }
}
