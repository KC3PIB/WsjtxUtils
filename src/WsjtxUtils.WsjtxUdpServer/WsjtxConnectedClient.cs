using System;
using System.Net;
using WsjtxUtils.WsjtxMessages.Messages;

namespace WsjtxUtils.WsjtxUdpServer
{
    /// <summary>
    /// A client connected to the WSJT-X UDP server
    /// </summary>
    public class WsjtxConnectedClient
    {
        /// <summary>
        /// Constructs a connected client tracking object
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="endpoint"></param>
        /// <param name="status"></param>
        public WsjtxConnectedClient(string clientId, EndPoint endpoint, Status? status = null)
        {
            ClientId = clientId;
            Endpoint = endpoint;
            Status = status;
            LastCommunications = DateTime.UtcNow;
        }

        /// <summary>
        /// The unique id for the connected client
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// The UDP enpoint of the connected client
        /// </summary>
        public EndPoint Endpoint { get; set; }

        /// <summary>
        /// The last status message from the connected client
        /// </summary>
        public Status? Status { get; set; }

        /// <summary>
        /// The last datetime of communications from the client
        /// </summary>
        public DateTime LastCommunications { get; set; }
    }
}
