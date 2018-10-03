using System.Linq;
using System.Threading.Tasks;
//using AspNetCoreSpa.Core.ViewModels.ManageViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Encodings.Web;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.ViewModels.ManageViewModels;
using CleanArchitecture.Web.Filters;
using CleanArchitecture.Web.Extensions;
using CleanArchitecture.Core.ViewModels.AccountViewModels;
using CleanArchitecture.Core.ViewModels.AccountViewModels.SignUp;
using CleanArchitecture.Core.Entities.User;

namespace CleanArchitecture.Web.API
{
    [Route("api/[controller]")]
    public class ManageController : BaseController
    {
        #region Field 

        private readonly CleanArchitectureContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly UrlEncoder _urlEncoder;

        #endregion

        #region Ctore
        public ManageController(
        CleanArchitectureContext context,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        ILoggerFactory loggerFactory,
        UrlEncoder urlEncoder)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = loggerFactory.CreateLogger<ManageController>();
            _urlEncoder = urlEncoder;
        }
        #endregion

        #region Method

        [HttpGet("userinfo")]
        public async Task<IActionResult> UserInfo()
        {
            var user = await GetCurrentUserAsync();

            return Ok(new IndexViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                IsEmailConfirmed = user.EmailConfirmed,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            });
        }

        [HttpPost("userinfo")]
        public async Task<IActionResult> UserInfo([FromBody]IndexViewModel model)
        {

            var user = await GetCurrentUserAsync();

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserName = string.IsNullOrEmpty(model.Username) ? user.UserName : model.Username;
            user.Email = string.IsNullOrEmpty(model.Email) ? user.Email : model.Email;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result == IdentityResult.Success)
            {
                return Ok(new IndexViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.UserName,
                    IsEmailConfirmed = user.EmailConfirmed,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                });
            }

            return BadRequest(new ApiError("Unable to update user info"));
        }

        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordViewModel model)
        {
            var user = await GetCurrentUserAsync();
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                _logger.LogInformation(3, "User changed their password successfully.");
                return NoContent();
            }
            return BadRequest(new ApiError("Unable to change password"));
        }

        [HttpPost("setpassword")]
        public async Task<IActionResult> SetPassword([FromBody]SetPasswordViewModel model)
        {

            var user = await GetCurrentUserAsync();
            var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest(new ApiError("Unable to set password"));
        }

        [HttpGet("photo")]
        public IActionResult UserPhoto()
        {

            var profileImage = _context.ApplicationUserPhotos.Include(i => i.ApplicationUser).FirstOrDefault(i => i.ApplicationUser.Id == User.GetUserId());

            if (profileImage == null)
            {
                return NotFound();
            }

            return Ok(Convert.ToBase64String(profileImage.Content));
        }

        [HttpPost("photo")]
        public async Task<IActionResult> UserPhoto(IFormFile file)
        {
            {
                if (string.IsNullOrEmpty(file?.ContentType) || (file.Length == 0)) return BadRequest(new ApiError("Image provided is invalid"));

                var size = file.Length;

                if (size > Convert.ToInt64(Startup.Configuration["MaxImageUploadSize"])) return BadRequest(new ApiError("Image size greater than allowed size"));

                using (var memoryStream = new MemoryStream())
                {
                    var existingImage = _context.ApplicationUserPhotos.FirstOrDefault(i => i.ApplicationUserId == User.GetUserId());

                    await file.CopyToAsync(memoryStream);

                    if (existingImage == null)
                    {
                        var userImage = new ApplicationUserPhotos
                        {
                            ContentType = file.ContentType,
                            Content = memoryStream.ToArray(),
                            ApplicationUserId = User.GetUserId()
                        };
                        _context.ApplicationUserPhotos.Add(userImage);
                    }
                    else
                    {
                        existingImage.ContentType = file.ContentType;
                        existingImage.Content = memoryStream.ToArray();
                        _context.ApplicationUserPhotos.Update(existingImage);
                    }
                    await _context.SaveChangesAsync();
                }

                return NoContent();
            }
        }

        [HttpGet("getlogins")]
        public async Task<IActionResult> GetLogins()
        {
            var user = await GetCurrentUserAsync();
            return Ok(await _userManager.GetLoginsAsync(user));
        }

        [HttpPost("removelogin")]
        public async Task<IActionResult> RemoveLogin([FromBody]RemoveLoginViewModel account)
        {
            var user = await GetCurrentUserAsync();
            var result = await _userManager.RemoveLoginAsync(user, account.LoginProvider, account.ProviderKey);
            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest(new ApiError("Login cannot be removed"));
        }

        [HttpGet("managelogins")]
        public async Task<IActionResult> ManageLogins()
        {
            var user = await GetCurrentUserAsync();
            var userLogins = await _userManager.GetLoginsAsync(user);
            var schemes = await _signInManager.GetExternalAuthenticationSchemesAsync();
            var otherLogins = schemes.Where(auth => userLogins.All(ul => auth.Name != ul.LoginProvider)).ToList();
            // ViewData["ShowRemoveButton"] = user.PasswordHash != null || userLogins.Count > 1;
            return Ok(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        [HttpPost("linklogin")]
        public IActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = Url.Action("LinkLoginCallback", "Manage");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
            return Challenge(properties, provider);
        }

        [HttpGet("linklogincallback")]
        public async Task<ActionResult> LinkLoginCallback()
        {
            var user = await GetCurrentUserAsync();
            var info = await _signInManager.GetExternalLoginInfoAsync(await _userManager.GetUserIdAsync(user));
            if (info == null)
            {
                return BadRequest(new ApiError("Unable to find linked login info"));
            }
            var result = await _userManager.AddLoginAsync(user, info);
            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest(new ApiError("Unable to link login"));

        }

        #endregion

        #region Helpers

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

        //private string GenerateQrCodeUri(string email, string unformattedKey)
        //{
        //    return string.Format(
        //        AuthenicatorUriFormat,
        //        _urlEncoder.Encode("AspNetCoreSpa"),
        //        _urlEncoder.Encode(email),
        //        unformattedKey);
        //}

        #endregion
    }
}