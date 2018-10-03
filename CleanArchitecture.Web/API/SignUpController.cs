using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using CleanArchitecture.Core.Entities.User;
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
        private readonly string _AESSalt = "rz8LuOtFBXphj9WQfvFh";
        private readonly ITempUserRegisterService _tempUserRegisterService;


        #endregion

        #region Ctore
        public SignUpController(UserManager<ApplicationUser> userManager, ILoggerFactory loggerFactory, IEmailSender emailSender, IUserService userdata, ITempUserRegisterService tempUserRegisterService)
        {
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<SignUpController>();
            _userdata = userdata;
            _tempUserRegisterService = tempUserRegisterService;
            _mediator = mediator;
            _encdecAEC = encdecAEC;
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
                byte[] DecpasswordBytes = _encdecAEC.GetPasswordBytes(_AESSalt);

                string DecryptToken = EncyptedDecrypted.Decrypt(emailConfirmCode, DecpasswordBytes);

                LinkTokenViewModel dmodel = JsonConvert.DeserializeObject<LinkTokenViewModel>(DecryptToken);
                if (dmodel?.Expirytime >= DateTime.UtcNow)
                {
                    if (string.IsNullOrEmpty(dmodel?.Id))
                    {
                        //return View("Error");
                        ModelState.AddModelError(string.Empty, "Error.");
                        return BadRequest(new ApiError(ModelState));
                    }
                    var user = await _userManager.FindByIdAsync(dmodel.Id);
                    if (user == null)
                    {
                        //return View("Error");
                        ModelState.AddModelError(string.Empty, "Error");
                        return BadRequest(new ApiError(ModelState));
                    }
                    else if (!user.EmailConfirmed)
                    {
                        user.EmailConfirmed = true;
                        var result = await _userManager.UpdateAsync(user); 
                        //var result = await _userManager.ConfirmEmailAsync(user, emailConfirmCode);
                        return View(result.Succeeded ? "ConfirmEmail" : "Error");
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
                ModelState.AddModelError(string.Empty, "Reset links immediately not valid or expired.");
                return BadRequest(new ApiError(ModelState));
            }
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
                    LinkTokenViewModel linkToken = new LinkTokenViewModel();

                    linkToken.Id = currentUser.Id.ToString();
                    linkToken.Username = model.Email;
                    linkToken.Email = model.Email;
                    linkToken.Firstname = model.Firstname;
                    linkToken.Lastname = model.Lastname;
                    linkToken.Mobile = model.Mobile;
                    linkToken.CurrentTime = DateTime.UtcNow;
                    linkToken.Expirytime = DateTime.UtcNow + TimeSpan.FromHours(2);


                    byte[] passwordBytes = _encdecAEC.GetPasswordBytes(_AESSalt);

                    string UserDetails = JsonConvert.SerializeObject(linkToken);

                    string SubScriptionKey = EncyptedDecrypted.Encrypt(UserDetails, passwordBytes);

                    //string ctoken = _userManager.GenerateEmailConfirmationTokenAsync(currentUser).Result;
                    string ctokenlink = Url.Action("ConfirmEmail", "SignUp", new
                    {
                        //userId = currentUser.Id,
                        emailConfirmCode = SubScriptionKey
                    }, protocol: HttpContext.Request.Scheme);

                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(currentUser);

                    //var host = Request.Scheme + "://" + Request.Host;
                    //var callbackUrl = host+ "/api/Account/ConfirmEmail" + "?userId=" + currentUser.Id + "&emailConfirmCode=" + code;
                    var confirmationLink = "<a class='btn-primary' href=\"" + ctokenlink + "\">Confirm email address</a>";

                    SendEmailRequest request = new SendEmailRequest();
                    request.Recepient = model.Email;
                    request.Subject = "Registration confirmation email";
                    request.Body = confirmationLink;

                    CommunicationResponse CommResponse = await _mediator.Send(request);
                    _logger.LogInformation(3, "User created a new account with password.");

                    //await _mediator.Publish(new EmailHandler())
                    //await _emailSender.SendEmailAsync(model.Email, "Registration confirmation email", confirmationLink);



                    RegisterResponse response = new RegisterResponse();
                    response.ReturnCode = 200;
                    response.StatusMessage = "Success";
                    response.StatusCode = 200;
                    response.ReturnMsg = "Your account has been created, <br /> please verify it by clicking the activation link that has been send to your email.";
                    return Ok(response);

                    ////await _emailSender.SendEmailAsync(model.Email, "Registration confirmation email", confirmationLink);
                    //return Ok("User created a new account with password.");

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
                    UserName = model.Email,
                    PasswordHash = model.Password
                };

                var result = await _userManager.CreateAsync(currentUser,model.Password);
                if (result.Succeeded)
                {
                    // Add to roles
                    var roleAddResult = await _userManager.AddToRoleAsync(currentUser, "User");

                    if (roleAddResult.Succeeded)
                    {
                        LinkTokenViewModel linkToken = new LinkTokenViewModel();

                        linkToken.Id = currentUser.Id.ToString();
                        linkToken.Username = model.Email;
                        linkToken.Email = model.Email;
                        linkToken.CurrentTime = DateTime.UtcNow;
                        linkToken.Expirytime = DateTime.UtcNow + TimeSpan.FromHours(2);

                        byte[] passwordBytes = _encdecAEC.GetPasswordBytes(_AESSalt);
                        string UserDetails = JsonConvert.SerializeObject(linkToken);
                        string SubScriptionKey = EncyptedDecrypted.Encrypt(UserDetails, passwordBytes);

                        //string ctoken = _userManager.GenerateEmailConfirmationTokenAsync(currentUser).Result;
                        string ctokenlink = Url.Action("ConfirmEmail", "SignUp", new
                        {
                            //userId = currentUser.Id,
                            emailConfirmCode = SubScriptionKey
                        }, protocol: HttpContext.Request.Scheme);

                        var confirmationLink = "<a class='btn-primary' href=\"" + ctokenlink + "\">Confirm email address</a>";
                        _logger.LogInformation(3, "User created a new account with password.");

                        SendEmailRequest request = new SendEmailRequest();
                        request.Recepient = model.Email;
                        request.Subject = "Registration confirmation email";
                        request.Body = confirmationLink;

                        CommunicationResponse CommResponse = await _mediator.Send(request);

                        //await _emailSender.SendEmailAsync(model.Email, "Registration confirmation email", confirmationLink);

                        SignUpWithEmailResponse response = new SignUpWithEmailResponse();
                        response.ReturnCode = 200;
                        response.StatusMessage = "Success";
                        response.StatusCode = 200;
                        response.ReturnMsg = "Your account has been created, <br /> please verify it by clicking the activation link that has been send to your email.";

                        return Ok(response);
                    }
                }
                AddErrors(result);
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
                bool IsSignTempMobile = _tempUserRegisterService.GetMobileNumber(model.Mobile);
                if (IsSignTempMobile)
                {
                   var result = await _tempUserRegisterService.AddTempRegister(model);

                    if(result != null )
                    {
                        response.ReturnCode = 200;
                        response.ReturnMsg = "Success";
                        response.StatusCode = 200;
                        response.StatusMessage = "Done";
                        return Ok(response);
                    }
                    //var currentUser = new TempUserRegister
                    //{
                    //    Mobile = model.Mobile,
                    //    UserName = model.Mobile,
                     //  TempOtpMaster. OTP = _userdata.GenerateRandomOTP()
                    //};

                    /*

                    //var result = await _userManager.CreateAsync(currentUser);
                    if (result.Succeeded)
                    {
                        var officeClaim = new Claim(OpenIdConnectConstants.Claims.PhoneNumber, currentUser.Mobile, ClaimValueTypes.Integer);
                        await _userManager.AddClaimAsync(currentUser, officeClaim);
                        ////// Add to roles
                        ////var roleAddResult = await _userManager.AddToRoleAsync(currentUser, "User");

                        //if (roleAddResult.Succeeded)
                        //{
                        //await _messageSender.SendSMSAsync(model.Mobile, "");
                        // SignUpMobileWithOTPResponse response = new SignUpMobileWithOTPResponse();
                        response.ReturnCode = 200;
                        response.ReturnMsg = "Success";
                        response.StatusCode = 200;
                        response.StatusMessage = "Done";
                        return Ok(response);
                        //}
                    }
                    */
                }
                //AddErrors(result);

                //response.ReturnCode = 200;
                //response.ReturnMsg = "Success";
                //response.StatusCode = 200;
                //response.StatusMessage = "Done";
                //return Ok(response);
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
                OTPResponse response = new OTPResponse();
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