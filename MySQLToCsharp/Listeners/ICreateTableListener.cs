using Antlr4.Runtime.Tree;

namespace MySQLToCsharp.Listeners
{
    /// <summary>
    /// Interface to get TableDefinition after Create Table Statement has parsed.
    /// </summary>
    public interface ICreateTableListener : IParseTreeListener 
    {
        bool IsParseBegin { get; set; }
        bool IsParseCompleted { get; set; }
        bool IsTargetStatement { get; }
        MySqlTableDefinition TableDefinition { get; }
    }
}