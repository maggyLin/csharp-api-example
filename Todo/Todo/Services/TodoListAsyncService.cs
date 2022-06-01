using Microsoft.Data.SqlClient;
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
    public class TodoListAsyncService
    {
        private readonly TodoContext _todoContext;
        public TodoListAsyncService(TodoContext todoContext)
        {
            _todoContext = todoContext;
        }


        public async Task<IEnumerable<TodoList>> 取得資料(int order)
        {
            var result = from a in _todoContext.TodoLists
                         where a.Orders == order
                         select a;

            var temp = await result.ToListAsync();
            return temp;
        }

        public async Task 新增資料(TodoListPostDto value)
        {
            TodoList insert = new TodoList
            {
                InsertTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                InsertEmployeeId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                UpdateEmployeeId = Guid.Parse("00000000-0000-0000-0000-000000000001")
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
            await _todoContext.SaveChangesAsync();
        }

    }
}
