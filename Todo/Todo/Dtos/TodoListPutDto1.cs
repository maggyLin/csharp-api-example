using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Todo.Dtos
{
    public class TodoListPutDto1
    {
        public Guid TodoId { get; set; }
        public string Name { get; set; }
        public int Orders { get; set; }
    }
}
