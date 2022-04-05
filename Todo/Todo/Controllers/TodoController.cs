using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Dtos;
using Todo.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _todoContext;

        public TodoController(TodoContext todoContext)
        {
            _todoContext = todoContext;
        }

        ////GET: api/<TodoController>
        //[HttpGet]
        //public IEnumerable<TodoList> Get()
        //{
        //    //return new string[] { "value1", "value2" };
        //    var res = _todoContext.TodoLists;
        //    if (res == null)
        //    {
        //        //直接指定http status code
        //        Response.StatusCode = 404;
        //    }
        //    return res;
        //}

        //GET: api/<TodoController>
        //[HttpGet]
        //public IActionResult Get()
        //{
        //    //使用 http status code => IActionResult (不指定資料格式)
        //    //https://docs.microsoft.com/zh-tw/dotnet/api/system.net.httpstatuscode?view=net-5.0

        //    var res = _todoContext.TodoLists;
        //    if (res == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(res);
        //}

        // GET: api/<TodoController>
        [HttpGet]
        public IActionResult Get()
        {
            //使用 DTO(data transfer object) 回傳資料格式化
            // include 有使用到 資料表設定關聯
            var res = _todoContext.TodoLists.Include(a => a.UpdateEmployee).Include(a => a.InsertEmployee)
                .Select(a => new TodoListSelectDto
                {
                    Enable = a.Enable,
                    InsertEmployeeName = a.InsertEmployee.Name,
                    InsertTime = a.InsertTime,
                    Name = a.Name,
                    Orders = a.Orders,
                    TodoId = a.TodoId,
                    UpdateEmployeeName = a.UpdateEmployee.Name,
                    UpdateTime = a.UpdateTime
                });

            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }

        // GET: api/<TodoController>/i/order
        [HttpGet("{i}/{order}")]
        public IEnumerable<TodoList> Get(int i, int order)
        {
            //使用 LINQ
            var result = from a in _todoContext.TodoLists
                         where a.Orders == order
                         select a;
            // LINQ lambda
            var result2 = _todoContext.TodoLists.Where(a => a.Orders == order);

            return result;
        }

        // GET api/<TodoController>/5
        [HttpGet("{id}")]
        public ActionResult<TodoList> Get(Guid id)
        {
            var result = _todoContext.TodoLists.Find(id);
            if (result == null)
            {
                return NotFound("not found.");
            }
            return result;
        }

        // POST api/<TodoController>
        [HttpPost]
        public ActionResult<TodoList> Post([FromBody] TodoList value)
        {
            _todoContext.TodoLists.Add(value);
            _todoContext.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = value.TodoId }, value);
        }

        // PUT api/<TodoController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] TodoList value)
        {

            if (id != value.TodoId)
            {
                return BadRequest();
            }

            _todoContext.Entry(value).State = EntityState.Modified;

            try
            {
                _todoContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_todoContext.TodoLists.Any(e => e.TodoId == id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(500, "存取發生錯誤");
                }
            }

            //更新成功,不回傳任何內容
            return NoContent();
        }

        // DELETE api/<TodoController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var result = _todoContext.TodoLists.Find(id);

            if (result == null)
            {
                return NotFound();
            }

            _todoContext.TodoLists.Remove(result);
            _todoContext.SaveChanges();

            return NoContent();
        }
    }
}
