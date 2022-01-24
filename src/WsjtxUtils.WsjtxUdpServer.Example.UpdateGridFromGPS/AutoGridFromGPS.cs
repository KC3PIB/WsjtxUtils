using NmeaParser;
using System.Net;
using System.Text;
using WsjtxUtils.WsjtxMessages.Messages;

namespace WsjtxUtils.WsjtxUdpServer.Example.UpdateGridFromGPS
{
    /// <summary>
    /// Uses GPS to update the grid square to all connected WSJT-X clients.
    /// </summary>
    public class AutoGridFromGPS : WsjtxUdpServerBaseAsyncMessageHandler
    {
        /// <summary>
        /// Start of the uppercase alphabet in ASCII
        /// </summary>
        private const int UppercaseAlphaCharCodeStart = 65;

        /// <summary>
        /// Start of the lowercase alphabet in ASCII
        /// </summary>
        private const int LowercaseAlphaCharCodeStart = 97;

        private readonly NmeaDevice _gps;
        private readonly WsjtxUdpServer _server;

        private string? _currentGridsquare;
        private string? _lastGridsquare;

        /// <summary>
        /// Constructor for WSJT-X AutoGrid update from GPS server
        /// </summary>
        /// <param name="gps"></param>
        /// <param name="address"></param>
        /// <param name="port"></param>
        public AutoGridFromGPS(NmeaDevice gps, IPAddress address, int port = 2237)
        {
            _gps = gps;
            _server = new WsjtxUdpServer(this, address, port);
            _gps.MessageReceived += OnNmeaMessageReceived;
            ClientConnectedCallback = (client) => CheckStateAndSendLocationIfRequiredAsync(client);
        }

        /// <summary>
        /// Run AutoGrid
        /// </summary>
        /// <param name="cancellationTokenSource"></param>
        /// <returns></returns>
        public async Task RunAsync(CancellationTokenSource cancellationTokenSource)
        {
            // start the GPS device and WSJT-X server
            await _gps.OpenAsync();
            _server.Start();

            try
            {
                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    // check if the gridsquare has changed since the last pass
                    // and send the updated location if the client requires
                    if (_currentGridsquare != _lastGridsquare)
                    {
                        _lastGridsquare = _currentGridsquare;
                        foreach (var client in ConnectedClients)
                            await CheckStateAndSendLocationIfRequiredAsync(client.Value, cancellationTokenSource.Token);
                    }
                    await Task.Delay(1000, cancellationTokenSource.Token);
                }
            }
            finally
            {
                _server.Stop();
                await _gps.CloseAsync();
            }
        }

        /// <summary>
        /// Check the state of a client and send a location message if needed
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        private async Task CheckStateAndSendLocationIfRequiredAsync(WsjtxConnectedClient client, CancellationToken cancellationToken = default)
        {
            if (_currentGridsquare != null && (client.Status == null || client.Status.DEGrid != _currentGridsquare))
                await _server.SendMessageToAsync(client.Endpoint, new Location(client.ClientId, _currentGridsquare), cancellationToken);
        }

        /// <summary>
        /// Handle NEMA messages from the GPS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNmeaMessageReceived(object? sender, NmeaMessageReceivedEventArgs e)
        {
            if (e.Message is NmeaParser.Messages.Gll gll && gll.DataActive)
                _currentGridsquare = LatitudeLongitudeToMaidenheadLocator(gll.Latitude, gll.Longitude);
        }

        /// <summary>
        /// Converts a latitude and longitude values into the maidenhead locator system
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        private static string LatitudeLongitudeToMaidenheadLocator(double latitude, double longitude)
        {
            var gridLatitude = latitude + 90;
            var gridLongitude = longitude + 180;
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(Convert.ToChar(UppercaseAlphaCharCodeStart + (int)Math.Floor(gridLongitude / 20)));
            stringBuilder.Append(Convert.ToChar(UppercaseAlphaCharCodeStart + (int)Math.Floor(gridLatitude / 10)));

            gridLongitude %= 20;
            gridLatitude %= 10;

            stringBuilder.Append((int)Math.Floor(gridLongitude / 2));
            stringBuilder.Append((int)Math.Floor(gridLatitude / 1));

            gridLongitude %= 2;
            gridLatitude %= 1;

            stringBuilder.Append(Convert.ToChar(LowercaseAlphaCharCodeStart + (int)Math.Floor(gridLongitude * 12)));
            stringBuilder.Append(Convert.ToChar(LowercaseAlphaCharCodeStart + (int)Math.Floor(gridLatitude * 24)));

            return stringBuilder.ToString();
        }
    }
}
