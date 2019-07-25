using Antlr4.Runtime.Misc;
using MySQLToCSharp.Parsers.MySql;

namespace MySQLToCSharp
{
    partial class Program
    {
        public class StatementDetectListener : MySqlParserBaseListener
        {
            // sql statement
            public bool IsDmlStatement { get; private set; }
            public bool IsDdlStatement { get; private set; }
            public bool IsPreparedStatement { get; private set; }
            public bool IsReplicationStatement { get; private set; }
            public bool IsTransactionStatement { get; private set; }
            public bool IsUtilityStatement { get; private set; }
            public bool IsAdministrationStatement { get; private set; }

            // dml statement
            public bool IsCallStatement { get; private set; }
            public bool IsDeleteStatement { get; private set; }
            public bool IsDoStatement { get; private set; }
            public bool IsHandlerStatement { get; private set; }
            public bool IsInsertStatement { get; private set; }
            public bool IsLoadDataStatement { get; private set; }
            public bool IsLoadXmlStatement { get; private set; }
            public bool IsReplaceStatement { get; private set; }
            public bool IsSelectStatementFound { get; private set; }
            public bool IsUpdateStatement { get; private set; }

            // ddl
            public bool IsAlterDatabaseStatement { get; private set; }
            public bool IsAlterEventStatement { get; private set; }
            public bool IsAlterFunctionStatement { get; private set; }
            public bool IsAlterInstanceStatement { get; private set; }
            public bool IsAlterLogfileGroupStatement { get; private set; }
            public bool IsAlterProcedureStatement { get; private set; }
            public bool IsAlterServerStatement { get; private set; }
            public bool IsAlterTableStatement { get; private set; }
            public bool IsAlterTablespaceStatement { get; private set; }
            public bool IsAlterViewStatement { get; private set; }
            public bool IsCreateDatabaseStatement { get; private set; }
            public bool IsCreateEventStatement { get; private set; }
            public bool IsCreateFunctionStatement { get; private set; }
            public bool IsCreateIndexStatement { get; private set; }
            public bool IsCreateLogfileGroupStatement { get; private set; }
            public bool IsCreateProcedureStatement { get; private set; }
            public bool IsCreateServerStatement { get; private set; }
            public bool IsCreateTableStatement { get; private set; }
            public bool IsCreateTablespaceInnodbStatement { get; private set; }
            public bool IsCreateTriggerStatement { get; private set; }
            public bool IsCreateViewStatement { get; private set; }
            public bool IsDropeDatabaseStatement { get; private set; }
            public bool IsDropeEventStatement { get; private set; }
            public bool IsDropeFunctionStatement { get; private set; }
            public bool IsDropIndexStatement { get; private set; }
            public bool IsDropLogfileGroupStatement { get; private set; }
            public bool IsDropProcedureStatement { get; private set; }
            public bool IsDropServerStatement { get; private set; }
            public bool IsDropTableStatement { get; private set; }
            public bool IsDropTablespaceStatement { get; private set; }
            public bool IsDropTriggerStatement { get; private set; }
            public bool IsDropViewStatement { get; private set; }

            public override void EnterSqlStatement([NotNull] MySqlParser.SqlStatementContext context)
            {
                base.EnterSqlStatement(context);
                IsAdministrationStatement = context.administrationStatement() != null;
                IsDmlStatement = context.dmlStatement() != null;
                IsDdlStatement = context.ddlStatement() != null;
                IsPreparedStatement = context.preparedStatement() != null;
                IsReplicationStatement = context.replicationStatement() != null;
                IsTransactionStatement = context.transactionStatement() != null;
                IsUtilityStatement = context.utilityStatement() != null;
            }

            public override void EnterDmlStatement([NotNull] MySqlParser.DmlStatementContext context)
            {
                base.EnterDmlStatement(context);
                IsCallStatement = context.callStatement() != null;
                IsDeleteStatement = context.deleteStatement() != null;
                IsDoStatement = context.doStatement() != null;
                IsHandlerStatement = context.handlerStatement() != null;
                IsInsertStatement = context.insertStatement() != null;
                IsLoadDataStatement = context.loadDataStatement() != null;
                IsLoadXmlStatement = context.loadXmlStatement() != null;
                IsReplaceStatement = context.replaceStatement() != null;
                IsSelectStatementFound = context.selectStatement() != null;
                IsUpdateStatement = context.updateStatement() != null;
            }

            public override void EnterDdlStatement([NotNull] MySqlParser.DdlStatementContext context)
            {
                base.EnterDdlStatement(context);
                IsAlterDatabaseStatement = context.alterDatabase() != null;
                IsAlterEventStatement = context.alterEvent() != null;
                IsAlterFunctionStatement = context.alterFunction() != null;
                IsAlterInstanceStatement = context.alterInstance() != null;
                IsAlterLogfileGroupStatement = context.alterLogfileGroup() != null;
                IsAlterProcedureStatement = context.alterProcedure() != null;
                IsAlterServerStatement = context.alterServer() != null;
                IsAlterTableStatement = context.alterTable() != null;
                IsAlterTablespaceStatement = context.alterTablespace() != null;
                IsAlterViewStatement = context.alterView() != null;

                IsCreateDatabaseStatement = context.createDatabase() != null;
                IsCreateEventStatement = context.createEvent() != null;
                IsCreateFunctionStatement = context.createFunction() != null;
                IsCreateIndexStatement = context.createIndex() != null;
                IsCreateLogfileGroupStatement = context.createLogfileGroup() != null;
                IsCreateProcedureStatement = context.createProcedure() != null;
                IsCreateServerStatement = context.createServer() != null;
                IsCreateTableStatement = context.createTable() != null;
                IsCreateTablespaceInnodbStatement = context.createTablespaceInnodb() != null;
                IsCreateTriggerStatement = context.createTrigger() != null;
                IsCreateViewStatement = context.createView() != null;

                IsDropeDatabaseStatement = context.dropDatabase() != null;
                IsDropeEventStatement = context.dropEvent() != null;
                IsDropeFunctionStatement = context.dropFunction() != null;
                IsDropIndexStatement = context.dropIndex() != null;
                IsDropLogfileGroupStatement = context.dropLogfileGroup() != null;
                IsDropProcedureStatement = context.dropProcedure() != null;
                IsDropServerStatement = context.dropServer() != null;
                IsDropTableStatement = context.dropTable() != null;
                IsDropTablespaceStatement = context.dropTablespace() != null;
                IsDropTriggerStatement = context.dropTrigger() != null;
                IsDropViewStatement = context.dropView() != null;
            }
        }
    }
}
