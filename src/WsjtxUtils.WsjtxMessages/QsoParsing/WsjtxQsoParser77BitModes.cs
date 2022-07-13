using System;
using System.Linq;
using WsjtxUtils.WsjtxMessages.Messages;

namespace WsjtxUtils.WsjtxMessages.QsoParsing
{
    /// <summary>
    /// QSO parser for WSJT-X modes with 77-bit message payloads
    /// </summary>
    internal class WsjtxQsoParser77BitModes : BaseOsoParser
    {
        private readonly Decode _decodeMessage;
        private readonly string[] _parts;
        private int _deCallSignIndex;
        private int _dxCallSignIndex;

        /// <summary>
        /// Constructs a QSO parser for WSJT-X modes with 77-bit message payloads
        /// </summary>
        /// <param name="decodeMessage"></param>
        public WsjtxQsoParser77BitModes(Decode decodeMessage)
        {
            _decodeMessage = decodeMessage;
            _parts = NormalizedSplit(_decodeMessage.Message);
        }

        /// <summary>
        /// Parse the QSO
        /// </summary>
        /// <returns></returns>
        public override WsjtxQso Parse()
        {
            // setup result object
            WsjtxQso result = new WsjtxQso(_decodeMessage);

            // if there is less than two parts, it's telemetry, freetext
            // and in an unknown QSO state
            var length = _parts.Length;
            if (length < 2)
            {
                result.Report = string.Join(" ", _parts);
                return result;
            }

            // check if the message used priori data and setup indexes
            result.UsedPriori = PrioriRegex.Match(_parts.Last()).Success;
            _deCallSignIndex = 1;
            _dxCallSignIndex = 0;

            // check if the decoded message is a station calling CQ
            if (IsCQ(_parts.First()))
            {
                ParseCQ(result);
            }
            else
            {
                // it is not a CQ, so calculate the number of parts and process accordingly
                var numOfParts = CalculateNumberOfParts(result);

                if (numOfParts == 2)
                    ParseTwoParts(result);
                else if (numOfParts == 3)
                    ParseThreeParts(result);
                else if (numOfParts > 3)
                    ParseFourOrMoreParts(result);
            }

            // process the callsigns
            ParseCallsigns(result);
            return result;
        }

        #region Private Methods
        /// <summary>
        /// Calculate the number of parts corrected for priori
        /// </summary>
        /// <param name="qso"></param>
        /// <returns></returns>
        private int CalculateNumberOfParts(WsjtxQso qso)
        {
            return _parts.Length - (qso.UsedPriori ? 1 : 0);
        }

        /// <summary>
        /// The index to the last part of the message corrected for priori
        /// </summary>
        /// <param name="qso"></param>
        /// <returns></returns>
        private int LastPartIndex(WsjtxQso qso)
        {
            return _parts.Length - (qso.UsedPriori ? 2 : 1);
        }

        /// <summary>
        /// The last part of the message corrected for priori
        /// </summary>
        /// <param name="qso"></param>
        /// <returns></returns>
        private string LastPart(WsjtxQso qso)
        {
            return _parts[LastPartIndex(qso)];
        }

        /// <summary>
        /// Parse a CQ message
        /// </summary>
        /// <param name="qso"></param>
        private void ParseCQ(WsjtxQso qso)
        {
            _dxCallSignIndex = -1;
            qso.QsoState = WsjtxQsoState.CallingCq;
            var lastPartIndex = LastPartIndex(qso);

            // Does the CQ call have a modifer (DX, POTA, TEST, etc.)
            if (lastPartIndex == 3)
            {
                qso.CallingModifier = _parts[1];
                _deCallSignIndex++;
            }

            // Find the gridsquare if provided
            if (_deCallSignIndex + (qso.UsedPriori ? 2 : 1) < _parts.Length && TryGetGridsquare(_parts[_deCallSignIndex + 1], out string gridSquare))
            {
                qso.GridSquare = gridSquare;
            }
        }

