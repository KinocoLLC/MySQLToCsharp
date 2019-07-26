using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using MySQLToCsharp.Parsers.MySql;
using System;
using System.Linq;
using static MySQLToCsharp.Parsers.MySql.MySqlParser;

namespace MySQLToCsharp.Listeners
{
    /// <summary>
    /// Listener to parse CreateTable sql into <see cref="MySqlTableDefinition"/>
    /// </summary>
    public class CreateTableStatementDetectListener : MySqlParserBaseListener, ICreateTableListener
    {
        public bool IsTargetStatement { get; private set; }
        public bool IsParseBegin { get; set; }
        public bool IsParseCompleted { get; set; }
        public MySqlTableDefinition TableDefinition { get; private set; }

        /// <summary>
        /// pick timing when listener begin (initializer)
        /// </summary>
        /// <param name="context"></param>
        public override void EnterSqlStatement([NotNull] SqlStatementContext context)
        {
            base.EnterSqlStatement(context);
            TableDefinition = null;
            IsParseBegin = true;
            IsParseCompleted = false;
        }
        /// <summary>
        /// pick timing when listener exit (finalizer)
        /// </summary>
        /// <param name="context"></param>
        public override void ExitSqlStatement([NotNull] SqlStatementContext context)
        {
            base.ExitSqlStatement(context);
            IsParseCompleted = true;
        }

        /// <summary>
        /// Listener for Table Name detection
        /// </summary>
        /// <param name="context"></param>
        public override void EnterTableName([NotNull] TableNameContext context)
        {
            base.EnterTableName(context);
            // table name
            (TableDefinition.SchemaName, TableDefinition.Name) = MySqlTableDefinition.ExtractTableName(context);
        }

        /// <summary>
        /// Listener for Column definition detection
        /// </summary>
        /// <param name="context"></param>
        public override void EnterColumnCreateTable([NotNull] MySqlParser.ColumnCreateTableContext context)
        {
            base.EnterColumnCreateTable(context);
            IsTargetStatement = true;
            TableDefinition = new MySqlTableDefinition();

            if (context.IsEmpty)
            {
                Console.WriteLine("Empty query detected");
            }

            // column definitions
            var createDefinitions = context.GetChild<CreateDefinitionsContext>();
            TableDefinition.Columns = Enumerable.Range(0, createDefinitions.ChildCount)
                .Select(x => createDefinitions.GetChild<ColumnDeclarationContext>(x))
                .Select(x => MySqlColumnDefinition.Extract(x))
                .Where(x => x != null)
                .Select((x, i) =>
                {
                    x.Order = i;
                    return x;
                })
                .ToArray();

            // debug
            // createDefinitions.GetChildlen();
            // var column = definition.GetChild<ColumnDeclarationContext>();
            // definition.GetChildlen<CreateDefinitionsContext, ColumnDeclarationContext>();
            // column.GetChildlen();
        }

        /// <summary>
        /// Listener for Primary Key detection
        /// </summary>
        /// <param name="context"></param>
        public override void EnterPrimaryKeyTableConstraint([NotNull] PrimaryKeyTableConstraintContext context)
        {
            base.EnterPrimaryKeyTableConstraint(context);

            // primary key (pk)
            var definition = MySqlKeyDefinition.ExtractPrimaryKey(context);
            TableDefinition.PrimaryKey = definition;

            // map PrimaryKey and existing Column reference
            definition.AddPrimaryKeyReferenceOnColumn(TableDefinition.Columns);
        }

        /// <summary>
        /// Listener for Unique Key detection
        /// </summary>
        /// <param name="context"></param>
        public override void EnterUniqueKeyTableConstraint([NotNull] UniqueKeyTableConstraintContext context)
        {
            base.EnterUniqueKeyTableConstraint(context);

            // unique key
            var definition = MySqlKeyDefinition.ExtractUniqueKey(context);
            TableDefinition.AddUniqueKey(definition);

            // map UniqueKey and existing Column reference
            definition.AddUniqueKeyReferenceOnColumn(TableDefinition.Columns);
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
            var definition = MySqlKeyDefinition.ExtractIndexKey(context);
            TableDefinition.AddIndexKey(definition);

            // map IndexKey and existing Column reference
            definition.AddIndexKeyReferenceOnColumn(TableDefinition.Columns);
        }

        /// <summary>
        /// Listener for Generated column detection
        /// </summary>
        /// <param name="context"></param>
        public override void EnterGeneratedColumnConstraint([NotNull] GeneratedColumnConstraintContext context)
        {
            base.EnterGeneratedColumnConstraint(context);
            // generated column
            var column = TableDefinition.LookupColumnDefinition(context);
            var generatedColumn = GeneratedColumnDefinition.Extract(context);
            column.GeneratedColumn = generatedColumn;
        }

        /// <summary>
        /// Listener for Reference column detection
        /// </summary>
        /// <param name="context"></param>
        public override void EnterReferenceColumnConstraint([NotNull] ReferenceColumnConstraintContext context)
        {
            base.EnterReferenceColumnConstraint(context);
            // reference column
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
            // collation
            var name = context.GetText();
            TableDefinition.Collation = name?.RemoveSingleQuote();
        }

        /// <summary>
        /// Listener for Engine detection
        /// </summary>
        /// <param name="context"></param>
        public override void EnterTableOptionEngine([NotNull] TableOptionEngineContext context)
        {
            base.EnterTableOptionEngine(context);
            // engine
            var engine = context.GetChild<EngineNameContext>();
            var name = engine.GetText();
            TableDefinition.Engine = name;
        }
    }
}
