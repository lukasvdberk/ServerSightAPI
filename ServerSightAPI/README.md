# Server sight backend

The server sight backend written in .NET Core 5 as REST API.

### Installation
* Install all the nuget packages
* copy the appsettings.Example.json to appsettings.json and fill out all the fields.
    * Postgres is used as a database so you need to setup a postgres database and fill out the connection string
    * Also hangfire requires a separate postgres database so also create one for that.
    * In the Firebase sectins fill int he json content from Project Settings -> Service accounts. 
  
Then migrate your database with dotnet ef 
```bash
dotnet ef database update
```

Start the application in your IDE or with 
```bash
dotnet run
```
_For hangfire visit https://localhost:5001/hangfire-jobs_
