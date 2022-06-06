using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Todo.Dtos;
using Todo.Models;

namespace Todo.ValidationAttributes
{
    public class StartEndAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            var st = (TodoListPostDto)value;

            if (st.StartTime >= st.EndTime)
            {
                return new ValidationResult("開始時間不可以大於結束時間", new string[] { "time" });
            }

            return ValidationResult.Success;
        }
    }
}
