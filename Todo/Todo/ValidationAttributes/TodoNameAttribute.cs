using System.ComponentModel.DataAnnotations;
using Todo.Models;
using System.Linq;
using Todo.Dtos;

namespace Todo.ValidationAttributes
{
    //自定義資料驗證
    public class TodoNameAttribute : ValidationAttribute
    {
        //參考使用到 Dtos/TodoListInsertDto
        //object value => 要驗證的資料
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //透過ValidationContext來取得相關service
            TodoContext _todoContext = (TodoContext)validationContext.GetService(typeof(TodoContext));

            //判斷是否已經有這個名稱了 => 回傳錯誤訊息
            var name = (string)value;

            var findName = from a in _todoContext.TodoLists
                           where a.Name == name
                           select a;

            //如果有其他Dto有用到,可以判斷目前使用Dto而不同判斷
            //var dto = validationContext.ObjectInstance;
            //if (dto.GetType() == typeof(TodoListPutDto1))

            if (findName.FirstOrDefault() != null)
            {
                return new ValidationResult("已存在相同的代辦事項");
            }

            return ValidationResult.Success;
        }

    }
}
