using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace UVACanvasAccess.Util {
    
    /*
     * Certain Canvas endpoints send payloads which are sometimes, but not always, gzip-compressed. Canvas claims to send `Content-Encoding: gzip` for
     * gzip-compressed payloads, but it doesn't. As a result, we need this ridiculous thing, which lets us peek at the first few bytes of the payload to
     * identify the presence of gzip's magic number.
     */
    internal sealed class PrefixedReadStream : Stream {
        private readonly byte[] prefix;
        private readonly int prefixLength;
        private readonly Stream inner;
        private readonly long innerDataStartPosition;
        private int prefixOffset;

        public PrefixedReadStream(byte[] prefix, int prefixLength, Stream inner) {
            this.prefix = prefix ?? throw new ArgumentNullException(nameof(prefix));
            this.inner = inner ?? throw new ArgumentNullException(nameof(inner));

            if (prefixLength < 0 || prefixLength > prefix.Length) {
                throw new ArgumentOutOfRangeException(nameof(prefixLength));
            }

            this.prefixLength = prefixLength;
            innerDataStartPosition = inner.CanSeek ? inner.Position : 0L;
        }

        public override bool CanRead => inner.CanRead;
        public override bool CanSeek => inner.CanSeek;
        public override bool CanWrite => false;
        public override long Length {
            get {
                if (!CanSeek) {
                    throw new NotSupportedException();
                }

                return prefixLength + (inner.Length - innerDataStartPosition);
            }
        }

        public override long Position {
            get {
                if (!CanSeek) {
                    throw new NotSupportedException();
                }

                if (prefixOffset < prefixLength) {
                    return prefixOffset;
                }

                return prefixLength + (inner.Position - innerDataStartPosition);
            }
            set {
                if (!CanSeek) {
                    throw new NotSupportedException();
                }

                if (value < 0 || value > Length) {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                if (value < prefixLength) {
                    prefixOffset = (int) value;
                    inner.Position = innerDataStartPosition;
                    return;
                }

                prefixOffset = prefixLength;
                inner.Position = innerDataStartPosition + (value - prefixLength);
            }
        }

        public override void Flush() {
            inner.Flush();
        }

        public override int Read(byte[] buffer, int bufferOffset, int count) {
            var copied = CopyPrefix(buffer, bufferOffset, count);
            if (copied == count) {
                return copied;
            }

            return copied + inner.Read(buffer, bufferOffset + copied, count - copied);
        }

        public override async Task<int> ReadAsync(byte[] buffer, int bufferOffset, int count, CancellationToken cancellationToken) {
            var copied = CopyPrefix(buffer, bufferOffset, count);
            if (copied == count) {
                return copied;
            }

            var read = await inner.ReadAsync(buffer, bufferOffset + copied, count - copied, cancellationToken);
            return copied + read;
        }

        private int CopyPrefix(byte[] buffer, int bufferOffset, int count) {
            var remaining = prefixLength - prefixOffset;
            if (remaining <= 0) {
                return 0;
            }

            var toCopy = Math.Min(count, remaining);
            Buffer.BlockCopy(prefix, prefixOffset, buffer, bufferOffset, toCopy);
            prefixOffset += toCopy;
            return toCopy;
        }

        public override long Seek(long offset, SeekOrigin origin) {
            if (!CanSeek) {
                throw new NotSupportedException();
            }

            long target = origin switch {
                SeekOrigin.Begin => offset,
                SeekOrigin.Current => Position + offset,
                SeekOrigin.End => Length + offset,
                _ => throw new ArgumentOutOfRangeException(nameof(origin), origin, null)
            };

            Position = target;
            return Position;
        }

        public override void SetLength(long value) {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int bufferOffset, int count) {
            throw new NotSupportedException();
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                inner.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
