using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileTools.PDFHasher
{
    public class CombinationStream : Stream
    {
        private readonly Queue<Stream> _streams;
        private Stream _currentStream;
        private Lazy<long> _length;
        private long _postion;

        public CombinationStream(IEnumerable<Stream> streams)
        {
            var str = streams ?? throw new ArgumentNullException(nameof(streams));
            _streams = new Queue<Stream>(streams);
            _length = new Lazy<long>(() => str is ICollection<Stream> ? str.Sum(p => p.Length) : throw new NotImplementedException());
            _streams.TryDequeue(out _currentStream);
        }

        public override void Flush()
        {
            if (_currentStream != null)
            {
                _currentStream.Flush();
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new InvalidOperationException("Stream is not seekable.");
        }

        public override void SetLength(long value)
        {
            throw new InvalidOperationException("The length cannot be set.");
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int result = 0;
            int buffPostion = offset;

            while (_currentStream != null && count > 0)
            {
                int bytesRead = _currentStream.Read(buffer, buffPostion, count);
                result += bytesRead;
                buffPostion += bytesRead;
                _postion += bytesRead;

                if (bytesRead <= count)
                {
                    count -= bytesRead;
                }

                if (count > 0)
                {
                    _streams.TryDequeue(out _currentStream);
                }
            }

            return result;
        }

        public override long Length => _length.Value;

        public override long Position
        {
            get { return this._postion; }
            set { throw new NotImplementedException(); }
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new InvalidOperationException("Stream is not writable");
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;
    }
}
