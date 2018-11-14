using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Web.Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ViewModels.MasterConfiguration;
using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Core.ViewModels.Configuration;

namespace CleanArchitecture.Web.API.Configuration
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class MasterConfigurationController : Controller
    {
        #region "Ctor"
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMasterConfiguration _masterConfiguration;
        private readonly ICommunicationService _communicationService;

        public MasterConfigurationController(UserManager<ApplicationUser> userManager, IBasePage basePage, IMasterConfiguration masterConfiguration, ICommunicationService communicationService)
        {
            _userManager = userManager;
            _masterConfiguration = masterConfiguration;
            _communicationService = communicationService;
        }
        #endregion

        #region "CommServiceTypeMaster"

        //vsoalnki 13-11-2018
        [HttpGet]
        public async Task<IActionResult> GetAllCommServiceType()
        {
            CommServiceTypeRes Response = new CommServiceTypeRes();
            try
            {
                ApplicationUser user = new ApplicationUser(); user.Id = 1;
                //ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    Response = _communicationService.GetAllCommServiceTypeMaster();
                }
                var respObj = JsonConvert.SerializeObject(Response);
                dynamic respObjJson = JObject.Parse(respObj);
                return Ok(respObjJson);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        #endregion

        #region "TemplateMaster"

        //vsoalnki 13-11-2018
        [HttpGet]
        public async Task<IActionResult> GetAllTemplate()
        {
            ListTemplateMasterRes Response = new ListTemplateMasterRes();
            try
            {
                ApplicationUser user = new ApplicationUser(); user.Id = 1;
                //ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    Response = _communicationService.GetAllTemplateMaster();
                }
                var respObj = JsonConvert.SerializeObject(Response);
                dynamic respObjJson = JObject.Parse(respObj);
                return Ok(respObjJson);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        //vsoalnki 13-11-2018
        [HttpPost]
        public async Task<IActionResult> AddTemplate([FromBody]TemplateMasterReq Request)
        {
            BizResponseClass Response = new BizResponseClass();
            try
            {
                ApplicationUser user = new ApplicationUser(); user.Id = 1;
                //ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    Response = _communicationService.AddTemplateMaster(Request, user.Id);
                }
                var respObj = JsonConvert.SerializeObject(Response);
                dynamic respObjJson = JObject.Parse(respObj);
                return Ok(respObjJson);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        //vsoalnki 13-11-2018
        [HttpPost("{TemplateMasterId}")]
        public async Task<IActionResult> UpdateTemplate([FromBody]TemplateMasterReq Request, long TemplateMasterId)
        {
            BizResponseClass Response = new BizResponseClass();
            try
            {
                ApplicationUser user = new ApplicationUser(); user.Id = 1;
                //ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    Response = _communicationService.UpdateTemplateMaster(TemplateMasterId, Request, user.Id);
                }
                var respObj = JsonConvert.SerializeObject(Response);
                dynamic respObjJson = JObject.Parse(respObj);
                return Ok(respObjJson);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        //vsoalnki 13-11-2018
        [HttpPost("{TemplateMasterId}")]
        public async Task<IActionResult> DisableTemplate(long TemplateMasterId)
        {
            BizResponseClass Response = new BizResponseClass();
            try
            {
                ApplicationUser user = new ApplicationUser(); user.Id = 1;
                //ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    Response = _communicationService.DisableTemplateMaster(TemplateMasterId);
                }
                var respObj = JsonConvert.SerializeObject(Response);
                dynamic respObjJson = JObject.Parse(respObj);
                return Ok(respObjJson);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        //vsoalnki 13-11-2018
        [HttpGet("{TemplateMasterId}")]
        public async Task<IActionResult> GetTemplateById(long TemplateMasterId)
        {
            TemplateMasterRes Response = new TemplateMasterRes();
            try
            {
                ApplicationUser user = new ApplicationUser(); user.Id = 1;
                //ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    Response = _communicationService.GetTemplateMasterById(TemplateMasterId);
                }
                var respObj = JsonConvert.SerializeObject(Response);
                dynamic respObjJson = JObject.Parse(respObj);
                return Ok(respObjJson);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        #endregion

        #region "MessagingQueue"

        //vsolanki 13-11-2018
        [HttpGet("{FromDate}/{ToDate}/{Page}")]
        public async Task<IActionResult> GetMessagingQueue(DateTime FromDate, DateTime ToDate, short? Status, long? MobileNo, int Page)
        {
            ListMessagingQueueRes Response = new ListMessagingQueueRes();
            try
            {
                ApplicationUser user = new ApplicationUser(); user.Id = 1;
                //ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    Response = _communicationService.GetMessagingQueue(FromDate,ToDate,Status,MobileNo,Page);
                }
                var respObj = JsonConvert.SerializeObject(Response);
                dynamic respObjJson = JObject.Parse(respObj);
                return Ok(respObjJson);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        #endregion

        #region "EmailQueue"

        //vsolanki 14-11-2018
        [HttpGet("{FromDate}/{ToDate}/{Page}")]
        public async Task<IActionResult> GetEmailQueue(DateTime FromDate, DateTime ToDate, short? Status, string Email, int Page)
        {
            ListEmailQueueRes Response = new ListEmailQueueRes();
            try
            {
                ApplicationUser user = new ApplicationUser(); user.Id = 1;
                //ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    Response = _communicationService.GetEmailQueue(FromDate, ToDate, Status, Email, Page);
                }
                var respObj = JsonConvert.SerializeObject(Response);
                dynamic respObjJson = JObject.Parse(respObj);
                return Ok(respObjJson);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        #endregion

        #region MasterConfiguration

        #region Insert Method
        [HttpPost]
        public async Task<IActionResult> AddCountry(AddCountryReq Request)//[Required]string CountryName, [Required]string CountryCode
        {
            BizResponseClass Response = new BizResponseClass();
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    Response = _masterConfiguration.AddCountry(Request,user.Id);//Request.CountryName,Request.CountryCode,user.Id,Request.Status
                }
                var respObj = JsonConvert.SerializeObject(Response);
                dynamic respObjJson = JObject.Parse(respObj);
                return Ok(respObjJson);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddState(AddStateReq Request)//string StateName, string StateCode, long CountryID
        {
            BizResponseClass Response = new BizResponseClass();
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    Response = _masterConfiguration.AddState(Request, user.Id);//Request.StateName, Request.StateCode, Request.CountryID
                }
                var respObj = JsonConvert.SerializeObject(Response);
                dynamic respObjJson = JObject.Parse(respObj);
                return Ok(respObjJson);

            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCity(AddCityReq Request)//string CityName, long StateID
        {
            BizResponseClass Response = new BizResponseClass();
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    Response = _masterConfiguration.AddCity(Request, user.Id);//user.Id--71   Request.CityName, Request.StateID
                }
                var respObj = JsonConvert.SerializeObject(Response);
                dynamic respObjJson = JObject.Parse(respObj);
                return Ok(respObjJson);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddZipCode(AddZipCodeReq Request)//long ZipCode, string AreaName, long CityID
        {
            BizResponseClass Response = new BizResponseClass();
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    Response = _masterConfiguration.AddZipCode(Request, user.Id);//Request.ZipCode, Request.AreaName, Request.CityID
                }
                var respObj = JsonConvert.SerializeObject(Response);
                dynamic respObjJson = JObject.Parse(respObj);
                return Ok(respObjJson);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        #endregion

        #region Update Methods

        [HttpPost]
        public async Task<IActionResult> UpdateCountryDetail(AddCountryReq Request)
        {
            BizResponseClass Response = new BizResponseClass();
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else if(Request.CountryID == null || Request.CountryID == 0)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.MasterConfig;
                    Response.ErrorCode = enErrorCode.InvalidMasterID;
                }
                else
                {
                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    Response = _masterConfiguration.UpdateCountry(Request, user.Id);
                }
                var respObj = JsonConvert.SerializeObject(Response);
                dynamic respObjJson = JObject.Parse(respObj);
                return Ok(respObjJson);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStateDetail(AddStateReq Request)
        {
            BizResponseClass Response = new BizResponseClass();
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else if (Request.StateID == null || Request.StateID == 0)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.MasterConfig;
                    Response.ErrorCode = enErrorCode.InvalidMasterID;
                }
                else
                {
                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    Response = _masterConfiguration.UpdateState(Request, user.Id);
                }
                var respObj = JsonConvert.SerializeObject(Response);
                dynamic respObjJson = JObject.Parse(respObj);
                return Ok(respObjJson);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCityDetail(AddCityReq Request)
        {
            BizResponseClass Response = new BizResponseClass();
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else if (Request.CityID == null || Request.CityID == 0)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.MasterConfig;
                    Response.ErrorCode = enErrorCode.InvalidMasterID;
                }
                else
                {
                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    Response = _masterConfiguration.UpdateCity(Request, user.Id);
                }
                var respObj = JsonConvert.SerializeObject(Response);
                dynamic respObjJson = JObject.Parse(respObj);
                return Ok(respObjJson);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateZipCodeDetail(AddZipCodeReq Request)
        {
            BizResponseClass Response = new BizResponseClass();
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else if (Request.ZipCodeID == null || Request.ZipCodeID == 0)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.MasterConfig;
                    Response.ErrorCode = enErrorCode.InvalidMasterID;
                }
                else
                {
                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    Response = _masterConfiguration.UpdateZipCode(Request, user.Id);
                }
                var respObj = JsonConvert.SerializeObject(Response);
                dynamic respObjJson = JObject.Parse(respObj);
                return Ok(respObjJson);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        #endregion

        #endregion
    }
}