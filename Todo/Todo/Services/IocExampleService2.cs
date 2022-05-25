using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Interfaces;

namespace Todo.Services
{
    public class IocExampleService2 : IIocExampleService
    {
        public IEnumerable<string> Get()
        {
            return new string[] { "This is ", "IocExampleService 2" };
        }

    }
}
