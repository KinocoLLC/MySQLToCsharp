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
    public static class Parser
    {
        public static void Parse(string input, IParseTreeListener listener)
        {
            ICharStream stream = CharStreams.fromstring(input);
            stream = new ToUpperStream(stream);
            ITokenSource lexer = new MySqlLexer(stream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new MySqlParser(tokens)
            {
                BuildParseTree = true,
            };
            // parser.Parse() not exists?
        }
    }
}
