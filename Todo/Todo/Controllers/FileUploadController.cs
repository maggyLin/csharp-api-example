using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using Todo.Models;
using System.Text.Json;
using Todo.Dtos;
using System;
using Todo.Binder;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {

        //單一檔案：IFormFile
        //複數檔案：IFormFileCollection、IEnumerable<IFormFile>、List<IFormFile>、ICollection<IFormFile>

        //取得專案位置可以使用注入IWebHostEnvironment，_env.ContentRootPath來取得
        private readonly IWebHostEnvironment _env;

        private readonly TodoContext _todoContext;

        public FileUploadController(IWebHostEnvironment env, TodoContext todoContext)
        {
            _env = env;
            _todoContext = todoContext;
        }

        // POST api/<FileUploadController>
        [HttpPost("OnlyFile")]
        public void OnlyFile(ICollection<IFormFile> files)
        {
            //存放在根目錄下的 wwwroot (預設靜態目錄) 
            var rootPath = _env.ContentRootPath + @"\wwwroot\";

            //先確認檔案位置是否存在
            if (!Directory.Exists(rootPath))
            {
                //不存在 先創造資料夾
                Directory.CreateDirectory(rootPath);
            }

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var filePath = file.FileName;
                    using (var stream = System.IO.File.Create(rootPath + filePath))
                    {
                        file.CopyTo(stream);
                    }
                }
            }
        }

        [HttpPost("MultipleDataWithBinder")]
        //public void MultipleDataWithBinder([FromForm][ModelBinder(BinderType = typeof(FormDataJsonBinder))] TodoListPostDto value, IFormFileCollection files)
        public void MultipleDataWithBinder([FromForm] UploadFileWithBinder value)
        {
            //資料無法進入binder???? 一直報 throw new ArgumentNullException ?? binder抓不到資料 ?? 不知哪裡問題
            TodoList insert = new TodoList
            {
                InsertTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                InsertEmployeeId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                UpdateEmployeeId = Guid.Parse("00000000-0000-0000-0000-000000000001")
            };

            _todoContext.TodoLists.Add(insert).CurrentValues.SetValues(value.TodoList);
            _todoContext.SaveChanges();

            //直接寫入沒有問題
            //TodoList insert = new TodoList
            //{
            //    Name = "aaa",
            //    Enable = true,
            //    Orders = 10,
            //    InsertTime = DateTime.Now,
            //    UpdateTime = DateTime.Now,
            //    InsertEmployeeId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            //    UpdateEmployeeId = Guid.Parse("00000000-0000-0000-0000-000000000001")
            //};
            //_todoContext.TodoLists.Add(insert);
            //_todoContext.SaveChanges();


            //file 寫入
            string rootRoot = _env.ContentRootPath + @"\wwwroot\UploadFiles\" + insert.TodoId + @"\";

            if (!Directory.Exists(rootRoot))
            {
                Directory.CreateDirectory(rootRoot);
            }

            foreach (var file in value.files)
            {
                string fileName = file.FileName;

                using (var stream = System.IO.File.Create(rootRoot + fileName))
                {
                    file.CopyTo(stream);

                    var insert2 = new UploadFile
                    {
                        Name = fileName,
                        Src = "/UploadFiles/" + insert.TodoId + "/" + fileName,
                        TodoId = insert.TodoId
                    };

                    _todoContext.UploadFiles.Add(insert2);
                }
            }

            _todoContext.SaveChanges();
        }

        [HttpPost("JsonStringData")]
        public string JsonStringData(string value)
        {
            // value example : {"Name":"Test","Orders":2}

            //傳入string 做序列化
            //https://docs.microsoft.com/zh-tw/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-5-0
            TodoListInsertDto data = JsonSerializer.Deserialize<TodoListInsertDto>(value);

            return data.Name + " : " + data.Orders;

        }

    }
}
