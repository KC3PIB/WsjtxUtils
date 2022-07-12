using System.Text.RegularExpressions;
using WsjtxUtils.WsjtxMessages.Messages;

namespace WsjtxUtils.WsjtxMessages.QsoParsing
{
    /// <summary>
    /// Base QSO parsing class
    /// </summary>
    internal abstract class BaseOsoParser
    {
        /// <summary>
        /// Regular expression to match ARRL sections
        /// </summary>
        protected static Regex ArrlSectionRegex = new Regex(@"^(A[BKLRZ]|BC|C[OT]|D[EX]|EB|E[MNPW][AY]|GA|GTA|I[ADLN]|K[SY]|LA[X]?|M[ABDEINOST][CR]?|N[C-FHL-NTV][IJLYX]?|O[HKNR][ENSG]?|PAC|P[ER]|QC|RI|S[CDFJKNTV][GJLVX]?|TN|UT|V[AIT]|W[CIMNPTVWY][AFYX]?)$");

        /// <summary>
        /// Regular expression to match field day exchange
        /// </summary>
        protected static Regex FieldDayExchangeRegex = new Regex(@"^([\d]+[A-F])$");

        /// <summary>
        /// Regular expression to match signal reports
        /// </summary>
        protected static Regex ReceptionReportRegex = new Regex(@"^([R]?[+-]+[\d]+|0)$", RegexOptions.Compiled);

        /// <summary>
        /// Regular expression to match 4 or 6 character grid squares
        /// </summary>
        protected static Regex GridSquareLocatorRegex = new Regex(@"^([A-R]{2}[\d]{2}([A-Z]{2})?)$", RegexOptions.Compiled);

        /// <summary>
        /// Regular expression to match Canadian provinces
        /// </summary>
        protected static Regex CanadianProvincesRegex = new Regex(@"^(?:AB|BC|MB|N[BLTSU]|ON|PE|QC|SK|YT)$", RegexOptions.Compiled);

        /// <summary>
        /// Regular expression to match US states
        /// </summary>
        protected static Regex USStatesRegex = new Regex(@"^(?:A[KLRZ]|C[AOT]|D[CE]|FL|GA|HI|I[ADLN]|K[SY]|LA|M[ADEINOST]|N[CDEHJMVY]|O[HKR]|PA|RI|S[CD]|T[NX]|UT|V[AT]|W[AIVY])$", RegexOptions.Compiled);

        /// <summary>
        /// Regular expression to match and validate callsigns
        /// </summary>
        protected static Regex CallsignRegex = new Regex(
            // portable prefix
            @"^(?<portableprefix>(?:(?:[A-NPR-Z](?:(?:[A-Z](?:\d[A-Z]?)?)|(?:\d[\dA-Z]?))?)|(?:[2-9][A-Z]{1,2}\d?))\/)?" +
            // prefix
            @"(?<callprefix>(?:(?:[A-NPR-Z][A-Z]?)|(?:[2-9][A-Z]{1,2}))\d)" +
            // suffix
            @"(?<callsuffix>\d{0,3}[A-Z]{1,6})" +
            // modifier
            @"(?<modifier>\/[\dA-Z]{1,4})?$",
            RegexOptions.Compiled);

        /// <summary>
        /// Regular expression to match the use of priori
        /// </summary>
        protected static Regex PrioriRegex = new Regex(@"^(a[1-6])$");

        /// <summary>
        /// Parse a <see cref="Decode"/> message into a <see cref="WsjtxQso"/>
        /// </summary>
        /// <returns></returns>
        public abstract WsjtxQso Parse();



        #region Protected Methods
        /// <summary>
        /// Try to get a signal report value from the input
        /// </summary>
        /// <param name="input"></param>
        /// <param name="report"></param>
        /// <returns></returns>
        protected bool TryGetReceptionReport(string input, out string report)
        {
            return TryGetRegex(ReceptionReportRegex, input, out report);
        }

        /// <summary>
        /// Try to get a gridsquare value from the input
        /// </summary>
        /// <param name="input"></param>
        /// <param name="gridSquare"></param>
        /// <returns></returns>
        protected bool TryGetGridsquare(string input, out string gridSquare)
        {
            return TryGetRegex(GridSquareLocatorRegex, input, out gridSquare);
        }

        /// <summary>
        /// Try to get a callsign value from the input
        /// </summary>
        /// <param name="input"></param>
        /// <param name="callsign"></param>
        /// <returns></returns>
        protected bool TryGetCallsign(string input, out string callsign)
        {
            return TryGetRegex(CallsignRegex, input, out callsign);
        }

        /// <summary>
        /// Is the input a callsign
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected bool IsValidCallsign(string input)
        {
            return CallsignRegex.Match(input).Success;
        }

        /// <summary>
        /// Is the input an ARRL Section
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected bool IsArrlSection(string input)
        {
            return ArrlSectionRegex.Match(input).Success;
        }

        /// <summary>
        /// Is the input a field day exchange
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected bool IsFieldDayExchange(string input)
        {
            return FieldDayExchangeRegex.Match(input).Success;
        }

        /// <summary>
        /// Is the input a US state or Canadian province abbreviation
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected bool IsUSStateOrCanadianProvinces(string input)
        {
            return USStatesRegex.Match(input).Success || CanadianProvincesRegex.Match(input).Success;
        }
        #endregion

        #region static methods
        /// <summary>
        /// Try to get the matching value in the regex
        /// </summary>
        /// <param name="regex"></param>
        /// <param name="input"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool TryGetRegex(Regex regex, string input, out string value)
        {
            value = string.Empty;
            var match = regex.Match(input);
            if (!match.Success)
                return false;

            value = match!.Value;
            return true;
        }
        #endregion
    }
}
