using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.User;
using CleanArchitecture.Core.ViewModels.AccountViewModels;
using CleanArchitecture.Core.ViewModels.AccountViewModels.Login;
using CleanArchitecture.Core.ViewModels.AccountViewModels.OTP;
using CleanArchitecture.Core.ViewModels.AccountViewModels.SignUp;
using CleanArchitecture.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly IEmailSender _emailSender;
        private readonly IUserService _userdata;
        #endregion

        #region Ctore
        public SignUpController(UserManager<ApplicationUser> userManager, ILoggerFactory loggerFactory, IEmailSender emailSender, IUserService userdata)
        {
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<SignUpController>();
            _emailSender = emailSender;
            _userdata = userdata;
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
        public async Task<IActionResult> ConfirmEmail(string userId, string emailConfirmCode)
        {
            if (userId == null || emailConfirmCode == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }

            var result = await _userManager.ConfirmEmailAsync(user, emailConfirmCode);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }
        #endregion

        #region Methods

        #region Default register

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model, string returnUrl = null)
        {
            var currentUser = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.Firstname,
                LastName = model.Lastname,
                Mobile = model.Mobile
                //Balance = model.Balance
            };

            var result = await _userManager.CreateAsync(currentUser, model.Password);
            if (result.Succeeded)
            {

                //// ASP.NET Identity does not remember claim value types. So, if it was important that the office claim be an integer(rather than a string)
                var officeClaim = new Claim(OpenIdConnectConstants.Claims.PhoneNumber, currentUser.Mobile.ToString(), ClaimValueTypes.Integer);

                await _userManager.AddClaimAsync(currentUser, officeClaim);

                // Add to roles
                var roleAddResult = await _userManager.AddToRoleAsync(currentUser, "User");
                if (roleAddResult.Succeeded)
                {
                    string ctoken = _userManager.GenerateEmailConfirmationTokenAsync(currentUser).Result;
                    string ctokenlink = Url.Action("ConfirmEmail", "Account", new
                    {
                        userId = currentUser.Id,
                        emailConfirmCode = ctoken
                    }, protocol: HttpContext.Request.Scheme);

                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(currentUser);

                    //var host = Request.Scheme + "://" + Request.Host;
                    //var callbackUrl = host+ "/api/Account/ConfirmEmail" + "?userId=" + currentUser.Id + "&emailConfirmCode=" + code;
                    var confirmationLink = "<a class='btn-primary' href=\"" + ctokenlink + "\">Confirm email address</a>";
                    _logger.LogInformation(3, "User created a new account with password.");
                    await _emailSender.SendEmailAsync(model.Email, "Registration confirmation email", confirmationLink);
                    return Ok("User created a new account with password.");
                }
            }
            AddErrors(result);
            // If we got this far, something failed, redisplay form
            return BadRequest(new ApiError(ModelState));
        }

        #endregion

        #region DirectSignUpWithEmail

        /// <summary>
        ///  This method are Direct signUp with email using verified link
        /// </summary>        
        [HttpPost("DirectSignUpWithEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> DirectSignUpWithEmail([FromBody]SignUpWithEmailViewModel model)
        {
            var checkemail = await _userManager.FindByEmailAsync(model.Email);
            if (string.IsNullOrEmpty(checkemail?.Email))
            {
                var currentUser = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.Email
                };

                /*
                var result = await _userManager.CreateAsync(currentUser);
                if (result.Succeeded)
                {
                    // Add to roles
                    var roleAddResult = await _userManager.AddToRoleAsync(currentUser, "User");

                    if (roleAddResult.Succeeded)
                    {
                        string ctoken = _userManager.GenerateEmailConfirmationTokenAsync(currentUser).Result;
                        string ctokenlink = Url.Action("ConfirmEmail", "Account", new
                        {
                            userId = currentUser.Id,
                            emailConfirmCode = ctoken
                        }, protocol: HttpContext.Request.Scheme);

                        var confirmationLink = "<a class='btn-primary' href=\"" + ctokenlink + "\">Confirm email address</a>";
                        _logger.LogInformation(3, "User created a new account with password.");
                        await _emailSender.SendEmailAsync(model.Email, "Registration confirmation email", confirmationLink);

                        SignUpMobileWithOTPResponse response = new SignUpMobileWithOTPResponse();
                        response.ReturnCode = 200;
                        response.StatusMessage = "Success";
                        response.StatusCode = 200;
                        response.ReturnMsg = "Your account has been created, <br /> please verify it by clicking the activation link that has been send to your email.";
                                             
                        return Ok(response);
                    }
                }
                AddErrors(result);
                */

                SignUpMobileWithOTPResponse response = new SignUpMobileWithOTPResponse();
                response.ReturnCode = 200;
                response.StatusMessage = "Success";
                response.StatusCode = 200;
                response.ReturnMsg = "Your account has been created, <br /> please verify it by clicking the activation link that has been send to your email.";

                return Ok(response);
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

        #region DirectSignUpWithMobile

        /// <summary>
        ///  This method are Direct signUp with mobile sms using verified opt.
        /// </summary>        
        [HttpPost("DirectSignUpWithMobile")]
        [AllowAnonymous]
        public async Task<IActionResult> DirectSignUpWithMobile([FromBody]SignUpWithMobileViewModel model)
        {
            SignUpMobileWithOTPResponse response = new SignUpMobileWithOTPResponse();

            PhoneNumberUtil phoneUtil = PhoneNumberUtil.GetInstance();

            string countryCode = "IN";
            PhoneNumbers.PhoneNumber phoneNumber = phoneUtil.Parse(model.Mobile, countryCode);


            bool isValidNumber = phoneUtil.IsValidNumber(phoneNumber); // returns true for valid number    
            if (!isValidNumber)
            {
                response.ReturnCode = 200;
                response.ReturnMsg = "This mobile number is not Valid";
                response.ErrorCode = 401;
                response.StatusCode = 200;
                response.StatusMessage = "error";
                return Ok(response);
                //   return Ok("This mobile number is  Valid");
            }



            bool IsSignMobile = _userdata.GetMobileNumber(model.Mobile);
            if (IsSignMobile)
            {
                /*
                var currentUser = new ApplicationUser
                {
                    Mobile = model.Mobile,
                    UserName = model.Mobile,
                    OTP = _userdata.GenerateRandomOTP()
                };

                var result = await _userManager.CreateAsync(currentUser);
                if (result.Succeeded)
                {
                    var officeClaim = new Claim(OpenIdConnectConstants.Claims.PhoneNumber, currentUser.Mobile, ClaimValueTypes.Integer);
                    await _userManager.AddClaimAsync(currentUser, officeClaim);
                    // Add to roles
                    var roleAddResult = await _userManager.AddToRoleAsync(currentUser, "User");

                    if (roleAddResult.Succeeded)
                    {
                        //await _messageSender.SendSMSAsync(model.Mobile, "");
                        SignUpMobileWithOTPResponse response = new SignUpMobileWithOTPResponse();
                        response.ReturnCode = 200;
                        response.ReturnMsg = "Success";
                        response.StatusCode = 200;
                        response.StatusMessage = "Done";
                        return Ok(response);
                    }
                }
                AddErrors(result);
                */
                response.ReturnCode = 200;
                response.ReturnMsg = "Success";
                response.StatusCode = 200;
                response.StatusMessage = "Done";
                return Ok(response);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "This mobile number is already registered.");
                return BadRequest(new ApiError(ModelState));
            }
            // If we got this far, something failed, redisplay form
            return BadRequest(new ApiError(ModelState));
        }

        #endregion

        #region BlockChainSignUp

        [HttpPost("BlockChainSignUp")]
        [AllowAnonymous]
        public async Task<IActionResult> BlockChainSignUp([FromBody] BlockChainViewModel model)
        {
            BlockChainResponse response = new BlockChainResponse();
            response.ReturnCode = 200;
            response.ReturnMsg = "Success";
            response.StatusCode = 200;
            response.StatusMessage = "Done";

            return Ok(response);
        }

        #endregion

        #region SignUpOtpVerification

        /// <summary>
        ///  This method are Direct signUp with mobile sms using verified opt.
        /// </summary>        
        [HttpPost("DirectSignUpOtpVerification")]
        [AllowAnonymous]
        public async Task<IActionResult> DirectSignUpOtpVerification([FromBody]OTPViewModel model)
        {

            if (model != null)
            {
                SignUpMobileWithOTPResponse response = new SignUpMobileWithOTPResponse();
                response.ReturnCode = 200;
                response.ReturnMsg = "Success";
                response.StatusCode = 200;
                response.StatusMessage = "Done";
                return Ok(response);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid OTP.");
                return BadRequest(new ApiError(ModelState));
            }

        }

        /// <summary>
        ///  This method are Auto generate resend otp
        /// </summary>        
        [HttpPost("ReSendOtp")]
        [AllowAnonymous]
        public async Task<IActionResult> ReSendOtp()
        {
            SignUpMobileWithOTPResponse response = new SignUpMobileWithOTPResponse();
            response.ReturnCode = 200;
            response.ReturnMsg = "Success";
            response.StatusCode = 200;
            response.StatusMessage = "Done";
            return Ok(response);
        }

        #endregion


        #endregion
    }
}