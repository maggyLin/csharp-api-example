using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Todo.ValidationAttributes;

namespace Todo.Dtos
{
    public class TodoListInsertDto
    {
        //參數驗證 正規
        [RegularExpression(@"[a-zA-Z0-9]{1,40}$", ErrorMessage = "請輸入a-zA-Z0-9")]
        [Required]
        //自定義資料驗證 (檔案結尾為Attribute可以省略)
        [TodoName]
        public string Name { get; set; }
        public int Orders { get; set; }
    }
}
