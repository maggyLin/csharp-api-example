using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Todo.Dtos;
using Todo.Models;
using Todo.Parameters;

namespace Todo.Services
{
    public class TodoListService
    {
        private readonly TodoContext _todoContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TodoListService(TodoContext todoContext, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _todoContext = todoContext;
            _mapper = mapper; 
            _httpContextAccessor = httpContextAccessor;
        }


        public List<TodoListDto> 取得資料(TodoSelectParameter value)
        {
            var result = _todoContext.TodoLists
                .Include(a => a.InsertEmployee)
                .Include(a => a.UpdateEmployee)
                .Include(a => a.UploadFiles)
                .Select(a => a);

            if (!string.IsNullOrWhiteSpace(value.name))
            {
                result = result.Where(a => a.Name.IndexOf(value.name) > -1);
            }

            if (value.enable != null)
            {
                result = result.Where(a => a.Enable == value.enable);
            }

            if (value.InsertTime != null)
            {
                result = result.Where(a => a.InsertTime.Date == value.InsertTime);
            }

            if (value.minOrder != null && value.maxOrder != null)
            {
                result = result.Where(a => a.Orders >= value.minOrder && a.Orders <= value.maxOrder);
            }

            return result.ToList().Select(a => ItemToDto(a)).ToList();
        }

        public TodoListDto 取得單筆資料(Guid TodoId)
        {
            var result = (from a in _todoContext.TodoLists
                          where a.TodoId == TodoId
                          select a)
                .Include(a => a.InsertEmployee)
                .Include(a => a.UpdateEmployee)
                .Include(a => a.UploadFiles)
                .SingleOrDefault();

            if (result != null)
            {
                return ItemToDto(result);
            }

            return null;
        }

        public IEnumerable<TodoListDto> 使用AutoMapper取得資料(TodoSelectParameter value)
        {
            var result = _todoContext.TodoLists
                .Include(a => a.UpdateEmployee)
                .Include(a => a.InsertEmployee)
                .Include(a => a.UploadFiles)
                .Select(a => a);

            if (!string.IsNullOrWhiteSpace(value.name))
            {
                result = result.Where(a => a.Name.IndexOf(value.name) > -1);
            }

            if (value.enable != null)
            {
                result = result.Where(a => a.Enable == value.enable);
            }

            if (value.InsertTime != null)
            {
                result = result.Where(a => a.InsertTime.Date == value.InsertTime);
            }

            if (value.minOrder != null && value.maxOrder != null)
            {
                result = result.Where(a => a.Orders >= value.minOrder && a.Orders <= value.maxOrder);
            }

            return _mapper.Map<IEnumerable<TodoListDto>>(result);
        }

        public TodoList 新增資料(TodoListPostDto value)
        {
            var Claim = _httpContextAccessor.HttpContext.User.Claims.ToList();

            var employeeid = Claim.Where(a => a.Type == "EmployeeId").First().Value;
            TodoList insert = new TodoList
            {
                InsertTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                InsertEmployeeId = Guid.Parse(employeeid),
                UpdateEmployeeId = Guid.Parse(employeeid)
            };

            foreach (var temp in value.UploadFiles)
            {
                insert.UploadFiles.Add(new UploadFile()
                {
                    Src = temp.Src,
                    Name = temp.Name
                });
            }


            _todoContext.TodoLists.Add(insert).CurrentValues.SetValues(value);
            _todoContext.SaveChanges();

            return insert;
        }

        public void 使用AutoMapper新增資料(TodoListPostDto value)
        {
            var map = _mapper.Map<TodoList>(value);

            map.InsertTime = DateTime.Now;
            map.UpdateTime = DateTime.Now;
            map.InsertEmployeeId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            map.UpdateEmployeeId = Guid.Parse("00000000-0000-0000-0000-000000000001");

            _todoContext.TodoLists.Add(map);
            _todoContext.SaveChanges();
        }

        public int 修改資料(Guid id, TodoListPutDto value)
        {
            var update = (from a in _todoContext.TodoLists
                          where a.TodoId == id
                          select a).SingleOrDefault();

            if (update != null)
            {
                update.UpdateTime = DateTime.Now;
                update.UpdateEmployeeId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                _todoContext.TodoLists.Update(update).CurrentValues.SetValues(value);
                
            }
            return _todoContext.SaveChanges();
        }

        public int 刪除資料(Guid id)
        {
            var delete = (from a in _todoContext.TodoLists
                          where a.TodoId == id
                          select a).Include(c => c.UploadFiles).SingleOrDefault();

            if (delete != null)
            {
                _todoContext.TodoLists.Remove(delete);
            }

            return _todoContext.SaveChanges();
        }

        private static TodoListDto ItemToDto(TodoList a)
        {
            List<UploadFileDto> updto = new List<UploadFileDto>();

            foreach (var temp in a.UploadFiles)
            {
                UploadFileDto up = new UploadFileDto
                {
                    Name = temp.Name,
                    Src = temp.Src,
                    TodoId = temp.TodoId,
                    UploadFileId = temp.UploadFileId
                };
                updto.Add(up);
            }


            return new TodoListDto
            {
                Enable = a.Enable,
                InsertEmployeeName = a.InsertEmployee.Name,
                InsertTime = a.InsertTime,
                Name = a.Name,
                Orders = a.Orders,
                TodoId = a.TodoId,
                UpdateEmployeeName = a.UpdateEmployee.Name,
                UpdateTime = a.UpdateTime,
                UploadFiles = updto
            };
        }
    }
}
