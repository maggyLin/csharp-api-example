using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Dtos;
using Todo.Interfaces;
using Todo.Parameters;
using Todo.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoRController : ControllerBase
    {
        private readonly ITodoListService _todoListService;
        public TodoRController(ITodoListService todoListService)
        {
            _todoListService = todoListService;
        }

        // GET: api/<TodoIOCController>
        [HttpGet]
        public List<TodoListDto> Get([FromQuery]TodoSelectParameter value)
        {           
            return _todoListService.取得資料(value);
        }
    }
}
