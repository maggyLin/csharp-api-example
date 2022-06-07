using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Todo.Dtos;
using Todo.Models;

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly TodoContext _todoContext;
        private readonly IConfiguration _configuration;

        public LoginController(TodoContext todoContext, IConfiguration configuration)
        {
            _todoContext = todoContext;
            _configuration = configuration;
        }

        //參考 : https://talllkai.coderbridge.io/2021/09/05/JWT/
        //Startup.cs 要注入 JWT (參考startup)
        [HttpPost("JWTLoginExample")]
        public string login(LoginPost value)
        {
            var user = (from a in _todoContext.Employees
                        where a.Account == value.Account
                        && a.Password == value.Password
                        select a).SingleOrDefault();

            if (user == null)
            {
                return "帳號密碼錯誤";
            }
            else
            {
                //設定使用者資訊
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Email, user.Account),
                    new Claim("FullName", user.Name),
                    new Claim("EmployeeId", user.EmployeeId.ToString())
                };

                //設定 user 角色 (設定 : [Authorize(Roles ="admin")]  )
                claims.Add(new Claim(ClaimTypes.Role, "admin"));
                //可以抓取table存入
                //var role = from a in _todoContext.Roles
                //           where a.EmployeeId == user.EmployeeId
                //           select a;
                //foreach (var temp in role)
                //{
                //    claims.Add(new Claim(ClaimTypes.Role, temp.Name));
                //}

                //獲取 appsettings.json JWT KEY
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:KEY"]));

                //設定jwt相關資訊
                var jwt = new JwtSecurityToken
                (
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                );

                //產生JWT Token
                var token = new JwtSecurityTokenHandler().WriteToken(jwt);

                return token;
            }
        }

        
        [HttpGet("getDataWithAuth")]
        [Authorize]
        public IEnumerable<TodoList> GetDataWithAuth()
        {
            var result = from a in _todoContext.TodoLists select a;
            return result;
        }

        //限定角色才可使用
        [HttpGet("getDataWithAuthRole")]
        [Authorize(Roles ="admin")]  
        public IEnumerable<TodoList> GetDataWithAuthRole()
        {
            var result = from a in _todoContext.TodoLists select a;
            return result;
        }


        public class LoginPost
        {
            public string Account { get; set; }
            public string Password { get; set; }
        }



    }
}
