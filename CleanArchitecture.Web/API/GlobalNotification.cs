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
    }
}
