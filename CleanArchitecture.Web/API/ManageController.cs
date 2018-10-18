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
using Microsoft.AspNetCore.Identity.UI.Services;
using CleanArchitecture.Core.Services.RadisDatabase;
using CleanArchitecture.Core.Services.Session;
using CleanArchitecture.Core.Interfaces.Log;
using CleanArchitecture.Core.Interfaces.User;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Infrastructure.Interfaces;
using CleanArchitecture.Core.ViewModels.AccountViewModels.Log;

namespace CleanArchitecture.Web.API
{
    [Route("api/[controller]")]
    public class ManageController : BaseController
    {
        #region Field 

        private readonly CleanArchitectureContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly UrlEncoder _urlEncoder;
        private readonly IRedisConnectionFactory _fact;
        private readonly RedisSessionStorage _redisSessionStorage;
        private readonly IUserService _userdata;
        private readonly IipAddressService _ipAddressService;
        private readonly IDeviceIdService _iDeviceIdService;
        private readonly IBasePage _basePage;
        #endregion

        #region Ctore
        public ManageController(
        CleanArchitectureContext context,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILoggerFactory loggerFactory,
        UrlEncoder urlEncoder, IRedisConnectionFactory factory,
        RedisSessionStorage redisSessionStorage,
        IUserService userdata,
        IipAddressService ipAddressService,
         IBasePage basePage,
         IDeviceIdService iDeviceIdService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            //_emailSender = emailSender;
            _logger = loggerFactory.CreateLogger<ManageController>();
            _urlEncoder = urlEncoder;
            _fact = factory;
            _redisSessionStorage = redisSessionStorage;
            _userdata = userdata;
            _ipAddressService = ipAddressService;
            _basePage = basePage;
            _iDeviceIdService = iDeviceIdService;
        }
        #endregion

        #region Method

