using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.ViewModels.AccountViewModels.SignUp;
using CleanArchitecture.Core.ViewModels.ManageViewModels;
using CleanArchitecture.Infrastructure;
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

        private const string AuthenicatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        #endregion

        #region Ctore
        public TwoFASettingController(
        UserManager<ApplicationUser> userManager,
        ILoggerFactory loggerFactory,
        UrlEncoder urlEncoder)
        {
            _userManager = userManager;          
            _logger = loggerFactory.CreateLogger<ManageController>();
            _urlEncoder = urlEncoder;
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

        [HttpPost("Authentication2FA")]
        public async Task<IActionResult> Authentication2FA(TwoFactorAuthViewModel model)
        {

            if (model != null)
            {
                SignUpMobileWithOTPResponse response = new SignUpMobileWithOTPResponse();
                response.ReturnCode = enResponseCode.Success;
                response.ReturnMsg = "Success";
                return Ok(response);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid");
                return BadRequest(new ApiError(ModelState));
            }

            //var user = await GetCurrentUserAsync();

            //var model = new TwoFactorAuthenticationViewModel
            //{
            //    HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null,
            //    Is2faEnabled = user.TwoFactorEnabled,
            //    RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user),
            //};

            //return Ok(model);
        }

        [HttpPost("disable2fa")]
        public async Task<IActionResult> Disable2fa()
        {
            var user = await GetCurrentUserAsync();

            var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
            if (!disable2faResult.Succeeded)
            {
                return BadRequest(new ApiError($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'."));
            }

            _logger.LogInformation("User with ID {UserId} has disabled 2fa.", user.Id);

            return NoContent();
        }

        [HttpGet("enableauthenticator")]
        public async Task<IActionResult> EnableAuthenticator()
        {
            var user = await GetCurrentUserAsync();

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

            return Ok(model);
        }

        [HttpPost("enableauthenticator")]
        public async Task<IActionResult> EnableAuthenticator([FromBody]EnableAuthenticatorViewModel model)
        {

            var user = await GetCurrentUserAsync();

            // Strip spaces and hypens
            var verificationCode = model.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (!is2faTokenValid)
            {
                ModelState.AddModelError("model.Code", "Verification code is invalid.");
                return BadRequest(new ApiError(ModelState));
            }

            await _userManager.SetTwoFactorEnabledAsync(user, true);
            _logger.LogInformation("User with ID {UserId} has enabled 2FA with an authenticator app.", user.Id);
            return NoContent();
        }

        [HttpPost("resetauthenticator")]
        public async Task<IActionResult> ResetAuthenticator()
        {
            var user = await GetCurrentUserAsync();

            await _userManager.SetTwoFactorEnabledAsync(user, false);
            await _userManager.ResetAuthenticatorKeyAsync(user);
            _logger.LogInformation("User with id '{UserId}' has reset their authentication app key.", user.Id);

            return NoContent();
        }

        [HttpPost("generaterecoverycodes")]
        public async Task<IActionResult> GenerateRecoveryCodes()
        {
            var user = await GetCurrentUserAsync();

            if (!user.TwoFactorEnabled)
            {
                return BadRequest(new ApiError($"Cannot generate recovery codes for user with ID '{user.Id}' as they do not have 2FA enabled."));
            }

            var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            var model = new GenerateRecoveryCodesViewModel { RecoveryCodes = recoveryCodes.ToArray() };

            _logger.LogInformation("User with ID {UserId} has generated new 2FA recovery codes.", user.Id);

            return Ok(model);
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