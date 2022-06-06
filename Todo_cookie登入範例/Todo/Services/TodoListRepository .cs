using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Dtos;
using Todo.Interfaces;
using Todo.Models;
using Todo.Parameters;

namespace Todo.Services
{
    public class TodoListRepository : ITodoListRepository
    {
        private readonly TodoContext _todoContext;

        public TodoListRepository(TodoContext todoContext)
        {
            _todoContext = todoContext;
        }

        public IQueryable<TodoList> 取得資料()
        {
            var result = _todoContext.TodoLists
                .Include(a => a.InsertEmployee)
                .Include(a => a.UpdateEmployee)
                .Include(a => a.UploadFiles)
                .Select(a => a);            

            return result;
        }        
    }
}
