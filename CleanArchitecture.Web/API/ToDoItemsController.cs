using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.ViewModels;
using CleanArchitecture.Web.ApiModels;
using CleanArchitecture.Web.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.Api
{
    [Route("api/[controller]")]
    public class ToDoItemsController : Controller
    {
        private readonly IRepository<ToDoItem> _todoRepository;
        private readonly IMediator _mediator;

        public ToDoItemsController(IRepository<ToDoItem> todoRepository, IMediator mediator)
        {
            _todoRepository = todoRepository;
            _mediator = mediator;
        }
 
        
        // GET: api/ToDoItems
        [HttpGet]
        public IActionResult List()
        {
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
        public async Task<IActionResult> SendMessage(SendSMSRequest Request)
        {
            try
            {
                CommunicationResponse Response = await _mediator.Send(Request);
                return Ok(Response);
            }
            catch(Exception ex)
            {
                return BadRequest(Response);
            }
        }

        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail(SendEmailRequest Request)
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
        public async Task<IActionResult> SendNotification(SendNotificationRequest Request)
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
    }
}