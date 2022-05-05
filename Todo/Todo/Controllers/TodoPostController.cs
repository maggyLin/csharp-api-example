using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Dtos;
using Todo.Models;
using Todo.Parameters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoPostController : ControllerBase
    {
        private readonly TodoContext _todoContext;

        public TodoPostController(TodoContext todoContext)
        {
            _todoContext = todoContext;
        }

        // POST api/<TodoController>
        //[HttpPost]
        //public ActionResult<TodoList> Post([FromBody] TodoList value)
        //{
        //    _todoContext.TodoLists.Add(value);
        //    _todoContext.SaveChanges();

        //    return CreatedAtAction(nameof(Get), new { id = value.TodoId }, value);
        //}

        [HttpPost]
        public string Post([FromBody] TodoList value)
        {
            _todoContext.TodoLists.Add(value);
            _todoContext.SaveChanges();

            return "OK";
        }

        [HttpPost("insertEx")]
        public string insertEx([FromBody] TodoListInsertDto value)
        {
            TodoList insertData = new TodoList
            {
                Name = value.Name,
                Enable = true,  //新增時預設都為true
                Orders = value.Orders,
                UpdateTime = DateTime.Now,
                InsertEmployeeId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                UpdateEmployeeId = Guid.Parse("00000000-0000-0000-0000-000000000001")
            };

            _todoContext.TodoLists.Add(insertData);
            _todoContext.SaveChanges();

            return "OK";
        }

        [HttpPost("FKInsertEx")] //有FK , 一起新增關聯Table資料
        public string FKInsertEx([FromBody] TodoList value)
        {
            TodoList insertData = new TodoList
            {
                Name = value.Name,
                Enable = value.Enable,
                Orders = value.Orders,
                UpdateTime = DateTime.Now,
                InsertEmployeeId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                UpdateEmployeeId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                UploadFiles = value.UploadFiles //有建立 FK 關聯
            };

            _todoContext.TodoLists.Add(insertData);
            _todoContext.SaveChanges();

            return "OK";
        }

        [HttpPost("NoFKInsertEx")]  //沒有FK , 一起新增關聯Table資料
        public string NoFKInsertEx([FromBody] TodoList value)
        {
            // 先 insert TodoList data , 取得 GUID
            TodoList insertData = new TodoList
            {
                Name = value.Name,
                Enable = value.Enable,
                Orders = value.Orders,
                UpdateTime = DateTime.Now,
                InsertEmployeeId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                UpdateEmployeeId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            };

            _todoContext.TodoLists.Add(insertData);
            _todoContext.SaveChanges();

            // insert UploadFile table data
            foreach (var tmp in value.UploadFiles)
            {
                UploadFile data = new UploadFile
                {
                    Name = tmp.Name,
                    Src = tmp.Src,
                    TodoId = insertData.TodoId
                };

                _todoContext.UploadFiles.Add(data);
            }

            _todoContext.SaveChanges();

            return "OK";
        }


        [HttpPost("test")]
        public string PostTest()
        {
            return "aaaa";
        }


    }
}
