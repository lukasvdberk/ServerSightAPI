# Server sight backend

The server sight backend written in .NET Core 5 as REST API.

### Installation
* Install all the nuget packages
* copy the appsettings.Example.json to appsettings.json and fill out all the fields.
    * Postgres is used as a database so you need to setup a posgres database and fill out the connection string
    * Also hangfire requires a seperate postgres database so also create one for that.

Then migrate your database with dotnet ef 
```bash
database update First
```

Start the application in your IDE or with 
```bash
dotnet run
```