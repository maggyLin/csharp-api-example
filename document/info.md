# 參考
* https://talllkai.coderbridge.io/2021/04/13/DatabaseFirst/
* https://www.youtube.com/playlist?list=PLneJIGUTIItsqHp_8AbKWb7gyWDZ6pQyz

## 修改 port號, 路徑  等
* Properties/launchSettings.json

## dataBase first (匯入 model) sqlServer
連接 sqlServer > Scaffold-DbContext "Server=伺服器位置;Database=資料庫;Trusted_Connection=True;User ID=帳號;Password=密碼" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force

local sql > Scaffold-DbContext "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\code\csharp\dotnet5-api-example\localsql\Todo.mdf;Integrated Security=True;Connect Timeout=30" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force -CoNtext TodoContext