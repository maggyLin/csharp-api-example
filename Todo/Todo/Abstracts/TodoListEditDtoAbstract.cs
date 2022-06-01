using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Todo.Dtos;
using Todo.Models;

namespace Todo.Abstracts
{
    //Dto 與 資料驗證寫一起 (分開範例 ValidationAttributes)
    public abstract class TodoListEditDtoAbstract: IValidatableObject
    {
        public string Name { get; set; }
        public bool Enable { get; set; }
        public int Orders { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public List<UploadFilePostDto> UploadFiles { get; set; }

        public TodoListEditDtoAbstract()
        {
            UploadFiles = new List<UploadFilePostDto>();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            TodoContext _todoContext = (TodoContext)validationContext.GetService(typeof(TodoContext));

            var findName = from a in _todoContext.TodoLists
                           where a.Name == Name
                           select a;

            if (this.GetType() == typeof(TodoListPutDto))
            {
                var dtoUpdate = (TodoListPutDto)this;
                findName = findName.Where(a => a.TodoId != dtoUpdate.TodoId);
            }

            if (findName.FirstOrDefault() != null)
            {
                yield return new ValidationResult("已存在相同的代辦事項", new string[] { "Name" });
            }

            if (StartTime >= EndTime)
            {
                yield return new ValidationResult("開始時間不可以大於結束時間", new string[] { "Time" });
            }
        }
    }
}
