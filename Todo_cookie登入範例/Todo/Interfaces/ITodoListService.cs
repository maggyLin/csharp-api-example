using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Dtos;
using Todo.Parameters;

namespace Todo.Interfaces
{
    public interface ITodoListService
    {
        string type { get; }
        List<TodoListDto> 取得資料(TodoSelectParameter value);
    }
}
