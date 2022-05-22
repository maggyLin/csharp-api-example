using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using Todo.Dtos;
using Todo.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoDeleteController : ControllerBase
    {

        private readonly TodoContext _todoContext;

        public TodoDeleteController(TodoContext todoContext)
        {
            _todoContext = todoContext;
        }

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

        [HttpDelete("DeleteDataWithFK/{id}")]
        public IActionResult DeleteDataWithFK(Guid id)
        {
            //有設定FK時,刪除父資料"未同步"刪除子資料可能會報錯
            // .Include(c=>c.UploadFiles) => 同步刪除子資料

            var result = (from a in _todoContext.TodoLists
                          where a.TodoId == id
                          select a).Include(c=>c.UploadFiles).SingleOrDefault();

            if (result == null)
            {
                return NotFound();
            }

            //這邊只會有一筆(SingleOrDefault)=>Remove
            //如果多筆資料=>RemoveRange()
            _todoContext.TodoLists.Remove(result);
            _todoContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("MultipleData")]
        public IActionResult MultipleData(string ids)
        {
            //傳入string 做序列化
            //https://docs.microsoft.com/zh-tw/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-5-0

            List<Guid> dels = JsonSerializer.Deserialize<List<Guid>>(ids);

            var result = (from a in _todoContext.TodoLists
                          where dels.Contains(a.TodoId)
                          select a).Include(c => c.UploadFiles);

            //如果多筆資料=>RemoveRange()
            _todoContext.TodoLists.RemoveRange(result);
            _todoContext.SaveChanges();

            return NoContent();
        }

    }
}
