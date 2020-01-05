# Sql Database Binding
The solution adds new sql bindings to azure function v2 and v3

## Quick start
CLI:

```dotnet add package Pawelharacz.Webjobs.Extensions.MSSqlDatabase ```

csproj:

```<PackageReference Include="Pawelharacz.Webjobs.Extensions.MSSqlDatabase" Version="0.1.0" />```

### Binding
Currently solution supports only input bindnig. 
The Project adds a *MsSqlDb* attribute

* *SqlQuery* is required - it's a query which will be invoked 
* *ConnectionStringSetting* - name of application setting key. default key is "MsSqlDb"

Bining's mapping:  
* IEnumerable collection on our defined class
* Single item of defined class

#### Examples
```csharp
 [FunctionName("TestFunc")]
        public static  IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")]
            HttpRequest req, 
            [MsSqlDb(SqlQuery = "Select Id, Name from Examples", ConnectionStringSetting = "MsSqlConnectionString")] IEnumerable<Example> examples) 
```

```csharp
[FunctionName("TestFunc")]
        public static async Task<IActionResult> RunFilterOneAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "TestFunc/one/{id}")]
            HttpRequest req, 
            [MsSqlDb(SqlQuery = "Select Id, Name from Examples where id = {id}")] Example example)
```

Full examples are available [here ](PawelHaracz/SqlDatabaseBinding/blob/master/Pawelharacz.SqlTest/TestFunc.cs)

## Known issues
1. Azure function v3 requires functions v2 compatibility mode. it can be turned on using this app setting:
"FUNCTIONS_V2_COMPATIBILITY_MODE": "true"