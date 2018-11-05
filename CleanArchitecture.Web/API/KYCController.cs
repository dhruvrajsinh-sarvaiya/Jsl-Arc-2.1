using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces.KYC;
using CleanArchitecture.Core.Interfaces.User;
using CleanArchitecture.Core.ViewModels.KYC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class KYCController : ControllerBase
    {
        #region Field
        //private IHostingEnvironment _hostingEnvironment;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        private readonly IPersonalVerificationService _personalVerificationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        #endregion

        #region Ctore
        public KYCController(//IHostingEnvironment hostingEnvironment,
            Microsoft.Extensions.Configuration.IConfiguration configuration,
            IPersonalVerificationService personalVerificationService, UserManager<ApplicationUser> userManager)
        {
            //_hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _personalVerificationService = personalVerificationService;
            _userManager = userManager;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Thid method are used KYC 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>


        /// <summary>
        /// Thid method are used KYC 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("PersonalVerification")]
        //[AllowAnonymous]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> PersonalVerification() //(PersonalVerificationViewModel model)
        {
            try
            {
                var httpRequest = Request.Form;
                var user = await GetCurrentUserAsync();
                int userid = user.Id;



                PersonalVerificationViewModel model = new PersonalVerificationViewModel();
                //model.Id = ;
                //model.UserID = 

                if (String.IsNullOrEmpty(httpRequest["IPAddress"].ToString()))
                    return BadRequest(new PersonalVerificationResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.IpAddressInvalid, ErrorCode = enErrorCode.Status4020IpInvalid });
                ////// Ip Address Validate or not
                string CountryCode = await _userService.GetCountryByIP(httpRequest["IPAddress"].ToString());
                if (!string.IsNullOrEmpty(CountryCode) && CountryCode == "fail")
                {
                    return BadRequest(new PersonalVerificationResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.IpAddressInvalid, ErrorCode = enErrorCode.Status4020IpInvalid });
                }


                if (httpRequest.Files.Count == 0)
                    return BadRequest(new PersonalVerificationResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.ImageNotAvailable, ErrorCode = enErrorCode.Status4115ImageNotUpload });
                if (String.IsNullOrEmpty(httpRequest["Surname"].ToString()))
                    return BadRequest(new PersonalVerificationResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.Surname, ErrorCode = enErrorCode.Status4126SuranName });
                if (String.IsNullOrEmpty(httpRequest["GivenName"].ToString()))
                    return BadRequest(new PersonalVerificationResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.GivenName, ErrorCode = enErrorCode.Status4127GivenName });
                if (String.IsNullOrEmpty(httpRequest["ValidIdentityCard"].ToString()))
                    return BadRequest(new PersonalVerificationResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.ValidIdentityCard, ErrorCode = enErrorCode.Status4128ValidIdentityCard });


                model.UserId = userid;
                model.Surname = httpRequest["Surname"].ToString();
                model.GivenName = httpRequest["GivenName"].ToString();
                model.ValidIdentityCard = httpRequest["ValidIdentityCard"].ToString();
                model.EnableStatus = false;
                model.VerifyStatus = false;


                foreach (var file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file.Name];
                    if (file.Length > Convert.ToInt64(_configuration["KYCImageSize"]))
                    {
                        if (file.Name == "Front")
                        {
                            return BadRequest(new PersonalVerificationResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.FrontImageSizeLarger, ErrorCode = enErrorCode.Status4123FrontImageLarger });
                        }
                        if (file.Name == "Back")
                        {
                            return BadRequest(new PersonalVerificationResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.BackImageSizeLarger, ErrorCode = enErrorCode.Status4124BackImageLarger });
                        }
                        if (file.Name == "Selfie")
                        {
                            return BadRequest(new PersonalVerificationResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SelfieImageSizeLarger, ErrorCode = enErrorCode.Status4125SelfiImageLarger });
                        }

                    }
                }
                foreach (var file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file.Name];
                    string folderDirctory = model.UserId.ToString();
                    //string webRootPath = _hostingEnvironment.WebRootPath;
                    string webRootPath = _configuration["KYCImagePath"].ToString();
                    string newPath = Path.Combine(webRootPath, folderDirctory);
                    if (!Directory.Exists(newPath))
                    {
                        Directory.CreateDirectory(newPath);
                    }

                    //if (file.Length > 0)
                    //{
                    string fileName = ContentDispositionHeaderValue.Parse(postedFile.ContentDisposition).FileName.Trim('"');
                    //fileName = postedFile.FileName;
                    //string fullPath = Path.Combine(newPath, fileName);
                    string fullPath = newPath + "//" + fileName;
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        // postedFile.CopyTo(stream);
                        await postedFile.CopyToAsync(stream);
                    }

                    //using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                    //{
                    //    await file.CopyToAsync(fileStream);
                    //}

                    // }

                    if (file.Name == "Front")
                    {
                        model.FrontImage = fileName;
                    }
                    if (file.Name == "Back")
                    {
                        model.BackImage = fileName;
                    }
                    if (file.Name == "Selfie")
                    {
                        model.SelfieImage = fileName;
                    }

                }

                long verifyId = await _personalVerificationService.AddPersonalVerification(model);
                if (verifyId > 0)
                {
                    return Ok(new PersonalVerificationResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.PersonalIdentityInsertSuccessfull });
                }
                return BadRequest(new PersonalVerificationResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.PersonalIdentityNotInserted, ErrorCode = enErrorCode.Status4129PersonalIdentityNotInserted });

            }
            catch (Exception ex)
            {
                return BadRequest(new PersonalVerificationResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });

            }
        }

        /// <summary>
        /// Thid method are used GET KYC details
        /// </summary>
        /// <param name="model"></param>
        [HttpGet("GetKYCData")]
        public async Task<IActionResult> GetKYCData()
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user != null)
                {
                    var userkycdata = _personalVerificationService.GetPersonalVerification(user.Id);
                    if (userkycdata != null)
                    {
                        PersonalVerificationRequest userdata = new PersonalVerificationRequest();

                        userdata.Surname = userkycdata.Surname;
                        userdata.GivenName = userkycdata.GivenName;
                        userdata.ValidIdentityCard = userkycdata.ValidIdentityCard;
                        userdata.FrontImage = userkycdata.FrontImage;
                        userdata.BackImage = userkycdata.BackImage;
                        userdata.SelfieImage = userkycdata.SelfieImage;
                        userdata.EnableStatus = userkycdata.EnableStatus;
                        userdata.VerifyStatus = userkycdata.VerifyStatus;
                        userdata.KYCLevelId = userkycdata.KYCLevelId;

                        return Ok(new PersonalVerificationResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.GetPersonalIdentity, UserKYC = userdata });
                    }
                    else
                        return BadRequest(new PersonalVerificationResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.PersonalIdentityNotavailable, ErrorCode = enErrorCode.Status4130PersonalIdentityNotavailable });

                }
                else
                    return BadRequest(new PersonalVerificationResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status4063UserNotRegister });
            }

            catch (Exception ex)
            {
                return BadRequest(new PersonalVerificationResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });

            }
        }
        #endregion

        #region Helpers
        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
        #endregion
    }
}