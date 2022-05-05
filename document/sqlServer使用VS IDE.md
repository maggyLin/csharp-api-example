## 此專案為.net5 , 套件版本請參考[專案名稱].csproj

## 1.參考資料 : 
* https://talllkai.coderbridge.io/2021/04/13/DatabaseFirst/

## 2.安裝套件
-- framwork NuGet
* Microsoft.EntityFrameworkCore.SqlServer
* Microsoft.EntityFrameworkCore.Tools

-- cmd(vs code)
* dotnet add package <PACKAGE_NAME> [-v|--version <VERSION>]


-- 套件查看
* 檔案 : [專案名稱].csproj
* cmd : dotnet list package

## 3.使用 DataBase First (資料庫建立完成匯入專案)
* 使用指令產生EF實體,直接產生對應Model

```
Scaffold-DbContext "Server=伺服器位置;Database=資料庫;Trusted_Connection=True;User ID=帳號;Password=密碼" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force
```

## 4.資料庫帳密使用config檔注入
* 將Model中 [XXX]Context.cs 中 "optionsBuilder.UseSqlServer()"方法刪掉
* appsettings.json 中寫入連線資訊
```
    "ConnectionStrings": {
        "TodoDatabase": "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\code\\csharp\\dotnet5-api-example\\localsql\\Todo.mdf;Integrated Security=True;Connect Timeout=30"
    }
```
* Startup.cs 資料庫物件的DI注入
```
public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<TodoContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TodoDatabase")));
}
```

## 5.資料庫更新
日後如果在SQL Server上有資料表行進新增或更新時，只要在重下一次Scaffold-DbContext指令即可(步驟3)