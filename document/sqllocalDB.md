##  LocalDB 資料庫(mdf 檔)

### https://blog.darkthread.net/blog/create-localdb/

### window 基本內建會有
* >where sqllocaldb
* >sqllocaldb info

## vs2019 開啟localdb (2019版本以上)
> 檢視->伺服器總管->資料連接右鍵->加入連結 -> 1.資料來源:Microsoft SQL Server 資料庫檔案 (SqlClient) / 2.資料庫檔名: mdf路徑

## vs 更新 localDB (注意mdf檔案路徑) (use EF)
```
Scaffold-DbContext "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\code\csharp\dotnet-api-example\localsql\Todo.mdf;Integrated Security=True;Connect Timeout=30" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force -CoNtext TodoContext
 ```