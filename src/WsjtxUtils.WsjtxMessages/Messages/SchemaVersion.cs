namespace WsjtxUtils.WsjtxMessages.Messages
{
    /// <summary>
    /// QDataStream version for the underlying encoding schemes used to store data items
    /// </summary>
    public enum SchemaVersion : uint
    {
        /// <summary>
        /// QDataStream::Qt_5_0 version
        /// </summary>
        Version1 = 1,
        /// <summary>
        /// Qt_5_2 version
        /// </summary>
        Version2 = 2,
        /// <summary>
        /// Qt_5_4 version
        /// </summary>
        Version3 = 3
    }
}
