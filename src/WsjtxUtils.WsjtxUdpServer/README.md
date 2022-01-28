# WsjtxUtils.WsjtxUdpServer
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
