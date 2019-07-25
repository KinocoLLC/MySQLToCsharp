using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using MySQLToCSharp.Parsers.MySql;
using System;

namespace MySQLToCSharp
{
    public interface IParser
    {
        IParseTreeListener[] Listeners { get; }

        void Parse(string query, IParseTreeListener listener);
        void Parse(string query, IParseTreeListener[] listeners);
        void PrintTokens(bool showTypeHint);
    }

    public class Parser : IParser
    {
        public IParseTreeListener[] Listeners { get; private set; }

        private MySqlParser.SqlStatementContext context;

        public void Parse(string query, IParseTreeListener listener)
            => Parse(query, new[] { listener });

        public void Parse(string query, IParseTreeListener[] listeners)
        {
            ICharStream stream = CharStreams.fromstring(query);
            stream = new ToUpperStream(stream);
            ITokenSource lexer = new MySqlLexer(stream);
            ITokenStream tokens = new CommonTokenStream(lexer);
            var parser = new MySqlParser(tokens)
            {
                BuildParseTree = true,
            };

            // both is possible, let's detect every type of sql
            this.context = parser.sqlStatement();
            //var statement = parser.dmlStatement();
            //var statement = parser.selectStatement();

            // lisp like tree result will shown with `ToStringTree()`
            // ([] ([699] ([2948 699] select ([3401 2948 699] *) ([3405 2948 699] from ([3580 3405 2948 699] ([3242 3580 3405 2948 699] ([3250 3242 3580 3405 2948 699] ([3269 3250 3242 3580 3405 2948 699] ([5234 3269 3250 3242 3580 3405 2948 699] ([5228 5234 3269 3250 3242 3580 3405 2948 699] ([5312 5228 5234 3269 3250 3242 3580 3405 2948 699] hoge))))))) where ([3582 3405 2948 699] ([5986 3582 3405 2948 699] ([592 5986 3582 3405 2948 699] ([6003 592 5986 3582 3405 2948 699] ([6067 6003 592 5986 3582 3405 2948 699] ([5236 6067 6003 592 5986 3582 3405 2948 699] ([5312 5236 6067 6003 592 5986 3582 3405 2948 699] a))))) ([6006 5986 3582 3405 2948 699] =) ([6007 5986 3582 3405 2948 699] ([6003 6007 5986 3582 3405 2948 699] ([6066 6003 6007 5986 3582 3405 2948 699] ([5376 6066 6003 6007 5986 3582 3405 2948 699] 'b'))))))))))
            // Console.WriteLine(statement.ToStringTree());

            // just an text result
            // select*fromhogewherea='b'
            //Console.WriteLine(statement.GetChild(0).GetText());

            // listener pattern
            RegisterListener(listeners);

            // visitor pattern (not using but if needed)
            // TODO: implement visitor
            //var visitor = new MySqlParserBaseVisitor<string>();
            //Console.WriteLine(visitor.Visit(root));
        }

        public void PrintTokens(bool showTypeHint = false)
        {
            if (context == null) throw new ArgumentNullException($"missing {nameof(context)}. Please run Parse(qeury) beforehand.");
            Action<Type, string> action = null;
            if (showTypeHint)
            {
                action = (t, s) => Console.WriteLine($"{t.Name}: {s}");
            }
            else
            {
                action = (t, s) => Console.WriteLine(s);
            }

            DrilldownToken(this.context, this.context.Stop, (t, s) => action(t, s));
        }

        #region Debug

        private void RegisterListener(IParseTreeListener[] listeners)
        {
            if (context == null) throw new ArgumentNullException($"missing {nameof(context)}. Please run Parse(qeury) before register listener.");
            Listeners = listeners;

            // listener pattern
            var walker = new ParseTreeWalker();
            foreach (var item in Listeners)
            {
                walker.Walk(item, context);
            }
        }

        /// <summary>
        /// Handle each token
        /// </summary>
        /// <param name="parserRule"></param>
        /// <param name="stop"></param>
        /// <param name="action"></param>
        private static void DrilldownToken(IParseTree parserRule, IToken stop, Action<Type, string> action)
        {
            HandleToken(parserRule, stop, action);
            action(stop.GetType(), stop.Text);
        }

        /// <summary>
        /// Recursively drill down child and act to current token
        /// </summary>
        /// <param name="parserRule"></param>
        /// <param name="stop"></param>
        private static void HandleToken(IParseTree parserRule, IToken stop, Action<Type, string> action)
        {
            for (var i = 0; i < parserRule.ChildCount; i++)
            {
                var child = parserRule.GetChild(i);
                if (child == null) continue;
                if (i + 1 == parserRule.ChildCount)
                {
                    HandleToken(child, stop, action);
                }
                else
                {
                    action(child.GetType(), child.GetText());
                }
            }
        }
        #endregion
    }
}
