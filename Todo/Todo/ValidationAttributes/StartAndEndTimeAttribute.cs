using System.ComponentModel.DataAnnotations;
using Todo.Models;
using System.Linq;
using Todo.Dtos;

namespace Todo.ValidationAttributes
{
    public class StartAndEndTimeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var st = (ValiTestDto)value;
            if(st.StartTime> st.EndTime)
            {
                //(err message, err title)
                return new ValidationResult("The start time must less than the end time.", new string[] { "time err" });
            }
            return ValidationResult.Success;
        }

    }
}
