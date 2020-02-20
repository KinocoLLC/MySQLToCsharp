using System.Collections.Concurrent;
using System.Linq;

namespace MySQLToCsharp
{
    public class QueryToCsharpContext
    {
        internal static QueryToCsharpContext Current { get; } = new QueryToCsharpContext();

        private readonly ConcurrentBag<LogState> logs = new ConcurrentBag<LogState>();
        internal void AddLog(string id, string log) => logs.Add(new LogState { Id = id, Message = log });
        public string[] GetLogs(string id) => logs.Where(x => x.Id == id).Select(x => x.Message).ToArray();
        internal void Clear() => logs.Clear();
    }

    public struct LogState
    {
        public string Id { get; set; }
        public string Message { get; set; }
    }
}
