using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using MySQLToCSharp.Parsers.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static MySQLToCSharp.Parsers.MySql.MySqlParser;

namespace MySQLToCsharp.Listeners
{
    class CreateTableStatementDetectListener : MySqlParserBaseListener
    {
        public bool IsTargetStatement { get; private set; }
        public bool IsParsed { get; private set; }
        public MySqlTableDefinition TableDefinition { get; private set; }

        public override void EnterColumnCreateTable([NotNull] MySqlParser.ColumnCreateTableContext context)
        {
            base.EnterColumnCreateTable(context);
            IsTargetStatement = true;
            var tableDefinition = new MySqlTableDefinition();

            if (context.IsEmpty)
            {
                Console.WriteLine("Empty query detected");
            }

            // TableName
            tableDefinition.TableName = context.tableName().GetText()?.RemoveBackQuote();

            // table definitions
            var createDefinitions = context.GetChild<CreateDefinitionsContext>(0);

            // debug
            // GetChildlen(createDefinitions);
            // var column = definition.GetChild<ColumnDeclarationContext>(0);
            // var count = GetChildCount<CreateDefinitionsContext, ColumnDeclarationContext>(definition);
            // GetChildlen(column);

            // Column
            var columns = createDefinitions.ChildCount;
            tableDefinition.ColumnDefinition = Enumerable.Range(0, createDefinitions.ChildCount)
                .Select(x => createDefinitions.GetChild<ColumnDeclarationContext>(x))
                .Select(x => MySqlColumnDefinition.Extract(x))
                .Where(x => x.success)
                .Select(x => x.columnDefinition)
                .ToArray();

            TableDefinition = tableDefinition;
        }

        public override void EnterCopyCreateTable([NotNull] MySqlParser.CopyCreateTableContext context)
        {
            base.EnterCopyCreateTable(context);

            IsTargetStatement = true;
            foreach (var item in context.tableName())
            {
            }
        }

        public override void EnterQueryCreateTable([NotNull] MySqlParser.QueryCreateTableContext context)
        {
            base.EnterQueryCreateTable(context);

            IsTargetStatement = true;
            var table = context.TABLE().GetText();
        }

        #region debug method
        /// <summary>
        /// output child items recusively, Generics
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        private int GetChildCount<T, U>(T context)
            where T : ParserRuleContext
            where U : ParserRuleContext
        {
            var count = 0;

            for (var i = 0; i < context.ChildCount; i++)
            {
                var target = context.GetChild<U>(i);
                if (target == null) continue;

                Console.WriteLine(target.GetText());
                count = target.ChildCount;

                for (var j = 0; j < count; j++)
                {
                    var c = target.GetChild(j);
                    Console.WriteLine(c.GetText());
                    GetChildCount<U, ColumnDefinitionContext>(target);

                    // continue with column definition detail (autoincre, notnull....)
                }
            }
            return count;
        }

        /// <summary>
        /// Output child items
        /// </summary>
        /// <param name="context"></param>
        private void GetChildlen(IParseTree context)
        {
            for (var i = 0; i < context.ChildCount; i++)
            {
                var child = context.GetChild(i);
                Console.WriteLine(child.GetText());
            }
        }
        #endregion
    }
}
