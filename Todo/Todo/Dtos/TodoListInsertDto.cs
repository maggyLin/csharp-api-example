using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Todo.Dtos
{
    public class TodoListInsertDto
    {
        //參數驗證 正規
        [RegularExpression("a-z0-9",ErrorMessage = "請輸入a-z0-9")]
        [StringLength(100)]
        public string Name { get; set; }
        public int Orders { get; set; }
    }
}
