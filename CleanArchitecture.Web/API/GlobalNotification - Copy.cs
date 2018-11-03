using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities.Communication;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Helpers;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.SignalR;
using CleanArchitecture.Core.ViewModels.Transaction;
using CleanArchitecture.Core.ViewModels.Wallet;
using CleanArchitecture.Infrastructure.DTOClasses;
using CleanArchitecture.Infrastructure.Interfaces;
using CleanArchitecture.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanArchitecture.Web.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlobalNotification : Controller
    {
        private readonly ILogger<SocketController> _logger;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly IApplicationDataService _applicationDataService;

        public GlobalNotification(UserManager<ApplicationUser> userManager, ILogger<SocketController> logger,IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
            _userManager = userManager;
            //_applicationDataService = ApplicationDataService;
        }
        
        [HttpPost]
        [Route("News")]
        public async Task<IActionResult> News()
        {
            try
            {
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    string Content = await reader.ReadToEndAsync();
                    SignalRComm<string> CommonData = new SignalRComm<string>();
                    CommonData.Data = Content;
                    CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.BroadCast);
                    CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.News);
                    CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.BroadcastMessage);
                    CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.Broadcast);

                    SignalRData SendData = new SignalRData();
                    SendData.Method = enMethodName.News;
                    SendData.DataObj = JsonConvert.SerializeObject(CommonData);
                    await _mediator.Send(SendData);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return BadRequest();
            }
        }
        
        [HttpPost]
        [Route("Announcement")]
        public async Task<IActionResult> Announcement()
        {
            try
            {
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    string Content = await reader.ReadToEndAsync();
                    SignalRComm<string> CommonData = new SignalRComm<string>();
                    CommonData.Data = Content;
                    CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.BroadCast);
                    CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.Announcement);
                    CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.BroadcastMessage);
                    CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.Broadcast);

                    SignalRData SendData = new SignalRData();
                    SendData.Method = enMethodName.Announcement;
                    SendData.DataObj = JsonConvert.SerializeObject(CommonData);
                    await _mediator.Send(SendData);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return BadRequest();
            }
        }


        [HttpGet("tets1/{Data}")]
        public async Task<IActionResult> Test1(string Data)
        {
            try
            {
                long i = 0;
                for (i = 0; i <= 25; i++)
                {
                    GetBuySellBook model = new GetBuySellBook();
                    model.Amount = 3;
                    model.Price = 150;

                    GetBuySellBook temp = JsonConvert.DeserializeObject<GetBuySellBook>(Data);

                    SignalRComm<GetBuySellBook> CommonData = new SignalRComm<GetBuySellBook>();
                    CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                    CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.BuyerBook);
                    CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveBuyerBook);
                    CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                    CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.PairName);
                    CommonData.Data = temp;
                    CommonData.Parameter = "INR_BTC";

                    SignalRData SendData = new SignalRData();
                    SendData.Method = enMethodName.BuyerBook;
                    SendData.Parameter = CommonData.Parameter;
                    SendData.DataObj = JsonConvert.SerializeObject(CommonData);
                    await _mediator.Send(SendData);                    
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return BadRequest();
            }
        }

        [HttpGet("test2/{Data}")]
        //public async Task<IActionResult> Test2(string userid, [FromServices] ITokenService TS, [FromServices] IUserClaimsPrincipalFactory<ApplicationUser> principalFactory,[FromServices] IdentityServerOptions options)
        public async Task<IActionResult> Test2(string userid)
        {
            try
            {
                var userFromManager = await _userManager.FindByIdAsync(userid);
                var externalAccessToken = await _userManager.GetAuthenticationTokenAsync(userFromManager, "[AspNetUserStore]", "access_token");
                //var externalAccessToken = await _userManager.GetAuthenticationTokenAsync(userFromManager, "[AspNetUserStore]", "access_token");
                // externalAccessToken = await _userManager.GetAuthenticationTokenAsync(userFromManager, "Microsoft", "access_token");


                //var user = await _userManager.FindByIdAsync(userid);
                //var IdentityPricipal = await principalFactory.CreateAsync(user);
                //var IdServerPrincipal = IdentityServerPrincipal.Create(user.Id.ToString(), user.UserName);

                //Request.Subject = IdServerPrincipal;
                //Request.IncludeAllIdentityClaims = true;
                //Request.ValidatedRequest = new ValidatedRequest();
                //Request.ValidatedRequest.Subject = Request.Subject;
                //Request.ValidatedRequest.SetClient(Config.GetClient());
                //Request.Resources = new Resources(Config.GetResources(), Config.GetApiResources());
                //Request.ValidatedRequest.Options = options;
                //var Token = await TS.CreateAccessTokenAsync(Request);
                //Token.Issuer = "http://" + HttpContext.Request.Host.Value;

                //var TokenValue = await TS.CreateSecurityTokenAsync(Token);
                //return Ok(TokenValue);
               // var data = await _applicationDataService.GetApplicationData();
                return Ok(externalAccessToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return BadRequest();
            }
        }
    }
}
