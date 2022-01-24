using System.Net;
using System.Threading;
using System.Threading.Tasks;
using WsjtxUtils.WsjtxMessages.Messages;

namespace WsjtxUtils.WsjtxUdpServer
{
    /// <summary>
    /// Interface describing WSJT-X UDP Message Handler
    /// </summary>
    public interface IWsjtxUdpMessageHandler
    {
        /// <summary>
        /// Handle WSJT-X <see cref="Heartbeat"/> messages
        /// </summary>
        /// <param name="server"></param>
        /// <param name="message"></param>
        /// <param name="endPoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task HandleHeartbeatMessageAsync(WsjtxUdpServer server, Heartbeat message, EndPoint endPoint, CancellationToken cancellationToken = default);

        /// <summary>
        /// Handle WSJT-X <see cref="Status"/> messages
        /// </summary>
        /// <param name="server"></param>
        /// <param name="message"></param>
        /// <param name="endPoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task HandleStatusMessageAsync(WsjtxUdpServer server, Status message, EndPoint endPoint, CancellationToken cancellationToken = default);

        /// <summary>
        /// Handle WSJT-X <see cref="Decode"/> messages
        /// </summary>
        /// <param name="server"></param>
        /// <param name="message"></param>
        /// <param name="endPoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task HandleDecodeMessageAsync(WsjtxUdpServer server, Decode message, EndPoint endPoint, CancellationToken cancellationToken = default);

        /// <summary>
        /// Handle WSJT-X <see cref="Clear"/> messages
        /// </summary>
        /// <param name="server"></param>
        /// <param name="message"></param>
        /// <param name="endPoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task HandleClearMessageAsync(WsjtxUdpServer server, Clear message, EndPoint endPoint, CancellationToken cancellationToken = default);

        /// <summary>
        /// Handle WSJT-X <see cref="QsoLogged"/> messages
        /// </summary>
        /// <param name="server"></param>
        /// <param name="message"></param>
        /// <param name="endPoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task HandleQsoLoggedMessageAsync(WsjtxUdpServer server, QsoLogged message, EndPoint endPoint, CancellationToken cancellationToken = default);

        /// <summary>
        /// Handle WSJT-X <see cref="Close"/> messages
        /// </summary>
        /// <param name="server"></param>
        /// <param name="message"></param>
        /// <param name="endPoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task HandleClosedMessageAsync(WsjtxUdpServer server, Close message, EndPoint endPoint, CancellationToken cancellationToken = default);

        /// <summary>
        /// Handle WSJT-X <see cref="WSPRDecode"/> messages
        /// </summary>
        /// <param name="server"></param>
        /// <param name="message"></param>
        /// <param name="endPoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task HandleWSPRDecodeMessageAsync(WsjtxUdpServer server, WSPRDecode message, EndPoint endPoint, CancellationToken cancellationToken = default);

        /// <summary>
        /// Handle WSJT-X <see cref="LoggedAdif"/> messages
        /// </summary>
        /// <param name="server"></param>
        /// <param name="message"></param>
        /// <param name="endPoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task HandleLoggedAdifMessageAsync(WsjtxUdpServer server, LoggedAdif message, EndPoint endPoint, CancellationToken cancellationToken = default);
    }
}
