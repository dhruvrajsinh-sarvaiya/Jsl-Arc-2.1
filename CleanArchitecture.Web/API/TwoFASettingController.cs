using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.ViewModels.AccountViewModels;
using CleanArchitecture.Core.ViewModels.AccountViewModels.SignUp;
using CleanArchitecture.Core.ViewModels.ManageViewModels;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Infrastructure.Interfaces;
using CleanArchitecture.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Web.API
{
    [Route("api/[controller]")]
    public class TwoFASettingController : BaseController
    {
        #region Field 
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;
        private readonly UrlEncoder _urlEncoder;
        private readonly IBasePage _basePage;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private const string AuthenicatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        #endregion

        #region Ctore
        public TwoFASettingController(
        UserManager<ApplicationUser> userManager,
        ILoggerFactory loggerFactory,
        UrlEncoder urlEncoder, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<ManageController>();
            _urlEncoder = urlEncoder;
            _signInManager = signInManager;
        }
        #endregion

        #region Method

        [HttpPost("twofactorauthentication")]
        public async Task<IActionResult> TwoFactorAuthentication()
        {
            var user = await GetCurrentUserAsync();

            var model = new TwoFactorAuthenticationViewModel
            {
                HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null,
                Is2faEnabled = user.TwoFactorEnabled,
                RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user),
            };

            return Ok(model);
        }

        //[HttpPost("Authentication2FA")]
        //public async Task<IActionResult> Authentication2FA(TwoFactorAuthViewModel model)
        //{

        //    if (model != null)
        //    {
        //        return Ok("Success");
        //    }
        //    else
        //    {
        //        ModelState.AddModelError(string.Empty, "Invalid");
        //        //  return BadRequest(new ApiError(ModelState));
        //    }

        //    var user = await GetCurrentUserAsync();


        //    var model1 = new TwoFactorAuthenticationViewModel
        //    {
        //        HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null,
        //        Is2faEnabled = user.TwoFactorEnabled,
        //        RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user),
        //    };

        //    return Ok(model);
        //}

        [HttpPost("disable2fa")]
        public async Task<IActionResult> Disable2fa()
        {


            var user = await GetCurrentUserAsync();
            var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
            if (disable2faResult.Succeeded)
            {
                user.TwoFactorEnabled = false;
                await _userManager.UpdateAsync(user);
                _logger.LogInformation("User with ID {UserId} has disabled 2fa.", user.Id);
                return Ok(new TwoFactorAuthResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.DisableTroFactor });

            }
            else
            {

                return BadRequest(new TwoFactorAuthResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.DisableTroFactorError, ErrorCode = enErrorCode.Status4042DisableTroFactorError });
            }





        }

        [HttpGet("enableauthenticator")]
        public async Task<IActionResult> EnableAuthenticator()
        {
            try
            {
                var user = await GetCurrentUserAsync();


                user.TwoFactorEnabled = true;
                await _userManager.UpdateAsync(user);

                //return Ok(new TwoFactorAuthResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.EnableTroFactor });


                var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
                if (string.IsNullOrEmpty(unformattedKey))
                {
                    await _userManager.ResetAuthenticatorKeyAsync(user);
                    unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
                }

                var model = new EnableAuthenticatorViewModel
                {
                    SharedKey = FormatKey(unformattedKey),
                    AuthenticatorUri = GenerateQrCodeUri(user.Email, unformattedKey)
                };
                return Ok(new EnableAuthenticationResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.EnableTroFactor, enableAuthenticatorViewModel = model });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new TwoFactorAuthResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        [HttpPost("enableauthenticator")]
        public async Task<IActionResult> EnableAuthenticator([FromBody]EnableAuthenticatorViewModel model)
        {
            try
            {
                var user = await GetCurrentUserAsync();

                // Strip spaces and hypens
                var verificationCode = model.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

                var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                    user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

                if (!is2faTokenValid)
                {
                    return BadRequest(new EnableAuthenticationResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.TwoFactorVerification });

                }

                await _userManager.SetTwoFactorEnabledAsync(user, true);
                _logger.LogInformation("User with ID {UserId} has enabled 2FA with an authenticator app.", user.Id);
                return Ok(new EnableAuthenticationResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.EnableTroFactor });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new TwoFactorAuthResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });

            }
        }

        //[HttpPost("resetauthenticator")]
        //public async Task<IActionResult> ResetAuthenticator()
        //{
        //    var user = await GetCurrentUserAsync();

        //    await _userManager.SetTwoFactorEnabledAsync(user, false);
        //    await _userManager.ResetAuthenticatorKeyAsync(user);
        //    _logger.LogInformation("User with id '{UserId}' has reset their authentication app key.", user.Id);

        //    return NoContent();
        //}

        //[HttpPost("generaterecoverycodes")]
        //public async Task<IActionResult> GenerateRecoveryCodes()
        //{
        //    var user = await GetCurrentUserAsync();

        //    if (!user.TwoFactorEnabled)
        //    {
        //        return BadRequest(new ApiError($"Cannot generate recovery codes for user with ID '{user.Id}' as they do not have 2FA enabled."));
        //    }

        //    var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
        //    var model = new GenerateRecoveryCodesViewModel { RecoveryCodes = recoveryCodes.ToArray() };

        //    _logger.LogInformation("User with ID {UserId} has generated new 2FA recovery codes.", user.Id);

        //    return Ok(model);
        //}
        //
        [HttpPost("VerifyCode")]
        public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            // The following code protects for brute force attacks against the two factor codes.
            // If a user enters incorrect codes for a specified amount of time then the user account
            // will be locked out for a specified amount of time.
            //  var result = await _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe, model.RememberBrowser);

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            var authenticatorCode = model.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, model.RememberMe, model.RememberBrowser);
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

        #region Utility 

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                AuthenicatorUriFormat,
                _urlEncoder.Encode("CleanArchitecture"),
                _urlEncoder.Encode(email),
                unformattedKey);
        }

        #endregion
    }
}