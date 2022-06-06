### 參考 : https://talllkai.coderbridge.io/2021/08/22/CookieAuthentication/

### 首先我們先到Startup.cs加入以下設定
```
public void ConfigureServices(IServiceCollection services)
{
    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
    {
        //未登入時會自動導到這個網址
        option.LoginPath = new PathString("/api/Login/NoLogin");
        //沒有權限時會自動導到這個網址
        option.AccessDeniedPath = new PathString("/api/Login/NoAccess");
        //權限失效時間
        //option.ExpireTimeSpan = TimeSpan.FromSeconds(2);
    });        
}
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
   //順序要一樣
   app.UseCookiePolicy();
   app.UseAuthentication();
   app.UseAuthorization();
}
```

### LoginController
```
[HttpPost]
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
        var claims = new List<Claim>
            {
                //定義登入資訊
                //Conroller中可以使用 HttpContext.User.Claims取得資料
                //var Claim = HttpContext.User.Claims.ToList();
                //var type = Claim.Where(a=>a.Type=="type").First().Value;
                new Claim(ClaimTypes.Name, user.Account),
                new Claim("FullName", user.Name),
                new Claim("type", "test"),
                //定義使用者角色
                //contoller上面指定角色 [Authorize(Role='Administrator')]
                new Claim(ClaimTypes.Role, "Administrator")
            };
        var claimsIdentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(claimsIdentity));
    }
}
public class LoginPost{
    public string Account { get; set; }
    public string Password { get; set; }
}

[HttpDelete]
public void logout()
{     
    HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
}

[HttpGet("NoLogin")]
public string noLogin()
{
    return "未登入";
}

```

### 套用登入 Startup.cs設定全域套用
```
services.AddMvc(options =>
{
    options.Filters.Add(new AuthorizeFilter());
});
```

### 不需要驗證API加入[AllowAnonymous]
> EX : LoginController
```
[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class LoginController : ControllerBase
{
}
```