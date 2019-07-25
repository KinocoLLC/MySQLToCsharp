using System;
using System.Collections.Generic;
using System.Text;

namespace MySQLToCsharp
{
    public static class StringExtensions
    {
        public static string RemoveParenthesis(this string text) => text?.Replace("(", "").Replace(")", "");

        public static string RemoveBackQuote(this string text) => text?.Replace("`", "");

        public static string RemoveSingleQuote(this string text) => text?.Replace("'", "");

    }
}
