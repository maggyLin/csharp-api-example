﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Dtos;
using Todo.Interfaces;
using Todo.Parameters;
using Todo.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsyncController : ControllerBase
    {
        private readonly AsyncService _asyncService;
        public AsyncController(AsyncService asyncService)
        {
            _asyncService = asyncService;
        }

        [HttpGet]
        public async Task<int> Get()
        {          
            return await _asyncService.主作業();
        }
    }
}
