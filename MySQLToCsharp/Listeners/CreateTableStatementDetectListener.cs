using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using MySQLToCSharp.Parsers.MySql;
using System;
using System.Linq;
using static MySQLToCSharp.Parsers.MySql.MySqlParser;

namespace MySQLToCsharp.Listeners
{
    /// <summary>
    /// Listener to parse CreateTable sql into <see cref="MySqlTableDefinition"/>
    /// </summary>
    public class CreateTableStatementDetectListener : MySqlParserBaseListener
    {
        public bool IsTargetStatement { get; private set; }
        public bool IsParseBegin { get; set; }
        public bool IsParseCompleted { get; set; }
        public MySqlTableDefinition TableDefinition { get; private set; }

        public override void EnterSqlStatement([NotNull] SqlStatementContext context)
        {
            base.EnterSqlStatement(context);
            IsParseBegin = true;
            IsParseCompleted = false;
        }
        public override void ExitSqlStatement([NotNull] SqlStatementContext context)
        {
            base.ExitSqlStatement(context);
            IsParseCompleted = true;
        }

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

            // column definitions
            TableDefinition.ColumnDefinitions = Enumerable.Range(0, createDefinitions.ChildCount)
                .Select(x => createDefinitions.GetChild<ColumnDeclarationContext>(x))
                .Select(x => MySqlColumnDefinition.Extract(x))
                .Where(x => x.success)
                .Select((x, i) =>
                {
                    x.definition.Order = i;
                    return x.definition;
                })
                .ToArray();

            // debug
            // GetChildlen(createDefinitions);
            // var column = definition.GetChild<ColumnDeclarationContext>(0);
            // var count = GetChildCount<CreateDefinitionsContext, ColumnDeclarationContext>(definition);
            // GetChildlen(column);
        }

        /// <summary>
        /// Listener for Primary Key detection
        /// </summary>
        /// <param name="context"></param>
        public override void EnterPrimaryKeyTableConstraint([NotNull] PrimaryKeyTableConstraintContext context)
        {
            base.EnterPrimaryKeyTableConstraint(context);

            // primary key (pk)
            var extract = MySqlKeyDefinition.ExtractPrimaryKey(context);
            if (extract.success)
            {
                var pk = extract.definition;
                TableDefinition.PrimaryKey = pk;

                // map PrimaryKey and existing Column reference
                pk.AddPrimaryKeyReferenceOnColumn(TableDefinition.ColumnDefinitions);
            }
        }

        /// <summary>
        /// Listener for Unique Key detection
        /// </summary>
        /// <param name="context"></param>
        public override void EnterUniqueKeyTableConstraint([NotNull] UniqueKeyTableConstraintContext context)
        {
            base.EnterUniqueKeyTableConstraint(context);

            // unique key
            var extract = MySqlKeyDefinition.ExtractUniqueKey(context);
            if (extract.success)
            {
                var index = extract.definition;
                TableDefinition.AddUniqueKey(index);

                // map UniqueKey and existing Column reference
                index.AddUniqueKeyReferenceOnColumn(TableDefinition.ColumnDefinitions);
            }
        }

        /// <summary>
        /// Listener for Index Index detection
        /// </summary>
        /// <remarks>
        /// override method <see cref="EnterIndexColumnNames"/> includes PK and secondary key and it not contains index name. Therefore let's drill down declaration.
        /// </remarks>
        /// <param name="context"></param>
        public override void EnterIndexDeclaration([NotNull] IndexDeclarationContext context)
        {
            base.EnterIndexDeclaration(context);

            // index (secondary key)
            var extract = MySqlKeyDefinition.ExtractIndexKey(context);
            if (extract.success)
            {
                var index = extract.definition;
                TableDefinition.AddIndexKey(index);

                // map IndexKey and existing Column reference
                index.AddIndexKeyReferenceOnColumn(TableDefinition.ColumnDefinitions);
            }
        public override void EnterGeneratedColumnConstraint([NotNull] GeneratedColumnConstraintContext context)
        {
            base.EnterGeneratedColumnConstraint(context);
            var column = TableDefinition.LookupColumnDefinition(context);
            var generatedColumn = GeneratedColumnDefinition.Extract(context);
            column.GeneratedColumn = generatedColumn;
        }

        public override void EnterReferenceColumnConstraint([NotNull] ReferenceColumnConstraintContext context)
        {
            base.EnterReferenceColumnConstraint(context);
            var column = TableDefinition.LookupColumnDefinition(context);
            var referenceDefinitionContext = context.GetChild<ReferenceDefinitionContext>();
            var referenceColumn = ReferenceColumnDefinition.Extract(referenceDefinitionContext);
            column.ReferenceColumn = referenceColumn;
        }

        /// <summary>
        /// Listener for Collation detection
        /// </summary>
        /// <param name="context"></param>
        public override void EnterCollationName([NotNull] CollationNameContext context)
        {
            base.EnterCollationName(context);
            var name = context.GetText();
            TableDefinition.CollationName = name?.RemoveSingleQuote();
        }

        /// <summary>
        /// Listener for Engine detection
        /// </summary>
        /// <param name="context"></param>
        public override void EnterTableOptionEngine([NotNull] TableOptionEngineContext context)
        {
            base.EnterTableOptionEngine(context);
            var engine = context.GetChild<EngineNameContext>();
            var name = engine.GetText();
            TableDefinition.Engine = name;
        }
    }
}
