using Antlr4.Runtime.Tree;
using MySQLToCSharp.Parsers.MySql;
using System;

namespace MySQLToCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var listener = new MySqlParserBaseListener();
            Parser.Parse("select * from hoge;".ToUpper(), listener);
        }
    }
}
