using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Dtos;
using Todo.Interfaces;
using Todo.Parameters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoIOCController : ControllerBase
    {
        private readonly IEnumerable<ITodoListService> _todoListService;
        public TodoIOCController(IEnumerable<ITodoListService> todoListService)
        {
            _todoListService = todoListService;
        }

        // GET: api/<TodoIOCController>
        [HttpGet]
        public List<TodoListDto> Get([FromQuery]TodoSelectParameter value)
        {
            ITodoListService _todo;
            if (value.type=="fun")
            {
                _todo = _todoListService.Where(a => a.type == "fun").Single();
            }
            else
            {
                _todo = _todoListService.Where(a => a.type == "automapper").Single();
            }


            return _todo.取得資料(value);
        }
    }
}
