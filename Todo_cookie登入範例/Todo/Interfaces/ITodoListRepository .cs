using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Dtos;
using Todo.Models;
using Todo.Parameters;

namespace Todo.Interfaces
{
    public interface ITodoListRepository
    {
        IQueryable<TodoList> 取得資料();
    }
}
