using System;
using System.Buffers.Binary;
using System.Runtime.InteropServices;
using System.Text;
using WsjtxUtils.WsjtxMessages.Messages;

namespace WsjtxUtils.WsjtxMessages
{
    /// <summary>
    /// Reads WSJT-X messages from a memory source
    /// </summary>
    public class WsjtxMessageReader : WsjtxBaseReaderWriter
    {
        /// <summary>
        /// Constructs a WSJT-X message reader from the specified memory source
        /// </summary>
        /// <param name="source"></param>
        public WsjtxMessageReader(Memory<byte> source) : base(source)
        {
            if (source.Length < WsjtxConstants.HeaderLengthInBytes)
                throw new ArgumentException($"Expecting greater than {WsjtxConstants.HeaderLengthInBytes} bytes for a valid message, {buffer.Length} found.");

            if (!StartsWithMagicNumber(source.Span))
                throw new ArgumentException($"Expecting magic number 0xadbccbda, not found.");
        }

        /// <summary>
        /// Checks for the expected magic number 0xadbccbda in the first four bytes
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool StartsWithMagicNumber(ReadOnlySpan<byte> data)
        {
            return BinaryPrimitives.ReadUInt32BigEndian(data) == WsjtxConstants.MagicNumber;
        }

        /// <summary>
        /// Peek at the message type keeping original position
        /// </summary>
        /// <returns></returns>
        public MessageType PeekMessageType()
        {
            // store the original position and move to messagetype offset
            var originalPosition = Position;
            Position = WsjtxConstants.MessageTypePosition;

            var messageType = ReadMessageType();

            // restore the original position
            Position = originalPosition;

            return messageType;
        }

        /// <summary>
        /// Read a single byte from the buffer
        /// </summary>
        /// <returns></returns>
        public byte ReadByte()
        {
            return buffer.Span[Position++];
        }

        /// <summary>
        /// Read a boolean value from the buffer
        /// </summary>
        /// <returns></returns>
        public bool ReadBool()
        {
            return buffer.Span[Position++] == WsjtxConstants.TrueValue;
        }

        /// <summary>
        /// Read an Int16 to the buffer
        /// </summary>
        public short ReadInt16()
        {
            short result = BinaryPrimitives.ReadInt16BigEndian(buffer.Span.Slice(Position, 2));
            Position += WsjtxConstants.SizeOfShort;
            return result;
        }

        /// <summary>
        /// Read an UInt16 to the buffer
        /// </summary>
        public ushort ReadUInt16()
        {
            ushort result = BinaryPrimitives.ReadUInt16BigEndian(buffer.Span.Slice(Position, 2));
            Position += WsjtxConstants.SizeOfShort;
            return result;
        }

        /// <summary>
        /// Read an Int32 from the buffer
        /// </summary>
        /// <returns></returns>
        public int ReadInt32()
        {
            int result = BinaryPrimitives.ReadInt32BigEndian(buffer.Span.Slice(Position, 4));
            Position += WsjtxConstants.SizeOfInt;
            return result;
        }

        /// <summary>
        /// Read an Int32 from the buffer
        /// </summary>
        /// <returns></returns>
        public uint ReadUInt32()
        {
            uint result = BinaryPrimitives.ReadUInt32BigEndian(buffer.Span.Slice(Position, 4));
            Position += WsjtxConstants.SizeOfInt;
            return result;
        }

        /// <summary>
        /// Read an Int64 from the buffer
        /// </summary>
        /// <returns></returns>
        public long ReadInt64()
        {
            long result = BinaryPrimitives.ReadInt64BigEndian(buffer.Span.Slice(Position, 8));
            Position += WsjtxConstants.SizeOfLong;
            return result;
        }

        /// <summary>
        /// Read an Int64 from the buffer
        /// </summary>
        /// <returns></returns>
        public ulong ReadUInt64()
        {
            ulong result = BinaryPrimitives.ReadUInt64BigEndian(buffer.Span.Slice(Position, 8));
            Position += WsjtxConstants.SizeOfLong;
            return result;
        }

