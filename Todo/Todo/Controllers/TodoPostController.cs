using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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

        [HttpPost("insertDtoEx")]
        public string insertDtoEx([FromBody] TodoListInsertDto value)
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

        [HttpPost("insertAutoMap")]
        public string insertAutoMap([FromBody] TodoListInsertDto value)
        {
            TodoList insertData = new TodoList
            {
                Enable = true,
                UpdateTime = DateTime.Now,
                InsertEmployeeId = Guid.Parse("59308743-99e0-4d5a-b611-b0a7facaf21e"),
                UpdateEmployeeId = Guid.Parse("00000000-0000-0000-0000-000000000001")
            };

            // Add : 系統決定欄位 , SetValues 自動匹配欄位(注意欄位名稱)
            _todoContext.TodoLists.Add(insertData).CurrentValues.SetValues(value);
            _todoContext.SaveChanges();

            return "OK";
        }

        // 小心sql injection
        [HttpPost("insertBySqlScript")]
        public string insertBySqlScript([FromBody] TodoListInsertDto value)
        {
            string sql = @"INSERT INTO [dbo].[TodoList] (
                     [Name], [InsertTime], [UpdateTime], [Enable], [Orders], [InsertEmployeeId], [UpdateEmployeeId]) 
                    VALUES ( N'"+value.Name+ "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','true','" + value.Orders + "', '00000000-0000-0000-0000-000000000001', '59308743-99e0-4d5a-b611-b0a7facaf21e')";

            _todoContext.Database.ExecuteSqlRaw(sql);

            return "OK";
        }

        [HttpPost("insertBySqlParamter")]
        public string insertBySqlParamter([FromBody] TodoListInsertDto value)
        {
            //avoid sql injection
            var name = new SqlParameter("name", value.Name);
            var order = new SqlParameter("order", value.Orders);

            string sql = @"INSERT INTO [dbo].[TodoList] (
                     [Name], [InsertTime], [UpdateTime], [Enable], [Orders], [InsertEmployeeId], [UpdateEmployeeId]) 
                    VALUES ( @name, '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','true', @order , '00000000-0000-0000-0000-000000000001', '59308743-99e0-4d5a-b611-b0a7facaf21e')";

            _todoContext.Database.ExecuteSqlRaw(sql,name, order);

            return "OK";
        }

        //參考資料 : 
        /* 
           {
              "name": "FKTest01",
              "enable": true,
	            "insertTime": "2021-02-14T17:03:50",
                "updateTime": "2021-02-12T17:03:50.083",
              "orders": 10,
              "InsertEmployeeId":"00000000-0000-0000-0000-000000000001",
              "UpdateEmployeeId":"59308743-99e0-4d5a-b611-b0a7facaf21e",
              "uploadFiles": [
                {
                  "name": "file001",
                  "src": "src/file001",
                }
              ]
            }
         */

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
