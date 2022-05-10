# 在 Visual Studio Code 下執行專案

### 參考資料 : 
1. https://blog.darkthread.net/blog/write-aspnetcore-with-vscode/
2. https://docs.microsoft.com/zh-tw/dotnet/core/tools/dotnet-new

### 建立 .net6 webapi project
1. vscode 的 extensions 要有 Microsoft C# 模組 (ms-dotnettools.csharp)

2. 創建webapi dotnet new webapi [-n|--name <OUTPUT_NAME>]

3. 專案路徑下要有 .vscode 資料夾 設定檔 launch.json 與 tasks.json (參考專案內資料)

4. Properties/launchSettings.json 檔案
5. swagger修改為http (不然localhost會開不起來??)
```
"Todo": {
        "commandName": "Project",
        "dotnetRunMessages": "true",
        "launchBrowser": true,
        "launchUrl": "swagger",
        //"applicationUrl": "https://localhost:5001;http://localhost:5000",
        "applicationUrl": "http://localhost:5000",
        "environmentVariables": {
            "ASPNETCORE_ENVIRONMENT": "Development"
        }
    }
```

### 發布
* dotnet publish --configuration Release

### CORS 解決本地端開發跨網域問題
* 官方文件 : https://docs.microsoft.com/zh-tw/aspnet/core/security/cors?view=aspnetcore-6.0
* Program.cs 修改 

* 方法1:(參考官方文件位置)
1. var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

2. builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
                          policy =>
                          {
                              policy.WithOrigins("*")
                                    .AllowAnyHeader()
                                    .AllowAnyMethod();
                          });
});

3. app.UseCors(MyAllowSpecificOrigins);

* 方法2:
1. builder.Services.AddCors();
2. app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());


### swagger 正式環境需要 => Program.cs 
* 是否有判斷環境 if (app.Environment.IsDevelopment()) ??

### HTTP Error 500.35
* https://blog.no2don.com/2021/05/netcore-net-core-31-http-error-50035.html
* web.config文件中的hostingModel=“inprocess” 改为hostingModel="OutOfProcess"或者直接删掉hostingModel=“inprocess”
* 或是直接換台伺服器