        /// <summary>
        /// Parse a two part message
        /// </summary>
        /// <param name="qso"></param>
        private void ParseTwoParts(WsjtxQso qso)
        {
            // is this two callsigns with nothing else?
            if ((IsValidCallsign(_parts[_dxCallSignIndex]) || _parts[_dxCallSignIndex] == "...") &&
                (IsValidCallsign(_parts[_deCallSignIndex]) || _parts[_deCallSignIndex] == "..."))
            {
                qso.QsoState = WsjtxQsoState.CallingStation;
            }
            else
            {
                _deCallSignIndex = _dxCallSignIndex = -1;
                qso.Report = string.Join(" ", _parts);
            }

        }

        /// <summary>
        /// Parse a three part message
        /// </summary>
        /// <param name="qso"></param>
        private void ParseThreeParts(WsjtxQso qso)
        {
            var lastPart = LastPart(qso);
            qso.Report = string.Join(" ", _parts.Skip(2));

            if (lastPart == "RRR" || lastPart == "RR73")
            {
                qso.QsoState = WsjtxQsoState.Rogers;
            }
            else if (lastPart == "73")
            {
                qso.QsoState = WsjtxQsoState.Signoff;
            }
            else if (TryGetReceptionReport(lastPart, out string report))
            {
                qso.QsoState = lastPart.StartsWith("R") ?
                        WsjtxQsoState.RogerReport :
                        WsjtxQsoState.Report;
            }
            else if (TryGetGridsquare(lastPart, out string gridSquare))
            {
                qso.Report = string.Empty;
                qso.GridSquare = gridSquare;
                qso.QsoState = WsjtxQsoState.CallingStation;
            }
            else
            {
                _deCallSignIndex = _dxCallSignIndex = -1;
                qso.Report = string.Join(" ", _parts);
            }
        }

        /// <summary>
        /// Parse a four or more part message
        /// </summary>
        /// <param name="qso"></param>
        private void ParseFourOrMoreParts(WsjtxQso qso)
        {
            var lastPartIndex = LastPartIndex(qso);
            var lastPart = _parts[lastPartIndex];
            var lastPartPrefix = _parts[lastPartIndex - 1];

            if (TryGetGridsquare(lastPart, out string gridSquare) ||
                (IsArrlSection(lastPart) && IsFieldDayExchange(lastPartPrefix)) ||
                (IsUSStateOrCanadianProvinces(lastPart) && lastPartPrefix.All(char.IsNumber)) ||
                (lastPart.All(char.IsNumber) && lastPartPrefix.All(char.IsNumber)))
            {
                qso.GridSquare = gridSquare;
                qso.Report = string.Join(" ", _parts.Skip(2));

                qso.QsoState = (lastPartPrefix == "R" || lastPartIndex == 4) ?
                    WsjtxQsoState.RogerReport :
                    WsjtxQsoState.Report;
            }
            else
            {
                qso.Report = string.Join(" ", _parts);
            }
        }

        /// <summary>
        /// Parse callsigns if available
        /// </summary>
        /// <param name="qso"></param>
        private void ParseCallsigns(WsjtxQso qso)
        {
            if (_deCallSignIndex > -1 && TryGetCallsign(_parts[_deCallSignIndex], out string deCallsign))
            {
                qso.DECallsign = deCallsign;
            }

            if (_dxCallSignIndex > -1 && TryGetCallsign(_parts[_dxCallSignIndex], out string dxCallsign))
            {
                qso.DXCallsign = dxCallsign;
            }
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// Cleans an FT mode message split into relevant parts
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private static string[] NormalizedSplit(string message)
        {
            // remove semicolons
            var semicolonIndex = message.IndexOf(';');
            if (semicolonIndex > -1)
                message = message.Substring(semicolonIndex + 1);

            // remove lt, gt, and split on spaces
            return message
                .Replace("<", "")
                .Replace(">", "")
                .Split(new[] { ' ' }, options:StringSplitOptions.RemoveEmptyEntries)
                .Where(part => part != "?")
                .ToArray();
        }

        /// <summary>
        /// Is the part a CQ or QRZ
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        private static bool IsCQ(string part)
        {
            return part == "CQ" || part == "QRZ";
        }
        #endregion
    }
}
