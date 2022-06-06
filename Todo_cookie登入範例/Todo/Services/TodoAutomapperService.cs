using AutoMapper;
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
    public class TodoAutomapperService: ITodoListService
    {
        public string type => "automapper";
        private readonly TodoContext _todoContext;
        private readonly IMapper _mapper;
        public TodoAutomapperService(TodoContext todoContext, IMapper mapper)
        {
            _todoContext = todoContext;
            _mapper = mapper;
        }

        public List<TodoListDto> 取得資料(TodoSelectParameter value)
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

            return _mapper.Map<IEnumerable<TodoListDto>>(result).ToList();
        }
    }
}
