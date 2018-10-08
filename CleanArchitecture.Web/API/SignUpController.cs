using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.User;
using CleanArchitecture.Core.Services;
using CleanArchitecture.Core.ViewModels;
using CleanArchitecture.Core.ViewModels.AccountViewModels;
using CleanArchitecture.Core.ViewModels.AccountViewModels.Login;
using CleanArchitecture.Core.ViewModels.AccountViewModels.OTP;
using CleanArchitecture.Core.ViewModels.AccountViewModels.SignUp;
using CleanArchitecture.Infrastructure.Services;
using CleanArchitecture.Web.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PhoneNumbers;

namespace CleanArchitecture.Web.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : BaseController
    {
        #region Field
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;
        private readonly IUserService _userdata;
        private readonly IMediator _mediator;
        private readonly EncyptedDecrypted _encdecAEC;
        private readonly ITempUserRegisterService _tempUserRegisterService;
        private readonly IRegisterTypeService _registerTypeService;
        private readonly ITempOtpService _tempOtpService;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        #endregion

        #region Ctore
        public SignUpController(UserManager<ApplicationUser> userManager, ILoggerFactory loggerFactory, IUserService userdata, ITempUserRegisterService tempUserRegisterService, IMediator mediator, EncyptedDecrypted encdecAEC, IRegisterTypeService registerTypeService, ITempOtpService tempOtpService, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<SignUpController>();
            _userdata = userdata;
            _tempUserRegisterService = tempUserRegisterService;
            _mediator = mediator;
            _encdecAEC = encdecAEC;
            _registerTypeService = registerTypeService;
            _tempOtpService = tempOtpService;
            _configuration = configuration;
        }
        #endregion

        #region Utilities
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        //[ApiExplorerSettings(IgnoreApi = true)]
        // Email Link to direct call this method
        [HttpGet("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string emailConfirmCode)
        {
            if (!string.IsNullOrEmpty(emailConfirmCode))
            {
                byte[] DecpasswordBytes = _encdecAEC.GetPasswordBytes(_configuration["AESSalt"].ToString());

                string DecryptToken = EncyptedDecrypted.Decrypt(emailConfirmCode, DecpasswordBytes);

                LinkTokenViewModel dmodel = JsonConvert.DeserializeObject<LinkTokenViewModel>(DecryptToken);
                if (dmodel?.Expirytime >= DateTime.UtcNow)
                {
                    if (dmodel.Id == 0)
                    {
                        ModelState.AddModelError(string.Empty, "Error.");
                        return BadRequest(new ApiError(ModelState));
                    }
                    var user = await _tempUserRegisterService.FindById(dmodel.Id);
                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, "Error");
                        return BadRequest(new ApiError(ModelState));
                    }
                    else if (!user.RegisterStatus)
                    {
                        var currentUser = new ApplicationUser
                        {
                            UserName = user.Email,
                            Email = user.Email,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Mobile = user.Mobile,
                            PasswordHash = EncyptedDecrypted.Decrypt(user.PasswordHash, DecpasswordBytes)
                        };

                        var result = await _userManager.CreateAsync(currentUser, currentUser.PasswordHash);
                        if (result.Succeeded)
                        {
                            if (currentUser.Mobile != null)
                            {
                                var officeClaim = new Claim(OpenIdConnectConstants.Claims.PhoneNumber, currentUser.Mobile.ToString(), ClaimValueTypes.Integer);
                                await _userManager.AddClaimAsync(currentUser, officeClaim);
                            }
                            // Add to roles
                            var roleAddResult = await _userManager.AddToRoleAsync(currentUser, "User");
                            if (roleAddResult.Succeeded)
                            {
                                currentUser.EmailConfirmed = true;
                                var resultupdate = await _userManager.UpdateAsync(currentUser);
                                _tempUserRegisterService.Update(user.Id);

                                //return Ok("Your account has been activated, you can now login.");
                                return AppUtils.StanderdSignUp("Your account has been activated, you can now login.");
                                //return View(resultupdate.Succeeded ? "ConfirmEmail" : "Error");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "This email is already registered.");
                            return BadRequest(new ApiError(ModelState));
                        }
                        // return View(result.Succeeded ? "ConfirmEmail" : "Error");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "This email is already registered.");
                        return BadRequest(new ApiError(ModelState));
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Reset links immediately not valid or expired.");
                    return BadRequest(new ApiError(ModelState));
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please fill link url.");
                return BadRequest(new ApiError(ModelState));
            }
            ModelState.AddModelError(string.Empty, "Reset links immediately not valid or expired.");
            return BadRequest(new ApiError(ModelState));
        }
        #endregion

        #region Methods

        #region Default register

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            var result = await _userManager.FindByEmailAsync(model.Email);
            if (string.IsNullOrEmpty(result?.Email))
            {
                bool IsSignEmail = await _tempUserRegisterService.GetEmail(model.Email);
                if (IsSignEmail)
                {
                    byte[] passwordBytes = _encdecAEC.GetPasswordBytes(_configuration["AESSalt"].ToString());

                    var tempcurrentUser = new TempUserRegisterViewModel
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        FirstName = model.Firstname,
                        LastName = model.Lastname,
                        Mobile = model.Mobile,
                        PasswordHash = EncyptedDecrypted.Encrypt(model.Password, passwordBytes),
                        RegTypeId = await _registerTypeService.GetRegisterId(Core.Enums.enRegisterType.Standerd)
                    };
                    var resultdata = await _tempUserRegisterService.AddTempRegister(tempcurrentUser);
                    if (resultdata != null)
                    {
                        LinkTokenViewModel linkToken = new LinkTokenViewModel();
                        linkToken.Id = resultdata.Id;
                        linkToken.Username = model.Email;
                        linkToken.Email = model.Email;
                        linkToken.Firstname = model.Firstname;
                        linkToken.Lastname = model.Lastname;
                        linkToken.Mobile = model.Mobile;
                        linkToken.CurrentTime = DateTime.UtcNow;
                        linkToken.Expirytime = DateTime.UtcNow + TimeSpan.FromHours(2);
                        //linkToken.Password = tempcurrentUser.PasswordHash;

                        string UserDetails = JsonConvert.SerializeObject(linkToken);
                        string SubScriptionKey = EncyptedDecrypted.Encrypt(UserDetails, passwordBytes);
                        string ctokenlink = Url.Action("ConfirmEmail", "SignUp", new
                        {
                            //userId = currentUser.Id,
                            emailConfirmCode = SubScriptionKey
                        }, protocol: HttpContext.Request.Scheme);

                        var confirmationLink = "<a class='btn-primary' href=\"" + ctokenlink + "\">Confirm email address</a>";

                        SendEmailRequest request = new SendEmailRequest();
                        request.Recepient = model.Email;
                        request.Subject = "Registration confirmation email";
                        request.Body = confirmationLink;

                        await _mediator.Send(request);
                        _logger.LogInformation(3, "User created a new account with password.");

                        //await _emailSender.SendEmailAsync(model.Email, "Registration confirmation email", confirmationLink);
                        
                        //return Ok("Your account has been created, <br /> please verify it by clicking the activation link that has been send to your email.");
                        return AppUtils.StanderdSignUp("Your account has been created, please verify it by clicking the activation link that has been send to your email.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "This username or email is already registered.");
                    return BadRequest(new ApiError(ModelState));
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "This username or email is already registered.");
                return BadRequest(new ApiError(ModelState));
            }
            return BadRequest(new ApiError(ModelState));
        }

        #endregion

        #region SignUpWithEmail

        /// <summary>
        ///  This method are Direct signUp with email using verified link
        /// </summary>        
        [HttpPost("SignUpWithEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUpWithEmail(SignUpWithEmailViewModel model)
        {
            var result = await _userManager.FindByEmailAsync(model.Email);
            if (string.IsNullOrEmpty(result?.Email))
            {
                bool IsTempSignEmail = await _tempUserRegisterService.GetEmail(model.Email);
                if (IsTempSignEmail)
                {
                    var tempcurrentUser = new TempUserRegisterViewModel
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        RegTypeId = await _registerTypeService.GetRegisterId(enRegisterType.Email)
                    };

                    var resultdata = await _tempUserRegisterService.AddTempRegister(tempcurrentUser);
                    if (resultdata != null)
                    {
                        var resultotp = await _tempOtpService.GetTempData(Convert.ToInt32(resultdata.Id));
                        SendEmailRequest request = new SendEmailRequest();
                        request.Recepient = model.Email;
                        request.Subject = "Registration confirmation email";
                        request.Body = "use this code:" + resultotp.OTP + "";

                        await _mediator.Send(request);

                        //await _emailSender.SendEmailAsync(model.Email, "Registration confirmation email", confirmationLink);
                        return AppUtils.StanderdSignUp("Please verify it by clicking the otp that has been send to your email.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "This email is already registered.");
                    return BadRequest(new ApiError(ModelState));
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "This email is already registered.");
                return BadRequest(new ApiError(ModelState));
            }
            // If we got this far, something failed, redisplay form
            return BadRequest(new ApiError(ModelState));
        }

        #endregion

        #region SignUpWithMobile

        /// <summary>
        ///  This method are Direct signUp with mobile sms using verified opt.
        /// </summary>        
        [HttpPost("SignUpWithMobile")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUpWithMobile(SignUpWithMobileViewModel model)
        {

            PhoneNumberUtil phoneUtil = PhoneNumberUtil.GetInstance();
            string countryCode = "IN";
            PhoneNumbers.PhoneNumber phoneNumber = phoneUtil.Parse(model.Mobile, countryCode);

            bool isValidNumber = phoneUtil.IsValidNumber(phoneNumber); // returns true for valid number    

            if (isValidNumber)
            {
                var tempcurrentUser = new TempUserRegisterViewModel
                {
                    UserName = model.Mobile,
                    Mobile = model.Mobile,
                    RegTypeId = await _registerTypeService.GetRegisterId(enRegisterType.Mobile),
                };

                bool IsSignMobile = _userdata.GetMobileNumber(model.Mobile);
                if (IsSignMobile)
                {
                    bool IsSignTempMobile = _tempUserRegisterService.GetMobileNumber(model.Mobile);
                    if (IsSignTempMobile)
                    {
                        var result = await _tempUserRegisterService.AddTempRegister(tempcurrentUser);
                        if (result != null)
                        {
                            var tempotp = await _tempOtpService.GetTempData(Convert.ToInt32(result.Id));

                            SendSMSRequest request = new SendSMSRequest();
                            request.MobileNo = Convert.ToInt64(model.Mobile);
                            request.Message = "SMS for use this code " + tempotp.OTP + "";
                            await _mediator.Send(request);
                            return AppUtils.StanderdSignUp("Success");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "This mobile number is already registered.");
                        return BadRequest(new ApiError(ModelState));
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "This mobile number is already registered.");
                    return BadRequest(new ApiError(ModelState));
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "This mobile number is not valid.");
                return BadRequest(new ApiError(ModelState));
            }
            // If we got this far, something failed, redisplay form
            return BadRequest(new ApiError(ModelState));
        }

        #endregion

        #region BlockChainSignUp

        [HttpPost("BlockChainSignUp")]
        [AllowAnonymous]
        public async Task<IActionResult> BlockChainSignUp(BlockChainViewModel model)
        {
            return AppUtils.StanderdSignUp("Success");
        }

        #endregion

        #region SignUpOtpVerification

        /// <summary>
        ///  This method are Direct signUp with mobile sms using verified opt.
        /// </summary> 

        [HttpPost("MobileOtpVerification")]
        [AllowAnonymous]
        public async Task<IActionResult> MobileOtpVerification(OTPWithMobileViewModel model)
        {
            var tempdata = await _tempUserRegisterService.GetMobileNo(model.MobileNo);
            if (tempdata?.Id > 0)
            {
                var tempotp = await _tempOtpService.GetTempData(Convert.ToInt16(tempdata.Id));
                if (tempotp != null)
                {
                    if (tempotp?.ExpirTime >= DateTime.UtcNow)
                    {
                        if (tempdata.Id == 0 && tempotp.Id == 0)
                        {
                            ModelState.AddModelError(string.Empty, "Error.");
                            return BadRequest(new ApiError(ModelState));
                        }
                        else if (model.OTP == tempotp.OTP)
                        {
                            if (!tempdata.RegisterStatus && !tempotp.EnableStatus)
                            {
                                var currentUser = new ApplicationUser
                                {
                                    UserName = tempdata.Mobile,
                                    Mobile = tempdata.Mobile,
                                };
                                var result = await _userManager.CreateAsync(currentUser);
                                if (result.Succeeded)
                                {
                                    if (currentUser.Mobile != null)
                                    {
                                        var officeClaim = new Claim(OpenIdConnectConstants.Claims.PhoneNumber, currentUser.Mobile.ToString(), ClaimValueTypes.Integer);
                                        await _userManager.AddClaimAsync(currentUser, officeClaim);
                                    }
                                    // Add to roles
                                    var roleAddResult = await _userManager.AddToRoleAsync(currentUser, "User");
                                    if (roleAddResult.Succeeded)
                                    {
                                        _tempUserRegisterService.Update(tempdata.Id);
                                        _tempOtpService.Update(tempotp.Id);
                                        var mobileconfirmed = await _userManager.IsPhoneNumberConfirmedAsync(currentUser);
                                        return AppUtils.StanderdSignUp("You have successfully verified.");
                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError(string.Empty, "This mobile number is already registered.");
                                    return BadRequest(new ApiError(ModelState));
                                }
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "This user is already registered.");
                                return BadRequest(new ApiError(ModelState));
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid OTP or expired, resend OTP immediately.");
                            return BadRequest(new ApiError(ModelState));
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Resend OTP immediately not valid or expired.");
                        return BadRequest(new ApiError(ModelState));
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error.");
                return BadRequest(new ApiError(ModelState));
            }

            ModelState.AddModelError(string.Empty, "Resend OTP immediately not valid or expired.");
            return BadRequest(new ApiError(ModelState));
        }

        /// <summary>
        ///  This method are Direct signUp with email otp using verified.
        /// </summary>

        [HttpPost("EmailOtpVerification")]
        [AllowAnonymous]
        public async Task<IActionResult> EmailOtpVerification(OTPWithEmailViewModel model)
        {
            var tempdata = await _tempUserRegisterService.GetEmailDet(model.Email);
            if (tempdata?.Id > 0)
            {
                var tempotp = await _tempOtpService.GetTempData(Convert.ToInt16(tempdata.Id));
                if (tempotp != null)
                {
                    if (tempotp?.ExpirTime >= DateTime.UtcNow)
                    {
                        if (tempdata.Id == 0 && tempotp.Id == 0)
                        {
                            ModelState.AddModelError(string.Empty, "Error.");
                            return BadRequest(new ApiError(ModelState));
                        }
                        else if (model.OTP == tempotp.OTP)
                        {
                            if (!tempdata.RegisterStatus && !tempotp.EnableStatus)
                            {
                                var currentUser = new ApplicationUser
                                {
                                    UserName = tempdata.Email,
                                    Email = tempdata.Email,
                                };
                                var result = await _userManager.CreateAsync(currentUser);
                                if (result.Succeeded)
                                {
                                    if (currentUser.Mobile != null)
                                    {
                                        var officeClaim = new Claim(OpenIdConnectConstants.Claims.PhoneNumber, currentUser.Mobile.ToString(), ClaimValueTypes.Integer);
                                        await _userManager.AddClaimAsync(currentUser, officeClaim);
                                    }
                                    // Add to roles
                                    var roleAddResult = await _userManager.AddToRoleAsync(currentUser, "User");
                                    if (roleAddResult.Succeeded)
                                    {
                                        _tempUserRegisterService.Update(tempdata.Id);
                                        _tempOtpService.Update(tempotp.Id);
                                        var emailconfirmed = await _userManager.IsEmailConfirmedAsync(currentUser);
                                        //return Ok("Your account has been activated, you can now login.");
                                        return AppUtils.StanderdSignUp("You have successfully verified.");
                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError(string.Empty, "This Email is already registered.");
                                    return BadRequest(new ApiError(ModelState));
                                }
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "This user is already registered.");
                                return BadRequest(new ApiError(ModelState));
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid OTP or expired, resend OTP immediately.");
                            return BadRequest(new ApiError(ModelState));
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Resend OTP immediately not valid or expired.");
                        return BadRequest(new ApiError(ModelState));
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error.");
                return BadRequest(new ApiError(ModelState));
            }
            ModelState.AddModelError(string.Empty, "Resend OTP immediately not valid or expired.");
            return BadRequest(new ApiError(ModelState));
        }

        /// <summary>
        ///  This method are Auto generate resend otp in Mobile
        /// </summary>   

        [HttpPost("ReSendOtpWithMobile")]
        [AllowAnonymous]
        public async Task<IActionResult> ReSendOtpWithMobile(SignUpWithMobileViewModel model)
        {
            PhoneNumberUtil phoneUtil = PhoneNumberUtil.GetInstance();
            string countryCode = "IN";
            PhoneNumbers.PhoneNumber phoneNumber = phoneUtil.Parse(model.Mobile, countryCode);
            bool isValidNumber = phoneUtil.IsValidNumber(phoneNumber); // returns true for valid number    
            if (isValidNumber)
            {
                bool IsSignMobile = _userdata.GetMobileNumber(model.Mobile);
                if (IsSignMobile)
                {
                    var tempdata = await _tempUserRegisterService.GetMobileNo(model.Mobile);
                    var tempotp = await _tempOtpService.GetTempData(Convert.ToInt16(tempdata.Id));
                    if (!tempdata.RegisterStatus && !tempotp.EnableStatus && tempotp.ExpirTime <= DateTime.UtcNow)
                    {
                        _tempOtpService.Update(tempotp.Id);
                        var result = await _tempOtpService.AddTempOtp(Convert.ToInt16(tempdata.Id), Convert.ToInt16(enRegisterType.Mobile));

                        SendSMSRequest request = new SendSMSRequest();
                        request.MobileNo = Convert.ToInt64(model.Mobile);
                        request.Message = "SMS for use this code " + result.OTP + "";
                        await _mediator.Send(request);
                        return AppUtils.StanderdSignUp("You have successfully send Otp in mobile.");
                    }
                    else
                    {
                        SendSMSRequest request = new SendSMSRequest();
                        request.MobileNo = Convert.ToInt64(model.Mobile);
                        request.Message = "SMS for use this code " + tempotp.OTP + "";
                        await _mediator.Send(request);
                        return AppUtils.StanderdSignUp("Please verify it by clicking the otp that has been resend to your mobile.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "This mobile number is already registered.");
                    return BadRequest(new ApiError(ModelState));
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "This mobile number is not valid.");
                return BadRequest(new ApiError(ModelState));
            }
            //return BadRequest(new ApiError(ModelState));
        }

        /// <summary>
        ///  This method are Auto generate resend otp in Email
        /// </summary>  

        [HttpPost("ReSendOtpWithEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ReSendOtpWithEmail(SignUpWithEmailViewModel model)
        {
            var result = await _userManager.FindByEmailAsync(model.Email);
            if (string.IsNullOrEmpty(result?.Email))
            {
                var tempdata = await _tempUserRegisterService.GetEmailDet(model.Email);
                var tempotp = await _tempOtpService.GetTempData(Convert.ToInt16(tempdata.Id));
                if (!tempdata.RegisterStatus && !tempotp.EnableStatus && tempotp.ExpirTime <= DateTime.UtcNow)
                {
                    _tempOtpService.Update(tempotp.Id);
                    var resultdata = await _tempOtpService.AddTempOtp(Convert.ToInt16(tempdata.Id), Convert.ToInt16(enRegisterType.Email));

                    SendEmailRequest request = new SendEmailRequest();
                    request.Recepient = model.Email;
                    request.Subject = "Registration confirmation resend email";
                    request.Body = "use this code:" + resultdata.OTP + "";

                    await _mediator.Send(request);
                    _logger.LogInformation(3, "Email sent successfully with your account");

                    ///return Ok("Success");
                    return AppUtils.StanderdSignUp("You have successfully send Otp in email.");
                }
                else
                {
                    SendEmailRequest request = new SendEmailRequest();
                    request.Recepient = model.Email;
                    request.Subject = "Registration confirmation resend email";
                    request.Body = "use this code:" + tempotp.OTP + "";

                    await _mediator.Send(request);
                    _logger.LogInformation(3, "Email sent successfully with your account");

                    return AppUtils.StanderdSignUp("Please verify it by clicking the otp that has been resend to your email.");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "This username or email is already registered.");
                return BadRequest(new ApiError(ModelState));
            }
        }

        /// <summary>
        ///  This method are resend Email Link to direct call this method
        /// </summary>

        [HttpPost("ReSendRegisterlink")]
        [AllowAnonymous]
        public async Task<IActionResult> ReSendRegisterlink(SignUpWithEmailViewModel model)
        {
            try
            {
                var result = await _userManager.FindByEmailAsync(model.Email);
                if (string.IsNullOrEmpty(result?.Email))
                {
                    var tempdata = await _tempUserRegisterService.GetEmailDet(model.Email);
                    if (!tempdata.RegisterStatus)
                    {
                        byte[] passwordBytes = _encdecAEC.GetPasswordBytes(_configuration["AESSalt"].ToString());

                        LinkTokenViewModel linkToken = new LinkTokenViewModel();
                        linkToken.Id = tempdata.Id;
                        linkToken.Username = model.Email;
                        linkToken.Email = model.Email;
                        linkToken.CurrentTime = DateTime.UtcNow;
                        linkToken.Expirytime = DateTime.UtcNow + TimeSpan.FromHours(2);
                        linkToken.Password = tempdata.PasswordHash;

                        string UserDetails = JsonConvert.SerializeObject(linkToken);
                        string SubScriptionKey = EncyptedDecrypted.Encrypt(UserDetails, passwordBytes);
                        string ctokenlink = Url.Action("ConfirmEmail", "SignUp", new
                        {
                            emailConfirmCode = SubScriptionKey
                        }, protocol: HttpContext.Request.Scheme);

                        var confirmationLink = "<a class='btn-primary' href=\"" + ctokenlink + "\">Confirm email address</a>";

                        SendEmailRequest request = new SendEmailRequest();
                        request.Recepient = model.Email;
                        request.Subject = "Registration confirmation resend email";
                        request.Body = confirmationLink;

                        await _mediator.Send(request);
                        _logger.LogInformation(3, "Email sent successfully with your account");

                        return AppUtils.StanderdSignUp("Please verify it by clicking the activation link that has been resend to your email.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "This email is already registered.");
                        return BadRequest(new ApiError(ModelState));
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "This username or email is already registered.");
                    return BadRequest(new ApiError(ModelState));
                }
            }
            catch (Exception ex)
            {
                return null;
                //return await Task.FromResult(new SignUpWithEmailResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = "Message not sent." });
            }
        }

        #endregion


        #endregion
    }
}