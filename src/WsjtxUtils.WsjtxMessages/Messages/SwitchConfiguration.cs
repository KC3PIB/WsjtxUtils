namespace WsjtxUtils.WsjtxMessages.Messages
{
    /// <summary>
    /// WSJT-X SwitchConfiguration message
    /// </summary>
    /// <remarks>
    /// The server may send this message at any time. The message
    /// specifies the name of the configuration to switch to.
    /// </remarks>
    public class SwitchConfiguration : WsjtxMessage, IWsjtxDirectionIn
    {
        /// <summary>
        /// Constructs a default WSJT-X SwitchConfiguration message
        /// </summary>
        public SwitchConfiguration() : base(MessageType.SwitchConfiguration)
        {
            ConfigurationName = string.Empty;
        }

        /// <summary>
        /// Constructs a WSJT-X SwitchConfiguration message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="configurationName"></param>
        public SwitchConfiguration(string id, string configurationName) : base(MessageType.SwitchConfiguration)
        {
            Id = id;
            ConfigurationName = configurationName;
        }

        /// <summary>
        /// Selected configuration
        /// </summary>
        /// <remarks>
        /// The new configuration must exist.
        /// </remarks>
        public string ConfigurationName { get; set; }

        #region IWsjtxDirectionIn
        /// <summary>
        /// Using the <see cref="WsjtxMessageWriter"/>, serialize the message to raw bytes
        /// </summary>
        /// <param name="messageWriter"></param>
        public override void WriteMessage(WsjtxMessageWriter messageWriter)
        {
            base.WriteMessage(messageWriter);

            messageWriter.WriteString(ConfigurationName);
        }
        #endregion
    }
}
