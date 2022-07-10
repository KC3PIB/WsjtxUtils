using System;
using System.Buffers.Binary;
using System.Runtime.InteropServices;
using System.Text;
using WsjtxUtils.WsjtxMessages.Messages;

namespace WsjtxUtils.WsjtxMessages
{
    /// <summary>
    /// Writes WSJT-X messages to a memory source
    /// </summary>
    public class WsjtxMessageWriter : WsjtxBaseReaderWriter
    {
        /// <summary>
        /// Constructs a WSJT-X message writer to the specified memory source
        /// </summary>
        /// <param name="source"></param>
        public WsjtxMessageWriter(Memory<byte> source) : base(source)
        {
            if (source.Length < WsjtxConstants.HeaderLengthInBytes)
                throw new ArgumentException($"Expecting greater than {WsjtxConstants.HeaderLengthInBytes} bytes for space to fit a valid message, {buffer.Length} found. Recommended size is 1500 bytes.");
        }

        /// <summary>
        /// Write a single byte value to the buffer
        /// </summary>
        /// <returns></returns>
        public void WriteByte(byte value)
        {
            buffer.Span[Position++] = value;
        }

        /// <summary>
        /// Write a boolean value to the buffer
        /// </summary>
        /// <param name="value"></param>
        public void WriteBool(bool value)
        {
            WriteByte(value ? WsjtxConstants.TrueValue : WsjtxConstants.FalseValue);
        }

        /// <summary>
        /// Write an Int16 to the buffer
        /// </summary>
        /// <param name="value"></param>
        public void WriteInt16(short value)
        {
            BinaryPrimitives.WriteInt16BigEndian(buffer.Span.Slice(Position, WsjtxConstants.SizeOfShort), value);
            Position += WsjtxConstants.SizeOfShort;
        }

        /// <summary>
        /// Write an UInt16 to the buffer
        /// </summary>
        /// <param name="value"></param>
        public void WriteUInt16(ushort value)
        {
            BinaryPrimitives.WriteUInt16BigEndian(buffer.Span.Slice(Position, WsjtxConstants.SizeOfShort), value);
            Position += WsjtxConstants.SizeOfShort;
        }

        /// <summary>
        /// Write an Int32 to the buffer
        /// </summary>
        /// <returns></returns>
        public void WriteInt32(int value)
        {
            BinaryPrimitives.WriteInt32BigEndian(buffer.Span.Slice(Position, WsjtxConstants.SizeOfInt), value);
            Position += WsjtxConstants.SizeOfInt;
        }

        /// <summary>
        /// Write an Int32 to the buffer
        /// </summary>
        /// <returns></returns>
        public void WriteUInt32(uint value)
        {
            BinaryPrimitives.WriteUInt32BigEndian(buffer.Span.Slice(Position, WsjtxConstants.SizeOfInt), value);
            Position += WsjtxConstants.SizeOfInt;
        }

        /// <summary>
        /// Write an Int64 to the buffer
        /// </summary>
        /// <returns></returns>
        public void WriteInt64(long value)
        {
            BinaryPrimitives.WriteInt64BigEndian(buffer.Span.Slice(Position, WsjtxConstants.SizeOfLong), value);
            Position += WsjtxConstants.SizeOfLong;
        }

        /// <summary>
        /// Write an UInt64 to the buffer
        /// </summary>
        /// <returns></returns>
        public void WriteUInt64(ulong value)
        {
            BinaryPrimitives.WriteUInt64BigEndian(buffer.Span.Slice(Position, WsjtxConstants.SizeOfLong), value);
            Position += WsjtxConstants.SizeOfLong;
        }

        /// <summary>
        /// Write a double to the buffer
        /// </summary>
        /// <param name="value"></param>
        public void WriteDouble(double value)
        {
            WriteInt64(BitConverter.DoubleToInt64Bits(value));
        }

        /// <summary>
        /// Write a string to the buffer
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="InsufficientMemoryException">Exception thrown if the string size exceeds the allocated buffer</exception>
        public void WriteString(string value)
        {
            int textByteCount = string.IsNullOrEmpty(value) ? 0 : Encoding.UTF8.GetByteCount(value);

            BinaryPrimitives.WriteUInt32BigEndian(buffer.Span.Slice(Position, WsjtxConstants.SizeOfInt), Convert.ToUInt32(textByteCount));
            Position += WsjtxConstants.SizeOfInt;

            if (textByteCount == 0)
                return;

            if (!MemoryMarshal.TryGetArray(buffer.Slice(Position), out ArraySegment<byte> segment) && segment.Array != null)
                throw new InsufficientMemoryException("Unable to allocate the array from the underlying buffer.");

            Encoding.UTF8.GetBytes(value, 0, value.Length, segment.Array!, segment.Offset);
            Position += textByteCount;
        }

        /// <summary>
        /// Write a <see cref="QColor" /> to the buffer
        /// </summary>
        /// <param name="color"></param>
        public void WriteColor(QColor color)
        {
            /*
                https://doc.qt.io/archives/qt-5.11/datastreamformat.html
                Color spec (qint8)
                Alpha value (quint16)
                Red value (quint16)
                Green value (quint16)
                Blue value (quint16)
                Pad value (quint16)
            */

            // Write color spec
            WriteEnum(color.Spec);

            // Write color values
            WriteUInt16(color.Alpha);
            WriteUInt16(color.Red);
            WriteUInt16(color.Green);
            WriteUInt16(color.Blue);

            // Write pad value
            WriteUInt16(0);
        }

        /// <summary>
        /// Write an enumeration of type T to the buffer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <exception cref="NotImplementedException">Exception thrown if the underlying type is not implemented</exception>
        public void WriteEnum<T>(T value)
        {

            var enumType = typeof(T);
            var underlyingType = Enum.GetUnderlyingType(enumType);
            var typeCode = Type.GetTypeCode(underlyingType);

            switch (typeCode)
            {
                case TypeCode.Int32:
                    WriteInt32(Convert.ToInt32(value));
                    break;
                case TypeCode.UInt32:
                    WriteUInt32(Convert.ToUInt32(value));
                    break;
                case TypeCode.Int64:
                    WriteInt64(Convert.ToUInt32(value));
                    break;
                case TypeCode.UInt64:
                    WriteUInt64(Convert.ToUInt64(value));
                    break;
                case TypeCode.Byte:
                    WriteByte(Convert.ToByte(value));
                    break;
                default:
                    throw new NotImplementedException($"Enum {enumType.Name} has a type of {underlyingType.Name} which is not implemented");
            }
        }
    }
}
