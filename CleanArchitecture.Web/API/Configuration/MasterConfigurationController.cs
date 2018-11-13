﻿using System;
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

namespace CleanArchitecture.Web.API.Configuration
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterConfigurationController : Controller
    {
        #region Ctor
        private readonly IBasePage _basePage;
        private readonly ILogger<MasterConfigurationController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMasterConfiguration _masterConfiguration;

        public MasterConfigurationController(ILogger<MasterConfigurationController> logger, UserManager<ApplicationUser> userManager, IBasePage basePage,IMasterConfiguration masterConfiguration)
        {
            _logger = logger;
            _userManager = userManager;
            _masterConfiguration = masterConfiguration;
            _basePage = basePage;
        }
        #endregion

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
    }
}