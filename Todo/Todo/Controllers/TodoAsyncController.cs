using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Dtos;
using Todo.Interfaces;
using Todo.Parameters;
using Todo.Services;
using Todo.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoAsyncController : ControllerBase
    {
        private readonly TodoListAsyncService _todoListService;
        public TodoAsyncController(TodoListAsyncService todoListService)
        {
            _todoListService = todoListService;
        }

        [HttpGet]
        public async Task<IEnumerable<TodoList>> Get(int order)
        {          
            return await _todoListService.取得資料(order);
        }

        [HttpPost]
        public async Task Post([FromBody] TodoListPostDto value)
        {
            await _todoListService.新增資料(value);
        }
    }
}
