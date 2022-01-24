using NmeaParser;
using System.IO.Ports;
using System.Net;
using WsjtxUtils.WsjtxUdpServer.Example.UpdateGridFromGPS;

// parse command line options
string comPort = args.Length >= 1 ? args[0] : "COM3";
int baudRate = args.Length >= 2 ? int.Parse(args[1]) : 9600;
IPAddress address = args.Length >= 3 ? IPAddress.Parse(args[2]) : IPAddress.Loopback;
int port = args.Length >= 4 ? int.Parse(args[3]) : 2237;

// setup the GPS device and the AutoGrid server
var gps = new SerialPortDevice(new SerialPort(comPort, baudRate));
AutoGridFromGPS autoGrid = new AutoGridFromGPS(gps, address, port);

// run autogrid
await autoGrid.RunAsync(GenerateCancellationTokenSource());


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