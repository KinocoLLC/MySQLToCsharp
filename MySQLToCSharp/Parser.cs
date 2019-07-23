using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Tree;
using MySQLToCSharp.Parsers.MySql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MySQLToCSharp
{
    // https://saumitra.me/blog/antlr4-visitor-vs-listener-pattern/
    // https://github.com/JaCraig/SQLParser/blob/master/SQLParser/Parser.cs
    // https://github.com/antlr/antlr4/blob/master/doc/csharp-target.md
    // https://github.com/antlr/antlr4/tree/master/runtime/CSharp
    // https://github.com/sharwell/antlr4cs/blob/master/Readme.md
    // https://github.com/unosviluppatore/antlr-mega-tutorial/blob/master/antlr-csharp/README.md
    // https://github.com/unosviluppatore/antlr-mega-tutorial/tree/master/antlr-csharp/antlr-csharp
    // https://stackoverflow.com/questions/49769147/parsing-mysql-using-antlr4-simple-example
    public static class Parser
    {
        public static void Parse(string input, IParseTreeListener listener)
        {
            ICharStream stream = CharStreams.fromstring(input);
            stream = new ToUpperStream(stream);
            ITokenSource lexer = new MySqlLexer(stream);
            ITokenStream tokens = new CommonTokenStream(lexer);
            var parser = new MySqlParser(tokens)
            {
                BuildParseTree = true,
            };
            var root = parser.dmlStatement();
            Console.WriteLine(root.ToStringTree());
            var walker = new ParseTreeWalker();

            // listener pattern
            // TODO: implement listener
            var _listener = new MySqlParserBaseListener();
            walker.Walk(_listener, root);
            
            // visitor pattern
            // TODO: implement visitor
            var visitor = new MySqlParserBaseVisitor<string>();
            Console.WriteLine(visitor.Visit(root));
        }
    }
}
