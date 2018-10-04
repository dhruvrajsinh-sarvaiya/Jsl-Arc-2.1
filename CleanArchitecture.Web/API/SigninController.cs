using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.ViewModels;
using CleanArchitecture.Core.ViewModels.AccountViewModels;
using CleanArchitecture.Core.ViewModels.AccountViewModels.ForgotPassword;
using CleanArchitecture.Core.ViewModels.AccountViewModels.Login;
using CleanArchitecture.Core.ViewModels.AccountViewModels.ResetPassword;
using CleanArchitecture.Web.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

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
        #endregion

        #region Ctore
        public SigninController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILoggerFactory loggerFactory, IMediator mediator)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<SigninController>();
            _mediator = mediator;
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
                return BadRequest(new ApiError("Error"));
            }

            // Generate the token and send it
            var code = await _userManager.GenerateTwoFactorTokenAsync(user, model.SelectedProvider);
            if (string.IsNullOrWhiteSpace(code))
            {
                return BadRequest(new ApiError("Error"));
            }

            var message = "Your security code is: " + code;
            if (model.SelectedProvider == "Email")
            {
                SendEmailRequest request = new SendEmailRequest();
                request.Recepient = user.Email;
                request.Subject = "Security Code";
                request.Body = message;

                CommunicationResponse Response = await _mediator.Send(request);
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
                return BadRequest(new ApiError("Error"));
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
        public async Task<IActionResult> Login([FromBody]Core.ViewModels.AccountViewModels.LoginViewModel model)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var roles = await _userManager.GetRolesAsync(user);
                _logger.LogInformation(1, "User logged in.");
                return AppUtils.SignIn(user, roles);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToAction(nameof(SendCode), new { RememberMe = model.RememberMe });
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning(2, "User account locked out.");
                return BadRequest(new ApiError("Lockout"));
            }
            else
            {
                return BadRequest(new ApiError("Invalid login attempt."));
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
        public async Task<IActionResult> StandardLogin([FromBody]StandardLoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                var roles = await _userManager.GetRolesAsync(user);
                _logger.LogInformation(1, "User logged in.");

                StandardLoginResponse response = new StandardLoginResponse();
                response.ReturnCode = 200;
                response.ReturnMsg = "Success";
                response.StatusCode = 200;
                response.StatusMessage = "Success";

                return AppUtils.StanderdSignIn(user, response);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToAction(nameof(SendCode), new { RememberMe = model.RememberMe });
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning(2, "User account locked out.");
                return BadRequest(new ApiError("Lockout"));
            }
            else
            {
                return BadRequest(new ApiError("Invalid login attempt."));
            }


            //StandardLoginResponse response = new StandardLoginResponse();
            //response.ReturnCode = 200;
            //response.ReturnMsg = "Success";
            //response.StatusCode = 200;
            //response.StatusMessage = "Success";
            //return Ok(response);
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
        public async Task<IActionResult> LoginWithEmail([FromBody]LoginWithEmailViewModel model)
        {


            LoginWithEmailResponse response = new LoginWithEmailResponse();
            response.ReturnCode = enResponseCode.Success;
            response.ReturnMsg = "Success";
            return Ok(response);
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
        public async Task<IActionResult> LoginWithMobile([FromBody]LoginWithMobileViewModel model)
        {
            //var result = await _signInManager.PasswordSignInAsync(model.Mobile, model.Password, model.RememberMe, lockoutOnFailure: false);
            //if (result.Succeeded)
            //{
            //    var user = await _userManager.FindByEmailAsync(model.Mobile);
            //    var roles = await _userManager.GetRolesAsync(user);
            //    _logger.LogInformation(1, "User logged in.");

            //    StandardLoginResponse response = new StandardLoginResponse();
            //    response.ReturnCode = 200;
            //    response.ReturnMsg = "Success";
            //    response.StatusCode = 200;
            //    response.StatusMessage = "Success";

            //    return AppUtils.StanderdSignIn(user, response);
            //}
            //if (result.RequiresTwoFactor)
            //{
            //    return RedirectToAction(nameof(SendCode), new { RememberMe = model.RememberMe });
            //}
            //if (result.IsLockedOut)
            //{
            //    _logger.LogWarning(2, "User account locked out.");
            //    return BadRequest(new ApiError("Lockout"));
            //}
            //else
            //{
            //    return BadRequest(new ApiError("Invalid login attempt."));
            //}

            LoginWithMobileResponse response = new LoginWithMobileResponse();
            response.ReturnCode = enResponseCode.Success;
            response.ReturnMsg = "Success";
            return Ok(response);
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
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(ProviderName, redirectUrl);
            return new ChallengeResult(ProviderName, properties);
        }


        #endregion

        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody]ForgotPasswordViewModel model)
        {

            ForgotPasswordResponse response = new ForgotPasswordResponse();
            response.ReturnCode = enResponseCode.Success;
            response.ReturnMsg = "Success";
            return Ok(response);

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

        [HttpPost("resetpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);

            if (user == null)
            {
                // Don't reveal that the user does not exist
                return Ok("Reset confirmed");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return Ok("Reset confirmed"); ;
            }
            AddErrors(result);
            return BadRequest(new ApiError(ModelState));
        }

        #region Logout        
        [HttpPost("logout")]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return NoContent();
        }
        #endregion

        #endregion
    }

}