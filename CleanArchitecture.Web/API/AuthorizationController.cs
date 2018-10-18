using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces.Session;
using CleanArchitecture.Core.Interfaces.User;
using CleanArchitecture.Core.Services;
using CleanArchitecture.Core.Services.RadisDatabase;
using CleanArchitecture.Core.Services.Session;
using CleanArchitecture.Core.ViewModels.AccountViewModels.Login;
using CleanArchitecture.Infrastructure.Services.User;
using CleanArchitecture.Web.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Core;

namespace CleanArchitecture.Web.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IOptions<IdentityOptions> _identityOptions;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;
        private readonly IUserSessionService _userSessionService;
        //private readonly IRedisConnectionFactory _fact;
        private readonly RedisConnectionFactory _fact;
        private readonly RedisSessionStorage _redisSessionStora;
        private readonly IOtpMasterService _otpMasterService;
        private readonly ICustomPassword _customPassword;

        public AuthorizationController(IOptions<IdentityOptions> identityOptions,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILoggerFactory loggerFactory,
            IUserSessionService userSessionService, RedisSessionStorage redisSessionStora,
            RedisConnectionFactory factory,
            //IRedisConnectionFactory factory,
            IUserService userService,
            IOtpMasterService otpMasterService, ICustomPassword customPassword)
        {
            _identityOptions = identityOptions;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<AuthorizationController>();
            _userSessionService = userSessionService;
            _redisSessionStora = redisSessionStora;
            _fact = factory;
            _otpMasterService = otpMasterService;
            _customPassword = customPassword;
        }

        [AllowAnonymous]
        //[ServiceFilter(typeof(ApiResultFilter))]
        [HttpPost("~/connect/token"), Produces("application/json")]
        public async Task<IActionResult> Exchange(OpenIdConnectRequest request, string appkey)
        {
            Debug.Assert(request.IsTokenRequest(),
                "The OpenIddict binder for ASP.NET Core MVC is not registered. " +
                "Make sure services.AddOpenIddict().AddMvcBinders() is correctly called.");

            ApplicationUser user;  /// Create Application user Instance For set the user using Redis session Or using Database
            string RedisDBKey = string.Empty;
            if (!string.IsNullOrEmpty(appkey) && !string.IsNullOrEmpty(request.Password)) /// added by nirav savariya for login with email and mobile on 16-10-2018
            {
                var userdata = await _userManager.FindByNameAsync(request.Username);
                var model = await _customPassword.IsValidPassword(appkey, request.Password);
                if (model != null)
                {
                    request.Password = model.Password;
                    var newPassword = _userManager.PasswordHasher.HashPassword(userdata, model.Password);
                    userdata.PasswordHash = newPassword;
                    var res = await _userManager.UpdateAsync(userdata);
                    _customPassword.UpdateOtp(model.Id);
                }
                else
                {
                    return BadRequest(new Customtokenresponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidAppkey, ErrorCode = enErrorCode.Status4048Invalidappkey });
                    //return BadRequest("Invalid appkey.");
                }
            }

            if (string.IsNullOrEmpty(appkey) && request.IsPasswordGrantType())
            {
                string numeric = string.Empty;
                foreach (char str in request.Password)
                {
                    if (char.IsDigit(str))
                    {
                        if (numeric.Length < 6)
                            numeric += str.ToString();   
                    }
                }
                if (numeric.Length == 6)
                    return BadRequest(new Customtokenresponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.Appkey, ErrorCode = enErrorCode.Status4049appkey });
            }

            if (request.IsPasswordGrantType())
            {
                var Userdata = new RedisUserdata(); ///  If not find the RadisDbKey then we Set key 
                var redis = new RadisServices<RedisUserdata>(this._fact);

                user = await _userManager.FindByNameAsync(request.Username);
                if (user == null)
                {
                    //return BadRequest("The username/password couple is invalid.");
                    return BadRequest(new Customtokenresponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidUser, ErrorCode = enErrorCode.Status4050InvalidUser });
                }
                else
                {
                    Userdata.RedisDBKey = Guid.NewGuid().ToString();
                    Userdata.RedisSessionKey = Guid.NewGuid().ToString();
                    RedisDBKey = Userdata.RedisDBKey;
                    redis.Save(Userdata.RedisDBKey, Userdata);
                    _redisSessionStora.SetObject(Userdata.RedisSessionKey, user, RedisDBKey);
                }

                // Validate the username/password parameters and ensure the account is not locked out.
                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
                if (!result.Succeeded)
                {
                    return BadRequest(new Customtokenresponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidUser, ErrorCode = enErrorCode.Status4050InvalidUser });
                    //return BadRequest("The username/password couple is invalid.");
                }
                // Create a new authentication ticket.
                var ticket = await CreateTicketAsync(request, user, null, RedisDBKey);
                if (!string.IsNullOrEmpty(appkey))
                {
                    var setpwd = _userManager.PasswordHasher.HashPassword(user, DateTime.UtcNow.ToString());
                    user.PasswordHash = setpwd;
                    var res = await _userManager.UpdateAsync(user);
                }
                //return Ok(new Customtokenresponse { ReturnCode = enResponseCode.Success, ReturnMsg = "Success", SignIntoken = SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme)});

                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }

            else if (request.IsRefreshTokenGrantType())
            {
                // Retrieve the claims principal stored in the refresh token.
                var info = await HttpContext.AuthenticateAsync(OpenIdConnectServerDefaults.AuthenticationScheme);

                // Retrieve the user profile corresponding to the refresh token.
                // Note: if you want to automatically invalidate the refresh token
                // when the user password/roles change, use the following line instead:
                // var user = _signInManager.ValidateSecurityStampAsync(info.Principal);
                user = await _userManager.GetUserAsync(info.Principal);
                if (user == null)
                {
                    //return BadRequest(new OpenIdConnectResponse
                    //{
                    //    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    //    ErrorDescription = "The refresh token is no longer valid."
                    //});
                    return BadRequest(new Customtokenresponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.RefreshToken, ErrorCode = enErrorCode.Status4051RefreshToken });
                }

                // Ensure the user is still allowed to sign in.
                if (!await _signInManager.CanSignInAsync(user))
                {
                    //return BadRequest(new OpenIdConnectResponse
                    //{
                    //    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    //    ErrorDescription = "The user is no longer allowed to sign in."
                    //});
                    return BadRequest(new Customtokenresponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.UserToken, ErrorCode = enErrorCode.Status4052UserToken });
                }

                // Create a new authentication ticket, but reuse the properties stored
                // in the refresh token, including the scopes originally granted.
                var ticket = await CreateTicketAsync(request, user, info.Properties);

                //return Ok(new Customtokenresponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.LoginUserEmailOTP, SignIntoken = SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme)});

                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }

            //return BadRequest(new OpenIdConnectResponse
            //{
            //    Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
            //    ErrorDescription = "The specified grant type is not supported."
            //});
            return BadRequest(new Customtokenresponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.Granttype, ErrorCode = enErrorCode.Status4053Granttype });
        }


        [AllowAnonymous]
        [HttpGet("~/connect/authorize")]
        public async Task<IActionResult> Authorize(OpenIdConnectRequest request)
        {

            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                // If an identity provider was explicitly specified, redirect
                // the user agent to the AccountController.ExternalLogin action.
                var provider = (string)request["provider"];

                if (!string.IsNullOrEmpty(provider))
                {
                    // Request a redirect to the external login provider.
                    var returnUrl = Request.PathBase + Request.Path + Request.QueryString;
                    var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, returnUrl);
                    return Challenge(properties, provider);
                }

                return Render(ExternalLoginStatus.Error);
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

            if (result.Succeeded)
            {
                // Retrieve the profile of the logged in user.
                var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

                if (user == null)
                {
                    return Render(ExternalLoginStatus.Error);
                }

                _logger.LogInformation(5, $"User logged in with ${info.LoginProvider} provider.");
                var ticket = await CreateTicketAsync(request, user);
                // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }
            else
            {
                var email = (string)request["email"];
                if (!string.IsNullOrEmpty(email))
                {
                    var user = new ApplicationUser { UserName = email, Email = email };
                    var accountCreateResult = await _userManager.CreateAsync(user);
                    if (accountCreateResult.Succeeded)
                    {
                        accountCreateResult = await _userManager.AddLoginAsync(user, info);
                        if (accountCreateResult.Succeeded)
                        {
                            _logger.LogInformation(6, $"User created an account using ${info.LoginProvider} provider.");
                            var ticket = await CreateTicketAsync(request, user);
                            // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
                            return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
                        }
                    }
                    else
                    {
                        return BadRequest(new ApiError("Email already exists"));
                    }
                }
                else
                {
                    // External account doesn't have a local account so ask to create one
                    return Render(ExternalLoginStatus.CreateAccount);
                }

            }


            return Render(ExternalLoginStatus.Error);

        }


        private async Task<AuthenticationTicket> CreateTicketAsync(OpenIdConnectRequest request, ApplicationUser user,
            AuthenticationProperties properties = null, string token = "")
        {
            // Create a new ClaimsPrincipal containing the claims that
            // will be used to create an id_token, a token or a code.
            var principal = await _signInManager.CreateUserPrincipalAsync(user);

            // Create a new authentication ticket holding the user identity.
            var ticket = new AuthenticationTicket(principal, properties,
                OpenIdConnectServerDefaults.AuthenticationScheme);

            if (!request.IsRefreshTokenGrantType())
            {
                // Set the list of scopes granted to the client application.
                // Note: the offline_access scope must be granted
                // to allow OpenIddict to return a refresh token.
                ticket.SetScopes(new[]
                {
                    OpenIdConnectConstants.Scopes.OpenId,
                    OpenIdConnectConstants.Scopes.Email,
                    OpenIdConnectConstants.Scopes.Profile, // "name"
                    OpenIdConnectConstants.Scopes.OfflineAccess,
                    OpenIdConnectConstants.Scopes.Phone,
                    OpenIddictConstants.Scopes.Roles,                  
                    //"Read",
                    //"Balance",

                    //"UserId",
                    //"Address",
                    
                }.Intersect(request.GetScopes()));
            }

            if (!string.IsNullOrEmpty(token))///send the redisstorage token to frant using Resources parameter.
            {
                ticket.SetResources(token);
            }
            else
                ticket.SetResources("resource_server");
            // Note: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.

            foreach (var claim in ticket.Principal.Claims)
            {
                // Never include the security stamp in the access and identity tokens, as it's a secret value.
                if (claim.Type == _identityOptions.Value.ClaimsIdentity.SecurityStampClaimType)
                {
                    continue;
                }

                var destinations = new List<string>
                {
                    OpenIdConnectConstants.Destinations.AccessToken
                };

                // Only add the iterated claim to the id_token if the corresponding scope was granted to the client application.
                // The other claims will only be added to the access_token, which is encrypted when using the default format.
                if ((claim.Type == OpenIdConnectConstants.Claims.Name && ticket.HasScope(OpenIdConnectConstants.Scopes.Profile)) ||
                    (claim.Type == OpenIdConnectConstants.Claims.Email && ticket.HasScope(OpenIdConnectConstants.Scopes.Email)) ||
                    (claim.Type == OpenIdConnectConstants.Claims.PhoneNumber && ticket.HasScope(OpenIdConnectConstants.Scopes.Phone)) ||
                    (claim.Type == OpenIdConnectConstants.Claims.Role && ticket.HasScope(OpenIddictConstants.Claims.Roles))) // ||
                                                                                                                             //(claim.Type == "Read" && ticket.HasScope("Read")) || (claim.Type == "Balance" && ticket.HasScope("Balance")))
                {
                    destinations.Add(OpenIdConnectConstants.Destinations.IdentityToken);
                }

                claim.SetDestinations(destinations);
            }

            return ticket;
        }

        private IActionResult Render(ExternalLoginStatus status = ExternalLoginStatus.Ok)
        {
            if (status == ExternalLoginStatus.Ok)
            {
                return LocalRedirect("~/");
            }
            return LocalRedirect($"~/?externalLoginStatus={(int)status}");
        }
    }
}