using System;
using WsjtxUtils.WsjtxMessages.Messages;

namespace WsjtxUtils.WsjtxMessages.QsoParsing
{
    /// <summary>
    /// WSJT-X decode message QSO parser
    /// </summary>
    public static class WsjtxQsoParser
    {
        /// <summary>
        /// Attempt to parse as much information as possible about the OSO of a WSJT-X decode packet
        /// </summary>
        /// <param name="decode"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static WsjtxQso ParseDecode(string mode, Decode decode)
        {
            switch (mode)
            {
                case "FST4":
                case "FT4":
                case "FT8":
                case "MSK144":
                case "Q65":
                    return new WsjtxQsoParser77BitModes(decode).Parse();
                default:
                    throw new NotImplementedException($"A QSO parser for {decode.Mode} is not implemented");
            }
        }

        /// <summary>
        /// Attempt to parse as much information as possible about the OSO of a WSJT-X decode packet
        /// </summary>
        /// <param name="status"></param>
        /// <param name="decode"></param>
        /// <returns></returns>
        public static WsjtxQso ParseDecode(Status status, Decode decode)
        {
            return ParseDecode(status.Mode, decode);
        }
    }
}
