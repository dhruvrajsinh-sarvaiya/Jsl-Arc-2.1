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
using CleanArchitecture.Core.ViewModels.Configuration;

namespace CleanArchitecture.Web.API.Configuration
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MasterConfigurationController : Controller
    {
        #region "Ctor"
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMasterConfiguration _masterConfiguration;
        private readonly ICommunicationService _communicationService;

        public MasterConfigurationController(UserManager<ApplicationUser> userManager, IBasePage basePage,IMasterConfiguration masterConfiguration, ICommunicationService communicationService)
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
                    Response = _communicationService.AddTemplateMaster(Request,user.Id);
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
        public async Task<IActionResult> UpdateTemplate([FromBody]TemplateMasterReq Request,long TemplateMasterId)
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

        [HttpPost]
        public async Task<IActionResult> AddCountry(string CountryName, string CountryCode)
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
                    Response = _masterConfiguration.AddCountry(CountryName, CountryCode,user.Id);
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
        public async Task<IActionResult> AddState(string StateName, string StateCode, long CountryID)
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
                    Response = _masterConfiguration.AddState(StateName, StateCode, CountryID, user.Id);
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
        public async Task<IActionResult> AddCity(string CityName, long StateID)
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
                    Response = _masterConfiguration.AddCity(CityName, StateID, user.Id);
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
        public async Task<IActionResult> AddZipCode(long ZipCode, string AreaName, long CityID)
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
                    Response = _masterConfiguration.AddZipCode(ZipCode, AreaName, CityID, user.Id);
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
    }
}