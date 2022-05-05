 //.net core ef
### 要先安裝 EF
 * dotnet tool install --global dotnet-ef (請先安裝此套件,才能使用dotnet ef的指令)
 
 
### 套件查看
* 檔案 : [專案名稱].csproj
* cmd : dotnet list package

--- 有沒有安裝 sqlServer 相關套件
> Microsoft.EntityFrameworkCore                       5.0.10   5.0.10
> Microsoft.EntityFrameworkCore.Design                5.0.10   5.0.10
> Microsoft.EntityFrameworkCore.SqlServer             5.0.10   5.0.10
> Microsoft.EntityFrameworkCore.SqlServer.Design      1.1.6    1.1.6

* dotnet add package Microsoft.EntityFrameworkCore [-v|--version <VERSION>]


### DB fisrt(建立好資料庫,直接建立相關Models)
> dotnet ef dbcontext scaffold "server=127.0.0.1;Database=DB_Test;User=test;Password=test;" Microsoft.EntityFrameworkCore.SqlServer -o Models -c TCSContext -f	

* -c 與名稱Context沒有填的話，會以Database的名稱作為DbContext名稱

### 資料庫帳密使用config檔注入
* 將Model中 [XXX]Context.cs 中 "optionsBuilder.UseSqlServer()"方法刪掉
* appsettings.json 中寫入連線資訊
```
    "ConnectionStrings": {
        "MySQL": "server=127.0.0.1;userid=root;password=xxxxxx;database=test;"
    }
```

### startup.cs 注入
services.AddDbContext<TCSContext>(options =>options.UseSqlServer(Configuration.GetConnectionString("TCSDatabase")));

### you do not use startup.cs 使用 program.cs
builder.Services.AddDbContext<JMSContext>(options => options.UseSqlServer(Configuration.GetConnectionString("YourConnectionStringKeyName_From_AppSettings.json"));