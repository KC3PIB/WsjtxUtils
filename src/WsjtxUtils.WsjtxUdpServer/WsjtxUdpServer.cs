using System;
using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using WsjtxUtils.WsjtxMessages;
using WsjtxUtils.WsjtxMessages.Messages;

namespace WsjtxUtils.WsjtxUdpServer
{
    /// <summary>
    /// A UDP server for WSJT-X clients
    /// </summary>
    public class WsjtxUdpServer : IDisposable
    {
        /// <summary>
        /// The value used for the Default Maximum Transmission Unit (MTU)
        /// </summary>
        public const int DefaultMtu = 1500;
        
        /// <summary>
        /// Size of the datagram buffers in bytes
        /// </summary>
        private readonly int _datagramBufferSize;

        /// <summary>
        /// The socket used for communications
        /// </summary>
        private readonly Socket _socket;

        /// <summary>
        /// The target handler for WSJT-X messages
        /// </summary>
        private readonly IWsjtxUdpMessageHandler _messageHandler;

        /// <summary>
        /// The target logger for messages
        /// </summary>
        private readonly ILogger<WsjtxUdpServer> _logger;

        /// <summary>
        /// Source for cancellation tokens
        /// </summary>
        private CancellationTokenSource? _cancellationTokenSource;

        /// <summary>
        /// The datagram handling task
        /// </summary>
        private Task? _handleDatagramsTask;

        /// <summary>
        /// Get whether this object has been disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Is the specified address multicast
        /// </summary>
        public bool IsMulticast { get; private set; }

        /// <summary>
        /// Is the server currently running
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// The endpoint the server is bound to
        /// </summary>
        public IPEndPoint LocalEndpoint { get; private set; }

        /// <summary>
        /// Constructor for WSJT-X UDP server
        /// </summary>
        /// <param name="wsjtxUdpMessageHandler"></param>
        /// <param name="address"></param>
        /// <param name="port"></param>
        /// <param name="datagramBufferSize"></param>
        /// <param name="logger"></param>
        public WsjtxUdpServer(IWsjtxUdpMessageHandler wsjtxUdpMessageHandler, IPAddress address, int port = 2237,
            int datagramBufferSize = DefaultMtu, ILogger<WsjtxUdpServer>? logger = null)
        {
            // set the message handling object
            _messageHandler = wsjtxUdpMessageHandler;

            // size of the buffers to allocate for reading/writing
            _datagramBufferSize = datagramBufferSize;

            _logger = logger ?? NullLogger<WsjtxUdpServer>.Instance;

            // check if the address is multicast and setup accordingly
            IsMulticast = IsAddressMulticast(address);
            LocalEndpoint = IsMulticast ? new IPEndPoint(IPAddress.Any, port) : new IPEndPoint(address, port);

            // setup UDP socket allowing for shared addresses
            _socket = new Socket(SocketType.Dgram, ProtocolType.Udp)
            {
                ExclusiveAddressUse = false
            };
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _socket.Bind(LocalEndpoint);

            // if multicast join the group
            if (IsMulticast)
                _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership,
                    new MulticastOption(address, LocalEndpoint.Address));
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~WsjtxUdpServer()
        {
            Dispose(false);
        }

        /// <summary>
        /// Start the UDP sever and process datagrams
        /// </summary>
        /// <param name="cancellationTokenSource"></param>
        public void Start(CancellationTokenSource? cancellationTokenSource = null)
        {
            _logger.LogInformation("Starting WSJT-X UDP server on {Endpoint} (multicast={IsMulticast})", LocalEndpoint, IsMulticast);

            if (IsRunning)
                throw new InvalidOperationException("The server is already running.");

            IsRunning = true;
            _cancellationTokenSource = cancellationTokenSource ?? new CancellationTokenSource();
            _handleDatagramsTask = HandleDatagramLoopAsync(_cancellationTokenSource.Token);
        }

        /// <summary>
        /// Stop the UDP server and datagram processing
        /// </summary>
        public void Stop()
        {
            _logger.LogInformation("Stopping WSJT-X UDP server");

            if (!IsRunning)
                throw new InvalidOperationException("The server is not running.");

            try
            {
                _cancellationTokenSource?.Cancel();
                _handleDatagramsTask?.Wait(1000);
            }
            catch (AggregateException aggregateException)
            {
                aggregateException.Handle(ex => ex is TaskCanceledException);
            }
            finally
            {
                IsRunning = false;
            }
        }

        /// <summary>
        /// Send a WSJT-X message to the specified endpoint
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="remoteEndpoint"></param>
        /// <param name="message"></param>
        /// <returns>The number of bytes sent</returns>
        public int SendMessageTo<T>(EndPoint remoteEndpoint, T message) where T : WsjtxMessage, IWsjtxDirectionIn
        {
            if (string.IsNullOrEmpty(message.Id))
                throw new ArgumentException($"The client id can not be null or empty when sending {typeof(T).Name}.",
                    nameof(message));

            var datagramBuffer = ArrayPool<byte>.Shared.Rent(_datagramBufferSize);
            try
            {
                var bytesWritten = message.WriteMessageTo(datagramBuffer);
                return _socket.SendTo(datagramBuffer, bytesWritten, SocketFlags.None, remoteEndpoint);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(datagramBuffer);
            }
        }

