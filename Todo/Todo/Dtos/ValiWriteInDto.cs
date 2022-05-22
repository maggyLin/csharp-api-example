using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Todo.Models;
using System.Linq;
using Todo.Dtos;
using System;

namespace Todo.Dtos
{
    //直接在Dto實作資料驗證=>繼承IValidatableObject=>實作Validate
    public class ValiWriteInDto : IValidatableObject
    {
        //使用內建驗證
        [RegularExpression(@"[a-zA-Z0-9]{1,40}$", ErrorMessage = "請輸入a-zA-Z0-9")]
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        //自定義實作資料驗證(in Dto) =>繼承IValidatableObject
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //透過ValidationContext來取得相關service
            TodoContext _todoContext = (TodoContext)validationContext.GetService(typeof(TodoContext));

            var findName = from a in _todoContext.TodoLists
                           where a.Name == Name
                           select a;

            if (findName.FirstOrDefault() != null)
            {
                yield return new ValidationResult("已存在相同的Name", new string[] { "name err" });
            }

            if (StartTime > EndTime)
            {
                yield return new ValidationResult("The start time must less than the end time.", new string[] { "time err" });
            }

        }
    }
}
