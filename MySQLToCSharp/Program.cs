using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using MySQLToCSharp.Parsers.MySql;
using System;

namespace MySQLToCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var listener = new Listner();
            Parser.Parse("select * from hoge where a = 'b';", listener);
            var hoge = listener.IsSelectStatementFound;
        }

        public class Listner : MySqlParserBaseListener
        {
            public bool IsSelectStatementFound { get; set; }

            public override void EnterDmlStatement([NotNull] MySqlParser.DmlStatementContext context)
            {
                base.EnterDmlStatement(context);
                IsSelectStatementFound = context.selectStatement() != null;
            }

            public override void EnterExpressions([NotNull] MySqlParser.ExpressionsContext context)
            {
                base.EnterExpressions(context);
                var text = context.GetText();
            }
        }
    }
}
