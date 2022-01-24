using System.Net;
using WsjtxUtils.WsjtxUdpServer;
using WsjtxUtils.WsjtxUdpServer.Example.WriteJsonToConsole;

// parse command line for server address and port
IPAddress address = args.Length >= 1 ? IPAddress.Parse(args[0]) : IPAddress.Loopback;
int port = args.Length >= 2 ? int.Parse(args[1]) : 2237;

// create token source setup the message handler and
CancellationTokenSource cancellationTokenSource = GenerateCancellationTokenSource();
IWsjtxUdpMessageHandler messageHandler = new WriteMessageToConsoleAsJsonHandler();

// setup and start the WSJT-X UDP server
using var server = new WsjtxUdpServer(messageHandler, address, port);
Console.WriteLine($"Starting UDP server: {server.LocalEndpoint.Address}:{server.LocalEndpoint.Port} IsMulticast:{server.IsMulticast}");
server.Start(cancellationTokenSource);

// do stuff while the server is running
while (!cancellationTokenSource.IsCancellationRequested) { }

// stop the WSJT-X UDP server 
Console.WriteLine("Stopping the UDP server.");
server.Stop();
Console.WriteLine("Exiting...");


/// <summary>
/// Creates a <see cref="CancellationTokenSource"/> which will signal
/// the task cancellation on pressing CTRL-C in the console application
/// </summary>
/// <returns></returns>
static CancellationTokenSource GenerateCancellationTokenSource()
{
    var cancellationTokenSource = new CancellationTokenSource();
    Console.CancelKeyPress += (s, e) =>
    {
        Console.WriteLine("Canceling...");
        cancellationTokenSource.Cancel();
        e.Cancel = true;
    };
    return cancellationTokenSource;
}