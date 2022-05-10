## 此專案為.net5 , 套件版本請參考[專案名稱].csproj

## 1.參考資料 : 
* https://ithelp.ithome.com.tw/articles/10240045?sc=hot
* https://codingnote.cc/zh-tw/p/372703/

## 2.安裝套件
-- framwork NuGet
* Pomelo.EntityFrameworkCore.MySql 
* Microsoft.EntityFrameworkCore.Design

-- cmd(vs code) , 請注意套件版本與.net版本是否支援
* dotnet tool install --global dotnet-ef (請先安裝此套件,才能使用dotnet ef的指令)
* dotnet add package Pomelo.EntityFrameworkCore.MySql [-v|--version <VERSION>]
* dotnet add package Microsoft.EntityFrameworkCore.Design [-v|--version <VERSION>]


-- 套件查看
* 檔案 : [專案名稱].csproj
* cmd : dotnet list package

## 3.使用 DataBase First (資料庫建立完成匯入專案)
* 使用指令產生EF實體,直接產生對應Model

```
dotnet ef dbcontext scaffold "server=localhost;Port=3306;Database=Blog; User=root;Password=;" "Pomelo.EntityFrameworkCore.MySql" -o ./Models -c BlogContext -f
```
> -o 與輸出資料夾沒有填的話，會在當前目錄產生檔案
> -c 與名稱Context沒有填的話，會以Database的名稱作為DbContext名稱

## 4.資料庫帳密使用config檔注入
* 將Model中 [XXX]Context.cs 中 "optionsBuilder.UseSqlServer()"方法刪掉
* appsettings.json 中寫入連線資訊
```
    "ConnectionStrings": {
        "MySQL": "server=127.0.0.1;userid=root;password=xxxxxx;database=test;"
    }
```
* Startup.cs 資料庫物件的DI注入
```
    using MysqlExample.Models;
    using Microsoft.EntityFrameworkCore;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<MyDbContext>(options => options.UseMySql(Configuration.GetConnectionString("MySQL"), MySqlServerVersion.LatestSupportedServerVersion));
    }
```


