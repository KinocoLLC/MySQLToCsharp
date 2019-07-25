using System;
using System.Collections.Generic;
using System.Text;

namespace MySQLToCsharp
{
    public static class StringExtensions
    {
        /// <summary>
        /// remove () from string. (xxx) -> xxx
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveParenthesis(this string text) => text?.Replace("(", "").Replace(")", "");
        /// <summary>
        /// remove ` from string. `xxx` -> xxx
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveBackQuote(this string text) => text?.Replace("`", "");
        /// <summary>
        /// remove ' from string. 'xxx' -> xxx
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveSingleQuote(this string text) => text?.Replace("'", "");
        /// <summary>
        /// remove DEFAULT'' from string. DEFAULT'xxx' -> xxx
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveDefaultString(this string text) => text?.Substring(0, text.Length - 1).Replace("DEFAULT'", "");
    }
}
