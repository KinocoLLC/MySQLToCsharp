generate C# code from mysql CREATE TABLE query.

```shell
dotnet publish ..
dotnet ../src/MySQLToCSharp/bin/Debug/netcoreapp2.2/publish/MySQLToCsharp.dll --dir -i ./tables -o ./MySQLToCsharpSampleConsoleApp -n MySQLToCsharpSampleConsoleApp
```