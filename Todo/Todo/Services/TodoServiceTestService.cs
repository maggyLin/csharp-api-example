using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Dtos;
using Todo.Models;
using Todo.Parameters;

namespace Todo.Services
{
    public class TodoServiceTestService
    {
        private readonly TodoContext _todoContext;

        public TodoServiceTestService(TodoContext todoContext)
        {
            _todoContext = todoContext;
        }

        public List<TodoListSelectDto> Get(string name, bool? enable)
        {
            var res = _todoContext.TodoLists.Include(a => a.UpdateEmployee).Include(a => a.InsertEmployee).Select(a => a);

            if (!string.IsNullOrWhiteSpace(name))
            {
                res = res.Where(a => a.Name.Contains(name));
            }

            if (enable != null)
            {
                res = res.Where(a => a.Enable == enable);
            }

            return res.Select(a => ItemToDto(a)).ToList();
        }

        public List<TodoListSelectDto> GetByParameter(TodoSelectParameter value)
        {
            var res = _todoContext.TodoLists.Include(a => a.UpdateEmployee).Include(a => a.InsertEmployee).Select(a => a);

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

            return res.Select(a => ItemToDto(a)).ToList();
        }

        private static TodoListSelectDto ItemToDto(TodoList item)
        {
            if(item == null) return null;
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

        public int Put(TodoListPutDto1 value) 
        {
            var update = (from a in _todoContext.TodoLists
                          where a.TodoId == value.TodoId
                          select a).SingleOrDefault();

            if (update == null) return 0;
            else
            {
                update.UpdateTime = DateTime.Now;
                update.Name = value.Name;
                update.Orders = value.Orders;
            }

            //回傳修改筆數
            return _todoContext.SaveChanges();
        }

    }
}
