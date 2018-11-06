using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Enums;
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
        //private readonly ICustomRepository<OtpMaster> _customRepository;
        private readonly IMessageRepository<OtpMaster> _customRepository;
        private readonly IRegisterTypeService _registerTypeService;
        private readonly IMediator _mediator;
        private readonly IMessageConfiguration _messageConfiguration;
        public OtpMasterService(
            CleanArchitectureContext dbContext, IUserService userService,
            //ICustomRepository<OtpMaster> customRepository,
            IMessageRepository<OtpMaster> customRepository,
        IRegisterTypeService registerTypeService, IMediator mediator, IMessageConfiguration messageConfiguration)
        {
            _dbContext = dbContext;
            _userService = userService;
            _customRepository = customRepository;
            //_customRepository = customRepository;
            _registerTypeService = registerTypeService;
            _mediator = mediator;
            _messageConfiguration = messageConfiguration;
        }

        public async Task<OtpMasterViewModel> AddOtp(int UserId, string Email = null, string Mobile = null)
        {
            var checkotp = await GetOtpData(UserId);
            string OtpValue = string.Empty;
            if (checkotp != null)
                UpdateOtp(checkotp.Id);
            OtpValue = _userService.GenerateRandomOTPWithPassword().ToString();
            string alpha = string.Empty; string numeric = string.Empty;
            foreach (char str in OtpValue)
            {
                if (char.IsDigit(str))
                {
                    if (numeric.Length < 6)
                        numeric += str.ToString();
                    else
                        alpha += str.ToString();
                }
                else
                    alpha += str.ToString();
            }

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
                OTP = numeric,
                CreatedTime = DateTime.UtcNow,
                ExpirTime = DateTime.UtcNow.AddHours(2),
                Status = 0,
                CreatedDate = DateTime.Now,
                CreatedBy = UserId
            };
            _customRepository.Add(currentotp);
            
            if (!String.IsNullOrEmpty(Email))
            {
                SendEmailRequest request = new SendEmailRequest();
                request.Recepient = Email;
               // request.Subject = EnResponseMessage.LoginEmailSubject;
               // request.Body = EnResponseMessage.SendMailBody + numeric;
               


                IQueryable Result = await _messageConfiguration.GetTemplateConfigurationAsync(Convert.ToInt16(enCommunicationServiceType.Email), Convert.ToInt16(EnTemplateType.LoginWithOTP), 0);
                foreach (TemplateMasterData Provider in Result)
                {
                    Provider.Content = Provider.Content.Replace("###USERNAME###", string.Empty);
                    Provider.Content = Provider.Content.Replace("###Password###", numeric);
                    //string[] splitedarray = Provider.AdditionaInfo.Split(",");
                    //foreach (string s in splitedarray)
                    //{

                    //}
                    request.Body = Provider.Content;
                    request.Subject = Provider.AdditionalInfo;
                }

                await _mediator.Send(request);


            }
            if (!String.IsNullOrEmpty(Mobile))
            {
                SendSMSRequest request = new SendSMSRequest();
                request.MobileNo = Convert.ToInt64(Mobile);
                request.Message = EnResponseMessage.SendMailBody + numeric;
                await _mediator.Send(request);
            }

            string _Pass1 = alpha.Substring(0, 20);
            string _Pass11 = _Pass1 + numeric.Substring(0, 3);
            string _Pass2 = alpha.Substring(20, 10);
            string _Pass22 = _Pass2 + numeric.Substring(3, 3);
            string _Pass3 = alpha.Substring(30, 28);
            string password = _Pass11 + _Pass22 + _Pass3;

            OtpMasterViewModel model = new OtpMasterViewModel();
            if (currentotp != null)
            {
                model.UserId = currentotp.UserId;
                model.RegTypeId = currentotp.RegTypeId;
                model.OTP = currentotp.OTP;
                model.CreatedTime = currentotp.CreatedTime;
                model.ExpirTime = currentotp.ExpirTime;
                model.Status = currentotp.Status;
                model.Id = currentotp.Id;
                model.Password = password;
                model.appkey = alpha;
                return model;
            }
            else
                return null;
        }



        public async Task<OtpMasterViewModel> GetOtpData(int Id)
        {
            var otpmaster = _dbContext.OtpMaster.Where(i => i.UserId == Id).LastOrDefault();
            if (otpmaster != null)
            {
                OtpMasterViewModel model = new OtpMasterViewModel();

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

        public void UpdateOtp(long Id)
        {
            var tempdata = _customRepository.GetById(Convert.ToInt16(Id));
            tempdata.SetAsOTPStatus();
            tempdata.SetAsUpdateDate(tempdata.UserId);
            //tempdata.Status = true;
            _customRepository.Update(tempdata);
        }
    }
}
