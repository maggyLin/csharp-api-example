using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Todo.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly TodoContext _todoContext;
        private readonly IWebHostEnvironment _env;
        public FileUploadController(TodoContext todoContext, IWebHostEnvironment env)
        {
            _env = env;
            _todoContext = todoContext;
        }

        [HttpPost]
        public void Post([FromForm] IFormFileCollection files,[FromForm]Guid id)
        {
            string rootRoot = _env.ContentRootPath + @"\wwwroot\UploadFiles\"+id+"\\";

            if (!Directory.Exists(rootRoot))
            {
                Directory.CreateDirectory(rootRoot);
            }

            foreach(var file in files)
            {
                string fileName = file.FileName;

                using (var stream = System.IO.File.Create(rootRoot + fileName))
                {
                    file.CopyTo(stream);

                    var insert = new UploadFile
                    {
                        Name = fileName,
                        Src = "/UploadFiles/" + id + "/" + fileName,
                        TodoId = id
                    };

                    _todoContext.UploadFiles.Add(insert);
                }
            }

            _todoContext.SaveChanges();
        }
    }
}
