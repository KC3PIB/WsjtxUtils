namespace WsjtxUtils.WsjtxMessages.Messages;

/// <summary>
///     The server may send this message at any time. Sort orders can be used for sorting hound callers when in Fox mode. A
///     typical usage is to "score" callsigns based on number of bands and/or modes worked using an external logging
///     program during a DXpedition, to be able to give preference to calls that have not been worked before on any other
///     band or mode. An external program can watch decodes from wsjt-x, then use this message to annotate the calls with a
///     sort order. The hound queue can be displayed by that sort order.
/// </summary>
/// <remarks>
///     Invalid or  unrecognized values  will be silently ignored. A sort-order of ffffffff will remove the sort-order
///     value from the internal table. Callsigns without a sort order will be valued at zero for sorting purposes in the
///     hound display.
/// </remarks>
public class AnnotationInfo : WsjtxMessage, IWsjtxDirectionIn
{
    /// <summary>
    ///     Constructs a default WSJT-X AnnotationInfo message
    /// </summary>
    public AnnotationInfo(string id, MessageType messageType, SchemaVersion schemaVersion = SchemaVersion.Version2) :
        base(id, messageType, schemaVersion)
    {
    }

    /// <summary>
    ///     The specified callsign
    /// </summary>
    public string DXCallsign { get; set; } = string.Empty;

    /// <summary>
    ///     If a sort order is provided
    /// </summary>
    /// <remarks>If 'sort order provided' is true, the message also specifies a numeric sort order for the DX call.</remarks>
    public bool SortOrderProvided { get; set; }

    /// <summary>
    ///     The specified sort order
    /// </summary>
    public uint SortOrder { get; set; }

    /// <summary>
    ///     Using the <see cref="WsjtxMessageWriter" />, serialize the message to raw bytes
    /// </summary>
    /// <param name="writer"></param>
    public override void WriteMessage(WsjtxMessageWriter writer)
    {
        base.WriteMessage(writer);

        writer.WriteString(DXCallsign);
        writer.WriteBool(SortOrderProvided);
        writer.WriteUInt32(SortOrder);
    }

    /// <summary>
    ///     Using the <see cref="WsjtxMessageReader" />, deserialize the values to the current message
    /// </summary>
    /// <param name="reader"></param>
    public override void ReadMessage(WsjtxMessageReader reader)
    {
        base.ReadMessage(reader);

        DXCallsign = reader.ReadString();
        SortOrderProvided = reader.ReadBool();
        SortOrder = reader.ReadUInt32();
    }
}