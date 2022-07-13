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
        /// <returns></returns>
        public static WsjtxQso ParseDecode(Decode decode)
        {
            var mode = decode.DecodeModeNotationsToString();
            switch (mode)
            {
                case "FST4":
                case "FT4":
                case "FT8":
                case "MSK144":
                case "Q65":
                    return new WsjtxQsoParser77BitModes(decode).Parse();
                case "JT4":
                case "JT9":
                case "JT65":
                    //TODO: 72-bit message payloads: JT4, JT9, and JT65
                default:
                    throw new NotImplementedException($"A QSO parser for {mode} is not implemented");
            }
        }
    }
}
