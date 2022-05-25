using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Interfaces;

namespace Todo.Services
{
    public class IocMultipleExampleService2 : IIocMultipleExampleService
    {
        public int type => 2;
        public IEnumerable<string> MultipleIocExample()
        {
            return new string[] { "This is ", "example 2." };
        }

    }
}
