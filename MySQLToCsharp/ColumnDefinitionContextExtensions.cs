using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Text;
using static MySQLToCSharp.Parsers.MySql.MySqlParser;

namespace MySQLToCsharp
{
    public static class ColumnDefinitionContextExtensions
    {
        public static (string dataTypeName, int? dataLength) GetColumnDataDefinition(this ColumnDefinitionContext declares)
        {
            var dataType = declares.GetChild<DimensionDataTypeContext>(0);
            var dataName = dataType.GetChild<TerminalNodeImpl>(0);
            var dataTypeName = dataName.GetText();

            int? dataLength = null;
            var lengthOne = dataType.GetChild<LengthOneDimensionContext>(0);
            if (lengthOne != null)
            {
                dataLength = int.Parse(lengthOne.GetText().RemoveParenthesis());
            }
            return (dataTypeName, dataLength);
        }
    }
}
