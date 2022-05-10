using System;
using System.Collections.Generic;

#nullable disable

namespace MysqlExample.Models
{
    public partial class List
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
