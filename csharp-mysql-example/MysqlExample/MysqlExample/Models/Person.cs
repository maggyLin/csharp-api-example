using System;
using System.Collections.Generic;

#nullable disable

namespace MysqlExample.Models
{
    public partial class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
