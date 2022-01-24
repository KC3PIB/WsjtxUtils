using System;

namespace WsjtxUtils.WsjtxMessages
{
    /// <summary>
    /// Base class to support WSJTX readers and writers
    /// </summary>
    public abstract class WsjtxBaseReaderWriter
    {
        /// <summary>
        /// Current index into the buffer
        /// </summary>
        private int _position;

        /// <summary>
        /// Continuous memory buffer
        /// </summary>
        protected Memory<byte> buffer;

        /// <summary>
        /// Constructor for base message readers and writers
        /// </summary>
        /// <param name="buffer"></param>
        protected WsjtxBaseReaderWriter(Memory<byte> buffer)
        {
            this.buffer = buffer;
        }

        /// <summary>
        /// Length of the underlying buffer
        /// </summary>
        public int BufferLength { get => buffer.Length; }

        /// <summary>
        /// Get or set the current position within the message data
        /// </summary>
        public int Position { get => _position; set => SetPostion(value); }

        /// <summary>
        /// Set the index into the underlying array
        /// </summary>
        /// <param name="position"></param>
        private void SetPostion(int position)
        {
            if (position < 0 || position > buffer.Length)
                throw new ArgumentOutOfRangeException($"Postion {position} should not be less than zero or greater than {buffer.Length}.");

            _position = position;
        }
    }
}
