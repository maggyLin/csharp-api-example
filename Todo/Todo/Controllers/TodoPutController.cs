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
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoPutController : ControllerBase
    {
        private readonly TodoContext _todoContext;

        public TodoPutController(TodoContext todoContext)
        {
            _todoContext = todoContext;
        }


        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] TodoList value)
        {

            if (id != value.TodoId)
            {
                return BadRequest();
            }

            //_todoContext.Entry(value).State = EntityState.Modified;
            _todoContext.TodoLists.Update(value);

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

            //更新成功,不回傳任何內容 httpcode=204
            return NoContent();
        }

        [HttpPut("PutDto")]
        public IActionResult PutDto([FromBody] TodoListPutDto1 value)
        {
            //var update = _todoContext.TodoLists.Find(id);
            var update = (from a in _todoContext.TodoLists
                          where a.TodoId == value.TodoId
                          select a).SingleOrDefault();

            if (update == null) return BadRequest();
            else
            {
                update.UpdateTime = DateTime.Now;
                update.Name = value.Name;
                update.Orders = value.Orders;
                _todoContext.SaveChanges();
            }
            return NoContent();
        }

        [HttpPut("PutAutoSetVal")]
        public IActionResult PutAutoSetVal([FromBody] TodoListPutDto1 value)
        {
            var update = (from a in _todoContext.TodoLists
                          where a.TodoId == value.TodoId
                          select a).SingleOrDefault();

            if (update == null) return NotFound();
            else
            {
                update.UpdateTime = DateTime.Now;

                // SetValues 自動匹配欄位(注意欄位名稱)
                _todoContext.TodoLists.Update(update).CurrentValues.SetValues(value);
                _todoContext.SaveChanges();
                return NoContent();
            }

        }

        //多筆資料一起驗證
        [HttpPut("ValiTest")]
        public string ValiTest([FromBody] ValiTestDto value)
        {
            return "OK";
        }

        //多筆資料一起驗證
        [HttpPut("ValiWriteInDtoTest")]
        public string ValiWriteInDtoTest([FromBody] ValiWriteInDto value)
        {
            return "OK";
        }
    }
}
