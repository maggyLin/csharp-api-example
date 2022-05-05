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
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _todoContext;

        public TodoController(TodoContext todoContext)
        {
            _todoContext = todoContext;
        }

        //GET: api/<TodoController>
        [HttpGet("specificHttpCode")]
        public IEnumerable<TodoList> Get(int i)
        {
            //return new string[] { "value1", "value2" };
            var res = _todoContext.TodoLists;
            if (res == null)
            {
                //直接指定http status code
                Response.StatusCode = 404;
            }
            return res;
        }

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
                    InsertEmployeeName = a.InsertEmployee.Name, //資料表要有設定外部鍵,才能抓取,不然要使用join
                    InsertTime = a.InsertTime,
                    Name = a.Name,
                    Orders = a.Orders,
                    TodoId = a.TodoId,
                    UpdateEmployeeName = a.UpdateEmployee.Name,
                    UpdateTime = a.UpdateTime
                });

            if (res == null || res.Count()==0)
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
        public TodoListSelectDto Get(Guid id)
        {
            var result = (from a in _todoContext.TodoLists
                          join b in _todoContext.Employees on a.InsertEmployeeId equals b.EmployeeId
                          join c in _todoContext.Employees on a.UpdateEmployeeId equals c.EmployeeId
                          where a.TodoId == id
                          select new TodoListSelectDto
                          {
                              Enable = a.Enable,
                              InsertEmployeeName = b.Name,
                              InsertTime = a.InsertTime,
                              Name = c.Name,
                              Orders = a.Orders,
                              TodoId = a.TodoId,
                              UpdateEmployeeName = a.UpdateEmployee.Name,
                              UpdateTime = a.UpdateTime
                          }).SingleOrDefault();  //只有1筆資料或空回傳204
            return result;
        }

        [HttpGet("keyword")]
        public IEnumerable<TodoListSelectDto> Get(string name, bool? enable, DateTime? updateTime)
        {
            //var res = _todoContext.TodoLists.Include(a => a.UpdateEmployee).Include(a => a.InsertEmployee)
            //    .Select(a => new TodoListSelectDto
            //    {
            //        Enable = a.Enable,
            //        InsertEmployeeName = a.InsertEmployee.Name,
            //        InsertTime = a.InsertTime,
            //        Name = a.Name,
            //        Orders = a.Orders,
            //        TodoId = a.TodoId,
            //        UpdateEmployeeName = a.UpdateEmployee.Name,
            //        UpdateTime = a.UpdateTime
            //    });

            //函式化DTO - 不建議在中間做轉換格式(下面where容易抱錯)->return 前
            var res = _todoContext.TodoLists.Include(a => a.UpdateEmployee).Include(a => a.InsertEmployee)
                //.Select(a => ItemToDto(a));
                .Select(a => a);

            if (!string.IsNullOrWhiteSpace(name))
            {
                res = res.Where(a => a.Name.Contains(name));
            }

            if (enable != null)
            {
                res = res.Where(a => a.Enable == enable);
            }

            if (updateTime != null)
            {
                res = res.Where(a => a.UpdateTime.Date == updateTime);
            }

            return res.ToList().Select(a => ItemToDto(a));
        }

        [HttpGet("keywordByParameter")]
        public IEnumerable<TodoListSelectDto> Get([FromQuery] TodoSelectParameter value)
        {
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

            if (!string.IsNullOrWhiteSpace(value.name))
            {
                res = res.Where(a => a.Name.Contains(value.name));
            }

            if (value.enable != null)
            {
                res = res.Where(a => a.Enable == value.enable);
            }

            if (value.updateTime != null)
            {
                res = res.Where(a => a.UpdateTime.Date == value.updateTime);
            }

            if (value.minOrder != null && value.maxOrder != null)
            {
                res = res.Where(a => a.Orders >= value.minOrder && a.Orders <= value.maxOrder);
            }

            return res;
        }

        [HttpGet("includeUploadFile")]
        public TodoListSelectDto Get(int i,Guid id)
        {
            //抓取對應todoid的uploadifle 資料
            var res = (from a in _todoContext.TodoLists
                       where a.TodoId == id
                       select new TodoListSelectDto
                       {
                           Enable = a.Enable,
                           InsertEmployeeName = a.InsertEmployee.Name,
                           InsertTime = a.InsertTime,
                           Name = a.Name,
                           Orders = a.Orders,
                           TodoId = a.TodoId,
                           UpdateEmployeeName = a.UpdateEmployee.Name,
                           UpdateTime = a.UpdateTime,
                           UploadFiles = ( from b in _todoContext.UploadFiles
                                           where a.TodoId == b.TodoId 
                                           select new UploadFileDto
                                           {
                                               Name = b.Name,
                                               Src = b.Src,
                                               TodoId = b.TodoId,
                                               UploadFileId = b.UploadFileId
                                           }).ToList()
                       }).SingleOrDefault();
                       
            return res;
        }


        [HttpGet("FromExample/{id}")]
        //使用 [FromXXXX] 指定參數如何傳遞 , 沒有給定會自動預設
        //EX : [FromQuery] FromExample/{id}?id=XXX (會抓取?後面給的id)
        //EX : [FromRoute] FromExample/id (接在路徑後面)
        //不同的參數也可以指定不同方式(一樣參數前面[]指定)
        public dynamic GetFrom([FromQuery] string val)
        {
            return val;
        }

        //直接使用sql語法,不使用LINQ
        [HttpGet("RowSqlEx")]
        public IEnumerable<JobTitle> RowSqlEx()
        {
            return _todoContext.JobTitles.FromSqlRaw(" select * from JobTitle ");
        }

        //直接使用sql語法,不使用LINQ , 並指定自己設定資料格式
        [HttpGet("RowSqlExDto")]
        public IEnumerable<TodoListSelectDto2> RowSqlExDto()
        {
            string sql = @" select TodoId,
                            a.Name,
                            InsertTime,
                            UpdateTime,
                            Enable,
                            Orders,
                            b.Name as InsertEmployeeName , 
                            c.Name as UpdateEmployeeName 
                            from TodoList a 
                            join Employee b on a.InsertEmployeeId = b.EmployeeId
                            join Employee c on a.UpdateEmployeeId = c.EmployeeId ";

            //一定要擴充TodoContexX.cs的方法才行
            //注意抓出來資料格式要跟指定DTO相同
            return _todoContext.ExecSQL<TodoListSelectDto2>(sql);
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

        private static TodoListSelectDto ItemToDto(TodoList item)
        {
            return new TodoListSelectDto
            {
                Enable = item.Enable,
                InsertEmployeeName = item.InsertEmployee.Name,
                InsertTime = item.InsertTime,
                Name = item.Name,
                Orders = item.Orders,
                TodoId = item.TodoId,
                UpdateEmployeeName = item.UpdateEmployee.Name,
                UpdateTime = item.UpdateTime
            };
        }

    }
}
