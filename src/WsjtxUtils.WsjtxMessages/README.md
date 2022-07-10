# WsjtxUtils.WsjtxMessages
The [WsjtxMessages](https://github.com/KC3PIB/WsjtxUtils/tree/main/src/WsjtxUtils.WsjtxMessages) library contains the classes and methods needed to serialize and deserialize WSJT-X messages in the QT QDataStream format specified in the WSJT-X source code in [NetworkMessage.hpp](https://sourceforge.net/p/wsjt/wsjtx/ci/master/tree/Network/NetworkMessage.hpp).

We can easily read messages from a memory source using the extensions methods provided.
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

[WsjtxQsoParser](QsoParsing/WsjtxQsoParser.cs) is a utility class attempting to extract as much relevant QSO information from a WSJT-X [Decode](Messages/Decode.cs) packet. The returned [WsjtxQso](QsoParsing/WsjtxQso.cs) will have the state of the QSO in progress and callsigns, grid square, and report if available.
```csharp
 var qso = WsjtxQsoParser.ParseDecode(decode);
 
 var dxCallsign = qso.DXCallsign;
 var deCallsign = qso.DECallsign;
 var grid = qso.GridSquare;
 var report = qso.Report;
```

WsjtxMessages does not contain a server implementation to allow flexibility and use cases where no server or a custom server is required. A basic UDP server using [WsjtxUtils.WsjtxMessages](https://github.com/KC3PIB/WsjtxUtils/tree/main/src/WsjtxUtils.WsjtxMessages) is available with [WsjtxUtils.WsjtxUdpServer](https://github.com/KC3PIB/WsjtxUtils/tree/main/src/WsjtxUtils.WsjtxUdpServer).