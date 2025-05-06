using System;

namespace WsjtxUtils.WsjtxMessages.Messages
{
    /// <summary>
    /// WSJT-X QsoLogged message
    /// </summary>
    /// <remarks>
    /// The QSO logged message is  sent to the server(s)
    /// when the WSJT-X user accepts the "Log  QSO" dialog
    /// by clicking the "OK" button.
    /// </remarks>
    public class QsoLogged : WsjtxMessage, IWsjtxDirectionOut
    {
        /// <summary>
        /// Constructs a default WSJT-X QsoLogged message
        /// </summary>
        public QsoLogged() : base(string.Empty, MessageType.QSOLogged)
        {
            DXCall = string.Empty;
            DXGrid = string.Empty;
            Mode = string.Empty;
            ReportSent = string.Empty;
            ReportReceived = string.Empty;
            TXPower = string.Empty;
            Comments = string.Empty;
            Name = string.Empty;
            OperatorCall = string.Empty;
            MyCall = string.Empty;
            MyGrid = string.Empty;
            ExchangeSent = string.Empty;
            ExchangeReceived = string.Empty;
            AdifPropagationMode = string.Empty;
        }

        /// <summary>
        /// Date and time of the end of the QSO
        /// </summary>
        public DateTime DateTimeOff { get; set; }

        /// <summary>
        /// Value of the DX Call field
        /// </summary>
        public string DXCall { get; set; }

        /// <summary>
        /// Value of the DX grid field
        /// </summary>
        public string DXGrid { get; set; }

        /// <summary>
        /// TX Frequency (Hz)
        /// </summary>
        public ulong TXFrequencyInHz { get; set; }

        /// <summary>
        /// WSJT-X Operating mode
        /// </summary>
        public string Mode { get; set; }

        /// <summary>
        /// Signal report sent
        /// </summary>
        public string ReportSent { get; set; }

        /// <summary>
        /// Signal report received
        /// </summary>
        public string ReportReceived { get; set; }

        /// <summary>
        /// TX power
        /// </summary>
        public string TXPower { get; set; }

        /// <summary>
        /// Comments field
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Remote operator's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Date and time of the start of the QSO
        /// </summary>
        public DateTime DateTimeOn { get; set; }

        /// <summary>
        /// Local operator's call sign
        /// </summary>
        public string OperatorCall { get; set; }

        /// <summary>
        /// Call sign sent
        /// </summary>
        public string MyCall { get; set; }

        /// <summary>
        /// Madienhead gridsquare sent
        /// </summary>
        public string MyGrid { get; set; }

        /// <summary>
        /// Exchange message sent
        /// </summary>
        public string ExchangeSent { get; set; }

        /// <summary>
        /// Exchange message received
        /// </summary>
        public string ExchangeReceived { get; set; }

        /// <summary>
        /// Propagation mode
        /// </summary>
        public string AdifPropagationMode { get; set; }

        #region IWsjtxDirectionOut
        /// <summary>
        ///  Using the <see cref="WsjtxMessageReader"/>, deserialize the values to the current message
        /// </summary>
        /// <param name="messageReader"></param>
        public override void ReadMessage(WsjtxMessageReader messageReader)
        {
            base.ReadMessage(messageReader);

            DateTimeOff = messageReader.ReadQDateTime();
            DXCall = messageReader.ReadString();
            DXGrid = messageReader.ReadString();
            TXFrequencyInHz = messageReader.ReadUInt64();
            Mode = messageReader.ReadString();
            ReportSent = messageReader.ReadString();
            ReportReceived = messageReader.ReadString();
            TXPower = messageReader.ReadString();
            Comments = messageReader.ReadString();
            Name = messageReader.ReadString();
            DateTimeOn = messageReader.ReadQDateTime();
            OperatorCall = messageReader.ReadString();
            MyCall = messageReader.ReadString();
            MyGrid = messageReader.ReadString();

            // check if there is any remaining data. this is a workaround for #113, JTDX packets
            if (!messageReader.IsDataAvailable())
                return;
            
            ExchangeSent = messageReader.ReadString();
            ExchangeReceived = messageReader.ReadString();
            
            // check if this is a newer WSJT-X packet with Propagation Mode
            if (messageReader.IsDataAvailable())
                AdifPropagationMode = messageReader.ReadString();
        }
        #endregion
    }
}
