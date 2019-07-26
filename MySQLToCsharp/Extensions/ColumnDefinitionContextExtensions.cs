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

        /// <summary>
        /// Search <see cref="T"/> for Parent direction and return first match.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static T Ascendant<T>(this RuleContext context) where T : ParserRuleContext
        {
            if (context == null) throw new ArgumentNullException($"{nameof(context)} is null");
            if (context.Parent == null) return null;
            if (context.Parent is T type) return type;
            return Ascendant<T>(context.Parent);
        }

        /// <summary>
        /// Search <see cref="T"/> for Child direction and return first match.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static T Descendant<T>(this ParserRuleContext context) where T : ParserRuleContext
        {
            if (context == null) throw new ArgumentNullException($"{nameof(context)} is null");
            if (context is T type) return type;
            if (context.ChildCount != 0) return null;
            // first child's depth priority search. (not same depth search first.)
            for (var i = 0; i < context.ChildCount; i++)
            {
                var result = Descendant<T>(context.GetChild<T>(i));
                if (result != null) return result;
            }
            return null;
        }

        #region debug method
        /// <summary>
        /// output child items recusively, Generics
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void GetChildlen<T, U>(this T context)
            where T : ParserRuleContext
            where U : ParserRuleContext
        {
            for (var i = 0; i < context.ChildCount; i++)
            {
                var target = context.GetChild<U>(i);
                if (target == null) continue;

                Console.WriteLine(target.GetText());

                for (var j = 0; j < target.ChildCount; j++)
                {
                    var c = target.GetChild(j);
                    Console.WriteLine(c.GetText());
                    target.GetChildlen<U, ColumnDefinitionContext>();

                    // continue with column definition detail (autoincre, notnull....)
                }
            }
        }

        /// <summary>
        /// Output child items
        /// </summary>
        /// <param name="context"></param>
        public static void GetChildlen(this ParserRuleContext context, string indent = "")
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

    public static class ColumnDeclarationContextExtensions
    {
        public static string GetColumnName(this ColumnDeclarationContext context)
        {
            var uidContext = context.GetChild<UidContext>();
            var name = uidContext.GetText().RemoveBackQuote();
            return name;
        }
    }

    public static class ColumnDefinitionContextExtensions
    {
        /// <summary>
        /// Get Column detail from context
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static (string dataTypeName, int? dataLength, bool unsigned) ExtractColumnDataDefinition(this ColumnDefinitionContext context)
        {
            if (context == null) throw new ArgumentOutOfRangeException($"{nameof(context)} is null");

            // Normal route: id binary(16) NOT NULL
            var dimensionDataContext = context.GetChild<DimensionDataTypeContext>(0);
            if (dimensionDataContext != null)
            {
                return ExtractColumnData(dimensionDataContext);
            }

            // get dimension data from string: char(32)
            var stringDataContext = context.GetChild<StringDataTypeContext>(0);
            if (stringDataContext != null)
            {
                return ExtractColumnData(stringDataContext);
            }

            throw new ArgumentOutOfRangeException($"Could not retrieve column detail from {nameof(context)}");
        }

        private static (string dataTypeName, int? dataLength, bool unsigned) ExtractColumnData(ParserRuleContext dataType)
        {
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
