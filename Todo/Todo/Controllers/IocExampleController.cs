using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Interfaces;

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IocExampleController : ControllerBase
    {
        //必須在startup.cs DI注入(constructor才取的到)

        ////Interface與service 1:1
        private readonly IIocExampleService _iocExampleService;
        //Interface與service 1:多
        private readonly IEnumerable<IIocMultipleExampleService> _iocMultipleExampleService; 

        public IocExampleController(IIocExampleService iocExampleService, IEnumerable<IIocMultipleExampleService> iocMultipleExampleService)
        {
            _iocExampleService = iocExampleService;
            _iocMultipleExampleService = iocMultipleExampleService;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _iocExampleService.Get();
        }

        [HttpGet("MultipleIocExample")]
        public IEnumerable<string> MultipleIocExample(int type)
        {
            IIocMultipleExampleService _tmpService;
            if (type == 1)
            {
                _tmpService = _iocMultipleExampleService.Where(a => a.type == 1).Single();
            } 
            else
            {
                _tmpService = _iocMultipleExampleService.Where(a => a.type == 2).Single();
            }

            //_tmpService = _iocMultipleExampleService.Where(a => a.type == type).Single();

            return _tmpService.MultipleIocExample();
        }

    }
}
