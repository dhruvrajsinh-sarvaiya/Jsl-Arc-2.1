using System;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.ViewModels;
using CleanArchitecture.Web.ApiModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TwoFactorAuthNet;

namespace CleanArchitecture.Web.Api
{
    [Route("api/[controller]")]
    public class ToDoItemsController : Controller
    {
        private readonly IRepository<ToDoItem> _todoRepository;
        private readonly IMediator _mediator;
        ILogger<ToDoItemsController> _loggerFactory;
        private readonly string sKey;
        public ToDoItemsController(ILogger<ToDoItemsController> loggerFactory, IRepository<ToDoItem> todoRepository, IMediator mediator)
        {
            _loggerFactory = loggerFactory/*.CreateLogger<ToDoItemsController>()*/;
            _todoRepository = todoRepository;
            _mediator = mediator;
           
        }

        // GET: api/ToDoItems
        [HttpGet]
        public IActionResult List()
        {
            // _loggerFactory logger = _loggerFactory.CreateLogger("LoggerCategory");
            // _loggerFactory.LogInformation("Your MSg");
            var logger = NLog.LogManager.GetCurrentClassLogger();
            // logger.Info("Hello World");
            logger.Info("Hello World");
            //  logger.Debug("Sample debug message");
            // logger.Log(LogLevel.Information, "Sample informational message");
            logger.Error("ow noos!", "");
            var items = _todoRepository.List()
                            .Select(item => ToDoItemDTO.FromToDoItem(item));
            return Ok(items);
        }

        // GET: api/ToDoItems
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var item = ToDoItemDTO.FromToDoItem(_todoRepository.GetById(id));
            return Ok(item);
        }

        // POST: api/ToDoItems
        [HttpPost]
        public IActionResult Post([FromBody] ToDoItemDTO item)
        {
            var todoItem = new ToDoItem()
            {
                Title = item.Title,
                Description = item.Description
            };
            _todoRepository.Add(todoItem);
            return Ok(ToDoItemDTO.FromToDoItem(todoItem));
        }

        [HttpPatch("{id:int}/complete")]
        public IActionResult Complete(int id)
        {
            var toDoItem = _todoRepository.GetById(id);
            toDoItem.MarkComplete();
            _todoRepository.Update(toDoItem);

            return Ok(ToDoItemDTO.FromToDoItem(toDoItem));
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] SendSMSRequest Request)
        {
            try
            {
                CommunicationResponse Response = await _mediator.Send(Request);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailRequest Request)
        {
            try
            {
                CommunicationResponse Response = await _mediator.Send(Request);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                return BadRequest(Response);
            }
        }

        [HttpPost("SendNotification")]
        public async Task<IActionResult> SendNotification([FromBody] SendNotificationRequest Request)
        {
            try
            {
                CommunicationResponse Response = await _mediator.Send(Request);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                return BadRequest(Response);
            }
        }

     
        // POST api/values
        [HttpGet("GetQrCode")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetQrCode(string UserName)
        {
            try
            {
                TwoFactorAuth TFAuth = new TwoFactorAuth();
                string URL;
                string sKey = string.Empty;
                string sName = string.Empty;
                sKey = TFAuth.CreateSecret(160);
                sName = UserName; // dSetReq.Tables(0).Rows(0)("NAME");
                sKey = TFAuth.CreateSecret(160);
                URL = TFAuth.GetQrCodeImageAsDataUri(sName, sKey);
                string value = URL + "" + sKey;
                return Ok(new BizResponseClass { ReturnCode = enResponseCode.Success, ReturnMsg = value, });

            }
            catch (Exception ex)
            {
                //return BadRequest(ex.ToString());               
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }

        }


        // POST api/values
        [HttpPost("VerifyQrCode")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> VerifyQrCode(string UserName,string key)
        {
            try
            {
                TwoFactorAuth TFAuth = new TwoFactorAuth();

                string sCode = UserName;
                string sKey = string.Empty;

                sKey = key; //TFAuth.CreateSecret(160);
                bool st = TFAuth.VerifyCode(sKey, sCode, 5);
                if (st)
                    return Ok(new BizResponseClass { ReturnCode = enResponseCode.Success, ReturnMsg = "Success" });
                else
                    return Ok(new BizResponseClass { ReturnCode = enResponseCode.Success, ReturnMsg = "Fail" });

            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

    }
}