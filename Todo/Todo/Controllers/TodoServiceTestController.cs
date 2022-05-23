using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Services;
using Todo.Dtos;
using Todo.Parameters;

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoServiceTestController : ControllerBase
    {
        //必須在startup.cs DI注入(constructor才取的到)
        private readonly TodoServiceTestService _todoServiceTestService;

        public TodoServiceTestController(TodoServiceTestService todoServiceTestService)
        {
            _todoServiceTestService = todoServiceTestService;
        }

        [HttpGet]
        public IActionResult Get(string name, bool? enable)
        {
            var result = _todoServiceTestService.Get(name, enable);

            if(result == null || result.Count == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("GetByParameter")]
        public IActionResult GetByParameter([FromQuery] TodoSelectParameter value)
        {
            var result = _todoServiceTestService.GetByParameter(value);

            if (result == null || result.Count == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut]
        public IActionResult Put([FromBody] TodoListPutDto1 value)
        {
            int updataNum = _todoServiceTestService.Put(value);
            if(updataNum == 0) return NotFound("未更新資料");
            
            return NoContent();
        }




    }
}
