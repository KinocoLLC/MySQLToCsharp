using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace MySQLToCsharp.Parsers
{
    class ToUpperStream : ICharStream
    {
        private ICharStream internalStream;

        public ToUpperStream(ICharStream internalStream)
        {
            this.internalStream = internalStream;
        }

        public int Index => internalStream.Index;

        public int Size => internalStream.Size;

        public string SourceName => internalStream.SourceName;

        public void Consume() => internalStream.Consume();

        [return: NotNull]
        public string GetText(Interval interval) => internalStream.GetText(interval);

        public int LA(int i)
        {
            var c = internalStream.LA(i);
            return c <= 0 ? c : (int)char.ToUpperInvariant((char)c);
        }

        public int Mark() => internalStream.Mark();

        public void Release(int marker) => internalStream.Release(marker);

        public void Seek(int index) => internalStream.Seek(index);
    }
}
