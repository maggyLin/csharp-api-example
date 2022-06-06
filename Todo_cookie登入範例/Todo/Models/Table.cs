using System;
using System.Collections.Generic;

#nullable disable

namespace Todo.Models
{
    public partial class Table
    {
        public Guid RoleId { get; set; }
        public Guid EmployeeId { get; set; }
        public string Name { get; set; }
    }
}
