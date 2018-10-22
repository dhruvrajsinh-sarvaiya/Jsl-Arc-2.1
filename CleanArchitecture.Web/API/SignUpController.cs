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
using CleanArchitecture.Infrastructure.Interfaces;
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
        private readonly IBasePage _basePage;
        #endregion

        #region Ctore
        public SignUpController(UserManager<ApplicationUser> userManager, ILoggerFactory loggerFactory, IUserService userdata, ITempUserRegisterService tempUserRegisterService, IMediator mediator, EncyptedDecrypted encdecAEC, IRegisterTypeService registerTypeService, ITempOtpService tempOtpService, Microsoft.Extensions.Configuration.IConfiguration configuration, IBasePage basePage)
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
            _basePage = basePage;
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


        #endregion

        #region Methods

        #region Default register

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            try
            {
                string IpCountryCode = await _userdata.GetCountryByIP(model.IPAddress);
                if (!string.IsNullOrEmpty(IpCountryCode) && IpCountryCode == "fail")
                {
                    return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.IpAddressInvalid, ErrorCode = enErrorCode.Status4020IpInvalid });

                }

                string CountryCode = await _userdata.GetCountryByIP(model.IPAddress);
                if (!string.IsNullOrEmpty(CountryCode) && CountryCode == "fail")
                {

                    return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.IpAddressInvalid, ErrorCode = enErrorCode.Status4020IpInvalid });
                }

                bool isValidNumber = await _userdata.IsValidPhoneNumber(model.Mobile, CountryCode);

                if (!isValidNumber)
                {
                    return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.StandardSignUpPhonevelid, ErrorCode = enErrorCode.Status4013MobileInvalid });
                }

                var result = await _userManager.FindByEmailAsync(model.Email);
                if (string.IsNullOrEmpty(result?.Email))
                {
                    bool IsSignEmail = await _tempUserRegisterService.GetEmail(model.Email);
                    if (IsSignEmail)
                    {
                        byte[] passwordBytes = _encdecAEC.GetPasswordBytes(_configuration["AESSalt"].ToString());

                        var tempcurrentUser = new TempUserRegisterViewModel
                        {
                            UserName = model.Username,
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
                            linkToken.Username = model.Username;
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
                            request.Subject = EnResponseMessage.SendMailSubject;
                            request.Body = confirmationLink;

                            await _mediator.Send(request);
                            _logger.LogInformation(3, "User created a new account with password.");
                            //await _emailSender.SendEmailAsync(model.Email, "Registration confirmation email", confirmationLink);

                            return Ok(new RegisterResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.StandardSignUp });
                        }
                        else
                        {
                            return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status4033NotFoundRecored });
                        }
                    }
                    else
                    {
                        return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.Verificationpending, ErrorCode = enErrorCode.Status4036VerifyPending });
                    }
                }
                else
                {
                    return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpValidation, ErrorCode = enErrorCode.Status4062UseralreadRegister });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        //[ApiExplorerSettings(IgnoreApi = true)]
        // Email Link to direct call this method
        [HttpGet("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string emailConfirmCode)
        {
            try
            {
                string IpCountryCode = await _userdata.GetCountryByIP(model.IPAddress);
                if (!string.IsNullOrEmpty(IpCountryCode) && IpCountryCode == "fail")
                {
                    return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.IpAddressInvalid, ErrorCode = enErrorCode.Status4020IpInvalid });

                }

                if (!string.IsNullOrEmpty(emailConfirmCode))
                {
                    byte[] DecpasswordBytes = _encdecAEC.GetPasswordBytes(_configuration["AESSalt"].ToString());

                    string DecryptToken = EncyptedDecrypted.Decrypt(emailConfirmCode, DecpasswordBytes);

                    LinkTokenViewModel dmodel = JsonConvert.DeserializeObject<LinkTokenViewModel>(DecryptToken);
                    if (dmodel?.Expirytime >= DateTime.UtcNow)
                    {
                        if (dmodel.Id == 0)
                        {
                            return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignEmailUser, ErrorCode = enErrorCode.Status4033NotFoundRecored });
                        }
                        else
                        {
                            var user = await _tempUserRegisterService.FindById(dmodel.Id);
                            if (user == null)
                            {
                                return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignEmailUser, ErrorCode = enErrorCode.Status4033NotFoundRecored });
                            }
                            else if (!user.RegisterStatus)
                            {
                                var currentUser = new ApplicationUser
                                {
                                    UserName = user.UserName,
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
                                        //return AppUtils.StanderdSignUp("Your account has been activated, you can now login.");
                                        return Ok(new RegisterResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SignUpEmailConfirm });
                                    }
                                }
                                else
                                {
                                    return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUserNotRegister, ErrorCode = enErrorCode.Status4063UserNotRegister });
                                }
                                // return View(result.Succeeded ? "ConfirmEmail" : "Error");
                            }
                            else
                            {
                                return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpEmailValidation, ErrorCode = enErrorCode.Status4062UseralreadRegister });
                            }
                        }
                    }
                    else
                    {
                        //ModelState.AddModelError(string.Empty, "Reset links immediately not valid or expired.");
                        return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpEmailExpired, ErrorCode = enErrorCode.Status4039ResetPasswordLinkExpired });
                    }
                }
                //else
                //{
                    return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignEmailLink, ErrorCode = enErrorCode.Status4064EmailLinkBlanck });
                //}
                //ModelState.AddModelError(string.Empty, "Reset links immediately not valid or expired.");
                //return BadRequest(new ApiError(ModelState));
               // return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignEmailLink, ErrorCode = enErrorCode.Status400BadRequest });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
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
                string IpCountryCode = await _userdata.GetCountryByIP(model.IPAddress);
                if (!string.IsNullOrEmpty(IpCountryCode) && IpCountryCode == "fail")
                {
                    return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.IpAddressInvalid, ErrorCode = enErrorCode.Status4020IpInvalid });

                }

                var result = await _userManager.FindByEmailAsync(model.Email);
                if (string.IsNullOrEmpty(result?.Email))
                {
                    var tempdata = await _tempUserRegisterService.GetEmailDet(model.Email);
                    if (tempdata != null)
                    {

                        if (!tempdata.RegisterStatus)
                        {
                            byte[] passwordBytes = _encdecAEC.GetPasswordBytes(_configuration["AESSalt"].ToString());

                            LinkTokenViewModel linkToken = new LinkTokenViewModel();
                            linkToken.Id = tempdata.Id;
                            linkToken.Username = tempdata.UserName;
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
                            request.Subject = EnResponseMessage.ReSendMailSubject;
                            request.Body = confirmationLink;

                            await _mediator.Send(request);
                            _logger.LogInformation(3, "Email sent successfully with your account");

                            //return AppUtils.StanderdSignUp("Please verify it by clicking the activation link that has been resend to your email.");
                            return Ok(new RegisterResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.StandardResendSignUp });
                        }
                        else
                        {
                            return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpValidation, ErrorCode = enErrorCode.Status4062UseralreadRegister });
                        }
                    }
                    else
                    {
                        return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status4033NotFoundRecored });
                    }
                }
                else
                {
                    return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status4033NotFoundRecored });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
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
            try
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
                            request.Subject = EnResponseMessage.SendMailSubject;
                            //request.Body = "use this code:" + resultotp.OTP + "";
                            request.Body = EnResponseMessage.SendMailBody + resultotp.OTP;

                            await _mediator.Send(request);

                            //await _emailSender.SendEmailAsync(model.Email, "Registration confirmation email", confirmationLink);
                            //return AppUtils.StanderdSignUp("Please verify it by clicking the otp that has been send to your email.");
                            return Ok(new SignUpWithEmailResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SignWithEmail });
                        }
                        else
                        {
                            return BadRequest(new SignUpWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status400BadRequest });
                        }
                    }
                    else
                    {
                        return Ok(new SignUpWithEmailResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.Verificationpending, ErrorCode = enErrorCode.Status4036VerifyPending });
                    }
                }
                else
                {
                    return BadRequest(new SignUpWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpEmailValidation, ErrorCode = enErrorCode.Status400BadRequest });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new SignUpWithEmailResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }


        /// <summary>
        ///  This method are Direct signUp with email otp using verified.
        /// </summary>

        [HttpPost("EmailOtpVerification")]
        [AllowAnonymous]
        public async Task<IActionResult> EmailOtpVerification(OTPWithEmailViewModel model)
        {
            try
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
                                //ModelState.AddModelError(string.Empty, "Error.");
                                return BadRequest(new SignUpWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status400BadRequest });
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
                                            //return AppUtils.StanderdSignUp("You have successfully verified.");
                                            return Ok(new SignUpWithEmailResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SignUPVerification });
                                        }
                                        else
                                        {
                                            return BadRequest(new SignUpWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpRole, ErrorCode = enErrorCode.Status400BadRequest });
                                        }
                                    }
                                    else
                                    {
                                        //ModelState.AddModelError(string.Empty, "This Email is already registered.");
                                        return BadRequest(new SignUpWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpEmailValidation, ErrorCode = enErrorCode.Status400BadRequest });
                                    }
                                }
                                else
                                {
                                    return BadRequest(new SignUpWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpEmailValidation, ErrorCode = enErrorCode.Status400BadRequest });
                                    //return BadRequest(new ApiError(ModelState));
                                }
                            }
                            else
                            {
                                return BadRequest(new SignUpWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpOTP, ErrorCode = enErrorCode.Status400BadRequest });
                            }
                        }
                        else
                        {
                            //ModelState.AddModelError(string.Empty, "Resend OTP immediately not valid or expired.");
                            return BadRequest(new SignUpWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpResendOTP, ErrorCode = enErrorCode.Status400BadRequest });
                        }
                    }
                    else
                    {
                        return BadRequest(new SignUpWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status400BadRequest });
                    }
                }
                else
                {
                    return BadRequest(new SignUpWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status400BadRequest });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new SignUpWithEmailResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }


        /// <summary>
        ///  This method are Auto generate resend otp in Email
        /// </summary>  

        [HttpPost("ReSendOtpWithEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ReSendOtpWithEmail(SignUpWithEmailViewModel model)
        {
            try
            {
                var result = await _userManager.FindByEmailAsync(model.Email);
                if (string.IsNullOrEmpty(result?.Email))
                {
                    var tempdata = await _tempUserRegisterService.GetEmailDet(model.Email);
                    var tempotp = await _tempOtpService.GetTempData(Convert.ToInt16(tempdata?.Id));
                    //if (!tempdata.RegisterStatus && !tempotp.EnableStatus && tempotp.ExpirTime <= DateTime.UtcNow)  // Remove expiretime as per discuss with nishit bhai 10-09-2018
                    if (tempotp != null && tempdata != null)
                    {
                        if (!tempdata.RegisterStatus && !tempotp.EnableStatus)
                        {
                            _tempOtpService.Update(tempotp.Id);
                            var resultdata = await _tempOtpService.AddTempOtp(Convert.ToInt16(tempdata.Id), Convert.ToInt16(enRegisterType.Email));

                            SendEmailRequest request = new SendEmailRequest();
                            request.Recepient = model.Email;
                            request.Subject = EnResponseMessage.ReSendMailSubject;
                            request.Body = EnResponseMessage.SendMailBody + resultdata.OTP;

                            await _mediator.Send(request);
                            _logger.LogInformation(3, "Email sent successfully with your account");
                            //return AppUtils.StanderdSignUp("You have successfully send Otp in email.");
                            return Ok(new SignUpWithEmailResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SignUpWithResendEmail });
                        }
                        else
                        {
                            //ModelState.AddModelError(string.Empty, "This username or email is already registered.");
                            //return BadRequest(new ApiError(ModelState));
                            return BadRequest(new SignUpWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpEmailValidation, ErrorCode = enErrorCode.Status400BadRequest });

                            //SendEmailRequest request = new SendEmailRequest();
                            //request.Recepient = model.Email;
                            //request.Subject = "Registration confirmation resend email";
                            //request.Body = "use this code:" + tempotp.OTP + "";

                            //await _mediator.Send(request);
                            //_logger.LogInformation(3, "Email sent successfully with your account");

                            //return AppUtils.StanderdSignUp("Please verify it by clicking the otp that has been resend to your email.");
                        }
                    }
                    else
                    {
                        return BadRequest(new SignUpWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status400BadRequest });
                    }
                }
                else
                {
                    //ModelState.AddModelError(string.Empty, "This username or email is already registered.");
                    return BadRequest(new SignUpWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpEmailValidation, ErrorCode = enErrorCode.Status400BadRequest });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new SignUpWithEmailResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
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
            try
            {
                string CountryCode = await _userdata.GetCountryByIP(model.IPAddress);
                if (!string.IsNullOrEmpty(CountryCode) && CountryCode == "fail")
                {
                    return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.IpAddressInvalid, ErrorCode = enErrorCode.Status4020IpInvalid });
                }


                bool isValidNumber = await _userdata.IsValidPhoneNumber(model.Mobile, CountryCode);

                if (!isValidNumber)
                {
                    return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.StandardSignUpPhonevelid, ErrorCode = enErrorCode.Status4013MobileInvalid });
                }

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
                            request.Message = EnResponseMessage.SendMailBody + tempotp.OTP;
                            await _mediator.Send(request);
                            return Ok(new SignUpWithMobileResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SignUpWithMobile });
                        }
                        else
                        {
                            return BadRequest(new SignUpWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status400BadRequest });
                        }
                    }
                    else
                    {
                        return Ok(new SignUpWithEmailResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.Verificationpending, ErrorCode = enErrorCode.Status4036VerifyPending });                        
                    }
                }
                else
                {
                    return BadRequest(new SignUpWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUPMobileValidation, ErrorCode = enErrorCode.Status400BadRequest });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new SignUpWithMobileResponse
                {
                    ReturnCode = enResponseCode.InternalError,
                    ReturnMsg = ex.ToString(),
                    ErrorCode = enErrorCode.Status500InternalServerError
                });
            }
        }

        /// <summary>
        ///  This method are Direct signUp with mobile sms using verified opt.
        /// </summary> 

        [HttpPost("MobileOtpVerification")]
        [AllowAnonymous]
        public async Task<IActionResult> MobileOtpVerification(OTPWithMobileViewModel model)
        {
            try
            {
                var tempdata = await _tempUserRegisterService.GetMobileNo(model.Mobile);
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
                                return BadRequest(new SignUpWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status400BadRequest });
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
                                            //return AppUtils.StanderdSignUp("You have successfully verified.");
                                            return Ok(new SignUpWithMobileResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SignUPVerification });
                                        }
                                        else
                                        {
                                            return BadRequest(new SignUpWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpRole, ErrorCode = enErrorCode.Status400BadRequest });
                                        }
                                    }
                                    else
                                    {
                                        return BadRequest(new SignUpWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUPMobileValidation, ErrorCode = enErrorCode.Status400BadRequest });
                                    }
                                }
                                else
                                {
                                    return BadRequest(new SignUpWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUPMobileValidation, ErrorCode = enErrorCode.Status400BadRequest });
                                }
                            }
                            else
                            {
                                return BadRequest(new SignUpWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpOTP, ErrorCode = enErrorCode.Status400BadRequest });
                            }
                        }
                        else
                        {
                            return BadRequest(new SignUpWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpResendOTP, ErrorCode = enErrorCode.Status400BadRequest });
                        }
                    }
                    else
                    {
                        return BadRequest(new SignUpWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status400BadRequest });
                    }
                }
                else
                {
                    return BadRequest(new SignUpWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status400BadRequest });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new SignUpWithMobileResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }


        /// <summary>
        ///  This method are Auto generate resend otp in Mobile
        /// </summary>   

        [HttpPost("ReSendOtpWithMobile")]
        [AllowAnonymous]
        public async Task<IActionResult> ReSendOtpWithMobile(SignUpWithMobileViewModel model)
        {
            try
            {

                string CountryCode = await _userdata.GetCountryByIP(model.IPAddress);
                if (!string.IsNullOrEmpty(CountryCode) && CountryCode == "fail")
                {
                    return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.IpAddressInvalid, ErrorCode = enErrorCode.Status4020IpInvalid });
                }


                bool isValidNumber = await _userdata.IsValidPhoneNumber(model.Mobile, CountryCode);

                if (!isValidNumber)
                {
                    return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.StandardSignUpPhonevelid, ErrorCode = enErrorCode.Status4013MobileInvalid });
                }

                bool IsSignMobile = _userdata.GetMobileNumber(model.Mobile);
                if (IsSignMobile)
                {
                    var tempdata = await _tempUserRegisterService.GetMobileNo(model.Mobile);
                    var tempotp = await _tempOtpService.GetTempData(Convert.ToInt16(tempdata?.Id));
                    //if (!tempdata.RegisterStatus && !tempotp.EnableStatus && tempotp.ExpirTime <= DateTime.UtcNow) // Remove expiretime as per discuss with nishit bhai 10-09-2018
                    if (tempdata != null && tempotp != null)
                    {
                        if (!tempdata.RegisterStatus && !tempotp.EnableStatus)
                        {
                            _tempOtpService.Update(tempotp.Id);
                            var result = await _tempOtpService.AddTempOtp(Convert.ToInt16(tempdata.Id), Convert.ToInt16(enRegisterType.Mobile));

                            SendSMSRequest request = new SendSMSRequest();
                            request.MobileNo = Convert.ToInt64(model.Mobile);
                            request.Message = EnResponseMessage.SendSMSSubject + result.OTP;
                            await _mediator.Send(request);
                            //return AppUtils.StanderdSignUp("You have successfully send Otp in mobile.");
                            return Ok(new SignUpWithMobileResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SignUpWithResendMobile });
                        }
                        else
                        {
                            return BadRequest(new SignUpWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUPMobileValidation, ErrorCode = enErrorCode.Status400BadRequest });
                        }
                        //else
                        //{
                        //    SendSMSRequest request = new SendSMSRequest();
                        //    request.MobileNo = Convert.ToInt64(model.Mobile);
                        //    request.Message = "SMS for use this code " + tempotp.OTP + "";
                        //    await _mediator.Send(request);
                        //    return AppUtils.StanderdSignUp("Please verify it by clicking the otp that has been resend to your mobile.");
                        //}
                    }
                    else
                    {
                        return BadRequest(new SignUpWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status400BadRequest });
                    }
                }
                else
                {
                    //ModelState.AddModelError(string.Empty, "This mobile number is already registered.");
                    return BadRequest(new SignUpWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUPMobileValidation, ErrorCode = enErrorCode.Status400BadRequest });
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new SignUpWithMobileResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }
        #endregion

        #region BlockChainSignUp

        [HttpPost("BlockChainSignUp")]
        [AllowAnonymous]
        public async Task<IActionResult> BlockChainSignUp(BlockChainViewModel model)
        {
            try
            {
                return AppUtils.StanderdSignUp("Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest();
            }
        }

        #endregion

        #endregion
    }
}