        [HttpGet("userinfo")]
        public async Task<IActionResult> UserInfo() //[FromHeader] string RedisDBKey)
        {


            /*
            var Userdata = new RedisUserdata(); ///  If not find the RadisDbKey then we Set key 
            var redis = new RadisServices<RedisUserdata>(this._fact);

             ApplicationUser user = null;

            //// Perform Get Or set the redis Process operation

            if (!string.IsNullOrEmpty(RedisDBKey))
            {
                Userdata = redis.Get(RedisDBKey);

                if (Userdata != null && !string.IsNullOrEmpty(Userdata.RedisDBKey) && !string.IsNullOrEmpty(Userdata.RedisSessionKey))
                {
                    RedisDBKey = Userdata.RedisDBKey;
                    user = _redisSessionStorage.GetObjectFromJson<ApplicationUser>(Userdata.RedisSessionKey, Userdata.RedisDBKey);
                }

            }
            if (user == null)
            {
                user = await GetCurrentUserAsync();
                Userdata.RedisDBKey = Guid.NewGuid().ToString();
                Userdata.RedisSessionKey = Guid.NewGuid().ToString();
                RedisDBKey = Userdata.RedisDBKey;
                redis.Save(Userdata.RedisDBKey, Userdata);
                _redisSessionStorage.SetObject(Userdata.RedisSessionKey, user, RedisDBKey);
            }

            */

            try
            {
                var user = await GetCurrentUserAsync();
                var UserData = new IndexViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.UserName,
                    IsEmailConfirmed = user.EmailConfirmed,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                    // RedisDBKey = RedisDBKey
                };
                //string json = JsonConvert.SerializeObject(UserData);
                return Ok(new UserInfoResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SuccessfullGetUserData, UserData = UserData });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }


        }

        [HttpPost("userinfo")]
        public async Task<IActionResult> UserInfo([FromBody]UserInfoViewModel model)
        {
            try
            {

                var user = await GetCurrentUserAsync();
                if (!string.IsNullOrEmpty(model.FirstName))
                    user.FirstName = model.FirstName;
                if (!string.IsNullOrEmpty(model.LastName))
                    user.LastName = model.LastName;
                //user.UserName = string.IsNullOrEmpty(model.Username) ? user.UserName : model.Username;
                //user.Email = string.IsNullOrEmpty(model.Email) ? user.Email : model.Email;
                //if (!string.IsNullOrEmpty(model.PhoneNumber))
                //    user.PhoneNumber = model.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);
                if (result == IdentityResult.Success)
                {
                    var UserData = new IndexViewModel
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Username = user.UserName,
                        IsEmailConfirmed = user.EmailConfirmed,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber
                    };
                    return Ok(new UserInfoResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SuccessfullUpdateUserData, UserData = UserData });
                }
                return BadRequest(new UserInfoResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.Unableupdateuserinfo, ErrorCode = enErrorCode.Status4034UnableUpdateUser });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new RegisterResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }


        #region IpAddress
        [HttpPost("IpAddress")]
        public async Task<IActionResult> AddIpAddress([FromBody]IpAddressReqViewModel model)
        {
            try
            {
                string IpCountryCode = await _userdata.GetCountryByIP(model.IPAddress);
                if (!string.IsNullOrEmpty(IpCountryCode) && IpCountryCode == "fail")
                {
                    return BadRequest(new IpAddressResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.IpAddressInvalid, ErrorCode = enErrorCode.Status4020IpInvalid });

                }
                string UserCountryCode = await _userdata.GetCountryByIP(model.SelectedIPAddress);
                if (!string.IsNullOrEmpty(UserCountryCode) && UserCountryCode == "fail")
                {
                    return BadRequest(new IpAddressResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidUserSelectedIp, ErrorCode = enErrorCode.Status4045InvalidUserSelectedIp });
                }

                var user = await GetCurrentUserAsync();

                if (user != null)
                {
                    IpMasterViewModel imodel = new IpMasterViewModel();
                    imodel.UserId = user.Id;
                    imodel.IpAddress = model.SelectedIPAddress;

                    long id = await _ipAddressService.AddIpAddress(imodel);

                    if (id > 0)
                    {
                        return Ok(new IpAddressResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SuccessAddIpData });
                    }
                    else
                    {
                        return BadRequest(new IpAddressResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.IpAddressInsertError, ErrorCode = enErrorCode.Status400BadRequest });
                    }
                }
                else
                {
                    return BadRequest(new IpAddressResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status400BadRequest });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new IpAddressResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }

        }

        [HttpPost("DisableIpAddress")]
        public async Task<IActionResult> DisableIpAddress([FromBody]IpAddressReqViewModel model)
        {
            try
            {
                string IpCountryCode = await _userdata.GetCountryByIP(model.IPAddress);
                if (!string.IsNullOrEmpty(IpCountryCode) && IpCountryCode == "fail")
                {
                    return BadRequest(new IpAddressResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.IpAddressInvalid, ErrorCode = enErrorCode.Status4020IpInvalid });

                }
                string UserCountryCode = await _userdata.GetCountryByIP(model.SelectedIPAddress);
                if (!string.IsNullOrEmpty(UserCountryCode) && UserCountryCode == "fail")
                {
                    return BadRequest(new IpAddressResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidUserSelectedIp, ErrorCode = enErrorCode.Status4045InvalidUserSelectedIp });
                }

                var user = await GetCurrentUserAsync();

                if (user != null)
                {
                    IpMasterViewModel imodel = new IpMasterViewModel();
                    imodel.UserId = user.Id;
                    imodel.IpAddress = model.SelectedIPAddress;

                    long id = await _ipAddressService.DesableIpAddress(imodel);
                    if (id > 0)
                    {
                        return Ok(new IpAddressResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SuccessDesableIpStatus });
                    }
                    else
                    {
                        return BadRequest(new IpAddressResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.IpAddressUpdateError, ErrorCode = enErrorCode.Status4046NotUpdateIpStatus });
                    }
                }
                else
                {
                    return BadRequest(new IpAddressResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status400BadRequest });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new IpAddressResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }

        }


        [HttpPost("DeleteIpAddress")]
        public async Task<IActionResult> DeleteIpAddress([FromBody]IpAddressReqViewModel model)
        {
            try
            {
                string IpCountryCode = await _userdata.GetCountryByIP(model.IPAddress);
                if (!string.IsNullOrEmpty(IpCountryCode) && IpCountryCode == "fail")
                {
                    return BadRequest(new IpAddressResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.IpAddressInvalid, ErrorCode = enErrorCode.Status4020IpInvalid });
                }
                string UserCountryCode = await _userdata.GetCountryByIP(model.SelectedIPAddress);
                if (!string.IsNullOrEmpty(UserCountryCode) && UserCountryCode == "fail")
                {
                    return BadRequest(new IpAddressResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidUserSelectedIp, ErrorCode = enErrorCode.Status4045InvalidUserSelectedIp });
                }

                var user = await GetCurrentUserAsync();

                if (user != null)
                {
                    IpMasterViewModel imodel = new IpMasterViewModel();
                    imodel.UserId = user.Id;
                    imodel.IpAddress = model.SelectedIPAddress;

                    long id = await _ipAddressService.DeleteIpAddress(imodel);
                    if (id > 0)
                    {
                        return Ok(new IpAddressResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SuccessDeleteIpAddress });
                    }
                    else
                    {
                        return BadRequest(new IpAddressResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.IpAddressdeleteError, ErrorCode = enErrorCode.Status4047NotDeleteIp });
                    }
                }
                else
                {
                    return BadRequest(new IpAddressResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status400BadRequest });
                }



            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new IpAddressResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }

        }


        [HttpGet("GetIpAddress/{PageIndex}/{PAGE_SIZE}")]
        public async Task<IActionResult> GetIpAddress(int PageIndex = 0, int PAGE_SIZE = 0)
        {
            try
            {
                var user = await GetCurrentUserAsync();

                if (user != null)
                {
                    var IpList = await _ipAddressService.GetIpAddressListByUserId(user.Id);

                    if (IpList.Count > 0)
                    {

                        // Set the total count
                        // so GridView knows how many pages to create
                        var TotalRowCount = IpList.Count();
                        //int PageIndex = 1;
                        //int PAGE_SIZE = 10;

                        // Get only the rows we need for the page requested
                        var Ip = IpList.Skip(PageIndex * PAGE_SIZE).Take(PAGE_SIZE);


                        return Ok(new IpAddressResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SuccessGetIpData, IpList = Ip.ToList(), TotalRow = TotalRowCount });
                    }
                    else
                    {
                        return BadRequest(new IpAddressResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.IpAddressInsertError, ErrorCode = enErrorCode.Status400BadRequest });
                    }
                }
                else
                {
                    return BadRequest(new IpAddressResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status400BadRequest });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new IpAddressResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }

        }

        #endregion

        #region DeviceId
        [HttpPost("AddDevice")]
        public async Task<IActionResult> AddDevice([FromBody]DeviceIdReqViewModel model)
        {
            try
            {
                string IpCountryCode = await _userdata.GetCountryByIP(model.IPAddress);
                if (!string.IsNullOrEmpty(IpCountryCode) && IpCountryCode == "fail")
                {
                    return BadRequest(new IpAddressResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.IpAddressInvalid, ErrorCode = enErrorCode.Status4020IpInvalid });

                }

                var user = await GetCurrentUserAsync();

                if (user != null)
                {
                    DeviceMasterViewModel imodel = new DeviceMasterViewModel();
                    imodel.UserId = user.Id;
                    imodel.DeviceId = model.SelectedDeviceId;

                    long id = _iDeviceIdService.AddDeviceId(imodel);

                    if (id > 0)
                    {
                        return Ok(new DeviceIdResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SuccessAddDeviceData });
                    }
                    else
                    {
                        return BadRequest(new DeviceIdResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.DeviceidInsertError, ErrorCode = enErrorCode.Status4057DeviceIdNotAdd });
                    }
                }
                else
                {
                    return BadRequest(new DeviceIdResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status400BadRequest });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new DeviceIdResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }

        }


        [HttpPost("DisableDeviceId")]
        public async Task<IActionResult> DisableDeviceId([FromBody]DeviceIdReqViewModel model)
        {
            try
            {
                string IpCountryCode = await _userdata.GetCountryByIP(model.IPAddress);
                if (!string.IsNullOrEmpty(IpCountryCode) && IpCountryCode == "fail")
                {
                    return BadRequest(new DeviceIdResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.IpAddressInvalid, ErrorCode = enErrorCode.Status4020IpInvalid });

                }


                var user = await GetCurrentUserAsync();

                if (user != null)
                {
                    DeviceMasterViewModel imodel = new DeviceMasterViewModel();
                    imodel.UserId = user.Id;
                    imodel.DeviceId = model.SelectedDeviceId;

                    long id = _iDeviceIdService.DesableDeviceId(imodel);
                    if (id > 0)
                    {
                        return Ok(new DeviceIdResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SuccessDesableIpStatus });
                    }
                    else
                    {
                        return BadRequest(new DeviceIdResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.DeviceAddressUpdateError, ErrorCode = enErrorCode.Status4058DeviceAddress });
                    }
                }
                else
                {
                    return BadRequest(new DeviceIdResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status400BadRequest });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new DeviceIdResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }

        }


        [HttpPost("DeleteDeviceId")]
        public async Task<IActionResult> DeleteDeviceId([FromBody]DeviceIdReqViewModel model)
        {
            try
            {
                string IpCountryCode = await _userdata.GetCountryByIP(model.IPAddress);
                if (!string.IsNullOrEmpty(IpCountryCode) && IpCountryCode == "fail")
                {
                    return BadRequest(new DeviceIdResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.IpAddressInvalid, ErrorCode = enErrorCode.Status4020IpInvalid });
                }


                var user = await GetCurrentUserAsync();

                if (user != null)
                {
                    DeviceMasterViewModel imodel = new DeviceMasterViewModel();
                    imodel.UserId = user.Id;
                    imodel.DeviceId = model.SelectedDeviceId;

                    long id = _iDeviceIdService.DeleteDeviceId(imodel);
                    if (id > 0)
                    {
                        return Ok(new DeviceIdResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SuccessDeleteDevice });
                    }
                    else
                    {
                        return BadRequest(new DeviceIdResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.DeviceAddressdeleteError, ErrorCode = enErrorCode.Status4059NotDeleteDevice });
                    }
                }
                else
                {
                    return BadRequest(new DeviceIdResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status400BadRequest });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new DeviceIdResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }

        }

        [HttpGet("GetDeviceId/{PageIndex}/{PAGE_SIZE}")]
        public async Task<IActionResult> GetDeviceId(int PageIndex = 0, int PAGE_SIZE = 0)
        {
            try
            {
                var user = await GetCurrentUserAsync();

                if (user != null)
                {
                    var DeviceList = _iDeviceIdService.GetDeviceListByUserId(user.Id);

                    if (DeviceList.Count > 0)
                    {
                        // Set the total count
                        // so GridView knows how many pages to create
                        var TotalRowCount = DeviceList.Count();
                        //int PageIndex = 1;
                        //int PAGE_SIZE = 10;

                        // Get only the rows we need for the page requested
                        var Device = DeviceList.Skip(PageIndex * PAGE_SIZE).Take(PAGE_SIZE);


                        return Ok(new DeviceIdResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SuccessGetDeviceData, DeviceList = Device.ToList(), TotalRow = TotalRowCount });
                    }
                    else
                    {
                        return BadRequest(new DeviceIdResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.DeviceidInsertError, ErrorCode = enErrorCode.Status4057DeviceIdNotAdd });
                    }
                }
                else
                {
                    return BadRequest(new DeviceIdResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status400BadRequest });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new DeviceIdResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }

        }

        #endregion

        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordViewModel model)
        {

            try
            {
                if (!model.NewPassword.Equals(model.ConfirmPassword))
                {
                    return BadRequest(new ChangePasswordResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.ResetConfirmPassMatch, ErrorCode = enErrorCode.Status4042ResetConfirmPassMatch });
                }
                var user = await GetCurrentUserAsync();

                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    _logger.LogInformation(3, "User changed their password successfully.");
                    return Ok(new ChangePasswordResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.ChangePassword });

                }

                return BadRequest(new ChangePasswordResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.ResetConfirmOldNotMatch, ErrorCode = enErrorCode.Status4043ResetConfirmOldNotMatch });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest(new ChangePasswordResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }


            //return BadRequest(new ApiError("Unable to change password"));
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
