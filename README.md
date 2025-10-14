[![dotnet-build](https://github.com/KinocoLLC/MySQLToCsharp/actions/workflows/build.yaml/badge.svg)](https://github.com/KinocoLLC/MySQLToCsharp/actions/workflows/build.yaml)
[![NuGet](https://img.shields.io/nuget/v/MySQLToCsharp.svg?label=MySQLToCsharp%20nuget)](https://www.nuget.org/packages/MySQLToCsharp)

# MySQLToCsharp

MySQL version of [SqlToCsharp](https://github.com/ufcpp/SqlToCsharp).

A C# class generator from SQL CREATE TABLE Statements (MySQLs)

## Install

Use [binary from Release](https://github.com/KinocoLLC/MySQLToCsharp/releases/latest) or dotnet tool to install.

```shell
$ dotnet tool install --global MySQLToCsharp
```

## How to run

There are 3 options to generate C# code from MySQL Create Table query.

1. query: input sql string and generate a class.
1. file: read sql file and generate a class.
1. dir: read directory path and generate class for each *.sql file.

```shell
# Binary
$ MySQLToCsharp --help

Usage: MySQLToCsharp <Command>

Commands:
  query    Convert DDL sql query and generate C# class.
  file     Convert DDL sql file and generate C# class.
  dir      Convert DDL sql files in the folder and generate C# class.

# dotnet tool
$ dotnet mysql2csharp --help
Usage: dotnet mysql2csharp [command]

MySQLToCsharp

Commands:
  query    Convert DDL sql query and generate C# class.
  file     Convert DDL sql file and generate C# class.
  dir      Convert DDL sql files in the folder and generate C# class.

Options:
  -h, --help    Show help message
  --version     Show version
```

### query

You can pass a query to generate C# file.

```shell
$ MySQLToCsharp query --help

Usage: MySQLToCsharp query [options...]

Convert DDL sql query and generate C# class.

Options:
  -i, -input <String>        input mysql ddl query to parse (Required)
  -o, -output <String>       output directory path of generated C# class file (Required)
  -n, -namespace <String>    namespace to write (Required)
  -c, -converter <String>    converter name to use (Default: StandardConverter)
  -addbom <Boolean>           (Default: False)
  -dry <Boolean>              (Default: False)
```

sample

```shell
dotnet mysql2csharp query -i "CREATE TABLE sercol1 (id INT, val INT);" -o bin/out -n MyNameSpace.Data
```

### file

You can pass a file to generate C# file.

```shell
$ MySQLToCsharp file --help

Usage: MySQLToCsharp file [options...]

Convert DDL sql file and generate C# class.

Options:
  -i, -input <String>        input file path to parse mysql ddl query (Required)
  -o, -output <String>       output directory path of generated C# class file (Required)
  -n, -namespace <String>    namespace to write (Required)
  -c, -converter <String>    converter name to use (Default: StandardConverter)
  -addbom <Boolean>           (Default: False)
  -dry <Boolean>              (Default: False)
```

sample

```shell
dotnet mysql2csharp file -i "./MySQLToCsharp.Tests/test_data/sql/create_table.sql" -o bin/out -n MyNameSpace.Data
```

### dir

You can pass a directory path to generate C# files for each *.sql file.

```shell
$ MySQLToCsharp dir --help

Usage: MySQLToCsharp dir [options...]

Convert DDL sql files in the folder and generate C# class.

Options:
  -i, -input <String>        input folder path to parse mysql ddl query (Required)
  -o, -output <String>       output directory path of generated C# class files (Required)
  -n, -namespace <String>    namespace to write (Required)
  -c, -converter <String>    converter name to use (Default: StandardConverter)
  -addbom <Boolean>           (Default: False)
  -dry <Boolean>              (Default: False)
```

sample

```shell
dotnet mysql2csharp dir -i "./MySQLToCsharp.Tests/test_data/sql/" -o bin/out -n MyNameSpace.Data
```

## Conveters

You can choose a converter to convert MySQL data type to C# data type.

* StandardConverter: MySQL data types will convert to C# data types.
* StandardBitAsBoolConverter: MySQL `BIT` will convert to C# `bool`.
* StandardDateTimeAsOffsetConverter: MySQL `DATETIME` will convert to C# `DateTimeOffset`.

## Samples

There are sample projects in the `samples` directory.

Run following command in repository root directory.

```shell
dotnet run --project ./src/MySQLToCsharp/MySQLToCsharp.csproj -- dir -i ./samples/tables -o ./samples/MySQLToCsharpSampleConsoleApp -n MySQLToCsharpSampleConsoleApp

dir executed. Output Directory: ./samples/MySQLToCsharpSampleConsoleApp
[-] skipped: BinaryData.cs (no change)
[-] skipped: Character.cs (no change)
[-] skipped: CharacterSlot.cs (no change)
[-] skipped: Multi.cs (no change)
[-] skipped: Player.cs (no change)
[-] skipped: Room.cs (no change)
[-] skipped: String.cs (no change)
[-] skipped: Weapon.cs (no change)
```

change Converter to `StandardDateTimeAsOffsetConverter` and Debug run change CharacterSlot.cs as MySQL `DATETIME` will convert to C# `DateTimeOffset`.

```shell
dotnet run --project ./src/MySQLToCsharp/MySQLToCsharp.csproj -- dir -i ./samples/tables -o ./samples/MySQLToCsharpSampleConsoleApp -n MySQLToCsharpSampleConsoleApp -c StandardDateTimeAsOffsetConverter

dir executed. Output Directory: ./samples/MySQLToCsharpSampleConsoleApp
[-] skipped: BinaryData.cs (no change)
[-] skipped: Character.cs (no change)
[o] generate: CharacterSlot.cs
[-] skipped: Multi.cs (no change)
[-] skipped: Player.cs (no change)
[-] skipped: Room.cs (no change)
[-] skipped: String.cs (no change)
[-] skipped: Weapon.cs (no change)
```

## Generate MySQL Lexer/Parser/Listener/Visitor from ANTLR4 grammer

Referencing MySQL ANTLR4 Grammer from [antlr/grammars-v4](https://github.com/antlr/grammars-v4/tree/master/sql/mysql/Positive-Technologies).

Follow step to update lexer and parser.

1. Update MySqlLexer.g4 and MySqlParser.g4 to the latest.
1. Run script to generate C# class files.
  * it calls `docker compose up` and generate lexer, parser, listener and visitor class.
1. Run Build and Test and confirm what changed and actual effect.

```
# windows
gen.bat

# macos/linux
gen.sh
```

### ANTLR4 References

ANTLR4 Getting started references.

* [antlr4/csharp\-target\.md at master · antlr/antlr4](https://github.com/antlr/antlr4/blob/master/doc/csharp-target.md)
* [antlr4/runtime/CSharp at master · antlr/antlr4](https://github.com/antlr/antlr4/tree/master/runtime/CSharp)
* [antlr4cs/Readme\.md at master · sharwell/antlr4cs](https://github.com/sharwell/antlr4cs/blob/master/Readme.md)
* [antlr\-mega\-tutorial/README\.md at master · unosviluppatore/antlr\-mega\-tutorial](https://github.com/unosviluppatore/antlr-mega-tutorial/blob/master/antlr-csharp/README.md)
* [antlr\-mega\-tutorial/antlr\-csharp/antlr\-csharp at master · unosviluppatore/antlr\-mega\-tutorial](https://github.com/unosviluppatore/antlr-mega-tutorial/tree/master/antlr-csharp/antlr-csharp)

ANTLR4 samples and articles.

* [pyparsingをAntlr4で置き換えて性能を5倍にした \- Qiita](https://qiita.com/osamunmun/items/54a00e963d1a7db0cf59)
* [TreePatternTest in C\#](https://gist.github.com/sharwell/9912132)
* [Antlr4 \- Visitor vs Listener Pattern \- Saumitra's blog](https://saumitra.me/blog/antlr4-visitor-vs-listener-pattern/)
* [java \- Parsing mysql using ANTLR4 simple example \- Stack Overflow](https://stackoverflow.com/questions/49769147/parsing-mysql-using-antlr4-simple-example)

MSSQL Parser reference.

* [JaCraig/SQLParser: An SQL Parser/Lexer for C\#](https://github.com/JaCraig/SQLParser)
