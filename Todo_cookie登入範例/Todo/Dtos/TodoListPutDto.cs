using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Todo.Abstracts;
using Todo.Models;
using Todo.ValidationAttributes;

namespace Todo.Dtos
{
    public class TodoListPutDto: TodoListEditDtoAbstract
    {
        public Guid TodoId { get; set; }
    }
}