        /// <summary>
        /// Send a WSJT-X message to the specified endpoint
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="remoteEndpoint"></param>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>The number of bytes sent</returns>
        public async ValueTask<int> SendMessageToAsync<T>(EndPoint remoteEndpoint, T message,
            CancellationToken cancellationToken = default) where T : WsjtxMessage, IWsjtxDirectionIn
        {
            if (string.IsNullOrEmpty(message.Id))
                throw new ArgumentException($"The client id can not be null or empty when sending {typeof(T).Name}.",
                    nameof(message));

            var datagramBuffer = ArrayPool<byte>.Shared.Rent(_datagramBufferSize);
            try
            {
                var bytesWritten = message.WriteMessageTo(datagramBuffer);
                return await _socket.SendToAsync(new ArraySegment<byte>(datagramBuffer, 0, bytesWritten),
                    SocketFlags.None, remoteEndpoint);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(datagramBuffer);
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (IsRunning) Stop();
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #region Private Methods

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            // unmanaged items

            if (disposing)
            {
                // managed items
                _cancellationTokenSource?.Dispose();
                _socket?.Dispose();
            }

            IsDisposed = true;
        }

        /// <summary>
        /// Process incoming datagrams
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task HandleDatagramLoopAsync(CancellationToken cancellationToken)
        {
            var datagramBuffer = new byte[_datagramBufferSize];

            while (!cancellationToken.IsCancellationRequested)
            {
                // wait for and read the next datagram into the buffer
                try
                {
#if NETFRAMEWORK
                    var result = await _socket.ReceiveFromAsync(new ArraySegment<byte>(datagramBuffer),
                        SocketFlags.None, LocalEndpoint);
#else
                    var result =
     await _socket.ReceiveFromAsync(datagramBuffer, SocketFlags.None, LocalEndpoint, cancellationToken);
#endif

                    // extract the framed packet based on the number of bytes that were read
                    var frame = datagramBuffer.AsMemory(0, result.ReceivedBytes);
                    var message = frame.DeserializeWsjtxMessage();
                    if (message is null)
                    {
                        _logger?.LogWarning("Received unsupported WSJT-X frame, skipping.");
                        continue;
                    }

                    // get the correct handler for the given message type
                    var messageHandlingTask = message.MessageType switch
                    {
                        MessageType.Clear => _messageHandler.HandleClearMessageAsync(this, (Clear)message,
                            result.RemoteEndPoint, cancellationToken),
                        MessageType.Close => _messageHandler.HandleClosedMessageAsync(this, (Close)message,
                            result.RemoteEndPoint, cancellationToken),
                        MessageType.Decode => _messageHandler.HandleDecodeMessageAsync(this, (Decode)message,
                            result.RemoteEndPoint, cancellationToken),
                        MessageType.Heartbeat => _messageHandler.HandleHeartbeatMessageAsync(this, (Heartbeat)message,
                            result.RemoteEndPoint, cancellationToken),
                        MessageType.LoggedADIF => _messageHandler.HandleLoggedAdifMessageAsync(this,
                            (LoggedAdif)message,
                            result.RemoteEndPoint, cancellationToken),
                        MessageType.QSOLogged => _messageHandler.HandleQsoLoggedMessageAsync(this, (QsoLogged)message,
                            result.RemoteEndPoint, cancellationToken),
                        MessageType.Status => _messageHandler.HandleStatusMessageAsync(this, (Status)message,
                            result.RemoteEndPoint, cancellationToken),
                        MessageType.WSPRDecode => _messageHandler.HandleWSPRDecodeMessageAsync(this,
                            (WSPRDecode)message,
                            result.RemoteEndPoint, cancellationToken),
                        _ => null // no handler for unsupported messages
                    };

                    // add logging to faulted tasks
                    _ = messageHandlingTask?.ContinueWith(
                        t => _logger?.LogError(t.Exception, "Handler for {MessageType} threw an exception",
                            message.MessageType),
                        TaskContinuationOptions.OnlyOnFaulted);
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    // normal shutdown
                    break;
                }
                catch (SocketException socketException)
                {
                    _logger?.LogWarning("A socketException occured with: {ExceptionMessage}", socketException.Message);
                }
                catch (Exception exception)
                {
                    _logger?.LogError(exception, "Unexpected error in receive loop");
                }
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Determine if the address is a multicast group
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool IsAddressMulticast(IPAddress address)
        {
            if (address.IsIPv6Multicast)
                return true;

            var addressBytes = address.GetAddressBytes();
            if (addressBytes.Length == 4)
                return addressBytes[0] >= 224 && addressBytes[0] <= 239;

            return false;
        }

        #endregion
    }
}