        /// <summary>
        /// Read a double from the buffer
        /// </summary>
        /// <returns></returns>
        public double ReadDouble()
        {
            return BitConverter.Int64BitsToDouble(ReadInt64());
        }

        /// <summary>
        /// Read a string from the buffer
        /// </summary>
        /// <remarks>For simplicity, this method will return <see cref="String.Empty"/> for null values</remarks>
        /// <exception cref="InsufficientMemoryException">Exception thrown if the string size exceeds the allocated buffer</exception>
        /// <returns></returns>
        public string ReadString()
        {
            uint size = ReadUInt32();
            if (size == 0 || size == uint.MaxValue)
                return string.Empty;

            if (MemoryMarshal.TryGetArray(buffer[Position..], out ArraySegment<byte> segment) && segment.Array != null)
            {
                var length = Convert.ToInt32(size);
                Position += length;
                return Encoding.UTF8.GetString(segment.Array!, segment.Offset, length);
            }

            throw new InsufficientMemoryException("Unable to allocate the array from the buffer.");
        }

        /// <summary>
        /// Read the schema version
        /// </summary>
        /// <returns></returns>
        public SchemaVersion ReadSchemaVersion()
        {
            return ReadEnum<SchemaVersion>();
        }

        /// <summary>
        /// Read the message type
        /// </summary>
        /// <returns></returns>
        public MessageType ReadMessageType()
        {
            return ReadEnum<MessageType>();
        }

        /// <summary>
        /// Read the special operation mode
        /// </summary>
        /// <returns></returns>
        public SpecialOperationMode ReadSpecialOperationMode()
        {
            return ReadEnum<SpecialOperationMode>();
        }

        /// <summary>
        /// Read the bytes from the backing stream and convert into the enum of type T
        /// </summary>
        /// <typeparam name="T">Type of enumeration</typeparam>
        /// <exception cref="NotImplementedException">Exception thrown if the underlying type is not implemented</exception>
        /// <returns></returns>
        public T ReadEnum<T>() where T : Enum
        {
            var enumType = typeof(T);
            var underlyingType = Enum.GetUnderlyingType(enumType);
            var typeCode = Type.GetTypeCode(underlyingType);

            object value = typeCode switch
            {
                TypeCode.UInt32 => ReadUInt32(),
                TypeCode.Byte => ReadByte(),
                _ => throw new NotImplementedException($"Enum {enumType.Name} has a type of {underlyingType.Name} which is not implemented")
            };

            return (T)Enum.ToObject(enumType, value);
        }

        /// <summary>
        /// Read a QT Date value
        /// </summary>
        /// <returns></returns>
        public DateTime ReadQDate()
        {
            // Dates are stored internally as a Julian Day number, an integer count of every day in a contiguous range,
            // with 24 November 4714 BCE in the Gregorian calendar
            return DateTime.FromOADate(ReadInt64() - WsjtxConstants.JulianConstant).Date;
        }

        /// <summary>
        /// Read a QT Date time value
        /// </summary>
        /// <exception cref="NotImplementedException">Exception thrown if the <see cref="Timespec"/> is <see cref="Timespec.TimeZone"/> or unknown</exception>
        /// <returns></returns>
        public DateTime ReadQDateTime()
        {
            var result = ReadQDate();
            result = result.AddMilliseconds(ReadUInt32());
            var timespec = ReadEnum<Timespec>();

            DateTimeKind kind;
            switch (timespec)
            {
                case Timespec.Local:
                    kind = DateTimeKind.Local;
                    break;
                case Timespec.UTC:
                    kind = DateTimeKind.Utc;
                    break;
                case Timespec.OffsetFromUTCSeconds:
                    result.AddSeconds(ReadInt32());
                    kind = DateTimeKind.Utc;
                    break;
                default:
                    throw new NotImplementedException($"Timespec {timespec} is not implemented.");
            }

            return DateTime.SpecifyKind(result, kind);
        }
    }
}
