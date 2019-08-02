# MySQLToCsharp

[![CircleCI](https://circleci.com/gh/KinocoLLC/MySQLToCsharp.svg?style=svg)](https://circleci.com/gh/KinocoLLC/MySQLToCsharp) [![codecov](https://codecov.io/gh/KinocoLLC/MySQLToCsharp/branch/master/graph/badge.svg)](https://codecov.io/gh/KinocoLLC/MySQLToCsharp)

MySQL version of [SqlToCsharp](https://github.com/ufcpp/SqlToCsharp)

## How to run

There are 3 options to generate C# code from MySQL Create Table query.

1. input sql string and generate a class.
1. read sql file and generate a class.
1. read directory path and generate class for each *.sql file.

generate from query.

```shell
# query 
mysql2csharp --query -i "CREATE TABLE sercol1 (id INT, val INT);" -o bin/out -n MyNameSpace.Data
```

generate from file.

```shell
# file
dotnet mysql2csharp --file -i "./MySQLToCsharp.Tests/test_data/sql/create_table.sql" -o bin/out -n MyNameSpace.Data
```

read directory and generate for all *.sql

```shell
# dirctory
dotnet mysql2csharp --dir -i "./MySQLToCsharp.Tests/test_data/sql/" -o bin/out -n MyNameSpace.Data
```

## Generate MySQL Lexer/Parser/Listener/Visitor from ANTLR4 grammer

run script to generate C# class files.
it calls `docker-compose up` and generate lexer, parser, listener and visitor class.

```
# windows
gen.bat

# macos/linux
gen.sh
```


## Ref

getting started

* [antlr4/csharp\-target\.md at master · antlr/antlr4](https://github.com/antlr/antlr4/blob/master/doc/csharp-target.md)
* [antlr4/runtime/CSharp at master · antlr/antlr4](https://github.com/antlr/antlr4/tree/master/runtime/CSharp)
* [antlr4cs/Readme\.md at master · sharwell/antlr4cs](https://github.com/sharwell/antlr4cs/blob/master/Readme.md)
* [antlr\-mega\-tutorial/README\.md at master · unosviluppatore/antlr\-mega\-tutorial](https://github.com/unosviluppatore/antlr-mega-tutorial/blob/master/antlr-csharp/README.md)
* [antlr\-mega\-tutorial/antlr\-csharp/antlr\-csharp at master · unosviluppatore/antlr\-mega\-tutorial](https://github.com/unosviluppatore/antlr-mega-tutorial/tree/master/antlr-csharp/antlr-csharp)

good

* [pyparsingをAntlr4で置き換えて性能を5倍にした \- Qiita](https://qiita.com/osamunmun/items/54a00e963d1a7db0cf59)
* [TreePatternTest in C\#](https://gist.github.com/sharwell/9912132)
* [Antlr4 \- Visitor vs Listener Pattern \- Saumitra's blog](https://saumitra.me/blog/antlr4-visitor-vs-listener-pattern/)
* [java \- Parsing mysql using ANTLR4 simple example \- Stack Overflow](https://stackoverflow.com/questions/49769147/parsing-mysql-using-antlr4-simple-example)

ANTLR4 repos

* [JaCraig/SQLParser: An SQL Parser/Lexer for C\#](https://github.com/JaCraig/SQLParser)

