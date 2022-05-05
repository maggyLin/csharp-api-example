# 在 visual studio code 下執行專案

* 專案路徑下要有 .vscode 資料夾 設定檔 launch.json 與 tasks.json (參考專案內資料)

* Properties/launchSettings.json 檔案
> swagger修改為http (不然會開不起來)
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