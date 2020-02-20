using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using MySQLToCsharp;
using MySQLToCsharp.Listeners;
using MySQLToCsharp.Parsers.MySql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MySQLToCsharp.Parsers
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

        public void Parse(TextReader reader, IParseTreeListener listener)
        {
            var query = reader.ReadToEnd();
            Parse(query, new[] { listener });
        }

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

            // listener pattern
            RegisterListener(listeners);
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

        #region loader

        public static MySqlTableDefinition FromQuery(string query)
            => FromQuery(query, new CreateTableStatementDetectListener());

        public static MySqlTableDefinition FromQuery(string query, ICreateTableListener listener)
        {
            IParser parser = new Parser();
            parser.Parse(query, listener);
            return listener.TableDefinition;
        }

        /// <summary>
        /// load query from folder. specify sql file is utf8(bom) or utf8(bomless).
        /// </summary>
        /// <param name="path"></param>
        /// <param name="bom"></param>
        /// <returns></returns>
        public static IEnumerable<MySqlTableDefinition> FromFolder(string path, bool bom = false)
            => FromFolder(path, new CreateTableStatementDetectListener(), new UTF8Encoding(bom));

        /// <summary>
        /// load query from folder. specify sql file encoding.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="listener"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static IEnumerable<MySqlTableDefinition> FromFolder(string path, ICreateTableListener listener, Encoding encoding)
        {
            foreach (var file in Directory.EnumerateFiles(path, "*.sql", SearchOption.AllDirectories))
            {
                yield return FromFile(file, listener, encoding);
            }
        }

        /// <summary>
        /// load query from file. specify sql file is utf8(bom) or utf8(bomless).
        /// </summary>
        /// <param name="path"></param>
        /// <param name="bom"></param>
        /// <returns></returns>
        public static MySqlTableDefinition FromFile(string path, bool bom = false)
            => FromFile(path, new CreateTableStatementDetectListener(), new UTF8Encoding(bom));

        /// <summary>
        /// load query from file. specify sql file encoding.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="listener"></param>
        /// <param name="encoding"></param>
        public static MySqlTableDefinition FromFile(string path, ICreateTableListener listener, Encoding encoding)
        {
            using var reader = new StreamReader(path, encoding);
            var parser = new Parser();
            parser.Parse(reader, listener);
            return listener.TableDefinition;
        }
        #endregion

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
