﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces.User;
using CleanArchitecture.Core.Services;
using CleanArchitecture.Core.ViewModels;
using CleanArchitecture.Core.ViewModels.AccountViewModels;
using CleanArchitecture.Core.ViewModels.AccountViewModels.ForgotPassword;
using CleanArchitecture.Core.ViewModels.AccountViewModels.Login;
using CleanArchitecture.Core.ViewModels.AccountViewModels.Logoff;
using CleanArchitecture.Core.ViewModels.AccountViewModels.OTP;
using CleanArchitecture.Core.ViewModels.AccountViewModels.ResetPassword;
using CleanArchitecture.Core.ViewModels.AccountViewModels.SignUp;
using CleanArchitecture.Core.ViewModels.ManageViewModels.TwoFA;
using CleanArchitecture.Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using TwoFactorAuthNet;
using CleanArchitecture.Core.Interfaces.Log;
using CleanArchitecture.Core.ViewModels.AccountViewModels.Log;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.ApiModels;

namespace CleanArchitecture.Web.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SigninController : BaseController
    {
        #region Field
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IUserService _userService;
        private readonly IOtpMasterService _otpMasterService;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        private readonly IBasePage _basePage;
        private readonly EncyptedDecrypted _encdecAEC;
        private readonly ICustomPassword _custompassword;
        private readonly Dictionary<string, IUserTwoFactorTokenProvider<ApplicationUser>> _tokenProviders =
            new Dictionary<string, IUserTwoFactorTokenProvider<ApplicationUser>>();
        private readonly ITempUserRegisterService _tempUserRegisterService;
        private readonly IUserKeyMasterService _userKeyMasterService;
        private readonly IipHistory _iipHistory;
        private ApplicationUser _ApplicationUser;
        private readonly ILoginHistory _loginHistory;
        private readonly IMessageConfiguration _messageConfiguration;

        #endregion

        #region Ctore
        public SigninController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILoggerFactory loggerFactory, IMediator mediator, IUserService userService, IOtpMasterService otpMasterService, Microsoft.Extensions.Configuration.IConfiguration configuration, IBasePage basePage,
            EncyptedDecrypted encypted, ICustomPassword custompassword, ITempUserRegisterService tempUserRegisterService, IUserKeyMasterService userKeyMasterService, IipHistory iipHistory, ILoginHistory loginHistory,
            IMessageConfiguration messageConfiguration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<SigninController>();
            _mediator = mediator;
            _userService = userService;
            _otpMasterService = otpMasterService;
            _configuration = configuration;
            _basePage = basePage;
            _encdecAEC = encypted;
            _custompassword = custompassword;
            _tempUserRegisterService = tempUserRegisterService;
            _userKeyMasterService = userKeyMasterService;
            _iipHistory = iipHistory;
            _loginHistory = loginHistory;
            _messageConfiguration = messageConfiguration;
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

        // Login method to call direct send code
        [HttpGet("SendCode")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> SendCode(string returnUrl = null, bool rememberMe = false)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return BadRequest(new LoginResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.CommFailMsgInternal, ErrorCode = enErrorCode.Status400BadRequest });

            }
            //else

            //{
            //    var otpData = _otpMasterService.AddOtp(user.Id, user.Email, "");
            //    var message = "Your security code is: " + otpData;

            //        SendEmailRequest request = new SendEmailRequest();
            //        request.Recepient = user.Email;
            //        request.Subject = "Security Code";
            //        request.Body = message;

            //        await _mediator.Send(request);
            //        //await _emailSender.SendEmailAsync(user.Email, "Security Code", message);

            //}
            //return null;
            var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(user);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();

            var data = new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe, SelectedProvider = "cleanarchitecture" };
            return Ok(data);
            //return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [HttpPost("SendCode")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> SendCode([FromBody]SendCodeViewModel model)
        {

            try
            {
                var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
                if (user == null)
                {

                    return BadRequest(new LoginResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.CommFailMsgInternal, ErrorCode = enErrorCode.Status400BadRequest });
                }

                // Generate the token and send it
                var code = await _userManager.GenerateTwoFactorTokenAsync(user, model.SelectedProvider);
                if (string.IsNullOrWhiteSpace(code))
                {

                    return BadRequest(new LoginResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.CommFailMsgInternal, ErrorCode = enErrorCode.Status400BadRequest });
                }

                var message = "Your security code is: " + code;
                if (model.SelectedProvider == "Email")
                {
                    SendEmailRequest request = new SendEmailRequest();
                    request.Recepient = user.Email;
                    request.Subject = "Security Code";
                    request.Body = message;

                    await _mediator.Send(request);
                    //await _emailSender.SendEmailAsync(user.Email, "Security Code", message);
                }
                // else if (model.SelectedProvider == "Phone")
                // {
                //     await _smsSender.SendSmsAsync(await _userManager.GetPhoneNumberAsync(user), message);
                // }

                return RedirectToAction(nameof(VerifyCode), new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        // Login after verify code
        [HttpGet("VerifyCode")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> VerifyCode(string provider, bool rememberMe, string returnUrl = null)
        {
            // Require that the user has already logged in via username/password or external login
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {

                return BadRequest(new LoginResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.CommFailMsgInternal, ErrorCode = enErrorCode.Status400BadRequest });
            }
            return View(new VerifyCodeViewModel { RememberMe = rememberMe });
        }

        [HttpPost("VerifyCode")]
        [AllowAnonymous]
        // [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> VerifyCode(TwoFACodeVerifyViewModel model)
        {
            // The following code protects for brute force attacks against the two factor codes.
            // If a user enters incorrect codes for a specified amount of time then the user account
            // will be locked out for a specified amount of time.
            //  var result = await _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe, model.RememberBrowser);
            try
            {
                // var Key = await _custompassword.GetPassword(user.Id);
                var TwoFAToken = _userKeyMasterService.GetUserUniqueKey(model.TwoFAKey);
                if (TwoFAToken != null)
                {
                    if (TwoFAToken.UniqueKey != model.TwoFAKey)
                        return BadRequest(new VerifyCodeResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.FactorFail, ErrorCode = enErrorCode.Status4054FactorFail });


                    var user = await _userManager.FindByIdAsync(TwoFAToken.UserId.ToString());
                    if (user != null)
                    {
                        //if (user.lo .IsLockedOut)
                        //{
                        //    return BadRequest(new VerifyCodeResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.StandardLoginLockOut, ErrorCode = enErrorCode.Status423Locked });
                        //}

                        TwoFactorAuth TFAuth = new TwoFactorAuth();
                        //sKey = key; //TFAuth.CreateSecret(160);
                        string code = TFAuth.GetCode(user.PhoneNumber);
                        if (model.Code == code)
                        //bool status = TFAuth.VerifyCode(user.PhoneNumber, model.Code, 1, dt);
                        //if (status)
                        {
                            //// Valid Key and status Disable
                            _userKeyMasterService.UpdateOtp(TwoFAToken.Id);

                            return Ok(new VerifyCodeResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.StandardLoginSuccess });
                            // return RedirectToLocal(model.ReturnUrl);
                        }
                        else
                        {
                            return BadRequest(new VerifyCodeResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.FactorFail, ErrorCode = enErrorCode.Status4054FactorFail });
                        }
                        // }
                    }

                }
                return BadRequest(new VerifyCodeResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.FactorKeyFail, ErrorCode = enErrorCode.Status4107TwoFAKeyinvalid });

                //return BadRequest(new VerifyCodeResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.FactorFail, ErrorCode = enErrorCode.Status4054FactorFail });
                /*
                var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

                //var Key = await _custompassword.GetPassword(user.Id);

                //if(Key.Password != model.TwoFAKey)
                //    return BadRequest(new VerifyCodeResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.FactorFail, ErrorCode = enErrorCode.Status4054FactorFail });

                //// Valid Key and status Disable
                //_custompassword.UpdateOtp(Key.Id);

                var authenticatorCode = model.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

                //string tokenProvider = ProviderName;
                ////// Make sure the token is valid
                //////var result = await _tokenProviders[tokenProvider].ValidateAsync("TwoFactor", authenticatorCode, _userManager, user);

                ////return null;

                var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, false, false);
                if (result.Succeeded)
                {
                    return Ok(new VerifyCodeResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.StandardLoginSuccess });

                    // return RedirectToLocal(model.ReturnUrl);
                }
                else if (result.IsLockedOut)
                {
                    return BadRequest(new VerifyCodeResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.StandardLoginLockOut, ErrorCode = enErrorCode.Status423Locked });
                }
                else
                {
                    return BadRequest(new VerifyCodeResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.FactorFail, ErrorCode = enErrorCode.Status4054FactorFail });

                }
                */

                // //if (result.IsLockedOut)
                // //{
                // //    _logger.LogWarning(7, "User account locked out.");
                // //    return View("Lockout");
                // //}
                // //return BadRequest(new VerifyCodeResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.FactorFail, ErrorCode = enErrorCode.Status400BadRequest });

            }
            catch (Exception ex)
            {
                return BadRequest(new LoginResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });

                throw;
            }



        }
        #endregion

        #region Methods

        [HttpPost("login")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Login(Core.ViewModels.AccountViewModels.LoginViewModel model)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            try
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    var roles = await _userManager.GetRolesAsync(user);
                    return Ok(new LoginResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.StandardLoginSuccess });
                    //return AppUtils.SignIn(user, roles);
                }
                if (result.RequiresTwoFactor)
                {
                    return Ok(new StandardLoginResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.FactorRequired, ErrorCode = enErrorCode.Status4060VerifyMethod });

                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "User account locked out.");
                    return BadRequest(new LoginResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.StandardLoginLockOut, ErrorCode = enErrorCode.Status423Locked });
                }
                else
                {
                    return BadRequest(new LoginResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginWithOtpInvalidAttempt, ErrorCode = enErrorCode.Status423Locked });
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                //return BadRequest();
                return BadRequest(new LoginResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });

            }

        }

        #region Standerd Login
        /// <summary>
        /// Thid method are used standard login 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("StandardLogin")]
        [AllowAnonymous]
        public async Task<IActionResult> StandardLogin(StandardLoginViewModel model)
        {
            try
            {
                ////// Ip Address Validate or not
                string CountryCode = await _userService.GetCountryByIP(model.IPAddress);
                if (!string.IsNullOrEmpty(CountryCode) && CountryCode == "fail")
                {
                    return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.IpAddressInvalid, ErrorCode = enErrorCode.Status4020IpInvalid });
                }

                //var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, lockoutOnFailure: false);
                // var checkmail = await _userManager.FindByEmailAsync(model.Username);
                string Location = await _userService.GetLocationByIP(model.IPAddress);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Username);
                    _ApplicationUser = user;
                    var roles = await _userManager.GetRolesAsync(user);


                    //// added by nirav savariya for ip history user wise on 11-03-2018
                    var IpHistory = new IpHistoryViewModel()
                    {
                        IpAddress = model.IPAddress,
                        Location = Location,
                        UserId = user.Id,
                    };
                    _iipHistory.AddIpHistory(IpHistory);

                    var LoginhistoryViewModel = new LoginhistoryViewModel()
                    {
                        UserId = user.Id,
                        IpAddress = model.IPAddress,
                        Device = model.DeviceId,
                        Location = Location
                    };
                    _loginHistory.AddLoginHistory(LoginhistoryViewModel);

                    return Ok(new StandardLoginResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.StandardLoginSuccess });
                }
                if (result.RequiresTwoFactor)
                {

                    //// Start 2FA in Custome token Create 
                    var user = await _userManager.FindByNameAsync(model.Username);

                    //// added by nirav savariya for ip history user wise on 11-03-2018
                    var IpHistory = new IpHistoryViewModel()
                    {
                        IpAddress = model.IPAddress,
                        Location = Location,
                        UserId = user.Id,
                    };
                    _iipHistory.AddIpHistory(IpHistory);

                    var LoginhistoryViewModel = new LoginhistoryViewModel()
                    {
                        UserId = user.Id,
                        IpAddress = model.IPAddress,
                        Device = model.DeviceId,
                        Location = Location
                    };
                    _loginHistory.AddLoginHistory(LoginhistoryViewModel);

                    string TwoFAToken = _userKeyMasterService.Get2FACustomToken(user.Id);
                    //// End 2FA in Custome token Create 
                    return Ok(new StandardLogin2FAResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.FactorRequired, ErrorCode = enErrorCode.Status4060VerifyMethod, TwoFAToken = TwoFAToken });

                    //return Ok(new StandardLoginResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.FactorRequired, ErrorCode = enErrorCode.Status4060VerifyMethod });
                }
                if (result.IsLockedOut)
                {
                    //_logger.LogWarning(2, "User account locked out for two hours.");

                    return BadRequest(new StandardLoginResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.StandardLoginLockOut, ErrorCode = enErrorCode.Status423Locked });
                }

                ///////////////// Check bizUser  table in username  Exist or not
                var resultUserName = await _userManager.FindByNameAsync(model.Username);
                if (!string.IsNullOrEmpty(resultUserName?.UserName))
                {
                    return BadRequest(new StandardLoginResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.StandardLoginfailed, ErrorCode = enErrorCode.Status4032LoginFailed });
                    //return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpBizUserNameExist, ErrorCode = enErrorCode.Status4099BizUserNameExist });
                }
                ////////////////// check TempUser  table in username Exist and Verify Pending
                bool IsSignUserName = _tempUserRegisterService.GetUserName(model.Username);
                if (!IsSignUserName)
                {
                    return Ok(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpTempUserNameVerifyPending, ErrorCode = enErrorCode.Status4036VerifyPending });
                }

                return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status4033NotFoundRecored });

                //else
                //{
                //    if (checkmail != null)
                //        await _userManager.AccessFailedAsync(checkmail);
                //    //return BadRequest(new ApiError("Login failed : Invalid username or password."));
                //return BadRequest(new StandardLoginResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.StandardLoginfailed, ErrorCode = enErrorCode.Status4032LoginFailed });
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                //return BadRequest();
                return BadRequest(new StandardLoginResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }
        #endregion

        #region Login With Email
        /// <summary>
        /// This method are used login with notify to your email. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("LoginWithEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithEmail(LoginWithEmailViewModel model)
        {
            try
            {
                ////// Ip Address Validate or not
                string CountryCode = await _userService.GetCountryByIP(model.IPAddress);
                if (!string.IsNullOrEmpty(CountryCode) && CountryCode == "fail")
                {
                    return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.IpAddressInvalid, ErrorCode = enErrorCode.Status4020IpInvalid });
                }

                var user = await _userManager.FindByEmailAsync(model.Email);
              
                if (user != null)
                {
                    var otpData = await _otpMasterService.AddOtp(user.Id, user.Email, "");
                    if (otpData != null)
                    {
                        CustomtokenViewModel data = new CustomtokenViewModel(); // added by nirav savariya for login with mobile and email on 16-10-2018
                        data.Password = otpData.Password;
                        data.UserId = otpData.UserId;
                        data.EnableStatus = false;
                        await _custompassword.AddPassword(data);

                        return Ok(new LoginWithEmailResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.LoginWithEmailSuccessSend, Appkey = otpData.appkey });
                    }
                    else
                    {

                        _logger.LogWarning(2, "User Otp Data Not Send.");
                        return BadRequest(new LoginWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginWithOtpDatanotSend, ErrorCode = enErrorCode.Status4085LoginWithOtpDatanotSend });
                    }
                }
                else
                {
                    ////////////////////// check Tempuser table in Email verfy pending
                    bool IsSignEmailVerifyPending = _tempUserRegisterService.GetEmail(model.Email);
                    if (!IsSignEmailVerifyPending)
                    {
                        return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpTempUserEmailVerifyPending, ErrorCode = enErrorCode.Status4036VerifyPending });
                    }

                    //return BadRequest(new ApiError("Login failed: Invalid email."));
                    return BadRequest(new LoginWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginWithOtpLoginFailed, ErrorCode = enErrorCode.Status4086LoginWithOtpLoginFailed });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                //return BadRequest();
                return BadRequest(new LoginWithEmailResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        /// <summary>
        /// This method are used login with email otp base verification 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("EmailOtpVerification")]
        [AllowAnonymous]
        public async Task<IActionResult> EmailOtpVerification(OTPWithEmailViewModel model)
        {
            try
            {
                ////// Ip Address Validate or not
                string CountryCode = await _userService.GetCountryByIP(model.IPAddress);
                if (!string.IsNullOrEmpty(CountryCode) && CountryCode == "fail")
                {
                    return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.IpAddressInvalid, ErrorCode = enErrorCode.Status4020IpInvalid });
                }

                //var logindata = await _userService.FindByEmail(model.Email);
                var checkmail = await _userManager.FindByEmailAsync(model.Email);
                string Location = await _userService.GetLocationByIP(model.IPAddress);
                if (!string.IsNullOrEmpty(checkmail?.Email) && (checkmail.LockoutEnd <= DateTime.UtcNow || checkmail.LockoutEnd == null))
                {
                    if (checkmail?.Id > 0)
                    {
                        var tempotp = await _otpMasterService.GetOtpData(Convert.ToInt16(checkmail.Id));
                        if (tempotp != null)
                        {
                            if (tempotp?.ExpirTime >= DateTime.UtcNow)
                            {
                                if (model.OTP == tempotp.OTP)
                                {
                                    _otpMasterService.UpdateOtp(tempotp.Id);  /// Added by pankaj for update the opt enable status
                                    if (checkmail.TwoFactorEnabled)
                                    {
                                        //var result = await _signInManager.PasswordSignInAsync(checkmail.UserName, currenttime, false, lockoutOnFailure: false);
                                        //// Start 2FA in Custome token Create 
                                        var user = await _userManager.FindByEmailAsync(model.Email);
                                        //// added by nirav savariya for ip history user wise on 11-03-2018
                                        var IpHistory = new IpHistoryViewModel()
                                        {
                                            IpAddress = model.IPAddress,
                                            Location = Location,
                                            UserId = user.Id,
                                        };
                                        _iipHistory.AddIpHistory(IpHistory);

                                        var Loginhistory = new LoginhistoryViewModel()
                                        {
                                            UserId = user.Id,
                                            IpAddress = model.IPAddress,
                                            Device = model.DeviceId,
                                            Location = Location
                                        };
                                        _loginHistory.AddLoginHistory(Loginhistory);

                                        string TwoFAToken = _userKeyMasterService.Get2FACustomToken(user.Id);
                                        //// End 2FA in Custome token Create 


                                        return Ok(new LoginWithEmail2FAResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.FactorRequired, ErrorCode = enErrorCode.Status4060VerifyMethod, TwoFAToken = TwoFAToken });
                                        // return Ok(new OTPWithEmailResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.FactorRequired, ErrorCode = enErrorCode.Status4060VerifyMethod });



                                        //string currenttime = DateTime.UtcNow.ToString();

                                        //var newPassword = _userManager.PasswordHasher.HashPassword(checkmail, currenttime);
                                        //checkmail.PasswordHash = newPassword;
                                        //var res = await _userManager.UpdateAsync(checkmail);
                                        //if (res.Succeeded)
                                        //{
                                        //    //var result = await _signInManager.PasswordSignInAsync(checkmail.UserName, currenttime, false, lockoutOnFailure: false);
                                        //    //// Start 2FA in Custome token Create 
                                        //    var user = await _userManager.FindByEmailAsync(model.Email);
                                        //    string TwoFAToken = _userKeyMasterService.Get2FACustomToken(user.Id);
                                        //    //// End 2FA in Custome token Create 


                                        //    return Ok(new LoginWithEmail2FAResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.FactorRequired, ErrorCode = enErrorCode.Status4060VerifyMethod, TwoFAToken = TwoFAToken });
                                        //    // return Ok(new OTPWithEmailResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.FactorRequired, ErrorCode = enErrorCode.Status4060VerifyMethod });
                                        //}
                                        //else
                                        //    return BadRequest(new OTPWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.Userpasswordnotupdated, ErrorCode = enErrorCode.Status4061Userpasswordnotupdated });

                                    }
                                    _logger.LogWarning(1, "You are successfully login.");
                                    _otpMasterService.UpdateOtp(tempotp.Id);
                                    var roles = await _userManager.GetRolesAsync(checkmail);

                                    //// added by nirav savariya for ip history user wise on 11-03-2018
                                    var IpHistorydet = new IpHistoryViewModel()
                                    {
                                        IpAddress = model.IPAddress,
                                        Location = Location,
                                        UserId = checkmail.Id,
                                    };
                                    _iipHistory.AddIpHistory(IpHistorydet);

                                    var LoginhistoryViewModel = new LoginhistoryViewModel()
                                    {
                                        UserId = checkmail.Id,
                                        IpAddress = model.IPAddress,
                                        Device = model.DeviceId,
                                        Location = Location
                                    };
                                    _loginHistory.AddLoginHistory(LoginhistoryViewModel);
                                    //  return AppUtils.SignIn(checkmail, roles);
                                    return Ok(new OTPWithEmailResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.StandardLoginSuccess });
                                }
                                else
                                {
                                    if (checkmail?.AccessFailedCount < Convert.ToInt16(_configuration["MaxFailedAttempts"]))
                                    {
                                        checkmail.AccessFailedCount = checkmail.AccessFailedCount + 1;
                                        if (checkmail.AccessFailedCount == Convert.ToInt16(_configuration["MaxFailedAttempts"]))
                                        {
                                            checkmail.LockoutEnd = DateTime.UtcNow.AddHours(Convert.ToInt64(_configuration["DefaultLockoutTimeSpan"]));
                                            checkmail.AccessFailedCount = 0;
                                            await _userManager.UpdateAsync(checkmail);
                                            return BadRequest(new OTPWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginWithOtpInvalidAttempt, ErrorCode = enErrorCode.Status4088LoginWithOtpInvalidAttempt });
                                            // return BadRequest(new ApiError("Invalid login attempt."));
                                        }
                                        else
                                        {
                                            await _userManager.UpdateAsync(checkmail);
                                            return BadRequest(new OTPWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginWithOtpInvalidAttempt, ErrorCode = enErrorCode.Status4088LoginWithOtpInvalidAttempt });
                                        }
                                    }
                                    else
                                    {
                                        return BadRequest(new OTPWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginWithOtpInvalidAttempt, ErrorCode = enErrorCode.Status4088LoginWithOtpInvalidAttempt });
                                    }
                                }
                            }
                            else
                            {
                                //ModelState.AddModelError(string.Empty, "Resend OTP immediately not valid or expired.");
                                return BadRequest(new OTPWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpResendOTP, ErrorCode = enErrorCode.Status4076SignUpReSendOTP });
                            }
                        }
                        else
                        {
                            return BadRequest(new OTPWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status4033NotFoundRecored });
                        }
                    }
                    else
                    {
                        // ModelState.AddModelError(string.Empty, "Error.");
                        return BadRequest(new OTPWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginWithOtpLoginFailed, ErrorCode = enErrorCode.Status4086LoginWithOtpLoginFailed });
                    }
                }
                else
                {
                    if (checkmail?.LockoutEnd >= DateTime.UtcNow)
                    {
                        _logger.LogWarning(2, "User account locked out for two hours.");
                        // return BadRequest(new ApiError("User account locked out for two hours."));
                        return BadRequest(new OTPWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.StandardLoginLockOut, ErrorCode = enErrorCode.Status423Locked });
                    }
                    else
                    {
                        //ModelState.AddModelError(string.Empty, "Invalid Email.");
                        //  return BadRequest(new ApiError(ModelState));
                        return BadRequest(new OTPWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.EmailFail, ErrorCode = enErrorCode.Status4087EmailFail });
                    }
                }
                //ModelState.AddModelError(string.Empty, "Resend OTP immediately not valid or expired.");
                //return BadRequest(new OTPWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.EmailFail, ErrorCode = enErrorCode.Status423Locked });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new OTPWithEmailResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        /// <summary>
        /// This method are used login with email resend otp base verify 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("ReSendOtpWithEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ReSendOtpWithEmail(LoginWithEmailViewModel model)
        {
            try
            {
                ////// Ip Address Validate or not
                string CountryCode = await _userService.GetCountryByIP(model.IPAddress);
                if (!string.IsNullOrEmpty(CountryCode) && CountryCode == "fail")
                {
                    return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.IpAddressInvalid, ErrorCode = enErrorCode.Status4020IpInvalid });
                }

                var userdt = await _userManager.FindByEmailAsync(model.Email);
                if (!string.IsNullOrEmpty(userdt?.Email))
                {
                    var otpcheck = await _otpMasterService.GetOtpData(Convert.ToInt32(userdt?.Id));
                    if (otpcheck != null)
                    {
                        //if (otpcheck.ExpirTime <= DateTime.UtcNow && !otpcheck.EnableStatus) // Remove expiretime as per discuss with nishit bhai 10-09-2018
                        if (!otpcheck.EnableStatus)
                        {
                            _otpMasterService.UpdateOtp(otpcheck.Id);
                            var custompwd = await _custompassword.GetPassword(otpcheck.UserId);
                            if (custompwd != null)
                                _custompassword.UpdateOtp(custompwd.Id);
                            var otpData = await _otpMasterService.AddOtp(Convert.ToInt32(userdt.Id), userdt.Email, "");
                            if (otpData != null)
                            {
                                CustomtokenViewModel data = new CustomtokenViewModel(); // added by nirav savariya for login with mobile and email on 16-10-2018
                                data.Password = otpData.Password;
                                data.UserId = otpData.UserId;
                                data.EnableStatus = false;
                                await _custompassword.AddPassword(data);
                                _logger.LogWarning(1, "User Login with Email OTP Send Success.");
                                return Ok(new LoginWithEmailResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.LoginUserEmailOTP, Appkey = otpData.appkey });

                            }
                            else
                            {
                                _logger.LogWarning(2, "User Otp Data Not Send.");
                                return BadRequest(new LoginWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginEmailOTPNotsend, ErrorCode = enErrorCode.Status4089LoginEmailOTPNotsend });

                            }
                        }
                        else
                        {
                            return BadRequest(new LoginWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.EmailFail, ErrorCode = enErrorCode.Status4087EmailFail });
                        }
                    }
                    else
                    {
                        return BadRequest(new LoginWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status4033NotFoundRecored });
                    }
                }
                else
                {
                    return BadRequest(new LoginWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginWithOtpLoginFailed, ErrorCode = enErrorCode.Status4086LoginWithOtpLoginFailed });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new LoginWithEmailResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        #endregion

        #region Login With Mobile
        /// <summary>
        /// This method are used login with otp base verify 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("LoginWithMobile")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithMobile(LoginWithMobileViewModel model)
        {
            try
            {
                ////// Ip Address Validate or not
                string CountryCode = await _userService.GetCountryByIP(model.IPAddress);
                if (!string.IsNullOrEmpty(CountryCode) && CountryCode == "fail")
                {
                    return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.IpAddressInvalid, ErrorCode = enErrorCode.Status4020IpInvalid });
                }

                var userdt = await _userService.FindByMobileNumber(model.Mobile);
                if (userdt != null)
                {
                    var otpData = await _otpMasterService.AddOtp(Convert.ToInt32(userdt.Id), "", userdt.Mobile);
                    if (otpData != null)
                    {
                        CustomtokenViewModel data = new CustomtokenViewModel(); // added by nirav savariya for login with mobile and email on 16-10-2018
                        data.Password = otpData.Password;
                        data.UserId = otpData.UserId;
                        data.EnableStatus = false;
                        await _custompassword.AddPassword(data);
                        _logger.LogWarning(1, "User Login with Mobile Send Success.");
                        return Ok(new LoginWithMobileResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.OTPSendOnMobile, Appkey = otpData.appkey });
                    }
                    else
                    {
                        _logger.LogWarning(2, "User Otp Data Not Send.");
                        return BadRequest(new LoginWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.OTPNotSendOnMobile, ErrorCode = enErrorCode.Status4090OTPSendOnMobile });
                    }
                }
                else
                {

                    ///////////////// Check TempUser  table in mobile number  Exist  and verification pending or not
                    bool IsSignTempMobile = _tempUserRegisterService.GetMobileNumber(model.Mobile);
                    if (!IsSignTempMobile)
                    {
                        return Ok(new SignUpWithEmailResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SignUpTempUserMobileExistAndVerificationPending, ErrorCode = enErrorCode.Status4036VerifyPending });
                    }


                    return BadRequest(new LoginWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginWithMobileOtpLoginFailed, ErrorCode = enErrorCode.Status4106LoginFailMobileNotAvailable });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new LoginWithMobileResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        /// <summary>
        /// This method are used login with Mobile otp base verification 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("MobileOtpVerification")]
        [AllowAnonymous]
        public async Task<IActionResult> MobileOtpVerification(OTPWithMobileViewModel model)
        {
            try
            {
                ////// Ip Address Validate or not
                string CountryCode = await _userService.GetCountryByIP(model.IPAddress);
                if (!string.IsNullOrEmpty(CountryCode) && CountryCode == "fail")
                {
                    return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.IpAddressInvalid, ErrorCode = enErrorCode.Status4020IpInvalid });
                }

                var logindata = await _userService.FindByMobileNumber(model.Mobile);
                var result = await _userManager.FindByIdAsync(logindata?.Id.ToString());
                string Location = await _userService.GetLocationByIP(model.IPAddress);
                if (!string.IsNullOrEmpty(result?.Mobile) && (result.LockoutEnd <= DateTime.UtcNow || result.LockoutEnd == null))
                {
                    if (logindata?.Id > 0)
                    {
                        var tempotp = await _otpMasterService.GetOtpData(Convert.ToInt16(logindata.Id));
                        if (tempotp != null)
                        {
                            if (tempotp?.ExpirTime >= DateTime.UtcNow)
                            {
                                if (model.OTP == tempotp.OTP)
                                {
                                    _otpMasterService.UpdateOtp(tempotp.Id);  /// Added by pankaj for update the opt enable status
                                    if (result.TwoFactorEnabled)   /// Addede By Pankaj For TwoFactor Authentication Purporse
                                    {
                                        //// added by nirav savariya for ip history user wise on 11-03-2018
                                        var IpHistory = new IpHistoryViewModel()
                                        {
                                            IpAddress = model.IPAddress,
                                            Location = Location,
                                            UserId = result.Id,
                                        };
                                        _iipHistory.AddIpHistory(IpHistory);

                                        var Loginhistory = new LoginhistoryViewModel()
                                        {
                                            UserId = result.Id,
                                            IpAddress = model.IPAddress,
                                            Device = model.DeviceId,
                                            Location = Location
                                        };
                                        _loginHistory.AddLoginHistory(Loginhistory);

                                        ////// Start 2FA in Custome token Create                                    
                                        string TwoFAToken = _userKeyMasterService.Get2FACustomToken(logindata.Id);
                                        ////// End 2FA in Custome token Create 
                                        /////
                                        return Ok(new OtpWithMobile2FAResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.FactorRequired, ErrorCode = enErrorCode.Status4060VerifyMethod, TwoFAToken = TwoFAToken });


                                        //string currenttime = DateTime.UtcNow.ToString();

                                        //var newPassword = _userManager.PasswordHasher.HashPassword(result, currenttime);
                                        //result.PasswordHash = newPassword;
                                        //var res = await _userManager.UpdateAsync(result);
                                        //if (res.Succeeded)
                                        //{
                                        //    var resultdata = await _signInManager.PasswordSignInAsync(result.UserName, currenttime, false, lockoutOnFailure: false);

                                        //    //// Start 2FA in Custome token Create                                            
                                        //    //string TwoFAToken = await _custompassword.Get2FACustomToken(logindata.Id);
                                        //    //// End 2FA in Custome token Create 
                                        //    //return Ok(new OtpWithMobile2FAResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.FactorRequired, ErrorCode = enErrorCode.Status4060VerifyMethod, TwoFAToken = TwoFAToken });



                                        //    return Ok(new OTPWithMobileResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.FactorRequired, ErrorCode = enErrorCode.Status4060VerifyMethod });
                                        //}
                                        //else
                                        //    return BadRequest(new OTPWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.Userpasswordnotupdated, ErrorCode = enErrorCode.Status4061Userpasswordnotupdated });
                                    }

                                    //// added by nirav savariya for ip history user wise on 11-03-2018
                                    var IpHistorydet = new IpHistoryViewModel()
                                    {
                                        IpAddress = model.IPAddress,
                                        Location = Location,
                                        UserId = result.Id,
                                    };
                                    _iipHistory.AddIpHistory(IpHistorydet);

                                    var LoginhistoryViewModel = new LoginhistoryViewModel()
                                    {
                                        UserId = result.Id,
                                        IpAddress = model.IPAddress,
                                        Device = model.DeviceId,
                                        Location = Location
                                    };
                                    _loginHistory.AddLoginHistory(LoginhistoryViewModel);
                                    _otpMasterService.UpdateOtp(tempotp.Id);
                                    var roles = await _userManager.GetRolesAsync(result);
                                    return Ok(new OTPWithMobileResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.StandardLoginSuccess });
                                }
                                if (result?.AccessFailedCount < Convert.ToInt16(_configuration["MaxFailedAttempts"]))
                                {
                                    result.AccessFailedCount = result.AccessFailedCount + 1;
                                    if (result.AccessFailedCount == Convert.ToInt16(_configuration["MaxFailedAttempts"]))
                                    {
                                        result.LockoutEnd = DateTime.UtcNow.AddHours(Convert.ToInt64(_configuration["DefaultLockoutTimeSpan"]));
                                        result.AccessFailedCount = 0;
                                        await _userManager.UpdateAsync(result);
                                        return BadRequest(new OTPWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginMobileNumberInvalid, ErrorCode = enErrorCode.Status4091LoginMobileNumberInvalid });
                                    }
                                    else
                                    {
                                        await _userManager.UpdateAsync(result);
                                        return BadRequest(new OTPWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginWithOtpInvalidAttempt, ErrorCode = enErrorCode.Status4088LoginWithOtpInvalidAttempt });
                                    }
                                }
                                else
                                {
                                    return BadRequest(new OTPWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginWithOtpInvalidAttempt, ErrorCode = enErrorCode.Status4088LoginWithOtpInvalidAttempt });
                                }
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Resend OTP immediately not valid or expired.");
                                return BadRequest(new OTPWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginWithOtpInvalidAttempt, ErrorCode = enErrorCode.Status4088LoginWithOtpInvalidAttempt });
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Error.");
                        return BadRequest(new OTPWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginWithOtpInvalidAttempt, ErrorCode = enErrorCode.Status4088LoginWithOtpInvalidAttempt });
                    }
                }
                else
                {
                    if (result?.LockoutEnd >= DateTime.UtcNow)
                    {
                        _logger.LogWarning(2, "User account locked out for two hours.");

                        return BadRequest(new OTPWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.StandardLoginLockOut, ErrorCode = enErrorCode.Status423Locked });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid MobileNo.");
                        return BadRequest(new OTPWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginMobileNumberInvalid, ErrorCode = enErrorCode.Status4091LoginMobileNumberInvalid });
                    }
                }
                ModelState.AddModelError(string.Empty, "Resend OTP immediately not valid or expired.");
                return BadRequest(new OTPWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginMobileNumberInvalid, ErrorCode = enErrorCode.Status4091LoginMobileNumberInvalid });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new OTPWithMobileResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        /// <summary>
        /// This method are used login with mobile resend otp base verify 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("ReSendOtpWithMobile")]
        [AllowAnonymous]
        public async Task<IActionResult> ReSendOtpWithMobile(LoginWithMobileViewModel model)
        {
            try
            {

                ////// Ip Address Validate or not
                string CountryCode = await _userService.GetCountryByIP(model.IPAddress);
                if (!string.IsNullOrEmpty(CountryCode) && CountryCode == "fail")
                {
                    return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.IpAddressInvalid, ErrorCode = enErrorCode.Status4020IpInvalid });
                }

                var userdt = await _userService.FindByMobileNumber(model.Mobile);
                if (!string.IsNullOrEmpty(userdt?.Mobile))
                {
                    var otpcheck = await _otpMasterService.GetOtpData(Convert.ToInt32(userdt?.Id));
                    if (otpcheck != null)
                    {
                        //if (otpcheck.ExpirTime <= DateTime.UtcNow && !otpcheck.EnableStatus)  // Remove expiretime as per discuss with nishit bhai 10-09-2018
                        if (!otpcheck.EnableStatus)
                        {
                            _otpMasterService.UpdateOtp(otpcheck.Id);
                            var custompwd = await _custompassword.GetPassword(otpcheck.UserId);
                            if (custompwd != null)
                                _custompassword.UpdateOtp(custompwd.Id);
                            var otpData = await _otpMasterService.AddOtp(Convert.ToInt32(userdt.Id), "", userdt.Mobile);
                            if (otpData != null)
                            {
                                CustomtokenViewModel data = new CustomtokenViewModel(); // added by nirav savariya for login with mobile and email on 16-10-2018
                                data.Password = otpData.Password;
                                data.UserId = otpData.UserId;
                                data.EnableStatus = false;
                                await _custompassword.AddPassword(data);
                                _logger.LogWarning(1, "User Login with Mobile OTP Send Success.");
                                return Ok(new LoginWithMobileResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.OTPSendOnMobile, Appkey = otpData.appkey });
                            }
                            else
                            {
                                _logger.LogWarning(2, "User Otp Data Not Send.");
                                return BadRequest(new LoginWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.OTPNotSendOnMobile, ErrorCode = enErrorCode.Status4090OTPSendOnMobile });
                            }
                        }
                        else
                        {
                            return BadRequest(new LoginWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginMobileNumberInvalid, ErrorCode = enErrorCode.Status4090OTPSendOnMobile });

                            //return BadRequest(new ApiError("Invalid Mobileno."));
                            //SendSMSRequest request = new SendSMSRequest();
                            //request.MobileNo = Convert.ToInt64(model.Mobile);
                            //request.Message = "SMS for use this code " + otpcheck.OTP + "";

                            //await _mediator.Send(request);

                            //_logger.LogWarning(1, "User Login with Mobile OTP Send Success.");

                            //return Ok("User Login with Mobile OTP Send Success.");
                        }
                    }
                    else
                    {
                        return BadRequest(new LoginWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status4033NotFoundRecored });
                    }
                }
                else
                {
                    return BadRequest(new LoginWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginMobileNumberInvalid, ErrorCode = enErrorCode.Status4090OTPSendOnMobile });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new LoginWithMobileResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }
        #endregion

        #region Social Login

        /// <summary>
        ///  This method are use to Social Login method for google
        /// </summary>
        [HttpPost("ExternalLoginForGoogle")]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginForGoogle([FromBody] SocialLoginWithGoogleViewModel model)
        {
            try
            {
                var httpClient = new HttpClient();
                var appAccessTokenResponse = (dynamic)null;
                try
                {
                    appAccessTokenResponse = await httpClient.GetStringAsync(_configuration["SocialGoogle"].ToString() + model.access_token);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                    return BadRequest(new SocialLoginGoogleResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidGoogleToken, ErrorCode = enErrorCode.Status4068InvalidGoogleToken });
                }
                if (appAccessTokenResponse != null)
                {
                    var userAccessTokenValidation = JsonConvert.DeserializeObject<GoogleSocial>(appAccessTokenResponse);

                    if (userAccessTokenValidation.user_id == model.ProviderKey)
                    {
                        var result = await _signInManager.ExternalLoginSignInAsync(model.ProviderName, model.ProviderKey, isPersistent: false, bypassTwoFactor: true);

                        if (result.Succeeded)
                        {
                            var user = await _userManager.FindByLoginAsync(model.ProviderName, model.ProviderKey);

                            //var props = new AuthenticationProperties();
                            //var info = await _signInManager.GetExternalLoginInfoAsync();
                            //props.StoreTokens(info.AuthenticationTokens);
                            //await _signInManager.SignInAsync(user, props, model.ProviderName);

                            await _signInManager.SignInAsync(user, isPersistent: false);
                            //_logger.LogInformation(
                            //    "{Name} logged in with {LoginProvider} provider.",
                            //    info.Principal.Identity.Name, info.LoginProvider);
                            SocialCustomPasswordViewMoel socialCustomPasswordViewMoel = _userService.GenerateRandomSocialPassword(model.ProviderKey);

                            if (socialCustomPasswordViewMoel != null)
                            {
                                CustomtokenViewModel data = new CustomtokenViewModel(); // added by nirav savariya for login with mobile and email on 16-10-2018
                                data.Password = socialCustomPasswordViewMoel.Password;
                                data.UserId = user.Id;
                                data.EnableStatus = false;
                                await _custompassword.AddPassword(data);

                            }
                            _logger.LogInformation(1, "User logged in with social using.");
                            return Ok(new SocialLoginGoogleResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.StandardLoginSuccess, Appkey = socialCustomPasswordViewMoel.AppKey });

                        }
                        if (result.RequiresTwoFactor)
                        {
                            //return BadRequest(new SocialLoginGoogleResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.FactorRequired, ErrorCode = enErrorCode.Status4068InvalidGoogleToken });
                            return Ok(new StandardLoginResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.FactorRequired, ErrorCode = enErrorCode.Status4060VerifyMethod });
                        }
                        if (result.IsLockedOut)
                        {
                            return BadRequest(new SocialLoginGoogleResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.StandardLoginLockOut, ErrorCode = enErrorCode.Status423Locked });
                        }
                        else
                        {
                            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                            var userdet = await _userManager.CreateAsync(user);
                            var infodet = new UserLoginInfo(model.ProviderName, model.ProviderKey, model.ProviderName);
                            if (userdet.Succeeded)
                            {
                                var userlogin = await _userManager.AddLoginAsync(user, infodet);

                                if (userlogin.Succeeded)
                                {
                                    // Copy over the gender claim
                                    //await _userManager.AddClaimAsync(user,
                                    //    infodet.Principal.FindFirst(ClaimTypes.Gender));

                                    // Include the access token in the properties
                                    //var props = new AuthenticationProperties();
                                    //props.StoreTokens(infodet.AuthenticationTokens);
                                    await _signInManager.SignInAsync(user, isPersistent: false);
                                    //await _signInManager.SignInAsync(user, props,
                                    //    authenticationMethod: infodet.LoginProvider);
                                    //_logger.LogInformation(
                                    //    "User created an account using {Name} provider.",
                                    //    info.LoginProvider);

                                    SocialCustomPasswordViewMoel socialCustomPasswordViewMoel = _userService.GenerateRandomSocialPassword(model.ProviderKey);

                                    if (socialCustomPasswordViewMoel != null)
                                    {
                                        CustomtokenViewModel data = new CustomtokenViewModel(); // added by nirav savariya for login with mobile and email on 16-10-2018
                                        data.Password = socialCustomPasswordViewMoel.Password;
                                        data.UserId = user.Id;
                                        data.EnableStatus = false;
                                        await _custompassword.AddPassword(data);

                                    }


                                    _logger.LogInformation(1, "User logged in with social using.");
                                    return Ok(new SocialLoginGoogleResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.StandardLoginSuccess, Appkey = socialCustomPasswordViewMoel.AppKey });
                                }
                                else
                                {
                                    return BadRequest(new SocialLoginGoogleResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SocialUserInsertError, ErrorCode = enErrorCode.Status4070SocialUserInsertError });
                                }
                            }
                            else
                            {
                                return BadRequest(new SocialLoginGoogleResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SocialUserInsertError, ErrorCode = enErrorCode.Status4070SocialUserInsertError });
                            }
                        }
                    }
                    else
                    {
                        return BadRequest(new SocialLoginGoogleResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidGoogleProviderKey, ErrorCode = enErrorCode.Status4069InvalidGoogleProviderKey });
                    }
                }
                else
                {
                    return BadRequest(new SocialLoginGoogleResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidGoogleToken, ErrorCode = enErrorCode.Status4068InvalidGoogleToken });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new SocialLoginGoogleResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        /// <summary>
        /// This method created by pankaj for user perform to  External login with facebook.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("ExternalLoginForFacebook")]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginForFacebook([FromBody] SocialLoginWithfacebookViewModel model)
        {
            try
            {
                var appAccessTokenResponse = (dynamic)null;
                var httpClient = new HttpClient();

                try
                {
                    appAccessTokenResponse = await httpClient.GetStringAsync(_configuration["SocialFacebookToken"].ToString()
                        + _configuration["Authentication:Facebook:AppId"].ToString() + "&client_secret=" + _configuration["Authentication:Facebook:AppSecret"].ToString() +
                        "&grant_type=client_credentials");
                    var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessTokenViewModel>(appAccessTokenResponse);

                    string userAccessTokenValidationResponse = await httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={model.access_token}&access_token=" + appAccessToken.access_token);
                    //var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

                    var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidationViewModel>(userAccessTokenValidationResponse);

                    if (!userAccessTokenValidation.data.is_valid)
                    {
                        return BadRequest(new SocialLoginfacebookResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidFaceBookToken, ErrorCode = enErrorCode.Status4096InvalidFaceBookToken });

                    }

                    appAccessTokenResponse = await httpClient.GetStringAsync(_configuration["SocialFacebook"].ToString() + model.access_token);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                    return BadRequest(new SocialLoginfacebookResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidFaceBookToken, ErrorCode = enErrorCode.Status4096InvalidFaceBookToken });
                }
                if (appAccessTokenResponse != null)
                {
                    var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookSocial>(appAccessTokenResponse);


                    var result = await _signInManager.ExternalLoginSignInAsync(model.ProviderName, model.ProviderKey, isPersistent: false, bypassTwoFactor: true);

                    if (result.Succeeded)
                    {
                        var user = await _userManager.FindByLoginAsync(model.ProviderName, model.ProviderKey);

                        await _signInManager.SignInAsync(user, isPersistent: false);
                        SocialCustomPasswordViewMoel socialCustomPasswordViewMoel = _userService.GenerateRandomSocialPassword(model.ProviderKey);

                        if (socialCustomPasswordViewMoel != null)
                        {
                            CustomtokenViewModel data = new CustomtokenViewModel(); // added by nirav savariya for login with mobile and email on 16-10-2018
                            data.Password = socialCustomPasswordViewMoel.Password;
                            data.UserId = user.Id;
                            data.EnableStatus = false;
                            await _custompassword.AddPassword(data);

                        }
                        _logger.LogInformation(1, "User logged in with social using.");
                        return Ok(new SocialLoginfacebookResponse { ReturnCode = enResponseCode.Success, Appkey = socialCustomPasswordViewMoel.AppKey, ReturnMsg = EnResponseMessage.StandardLoginSuccess });



                    }
                    if (result.RequiresTwoFactor)
                    {
                        return Ok(new SocialLoginfacebookResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.FactorRequired, ErrorCode = enErrorCode.Status4060VerifyMethod });
                    }
                    if (result.IsLockedOut)
                    {
                        return BadRequest(new SocialLoginfacebookResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.StandardLoginLockOut, ErrorCode = enErrorCode.Status423Locked });
                    }
                    else
                    {

                        var user =
                        new ApplicationUser
                        {
                            UserName = userAccessTokenValidation.first_name,
                            Email = userAccessTokenValidation.email,
                            FirstName = userAccessTokenValidation.first_name,
                            LastName = userAccessTokenValidation.last_name
                        };   /// Here email address not set bacause of user can login with mobile number as well as email so.


                        var userdet = await _userManager.CreateAsync(user);


                        var infodet = new UserLoginInfo(model.ProviderName, model.ProviderKey, model.ProviderName);
                        if (userdet.Succeeded)
                        {
                            var userlogin = await _userManager.AddLoginAsync(user, infodet);

                            if (userlogin.Succeeded)
                            {
                                await _signInManager.SignInAsync(user, isPersistent: false);

                                _logger.LogInformation(1, "User logged in with social using.");
                                SocialCustomPasswordViewMoel socialCustomPasswordViewMoel = _userService.GenerateRandomSocialPassword(model.ProviderKey);


                                if (socialCustomPasswordViewMoel != null)
                                {
                                    CustomtokenViewModel data = new CustomtokenViewModel(); // added by nirav savariya for login with mobile and email on 16-10-2018
                                    data.Password = socialCustomPasswordViewMoel.Password;
                                    data.UserId = user.Id;
                                    data.EnableStatus = false;
                                    await _custompassword.AddPassword(data);
                                }
                                return Ok(new SocialLoginfacebookResponse { ReturnCode = enResponseCode.Success, Appkey = socialCustomPasswordViewMoel.AppKey, ReturnMsg = EnResponseMessage.StandardLoginSuccess });
                            }
                            else
                            {
                                return BadRequest(new SocialLoginfacebookResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SocialUserInsertError, ErrorCode = enErrorCode.Status4070SocialUserInsertError });
                            }
                        }
                        else
                        {
                            return BadRequest(new SocialLoginfacebookResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SocialUserInsertError, ErrorCode = enErrorCode.Status4070SocialUserInsertError });
                        }
                    }


                }
                else
                {
                    return BadRequest(new SocialLoginGoogleResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidGoogleToken, ErrorCode = enErrorCode.Status4068InvalidGoogleToken });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new SocialLoginGoogleResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }
        #endregion

        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            try
            {
                var currentUser = await _userManager.FindByEmailAsync(model.Email);

                if (currentUser == null) // || !(await _userManager.IsEmailConfirmedAsync(currentUser)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return BadRequest(new ForgotpassWordResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.ResetUserNotAvailable, ErrorCode = enErrorCode.Status4037UserNotAvailable });
                }

                string ctoken = GenerateRandomPassword();
                //    _userManager.GeneratePasswordResetTokenAsync(currentUser).Result;

                var ResetPassword = new ResetPasswordViewModel
                {
                    Email = currentUser.Email,
                    Code = ctoken,
                    Password = ctoken,
                    ConfirmPassword = ctoken,
                    Expirytime = DateTime.UtcNow + TimeSpan.FromHours(Convert.ToInt64(_configuration["DefaultValidateLinkTimeSpan"]))
                };
                byte[] passwordBytes = _encdecAEC.GetPasswordBytes(_configuration["AESSalt"].ToString());

                string UserDetails = JsonConvert.SerializeObject(ResetPassword);
                string SubScriptionKey = EncyptedDecrypted.Encrypt(UserDetails, passwordBytes);

                string name = currentUser.FirstName;


                //string ctokenlink = Url.Action("resetpassword", "Signin", new

                //{
                //    emailConfirmCode = SubScriptionKey
                //}, protocol: HttpContext.Request.Scheme);
                //string ctokenlink = _configuration["ResetPaswword"].ToString() + SubScriptionKey;
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(SubScriptionKey);
                string ctokenlink = _configuration["ResetPaswword"].ToString() + Convert.ToBase64String(plainTextBytes);








                //   var confirmationLink = "<a class='btn-primary' href=\"" + ctokenlink + "\">" + EnResponseMessage.ResetEmailMessage + "</a>";

                SendEmailRequest request = new SendEmailRequest();
                request.Recepient = model.Email;
               // request.Subject = EnResponseMessage.ForgotPasswordMail;
                //  request.Body = confirmationLink;



                IQueryable Result = await _messageConfiguration.GetTemplateConfigurationAsync(Convert.ToInt16(enCommunicationServiceType.Email), Convert.ToInt16(EnTemplateType.ForgotPassword), 0);
                foreach (TemplateMasterData Provider in Result)
                {

                    //string[] splitedarray = Provider.AdditionaInfo.Split(",");
                    //foreach (string s in splitedarray)
                    //{
                    Provider.Content = Provider.Content.Replace("###Link###", ctokenlink);
                    Provider.Content = Provider.Content.Replace("###USERNAME###", name);
                    //}
                    request.Body = Provider.Content;
                    request.Subject = Provider.AdditionalInfo;
                }

                //CommunicationResponse Response = await _mediator.Send(request);

                await _mediator.Send(request);

                var result = await _userManager.FindByEmailAsync(model.Email);
                if (result != null)
                {
                    result.EmailConfirmed = false;
                    await _userManager.UpdateAsync(result);
                }
                else
                {
                    return BadRequest(new ForgotpassWordResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser });
                }



                return Ok(new ForgotpassWordResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.ResetConfirmedLink });
                // return AppUtils.Standerdlogin("Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new ForgotpassWordResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }

        }


        /// <summary>
        ///  This method are use to create the Random Password
        /// </summary>
        /// <param name="opts"></param>
        /// <returns></returns>
        public static string GenerateRandomPassword(PasswordOptions opts = null)
        {
            if (opts == null) opts = new PasswordOptions()
            {
                RequiredLength = 6,
                RequiredUniqueChars = 4,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true,

            };

            string[] randomChars = new[] {
        "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
        "abcdefghijkmnopqrstuvwxyz",    // lowercase
        "0123456789",                   // digits
         "!@$^*"                        // non-alphanumeric
    };
            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }



        [HttpGet("resetpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string emailConfirmCode)  //ResetPasswordViewModel model)
        {
            try
            {
                if (!string.IsNullOrEmpty(emailConfirmCode))
                {
                    byte[] DecpasswordBytes = _encdecAEC.GetPasswordBytes(_configuration["AESSalt"].ToString());

                    var bytes = Convert.FromBase64String(emailConfirmCode);
                    var encodedString = Encoding.UTF8.GetString(bytes);
                    string DecryptToken = EncyptedDecrypted.Decrypt(encodedString, DecpasswordBytes);

                    ResetPasswordViewModel model = JsonConvert.DeserializeObject<ResetPasswordViewModel>(DecryptToken);
                    if (model?.Expirytime >= DateTime.UtcNow)
                    {
                        var user = await _userManager.FindByEmailAsync(model.Email);

                        if (user == null)
                        {
                            // Don't reveal that the user does not exist
                            //return Ok("Reset confirmed");
                            return BadRequest(new ResetPassWordResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.ResetPasswordUseNotexist, ErrorCode = enErrorCode.Status4038ResetUserNotAvailable });
                            //return AppUtils.Standerdlogin("Reset confirmed");
                        }

                        if (user.EmailConfirmed)
                        {
                            return BadRequest(new ResetPassWordResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.ResetConfirm, ErrorCode = enErrorCode.Status4041ResetPasswordConfirm });
                        }

                        string hashedNewPassword = string.Empty;

                        hashedNewPassword = _userManager.PasswordHasher.HashPassword(user, model?.Password);

                        user.PasswordHash = hashedNewPassword;
                        user.EmailConfirmed = true;
                        var result = await _userManager.UpdateAsync(user);

                        if (result.Succeeded)
                        {
                            SendEmailRequest request = new SendEmailRequest();
                            request.Recepient = model.Email;
                           // request.Subject = EnResponseMessage.ResetPasswordMail;
                            //request.Body = EnResponseMessage.EmailNewPassword + " : " + model.Password;


                            IQueryable Result = await _messageConfiguration.GetTemplateConfigurationAsync(Convert.ToInt16(enCommunicationServiceType.Email), Convert.ToInt16(EnTemplateType.ResetPassword), 0);
                            foreach (TemplateMasterData Provider in Result)
                            {

                                //string[] splitedarray = Provider.AdditionaInfo.Split(",");
                                //foreach (string s in splitedarray)
                                //{
                                Provider.Content = Provider.Content.Replace("###USERNAME###", user.FirstName);
                                Provider.Content = Provider.Content.Replace("###Password###", model.Password);
                                //}
                                request.Body = Provider.Content;
                                request.Subject = Provider.AdditionalInfo;
                            }




                            await _mediator.Send(request);
                            return Ok(new ResetPassWordResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.ResetResendEmail });
                        }
                    }
                    else
                    {
                        return BadRequest(new ResetPassWordResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.ResetPasswordEmailExpire, ErrorCode = enErrorCode.Status4039ResetPasswordLinkExpired });
                    }
                }
                else
                {
                    return BadRequest(new ResetPassWordResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.ResetPasswordEmailLinkBlank, ErrorCode = enErrorCode.Status4040ResetPasswordLinkempty });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new ResetPassWordResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }

            return BadRequest(new ResetPassWordResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = "null", ErrorCode = enErrorCode.Status400BadRequest });
        }

        #region UnLockUser
        /// <summary>
        /// This method are used for user unlock. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("UnLockUser")]
        public async Task<IActionResult> UnLockUser(UnLockUserViewModel model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId.ToString());
                if (user != null)
                {
                    user.AccessFailedCount = 0;
                    user.LockoutEnd = null;
                    var userUpdate = await _userManager.UpdateAsync(user);
                    if (userUpdate.Succeeded)
                    {
                        return Ok(new UnLockUserResponseViewModel { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.UnLockUser });
                    }
                    else
                    {
                        return BadRequest(new UnLockUserResponseViewModel { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.UnLockUserError, ErrorCode = enErrorCode.Status4077UserUnlockError });
                    }
                }
                else
                {
                    return BadRequest(new UnLockUserResponseViewModel { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status4033NotFoundRecored });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new UnLockUserResponseViewModel { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        #endregion

        #region Logout        
        [HttpPost("logout")]
        public async Task<IActionResult> LogOff()
        {
            try
            {
                await _signInManager.SignOutAsync();
                foreach (var cookieKey in Request.Cookies.Keys)
                {
                    Response.Cookies.Delete(cookieKey);
                }
                _logger.LogInformation(4, "User logged out.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);

                return BadRequest(new Logoffresponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }
        #endregion
        [HttpPost("GetSocailkey")]
        [AllowAnonymous]
        public IActionResult GetSocailkey(string Providername)
        {
            try
            {
                if (string.IsNullOrEmpty(Providername))
                {
                    return BadRequest(new SocialLoginGoogleResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InputProvider, ErrorCode = enErrorCode.Status4101InputProvider });
                }
                SocialKeyDetailViewModel socialKeyDetailViewModel = new SocialKeyDetailViewModel();
                if (Providername == "Facebook")
                {
                    socialKeyDetailViewModel.ProviderName = Providername;
                    socialKeyDetailViewModel.ClientId = _configuration["Authentication:Facebook:AppId"].ToString();
                    socialKeyDetailViewModel.ClientSecret = _configuration["Authentication:Facebook:AppSecret"].ToString();

                }
                else if (Providername == "Google")
                {
                    socialKeyDetailViewModel.ProviderName = Providername;
                    socialKeyDetailViewModel.ClientId = _configuration["Authentication:Google:ClientId"].ToString();
                    socialKeyDetailViewModel.ClientSecret = _configuration["Authentication:Google:ClientSecret"].ToString();
                }

                else if (Providername == "Twitter")
                {
                    socialKeyDetailViewModel.ProviderName = Providername;
                    socialKeyDetailViewModel.ClientId = _configuration["Authentication:Twitter:ConsumerKey"].ToString();
                    socialKeyDetailViewModel.ClientSecret = _configuration["Authentication:Twitter:ConsumerSecret"].ToString();

                }
                else if (Providername == "Microsoft")
                {
                    socialKeyDetailViewModel.ProviderName = Providername;
                    socialKeyDetailViewModel.ClientId = _configuration["Authentication:Microsoft:ClientId"].ToString();
                    socialKeyDetailViewModel.ClientSecret = _configuration["Authentication:Microsoft:ClientSecret"].ToString();

                }
                else
                {
                    return BadRequest(new SocialKeyDetailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.provideDetailNotAvailable, ErrorCode = enErrorCode.Status4100provideDetailNotAvailable });
                }
                return Ok(new SocialKeyDetailResponse { ReturnCode = enResponseCode.Success, socialKeyDetailViewModel = socialKeyDetailViewModel, ReturnMsg = EnResponseMessage.SocialLoginKey });


            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);

                return BadRequest(new SocialKeyDetailResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        #endregion
    }

}