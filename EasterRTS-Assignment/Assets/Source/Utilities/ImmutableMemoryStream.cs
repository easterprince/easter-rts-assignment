using EasterRts.Utilities.Collections;
using System;
using System.IO;

namespace EasterRts.Utilities {

    public class ImmutableMemoryStream : Stream {

        private static readonly ImmutableArray<byte> _empty = new ImmutableArray<byte>();

        private ImmutableArray<byte> _source;
        private int _nextIndex;
        
        public ImmutableMemoryStream() {
            _source = _empty;
            _nextIndex = 0;
        }

        public void Load(ImmutableArray<byte> source) {
            _source = source ?? _empty;
            _nextIndex = 0;
        }

        public void Unload() => Load(null);

        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();

        public override long Position {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override void Flush() {
            throw new NotSupportedException();
        }

        public override int ReadByte() {
            if (_nextIndex == _source.Count) {
                return -1;
            }
            var result = _source[_nextIndex];
            _nextIndex++;
            return result;
        }

        public override int Read(byte[] buffer, int offset, int count) {
            int available = _source.Count - _nextIndex;
            if (count > available) {
                count = available;
            }
            _source.CopyTo(_nextIndex, buffer, offset, count);
            _nextIndex += count;
            return count;
        }

        public override long Seek(long offset, SeekOrigin origin) {
            throw new NotSupportedException();
        }

        public override void SetLength(long value) {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count) {
            throw new NotSupportedException();
        }
    }
}
