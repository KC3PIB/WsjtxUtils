using System;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WsjtxUtils.WsjtxMessages.Messages;

namespace WsjtxUtils.WsjtxUdpServer.Example.WriteJsonToConsole
{
    /// <summary>
    /// A simple example of a WSJT-X UDP server message handler that outputs the
    /// WSJT-X message as a JSON string in the console
    /// </summary>
    public class WriteMessageToConsoleAsJsonHandler : WsjtxUdpServerBaseAsyncMessageHandler
    {
        private static void WriteMessageAsJsonToConsole<T>(T message) where T : IWsjtxDirectionOut
        {
            Console.WriteLine(JsonSerializer.Serialize(message, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            }));
        }

        #region IWsjtxUdpMessageHandler
        public override async Task HandleClearMessageAsync(WsjtxUdpServer server, Clear message, EndPoint endPoint, CancellationToken cancellationToken = default)
        {
            WriteMessageAsJsonToConsole(message);
            await base.HandleClearMessageAsync(server, message, endPoint, cancellationToken);
        }

        public override async Task HandleClosedMessageAsync(WsjtxUdpServer server, Close message, EndPoint endPoint, CancellationToken cancellationToken = default)
        {
            WriteMessageAsJsonToConsole(message);
            await base.HandleClosedMessageAsync(server, message, endPoint, cancellationToken);
        }

        public override async Task HandleDecodeMessageAsync(WsjtxUdpServer server, Decode message, EndPoint endPoint, CancellationToken cancellationToken = default)
        {
            WriteMessageAsJsonToConsole(message);
            await base.HandleDecodeMessageAsync(server, message, endPoint, cancellationToken);
        }

        public override async Task HandleHeartbeatMessageAsync(WsjtxUdpServer server, Heartbeat message, EndPoint endPoint, CancellationToken cancellationToken = default)
        {
            WriteMessageAsJsonToConsole(message);
            await base.HandleHeartbeatMessageAsync(server, message, endPoint, cancellationToken);
        }

        public override async Task HandleLoggedAdifMessageAsync(WsjtxUdpServer server, LoggedAdif message, EndPoint endPoint, CancellationToken cancellationToken = default)
        {
            WriteMessageAsJsonToConsole(message);
            await base.HandleLoggedAdifMessageAsync(server, message, endPoint, cancellationToken);
        }

        public override async Task HandleQsoLoggedMessageAsync(WsjtxUdpServer server, QsoLogged message, EndPoint endPoint, CancellationToken cancellationToken = default)
        {
            WriteMessageAsJsonToConsole(message);
            await base.HandleQsoLoggedMessageAsync(server, message, endPoint, cancellationToken);
        }

        public override async Task HandleStatusMessageAsync(WsjtxUdpServer server, Status message, EndPoint endPoint, CancellationToken cancellationToken = default)
        {
            WriteMessageAsJsonToConsole(message);
            await base.HandleStatusMessageAsync(server, message, endPoint, cancellationToken);
        }

        public override async Task HandleWSPRDecodeMessageAsync(WsjtxUdpServer server, WSPRDecode message, EndPoint endPoint, CancellationToken cancellationToken = default)
        {
            WriteMessageAsJsonToConsole(message);
            await base.HandleWSPRDecodeMessageAsync(server, message, endPoint, cancellationToken);
        }
        #endregion
    }
}
