﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces;
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
using CleanArchitecture.Infrastructure.Interfaces;
using CleanArchitecture.Web.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
        #endregion

        #region Ctore
        public SigninController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILoggerFactory loggerFactory, IMediator mediator, IUserService userService, IOtpMasterService otpMasterService, Microsoft.Extensions.Configuration.IConfiguration configuration, IBasePage basePage,
            EncyptedDecrypted encypted)
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
                return BadRequest(new ApiError("Error"));
            }
            var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(user);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [HttpPost("SendCode")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> SendCode([FromBody]SendCodeViewModel model)
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

        //Social Login method direct call this method
        [HttpGet("ExternalLoginCallback")]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                //return RedirectToLocal(returnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl });
            }
            if (result.IsLockedOut)
            {
                return View("Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLoginConfirmation", new SocialLoginWithEmailViewModel { Email = email });
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
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [HttpPost("VerifyCode")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            // The following code protects for brute force attacks against the two factor codes.
            // If a user enters incorrect codes for a specified amount of time then the user account
            // will be locked out for a specified amount of time.
            var result = await _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe, model.RememberBrowser);
            if (result.Succeeded)
            {
                // return RedirectToLocal(model.ReturnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning(7, "User account locked out.");
                return View("Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid code.");
                return View(model);
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
                    return RedirectToAction(nameof(SendCode), new { RememberMe = model.RememberMe });
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
                //var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, lockoutOnFailure: false);
                var checkmail = await _userManager.FindByEmailAsync(model.Username);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Username);
                    var roles = await _userManager.GetRolesAsync(user);
                    _logger.LogInformation(1, "User logged in.");
                    return Ok(new StandardLoginResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.StandardLoginSuccess });
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction(nameof(SendCode), new { RememberMe = false });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "User account locked out for two hours.");

                    return BadRequest(new StandardLoginResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.StandardLoginLockOut, ErrorCode = enErrorCode.Status423Locked });
                }
                else
                {
                    if (checkmail != null)
                        await _userManager.AccessFailedAsync(checkmail);
                    //return BadRequest(new ApiError("Login failed : Invalid username or password."));
                    return BadRequest(new StandardLoginResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.StandardLoginfailed, ErrorCode = enErrorCode.Status400BadRequest });
                }
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
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var otpData = _otpMasterService.AddOtp(user.Id, user.Email, "");
                    if (otpData != null)
                    {
                        _logger.LogWarning(1, "User Login with Email Send Success.");
                        //return AppUtils.Standerdlogin("You have send OTP on email");
                        return Ok(new LoginWithEmailresponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.LoginWithEmailSuccessSend });
                    }
                    else
                    {
                        _logger.LogWarning(2, "User Otp Data Not Send.");
                        //return BadRequest(new ApiError("Invalid login attempt."));
                        return BadRequest(new LoginWithEmailresponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginWithOtpDatanotSend, ErrorCode = enErrorCode.Status400BadRequest });

                    }
                }
                else
                {
                    //return BadRequest(new ApiError("Login failed: Invalid email."));
                    return BadRequest(new LoginWithEmailresponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginWithOtpLoginFailed, ErrorCode = enErrorCode.Status400BadRequest });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                //return BadRequest();
                return BadRequest(new LoginWithEmailresponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
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
                //var logindata = await _userService.FindByEmail(model.Email);
                var checkmail = await _userManager.FindByEmailAsync(model.Email);
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
                                    _logger.LogWarning(1, "You are successfully login.");
                                    _otpMasterService.UpdateOtp(tempotp.Id);
                                    var roles = await _userManager.GetRolesAsync(checkmail);
                                    //  return AppUtils.SignIn(checkmail, roles);
                                    return Ok(new OTPWithEmailResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.CommSuccessMsgInternal });
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
                                            return BadRequest(new OTPWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginWithOtpInvalidAttempt, ErrorCode = enErrorCode.Status400BadRequest });
                                            // return BadRequest(new ApiError("Invalid login attempt."));
                                        }
                                        else
                                        {
                                            await _userManager.UpdateAsync(checkmail);
                                            return BadRequest(new OTPWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginWithOtpInvalidAttempt, ErrorCode = enErrorCode.Status400BadRequest });
                                        }
                                    }
                                    else
                                    {
                                        return BadRequest(new OTPWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginWithOtpInvalidAttempt, ErrorCode = enErrorCode.Status400BadRequest });
                                    }
                                }
                            }
                            else
                            {
                                //ModelState.AddModelError(string.Empty, "Resend OTP immediately not valid or expired.");
                                return BadRequest(new OTPWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpResendOTP, ErrorCode = enErrorCode.Status400BadRequest });
                            }
                        }
                        else
                        {
                            return BadRequest(new OTPWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status400BadRequest });
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Error.");
                        return BadRequest(new OTPWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginWithOtpLoginFailed, ErrorCode = enErrorCode.Status400BadRequest });
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
                        return BadRequest(new OTPWithEmailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.EmailFail, ErrorCode = enErrorCode.Status423Locked });
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
                            var otpData = await _otpMasterService.AddOtp(Convert.ToInt32(userdt.Id), userdt.Email, "");
                            if (otpData != null)
                            {
                                _logger.LogWarning(1, "User Login with Email OTP Send Success.");

                                return Ok(new LoginWithEmailresponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.LoginUserEmailOTP });

                            }
                            else
                            {
                                _logger.LogWarning(2, "User Otp Data Not Send.");
                                return BadRequest(new LoginWithEmailresponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginEmailOTPNotsend, ErrorCode = enErrorCode.Status400BadRequest });

                            }
                        }
                        else
                        {
                            return BadRequest(new LoginWithEmailresponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.EmailFail, ErrorCode = enErrorCode.Status400BadRequest });
                            //SendEmailRequest request = new SendEmailRequest();
                            //request.Recepient = userdt.Email;
                            //request.Subject = "Login With Email Otp";
                            //request.Body = "use this code:" + otpcheck.OTP + "";

                            //await _mediator.Send(request);
                            //_logger.LogWarning(1, "User Login with Email OTP Send Success.");

                            //return Ok("User Login with Email OTP Send Success.");
                        }
                    }
                    else
                    {
                        return BadRequest(new LoginWithEmailresponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status400BadRequest });
                    }
                }
                else
                {
                    return BadRequest(new LoginWithEmailresponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.StandardLoginfailed, ErrorCode = enErrorCode.Status400BadRequest });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new LoginWithEmailresponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
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
                var userdt = await _userService.FindByMobileNumber(model.Mobile);
                if (userdt != null)
                {
                    var otpData = _otpMasterService.AddOtp(Convert.ToInt32(userdt.Id), "", userdt.Mobile);
                    if (otpData != null)
                    {
                        //var roles = await _userManager.GetRolesAsync(userdt.Result);
                        _logger.LogWarning(1, "User Login with Mobile Send Success.");
                        return Ok(new LoginWithMobileResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.OTPSendOnMobile });
                        // return AppUtils.Standerdlogin("You have send OTP on mobile.");
                    }
                    else
                    {

                        _logger.LogWarning(2, "User Otp Data Not Send.");
                        return BadRequest(new LoginWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.OTPNotSendOnMobile, ErrorCode = enErrorCode.Status400BadRequest });
                    }
                }
                else
                {
                    return BadRequest(new LoginWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginMobileNumberInvalid, ErrorCode = enErrorCode.Status400BadRequest });
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
                var logindata = await _userService.FindByMobileNumber(model.Mobile);
                var result = await _userManager.FindByIdAsync(logindata?.Id.ToString());
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
                                    _logger.LogWarning(1, "You are successfully login.");
                                    _otpMasterService.UpdateOtp(tempotp.Id);
                                    var roles = await _userManager.GetRolesAsync(result);
                                    return Ok(new OTPWithMobileResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.CommSuccessMsgInternal });
                                }
                                if (result?.AccessFailedCount < Convert.ToInt16(_configuration["MaxFailedAttempts"]))
                                {
                                    result.AccessFailedCount = result.AccessFailedCount + 1;
                                    if (result.AccessFailedCount == Convert.ToInt16(_configuration["MaxFailedAttempts"]))
                                    {
                                        result.LockoutEnd = DateTime.UtcNow.AddHours(Convert.ToInt64(_configuration["DefaultLockoutTimeSpan"]));
                                        result.AccessFailedCount = 0;
                                        await _userManager.UpdateAsync(result);
                                        return BadRequest(new OTPWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginMobileNumberInvalid, ErrorCode = enErrorCode.Status400BadRequest });
                                    }
                                    else
                                    {
                                        await _userManager.UpdateAsync(result);
                                        return BadRequest(new OTPWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginWithOtpInvalidAttempt, ErrorCode = enErrorCode.Status423Locked });
                                    }
                                }
                                else
                                {
                                    return BadRequest(new OTPWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginWithOtpInvalidAttempt, ErrorCode = enErrorCode.Status423Locked });
                                }
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Resend OTP immediately not valid or expired.");
                                return BadRequest(new OTPWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginWithOtpInvalidAttempt, ErrorCode = enErrorCode.Status423Locked });
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Error.");
                        return BadRequest(new OTPWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginWithOtpInvalidAttempt, ErrorCode = enErrorCode.Status423Locked });
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
                        return BadRequest(new OTPWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginMobileNumberInvalid, ErrorCode = enErrorCode.Status423Locked });
                    }
                }
                ModelState.AddModelError(string.Empty, "Resend OTP immediately not valid or expired.");
                return BadRequest(new OTPWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.EmailFail, ErrorCode = enErrorCode.Status400BadRequest });
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
                            var otpData = await _otpMasterService.AddOtp(Convert.ToInt32(userdt.Id), "", userdt.Mobile);
                            if (otpData != null)
                            {
                                _logger.LogWarning(1, "User Login with Mobile OTP Send Success.");
                                return Ok(new LoginWithMobileResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.OTPSendOnMobile });
                            }
                            else
                            {
                                _logger.LogWarning(2, "User Otp Data Not Send.");
                                return BadRequest(new LoginWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.OTPNotSendOnMobile, ErrorCode = enErrorCode.Status400BadRequest });
                            }
                        }
                        else
                        {
                            return BadRequest(new LoginWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginMobileNumberInvalid, ErrorCode = enErrorCode.Status400BadRequest });

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
                        return BadRequest(new LoginWithEmailresponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status400BadRequest });
                    }
                }
                else
                {
                    return BadRequest(new LoginWithMobileResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.LoginMobileNumberInvalid, ErrorCode = enErrorCode.Status400BadRequest });
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

        ///// <summary>
        /////  This method are used social media using to login.
        ///// </summary>  
        //[HttpPost("SocialLogin")]
        //[AllowAnonymous]
        ////[ValidateAntiForgeryToken]
        //public IActionResult Sociallogin([FromBody] SocialLoginWithEmailViewModel model, string returnUrl = null)
        //{
        //    var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
        //    var properties = _signInManager.ConfigureExternalAuthenticationProperties(model.ProviderName, redirectUrl);
        //    return new ChallengeResult(model.ProviderName, properties);
        //}

        /// <summary>
        ///  This method are used social media using to login.
        /// </summary>  
        [HttpPost("SocialLogin")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Sociallogin(string ProviderName, string returnUrl = null)
        {
            try
            {
                var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
                var properties = _signInManager.ConfigureExternalAuthenticationProperties(ProviderName, redirectUrl);
                return new ChallengeResult(ProviderName, properties);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new SocialViewModel { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }


        #endregion

        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            try
            {
                var currentUser = await _userManager.FindByNameAsync(model.Email);

                if (currentUser == null || !(await _userManager.IsEmailConfirmedAsync(currentUser)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return NoContent();
                }
                string ctoken = _userManager.GeneratePasswordResetTokenAsync(currentUser).Result;
                Random generator = new Random();
                String random = generator.Next(0, 999999).ToString("D6");
                var ResetPassword = new ResetPasswordViewModel
                {
                    Email = currentUser.Email,
                    Code = ctoken,
                    Password = ctoken,
                    ConfirmPassword = ctoken,
                    Expirytime = DateTime.UtcNow + TimeSpan.FromHours(2)
                };
                byte[] passwordBytes = _encdecAEC.GetPasswordBytes(_configuration["AESSalt"].ToString());

                string UserDetails = JsonConvert.SerializeObject(ResetPassword);
                string SubScriptionKey = EncyptedDecrypted.Encrypt(UserDetails, passwordBytes);



                string ctokenlink = Url.Action("resetpassword", "Signin", new

                {
                    emailConfirmCode = SubScriptionKey
                }, protocol: HttpContext.Request.Scheme);

                var confirmationLink = "<a class='btn-primary' href=\"" + ctokenlink + "\">Please reset your password by clicking here:</a>";

                SendEmailRequest request = new SendEmailRequest();
                request.Recepient = model.Email;
                request.Subject = EnResponseMessage.SendMailSubject;
                request.Body = confirmationLink + " " +EnResponseMessage.SendMailBody + random;

                await _mediator.Send(request);

                return Ok(new ForgotpassWordResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.ResetConfirmed });
                // return AppUtils.Standerdlogin("Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new ForgotpassWordResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
            /*
            var currentUser = await _userManager.FindByNameAsync(model.Email);
            if (currentUser == null || !(await _userManager.IsEmailConfirmedAsync(currentUser)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return NoContent();
            }
            // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
            // Send an email with this link
            var code = await _userManager.GeneratePasswordResetTokenAsync(currentUser);

            //string ctoken = _userManager.GeneratePasswordResetTokenAsync(currentUser).Result;
            //string ctokenlink = Url.Action("ResetPassword", "Account", new
            //{
            //    Email = currentUser.Email,
            //    emailConfirmCode = ctoken
            //}, protocol: HttpContext.Request.Scheme);


            var host = Request.Scheme + "://" + Request.Host;
            var callbackUrl = host + "?userId=" + currentUser.Id + "&passwordResetCode=" + code;
            var confirmationLink = "<a class='btn-primary' href=\"" + callbackUrl + "\">Reset your password</a>";
            //var confirmationLink = "<a class='btn-primary' href=\"" + ctokenlink + "\">Reset your password</a>";
            await _emailSender.SendEmailAsync(model.Email, "Forgotten password email", confirmationLink);
            return NoContent(); // sends 204
            */
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

                    string DecryptToken = EncyptedDecrypted.Decrypt(emailConfirmCode, DecpasswordBytes);

                    ResetPasswordViewModel model = JsonConvert.DeserializeObject<ResetPasswordViewModel>(DecryptToken);


                    if (model?.Expirytime >= DateTime.UtcNow)
                    {
                        var user = await _userManager.FindByNameAsync(model.Email);

                        if (user == null)
                        {
                            // Don't reveal that the user does not exist
                            //return Ok("Reset confirmed");
                            return BadRequest(new ResetPassWordResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.ResetConfirmed, ErrorCode = enErrorCode.Status400BadRequest });
                            //return AppUtils.Standerdlogin("Reset confirmed");
                        }
                        var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                        if (result.Succeeded)
                        {
                            //return Ok("Reset confirmed");
                            return Ok(new ResetPassWordResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.ResetConfirmed });
                        }
                    }
                    else
                    {
                        return BadRequest(new ResetPassWordResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignEmailLink, ErrorCode = enErrorCode.Status400BadRequest });
                    }





                }
                else
                {
                    return BadRequest(new ResetPassWordResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignEmailLink, ErrorCode = enErrorCode.Status400BadRequest });
                }
                return BadRequest(new ResetPassWordResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.ResetConfirmed, ErrorCode = enErrorCode.Status400BadRequest });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new ResetPassWordResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        #region Logout        
        [HttpPost("logout")]
        public async Task<IActionResult> LogOff()
        {
            try
            {
                await _signInManager.SignOutAsync();
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

        #endregion
    }

}