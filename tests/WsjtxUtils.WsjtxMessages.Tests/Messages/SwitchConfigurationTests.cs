using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using WsjtxUtils.WsjtxMessages.Messages;

namespace WsjtxUtils.WsjtxMessages.Tests.Messages
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SwitchConfigurationTests
    {
        internal static Memory<byte> SwitchConfigurationMessage = new(new byte[] { 0xAD, 0xBC, 0xCB, 0xDA, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x0E, 0x00, 0x00, 0x00, 0x06, 0x57, 0x53, 0x4A, 0x54, 0x2D, 0x58, 0x00, 0x00, 0x00, 0x07, 0x44, 0x65, 0x66, 0x61, 0x75, 0x6C, 0x74 });

        [TestMethod()]
        public void WriteSwitchConfigurationMessageWithDefaultConstructorTo_RawBytes_ProducesValidOutput()
        {
            SwitchConfiguration message = new()
            {
                Id = "WSJT-X",
                ConfigurationName = "Default"
            };

            // Allocate memory and write the message
            var buffer = new byte[SwitchConfigurationMessage.Length];
            var bufferMem = buffer.AsMemory();
            var written = message.WriteMessageTo(bufferMem);

            Assert.AreEqual(SwitchConfigurationMessage.Length, written);
            CollectionAssert.AreEqual(SwitchConfigurationMessage.ToArray(), buffer);
        }

        [TestMethod()]
        public void WriteSwitchConfigurationMessageTo_RawBytes_ProducesValidOutput()
        {
            SwitchConfiguration message = new("WSJT-X", "Default");

            // Allocate memory and write the message
            var buffer = new byte[SwitchConfigurationMessage.Length];
            var bufferMem = buffer.AsMemory();
            var written = message.WriteMessageTo(bufferMem);

            Assert.AreEqual(SwitchConfigurationMessage.Length, written);
            CollectionAssert.AreEqual(SwitchConfigurationMessage.ToArray(), buffer);
        }
    }
}
