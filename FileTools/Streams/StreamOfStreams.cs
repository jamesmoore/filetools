using System;
using System.Collections.Generic;
using System.IO;

namespace FileTools.Streams
{
	public class StreamOfStreams(IEnumerable<Stream> streams) : Stream
	{
		private readonly IEnumerator<Stream> enumerator = streams.GetEnumerator();

		private Stream currentStream = null;

		public override bool CanRead => true;

		public override bool CanSeek => false;

		public override bool CanWrite => false;

		public override long Length => throw new NotImplementedException();

		public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public override void Flush() => throw new NotImplementedException();

		public override int Read(byte[] buffer, int offset, int count)
		{
			if (currentStream == null)
			{
				currentStream = GetNextStream();
				if (currentStream == null)
				{
					return 0;
				}
			}

			var totalRead = 0;
			do
			{
				var read = currentStream.Read(buffer, offset + totalRead, count - offset - totalRead);
				totalRead += read;

				if (read == 0)
				{
					currentStream.Close();
                    currentStream = GetNextStream();
				}
			} while (currentStream != null && count - offset - totalRead > 0);

			return totalRead;
		}

		private Stream GetNextStream()
		{
			return enumerator.MoveNext() ? enumerator.Current : null;
		}

		public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();

		public override void SetLength(long value) => throw new NotImplementedException();

		public override void Write(byte[] buffer, int offset, int count) => throw new NotImplementedException();
	}
}
