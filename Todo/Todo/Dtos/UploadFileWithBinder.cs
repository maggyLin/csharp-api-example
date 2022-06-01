using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Todo.Abstracts;
using Todo.Models;
using Todo.ValidationAttributes;
using Todo.Binder;

namespace Todo.Dtos
{
    public class UploadFileWithBinder
    {
        //自訂處理 (模型繫結器Binder) ,下列為Data做序列化(string to Json)
        [ModelBinder(BinderType = typeof(FormDataJsonBinder))]
        public TodoListPostDto TodoList { get; set; }
        public IFormFileCollection files { get; set; }
    }
}
