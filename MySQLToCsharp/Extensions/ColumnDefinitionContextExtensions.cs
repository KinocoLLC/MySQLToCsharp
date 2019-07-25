using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Linq;
using static MySQLToCSharp.Parsers.MySql.MySqlParser;

namespace MySQLToCsharp
{
    public static class ParseRuleContextExtensions
    {
        /// <summary>
        /// proxy to GetChild<T>(0)
        /// </summary>
        /// <remarks>
        /// I don't want specify GetChild<T>(0) everytime.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static T GetChild<T>(this ParserRuleContext context) where T : IParseTree
            => context.GetChild<T>(0);
    }

    public static class PrimaryKeyTableConstraintContextExtensions
    {
        /// <summary>
        /// Get PrimaryKey names from context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string[] GetIndexNames(this PrimaryKeyTableConstraintContext context)
        {
            if (context == null) throw new ArgumentOutOfRangeException($"{nameof(context)} is null");
            var result = Enumerable.Range(0, context.ChildCount)
                .Select(x =>
                {
                    var child = context.GetChild(x);
                    if (child is IndexColumnNamesContext name)
                    {
                        return name.GetText();
                    }
                    return "";
                })
                .Where(x => !string.IsNullOrEmpty(x))
                .ToArray();
            return result;
        }
    }
    public static class ColumnDefinitionContextExtensions
    {
        /// <summary>
        /// Get Column detail from context
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static (string dataTypeName, int? dataLength, bool unsigned) GetColumnDataDefinition(this ColumnDefinitionContext context)
        {
            if (context == null) throw new ArgumentOutOfRangeException($"{nameof(context)} is null");

            var dataType = context.GetChild<DimensionDataTypeContext>(0);
            var dataName = dataType.GetChild<TerminalNodeImpl>(0);
            var dataTypeName = dataName.GetText();
            var unsigned = false;
            if (dataType.ChildCount >= 3)
            {
                // signed / unsigned
                var signed = dataType.GetChild<TerminalNodeImpl>(1);
                unsigned = signed.GetText() == "UNSIGNED"; // MUST BE
            }

            int? dataLength = null;
            var lengthOne = dataType.GetChild<LengthOneDimensionContext>(0);
            if (lengthOne != null)
            {
                dataLength = int.Parse(lengthOne.GetText().RemoveParenthesis());
            }

            return (dataTypeName, dataLength, unsigned);
        }
    }
}
