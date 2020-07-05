using System;
using System.Collections.Generic;
using System.Text;

namespace MySQLToCsharp.Internal
{
    internal class InternalUtils
    {
        /// <summary>
        /// Normalize NewLine (EndOfLine) with current Operating System.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string NormalizeNewLines(string content)
        {
            // The generated code may be text with mixed line ending types. (CR + CRLF)
            // We need to normalize the line ending type in each Operating Systems. (e.g. Windows=CRLF, Linux/macOS=LF)
            return content.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
        }
    }
}
