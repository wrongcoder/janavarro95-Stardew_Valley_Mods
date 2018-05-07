using Galaxy.Api;
using StardewValley;
using StardewValley.Network;
using StardewValley.SDKs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModdedUtilitiesNetworking.Framework.Clients
{
    class CustomGalaxyClient :GalaxyNetClient
    {
        private GalaxyID lobbyId;
        private GalaxySocket client;
        private GalaxyID serverId;

        public CustomGalaxyClient(GalaxyID lobbyId) : base(lobbyId)
        {
            this.lobbyId = lobbyId;
        }

        public override string getUserID()
        {
            return Convert.ToString(GalaxyInstance.User().GetGalaxyID().ToUint64());
        }

        protected override string getHostUserName()
        {
            return GalaxyInstance.Friends().GetFriendPersonaName(this.serverId);
        }

        protected override void connectImpl()
        {
            this.client = new GalaxySocket(ModCore.multiplayer.protocolVersion);
            GalaxyInstance.User().GetGalaxyID();
            this.client.JoinLobby(this.lobbyId);
            ModCore.monitor.Log("Success on generating modded galaxy client.");
        }

        public override void disconnect(bool neatly = true)
        {
            if (this.client == null)
                return;
            Console.WriteLine("Disconnecting from server {0}", (object)this.lobbyId);
            this.client.Close();
            this.client = (GalaxySocket)null;
            this.connectionMessage = (string)null;
        }

        protected override void receiveMessagesImpl()
        {
            if (this.client == null || !this.client.Connected)
                return;
            if (this.client.Connected && this.serverId == (GalaxyID)null)
                this.serverId = this.client.LobbyOwner;
            this.client.Receive(new Action<GalaxyID>(this.onReceiveConnection), new Action<GalaxyID, Stream>(this.onReceiveMessage), new Action<GalaxyID>(this.onReceiveDisconnect), new Action<string>(this.onReceiveError));
            this.client.Heartbeat(Enumerable.Repeat<GalaxyID>(this.serverId, 1));
            if (this.client.GetTimeSinceLastMessage(this.serverId) <= 30000L)
                return;
            this.timedOut = true;
            this.disconnect(true);
        }

        private void onReceiveConnection(GalaxyID peer)
        {
        }

        private void onReceiveMessage(GalaxyID peer, Stream messageStream)
        {
            if (peer != this.serverId)
                return;
            using (IncomingMessage message = new IncomingMessage())
            {
                using (BinaryReader reader = new BinaryReader(messageStream))
                {
                    message.Read(reader);
                    this.processIncomingMessage(message);
                }
            }
        }

        private void onReceiveDisconnect(GalaxyID peer)
        {
            if (peer != this.serverId)
                ModCore.multiplayer.playerDisconnected((long)peer.ToUint64());
            else
                this.timedOut = true;
        }

        private void onReceiveError(string messageKey)
        {
            this.connectionMessage = messageKey;
        }

        public override void sendMessage(OutgoingMessage message)
        {
            if (this.client == null || !this.client.Connected || this.serverId == (GalaxyID)null)
                return;
            this.client.Send(this.serverId, message);
        }

        protected override void processIncomingMessage(IncomingMessage message)
        {
            byte messageType = message.MessageType;
            if ((uint)messageType <= 9U)
            {
                switch (messageType)
                {
                    case 1:
                        this.receiveServerIntroduction(message.Reader);
                        return;
                    case 2:
                        this.userNames[message.FarmerID] = message.Reader.ReadString();
                        ModCore.multiplayer.processIncomingMessage(message);
                        return;
                    case 3:
                        ModCore.multiplayer.processIncomingMessage(message);
                        return;
                    case 9:
                        this.receiveAvailableFarmhands(message.Reader);
                        return;
                }
            }
            else if ((int)messageType != 11)
            {
                if ((int)messageType == 16)
                {
                    if (message.FarmerID != Game1.serverHost.Value.UniqueMultiplayerID)
                        return;
                    this.receiveUserNameUpdate(message.Reader);
                    return;
                }
            }
            else
            {
                this.connectionMessage = message.Reader.ReadString();
                return;
            }

            if (message.MessageType == 20)
            {
                ModCore.monitor.Log("JUMPING JELLYBEANS!!!");
            }

            ModCore.multiplayer.processIncomingMessage(message); //If we don't know how to initially process the message, send it to the multiplayer function.
        }
    }
}
