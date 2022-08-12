# WsjtxUtils
A class library and usage examples related to interacting with [WSJT-X](https://physics.princeton.edu/pulsar/k1jt/wsjtx.html) through the UDP interface in .NET 6 and .NET Framework 4.8. This library allows for parsing and creating WSJT-X >= 2.2 messages and a lightweight UDP server that supports reading and writing WSJT-X messages utilizing async/await.

## Packages
Precompiled packages are available via NuGet.
|Package|NuGet Stable|NuGet Pre-release|
|:--|:--|:--|
|[WsjtxUtils.WsjtxMessages](https://www.nuget.org/packages/WsjtxUtils.WsjtxMessages/)|[![WsjtxUtils.WsjtxMessages](https://img.shields.io/nuget/v/WsjtxUtils.WsjtxMessages.svg)](https://www.nuget.org/packages/WsjtxUtils.WsjtxMessages/)|[![WsjtxUtils.WsjtxMessages](https://img.shields.io/nuget/vpre/WsjtxUtils.WsjtxMessages.svg)](https://www.nuget.org/packages/WsjtxUtils.WsjtxMessages/)|
|[WsjtxUtils.WsjtxUdpServer](https://www.nuget.org/packages/WsjtxUtils.WsjtxUdpServer/)|[![WsjtxUtils.WsjtxUdpServer](https://img.shields.io/nuget/v/WsjtxUtils.WsjtxUdpServer.svg)](https://www.nuget.org/packages/WsjtxUtils.WsjtxUdpServer/)|[![WsjtxUtils.WsjtxUdpServer](https://img.shields.io/nuget/vpre/WsjtxUtils.WsjtxUdpServer.svg)](https://www.nuget.org/packages/WsjtxUtils.WsjtxUdpServer/)|

## Table of Contents
- [WsjtxUtils.WsjtxMessages](#wsjtxutilswsjtxmessages)
- [WsjtxUtils.WsjtxUdpServer](#wsjtxutilswsjtxudpserver)
- [WsjtxUtils.WsjtxUdpServer.Example.WriteJsonToConsole](#wsjtxutilswsjtxudpserverexamplewritejsontoconsole)
- [WsjtxUtils.WsjtxUdpServer.Example.UpdateGridFromGPS](#wsjtxutilswsjtxudpserverexampleupdategridfromgps)

## WsjtxUtils.WsjtxMessages
The [WsjtxMessages](src/WsjtxUtils.WsjtxMessages) library contains the classes and methods needed to serialize and deserialize WSJT-X messages in the QT QDataStream format specified in the WSJT-X source code in [NetworkMessage.hpp](https://sourceforge.net/p/wsjt/wsjtx/ci/master/tree/Network/NetworkMessage.hpp).

We can easily read messages from a memory source using the extension methods provided.
```csharp
Memory<byte> source= new(new byte[] { 0xAD, 0xBC, 0xCB, 0xDA, 0x00, 0x00, ... };
WsjtxMessage? message = source.DeserializeWsjtxMessage();
```

We can write messages to a memory source just as quickly.
```csharp
Clear message = new("WSJT-X", ClearWindow.BandActivity);

var buffer = GC.AllocateArray<byte>(1500, false);
var numberOfBytesWritten = message.WriteMessageTo(buffer);
```

[WsjtxQsoParser](https://github.com/KC3PIB/WsjtxUtils/blob/main/src/WsjtxUtils.WsjtxMessages/QsoParsing/WsjtxQsoParser.cs) is a utility class attempting to extract as much relevant QSO information from 77-bit modes (FST4, FT4, FT8, MSK144, Q65) WSJT-X [Decode](https://github.com/KC3PIB/WsjtxUtils/blob/main/src/WsjtxUtils.WsjtxMessages/Messages/Decode.cs) messages. The returned [WsjtxQso](src/WsjtxUtils.WsjtxMessages/QsoParsing/WsjtxQso.cs) will have the state of the QSO in progress and callsigns, grid square, and report if available.
```csharp
var qso = WsjtxQsoParser.ParseDecode(decode);
 
var dxCallsign = qso.DXCallsign;
var deCallsign = qso.DECallsign;
var grid = qso.GridSquare;
var report = qso.Report;
```

[WsjtxUtils.WsjtxMessages](src/WsjtxUtils.WsjtxMessages) does not contain a server implementation to allow flexibility and use cases where no server or a custom server is required.

## WsjtxUtils.WsjtxUdpServer
[WsjtxUdpServer](https://github.com/KC3PIB/WsjtxUtils/tree/main/src/WsjtxUtils.WsjtxUdpServer/WsjtxUdpServer.cs) is a lightweight, multicast-capable, asynchronous UDP server for WSJT-X clients. [IWsjtxUdpMessageHandler](https://github.com/KC3PIB/WsjtxUtils/tree/main/src/WsjtxUtils.WsjtxUdpServer/IWsjtxUdpMessageHandler.cs) describes an interface that allows WsjtxUdpServer to handle all potential incoming messages from WSJT-X.  Create a class that implements [IWsjtxUdpMessageHandler](https://github.com/KC3PIB/WsjtxUtils/tree/main/src/WsjtxUtils.WsjtxUdpServer/IWsjtxUdpMessageHandler.cs) and pass this class to the UDP server's constructor to begin processing messages asynchronously.
```csharp
IWsjtxUdpMessageHandler messageHandler = new SomeCustomMessageHandler();
var cancellationTokenSource = new CancellationTokenSource();

using var server = new WsjtxUdpServer(messageHandler, IPAddress.Any);
server.Start(cancellationTokenSource);

// do other stuff while the server is running and processing messages
while (!cancellationTokenSource.IsCancellationRequested) { }

server.Stop();
```
[WsjtxUdpServerBaseAsyncMessageHandler](https://github.com/KC3PIB/WsjtxUtils/tree/main/src/WsjtxUtils.WsjtxUdpServer/WsjtxUdpServerBaseAsyncMessageHandler.cs) is an abstract class that implements [IWsjtxUdpMessageHandler](https://github.com/KC3PIB/WsjtxUtils/tree/main/src/WsjtxUtils.WsjtxUdpServer/IWsjtxUdpMessageHandler.cs) and provides a basic set of capabilities. It tracks WSJT-X clients and, if available, the last state for each client that has communicated with the server during the previous timeout window, which defaults to 5 minutes. There are properties for connected clients and callbacks used to execute a target function based on specific events, newly connected WSJT-X clients, clients who send the close message, and clients who have not communicated with the server during the previous timeout window.


## WsjtxUtils.WsjtxUdpServer.Example.WriteJsonToConsole
An example console application that writes received WSJT-X messages to the console as JSON. [WriteMessageToConsoleAsJsonHandler](src/WsjtxUtils.WsjtxUdpServer.Example.WriteJsonToConsole/WriteMessageToConsoleAsJsonHandler.cs) provides an example of receiving and processing WSJT-X messages by implementing the abstract class [WsjtxUdpServerBaseAsyncMessageHandler](src/WsjtxUtils.WsjtxUdpServer/WsjtxUdpServerBaseAsyncMessageHandler.cs).

The server defaults to the local loopback address and a port of 2237 but can be overridden via command-line arguments.
```sh
WsjtxUtils.WsjtxUdpServer.Example.WriteJsonToConsole.exe <Server IP> <Server Port>
WsjtxUtils.WsjtxUdpServer.Example.WriteJsonToConsole.exe 224.0.0.1 2237
```
## WsjtxUtils.WsjtxUdpServer.Example.UpdateGridFromGPS
An example console application that demonstrates using a NEMA GPS device to determine the current maidenhead grid square and sends a Location message to all connected WSJT-X clients. The GPS device defaults to 'COM3' and 9600 baud, while the server defaults to the local loopback address and port 2237 but can be overridden via command-line arguments.

> **NOTE:** Enable the AutoGrid option in WSJT-X settings within Station Details to allow for the reception of Location UDP messages.
```sh
WsjtxUtils.WsjtxUdpServer.Example.UpdateGridFromGPS.exe <GPS COM> <GPS Baudrate> <Server IP> <Server Port>
WsjtxUtils.WsjtxUdpServer.Example.UpdateGridFromGPS.exe COM29 9600 224.0.0.1 2237
```