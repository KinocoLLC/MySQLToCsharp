﻿using Antlr4.Runtime;
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
            TableDefinition = new MySqlTableDefinition();

            if (context.IsEmpty)
            {
                Console.WriteLine("Empty query detected");
            }

            // tableName
            TableDefinition.TableName = context.tableName().GetText()?.RemoveBackQuote();

            // table definitions
            var createDefinitions = context.GetChild<CreateDefinitionsContext>(0);

            // debug
            // GetChildlen(createDefinitions);
            // var column = definition.GetChild<ColumnDeclarationContext>(0);
            // var count = GetChildCount<CreateDefinitionsContext, ColumnDeclarationContext>(definition);
            // GetChildlen(column);

            // column
            TableDefinition.ColumnDefinition = Enumerable.Range(0, createDefinitions.ChildCount)
                .Select(x => createDefinitions.GetChild<ColumnDeclarationContext>(x))
                .Select(x => MySqlColumnDefinition.Extract(x))
                .Where(x => x.success)
                .Select(x => x.columnDefinition)
                .ToArray();

            //GetChildlen(createDefinitions);

            // index
            // IndexDeclarationContext


            // collation

            // engine
        }

        /// <summary>
        /// Listener for Primary Key detection
        /// </summary>
        /// <param name="context"></param>
        public override void EnterPrimaryKeyTableConstraint([NotNull] PrimaryKeyTableConstraintContext context)
        {
            base.EnterPrimaryKeyTableConstraint(context);

            // primary key (pk)
            //GetChildlen(context);
            var pk = context.GetIndexNames();
            var pkNames = pk.Select(x => x.RemoveBackQuote().RemoveParenthesis()).ToArray();
            TableDefinition.PrimaryKeyColumns = pkNames;
        }

        /// <summary>
        /// Listener for Secondary Index detection
        /// </summary>
        /// <remarks>
        /// override method <see cref="EnterIndexColumnNames"/> includes PK and secondary key and it not contains index name. Therefore let's drill down declaration.
        /// </remarks>
        /// <param name="context"></param>
        public override void EnterIndexDeclaration([NotNull] IndexDeclarationContext context)
        {
            base.EnterIndexDeclaration(context);
            GetChildlen(context, "indexDeclaration: ");
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
        private void GetChildlen(IParseTree context, string indent = "")
        {
            for (var i = 0; i < context.ChildCount; i++)
            {
                var child = context.GetChild(i);
                var name = child.GetText();
                Console.WriteLine(indent + name);
            }
        }
        #endregion
    }
}
