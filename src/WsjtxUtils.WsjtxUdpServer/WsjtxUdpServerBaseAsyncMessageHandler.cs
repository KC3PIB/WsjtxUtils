using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using WsjtxUtils.WsjtxMessages.Messages;

namespace WsjtxUtils.WsjtxUdpServer
{
    /// <summary>
    /// A base WSJT-X UDP server message handler that tracks the client id and remote
    /// endpoint of WSJT-X clients that have recently communicated with the server
    /// </summary>
    public abstract class WsjtxUdpServerBaseAsyncMessageHandler : IWsjtxUdpMessageHandler
    {
        /// <summary>
        /// A function to be executed when a WSJT-X client connects for the first time
        /// </summary>
        public virtual Func<WsjtxConnectedClient, Task>? ClientConnectedCallback { get; set; }

        /// <summary>
        /// A function to be executed when a WSJT-X client closes the main window
        /// </summary>
        public virtual Func<WsjtxConnectedClient, Task>? ClientClosedCallback { get; set; }

        /// <summary>
        /// A function to be executed when a WSJT-X client is expired for a lack of communication which
        /// exceeds the <see cref="ConnectedClientExpiryInSeconds"/> period
        /// </summary>
        public virtual Func<WsjtxConnectedClient, Task>? ClientExpiredCallback { get; set; }

        /// <summary>
        /// List of connected WSJT-X clients
        /// </summary>
        public ConcurrentDictionary<string, WsjtxConnectedClient> ConnectedClients { get; protected set; }
            = new ConcurrentDictionary<string, WsjtxConnectedClient>();

        /// <summary>
        /// The period in seconds that a connected client will exist in the
        /// <see cref="ConnectedClients"/> with no communication
        /// </summary>
        public virtual int ConnectedClientExpiryInSeconds { get; set; } = 300; // default 5 mins

        #region Message Handlers
        /// <summary>
        /// Handle WSJT-X <see cref="Heartbeat"/> messages
        /// </summary>
        /// <param name="server"></param>
        /// <param name="message"></param>
        /// <param name="endPoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task HandleHeartbeatMessageAsync(WsjtxUdpServer server, Heartbeat message, EndPoint endPoint, CancellationToken cancellationToken = default)
        {
            AddUpdateOrExpireClient(message.Id, endPoint);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Handle WSJT-X <see cref="Status"/> messages
        /// </summary>
        /// <param name="server"></param>
        /// <param name="message"></param>
        /// <param name="endPoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task HandleStatusMessageAsync(WsjtxUdpServer server, Status message, EndPoint endPoint, CancellationToken cancellationToken = default)
        {
            AddUpdateOrExpireClient(message.Id, endPoint, message);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Handle WSJT-X <see cref="Decode"/> messages
        /// </summary>
        /// <param name="server"></param>
        /// <param name="message"></param>
        /// <param name="endPoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task HandleDecodeMessageAsync(WsjtxUdpServer server, Decode message, EndPoint endPoint, CancellationToken cancellationToken = default)
        {
            AddUpdateOrExpireClient(message.Id, endPoint);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Handle WSJT-X <see cref="Clear"/> messages
        /// </summary>
        /// <param name="server"></param>
        /// <param name="message"></param>
        /// <param name="endPoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task HandleClearMessageAsync(WsjtxUdpServer server, Clear message, EndPoint endPoint, CancellationToken cancellationToken = default)
        {
            AddUpdateOrExpireClient(message.Id, endPoint);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Handle WSJT-X <see cref="QsoLogged"/> messages
        /// </summary>
        /// <param name="server"></param>
        /// <param name="message"></param>
        /// <param name="endPoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task HandleQsoLoggedMessageAsync(WsjtxUdpServer server, QsoLogged message, EndPoint endPoint, CancellationToken cancellationToken = default)
        {
            AddUpdateOrExpireClient(message.Id, endPoint);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Handle WSJT-X <see cref="Close"/> messages
        /// </summary>
        /// <param name="server"></param>
        /// <param name="message"></param>
        /// <param name="endPoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task HandleClosedMessageAsync(WsjtxUdpServer server, Close message, EndPoint endPoint, CancellationToken cancellationToken = default)
        {
            // remove the client and fire the closed event
            if (ConnectedClients.TryRemove(message.Id, out WsjtxConnectedClient? target))
                ClientClosedCallback?.Invoke(target);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Handle WSJT-X <see cref="WSPRDecode"/> messages
        /// </summary>
        /// <param name="server"></param>
        /// <param name="message"></param>
        /// <param name="endPoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task HandleWSPRDecodeMessageAsync(WsjtxUdpServer server, WSPRDecode message, EndPoint endPoint, CancellationToken cancellationToken = default)
        {
            AddUpdateOrExpireClient(message.Id, endPoint);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Handle WSJT-X <see cref="LoggedAdif"/> messages
        /// </summary>
        /// <param name="server"></param>
        /// <param name="message"></param>
        /// <param name="endPoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task HandleLoggedAdifMessageAsync(WsjtxUdpServer server, LoggedAdif message, EndPoint endPoint, CancellationToken cancellationToken = default)
        {
            AddUpdateOrExpireClient(message.Id, endPoint);
            return Task.CompletedTask;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Adds or updates a client to the list of communicating clients
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="endpoint"></param>
        /// <param name="status"></param>
        private void AddUpdateOrExpireClient(string clientId, EndPoint endpoint, Status? status = null)
        {
            // add or update a connected client while updating the last communication time
            bool isNewClient = false;
            ConnectedClients.AddOrUpdate(clientId,
                (id) =>
                {
                    var client = new WsjtxConnectedClient(id, endpoint, status);
                    client.Status = status ?? client.Status;
                    isNewClient = true;
                    return client;
                },
                (id, client) =>
                {
                    client.Status = status ?? client.Status;
                    client.LastCommunications = DateTime.UtcNow;
                    return client;
                });

            // execute the client connected callback if this is a new client
            if (isNewClient)
                ClientConnectedCallback?.Invoke(ConnectedClients[clientId]);

            // build a list of all clients which have not communicated with
            // the server for the window specified in lastHeardWindowSeconds
            // and remove those clients from the connected clients list while
            // executing the client expired callback on each client found
            var expiredClients = ConnectedClients.Values
                   .Where(target => (DateTime.UtcNow - target.LastCommunications).TotalSeconds > ConnectedClientExpiryInSeconds)
                   .Select(target => target.ClientId);

            foreach (var id in expiredClients)
                if (ConnectedClients.TryRemove(id, out WsjtxConnectedClient? target))
                    ClientExpiredCallback?.Invoke(target);
        }
        #endregion
    }
}
