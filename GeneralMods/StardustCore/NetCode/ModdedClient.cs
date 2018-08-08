using System;
using System.IO;
using Lidgren.Network;
using StardewValley;
using StardewValley.Network;

namespace StardustCore.NetCode
{
    public class ModdedClient : Client
    {
        private string address;
        private NetClient client;
        private bool serverDiscovered;

        public ModdedClient(string address)
        {
            this.address = address;
        }

        public override string getUserID()
        {
            return "";
        }

        protected override string getHostUserName()
        {
            return this.client.ServerConnection.RemoteEndPoint.Address.ToString();
        }

        protected override void connectImpl()
        {
            NetPeerConfiguration config = new NetPeerConfiguration("StardewValley");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            config.ConnectionTimeout = 30f;
            config.PingInterval = 5f;
            config.MaximumTransmissionUnit = 1200;
            this.client = new NetClient(config);
            this.client.Start();
            int serverPort = 24642;
            if (this.address.Contains(":"))
            {
                string[] strArray = this.address.Split(':');
                this.address = strArray[0];
                serverPort = Convert.ToInt32(strArray[1]);
            }
            this.client.DiscoverKnownPeer(this.address, serverPort);
        }

        public override void disconnect(bool neatly = true)
        {
            if (this.client.ConnectionStatus != NetConnectionStatus.Disconnected && this.client.ConnectionStatus != NetConnectionStatus.Disconnecting)
            {
                if (neatly)
                    this.sendMessage(new OutgoingMessage((byte)19, Game1.player, new object[0]));
                this.client.FlushSendQueue();
                this.client.Disconnect("");
                this.client.FlushSendQueue();
            }
            this.connectionMessage = (string)null;
        }

        protected virtual bool validateProtocol(string version)
        {
            return version ==ModCore.multiplayer.protocolVersion;
        }

        protected override void receiveMessagesImpl()
        {
            NetIncomingMessage netIncomingMessage;
            while ((netIncomingMessage = this.client.ReadMessage()) != null)
            {
                switch (netIncomingMessage.MessageType)
                {
                    case NetIncomingMessageType.StatusChanged:
                        this.statusChanged(netIncomingMessage);
                        continue;
                    case NetIncomingMessageType.Data:
                        this.parseDataMessageFromServer(netIncomingMessage);
                        continue;
                    case NetIncomingMessageType.DiscoveryResponse:
                        Console.WriteLine("Found server at " + (object)netIncomingMessage.SenderEndPoint);
                        if (this.validateProtocol(netIncomingMessage.ReadString()))
                        {
                            this.serverName = netIncomingMessage.ReadString();
                            this.receiveHandshake(netIncomingMessage);
                            this.serverDiscovered = true;
                            continue;
                        }
                        this.connectionMessage = "Strings\\UI:CoopMenu_FailedProtocolVersion";
                        this.client.Disconnect("");
                        continue;
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        string str = netIncomingMessage.ReadString();
                        Console.WriteLine("{0}: {1}", (object)netIncomingMessage.MessageType, (object)str);
                        Game1.debugOutput = str;
                        continue;
                    default:
                        continue;
                }
            }
            if (this.client.ServerConnection == null || DateTime.Now.Second % 2 != 0)
                return;
            Game1.debugOutput = "Ping: " + (object)(float)((double)this.client.ServerConnection.AverageRoundtripTime * 1000.0) + "ms";
        }

        private void receiveHandshake(NetIncomingMessage msg)
        {
            this.client.Connect(msg.SenderEndPoint.Address.ToString(), msg.SenderEndPoint.Port);
        }

        private void statusChanged(NetIncomingMessage message)
        {
            switch ((NetConnectionStatus)message.ReadByte())
            {
                case NetConnectionStatus.Disconnecting:
                case NetConnectionStatus.Disconnected:
                    this.clientRemotelyDisconnected();
                    break;
            }
        }

        private void clientRemotelyDisconnected()
        {
            this.timedOut = true;
        }

        public override void sendMessage(OutgoingMessage message)
        {
            NetOutgoingMessage message1 = this.client.CreateMessage();
            using (NetBufferWriteStream bufferWriteStream = new NetBufferWriteStream((NetBuffer)message1))
            {
                using (BinaryWriter writer = new BinaryWriter((Stream)bufferWriteStream))
                    message.Write(writer);
            }
            int num = (int)this.client.SendMessage(message1, NetDeliveryMethod.ReliableOrdered);
        }

        private void parseDataMessageFromServer(NetIncomingMessage dataMsg)
        {
            using (IncomingMessage message = new IncomingMessage())
            {
                using (NetBufferReadStream bufferReadStream = new NetBufferReadStream((NetBuffer)dataMsg))
                {
                    using (BinaryReader reader = new BinaryReader((Stream)bufferReadStream))
                    {
                        while ((long)dataMsg.LengthBits - dataMsg.Position >= 8L)
                        {
                            message.Read(reader);
                            try
                            {
                                this.processIncomingMessage(message);
                            }
                            catch(Exception err)
                            {

                            }
                        }
                    }
                }
            }
        }
    }
}
