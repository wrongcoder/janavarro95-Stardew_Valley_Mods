using ModdedUtilitiesNetworking.Framework.Clients;
using ModdedUtilitiesNetworking.Framework.Messages;
using ModdedUtilitiesNetworking.Framework.Servers;
using StardewValley;
using StardewValley.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using static ModdedUtilitiesNetworking.Framework.Delegates.DelegateInfo;

namespace ModdedUtilitiesNetworking.Framework
{
    public class CustomMultiplayer : StardewValley.Multiplayer
    {

        public override bool isClientBroadcastType(byte messageType)
        {
            return true;
        }

        public override void processIncomingMessage(IncomingMessage msg)
        {
            base.processIncomingMessage(msg);
            if (msg.MessageType == 20)
            {
                ModCore.monitor.Log("CUSTOM FUNCTION???");
            }
        }

        /// <summary>
        /// Sends an outgoing message to appropriate players.
        /// </summary>
        /// <param name="message"></param>
        public void sendMessage(OutgoingMessage message)
        {
            if (Game1.server != null)
            {
                foreach (long peerId in (IEnumerable<long>)Game1.otherFarmers.Keys)
                {
                    Game1.server.sendMessage(peerId, message);
                }
            }
            if (Game1.client != null)
            {
                if (Game1.client is CustomLidgrenClient) {
                    (Game1.client as CustomLidgrenClient).sendMessage(message);
                    return;
                }
                if (Game1.client is CustomGalaxyClient)
                {
                    (Game1.client as CustomGalaxyClient).sendMessage(message);
                    return;
                }
                ModCore.monitor.Log("Error sending server message!!!");

            }
        }


        /// <summary>
        /// Updates the server.
        /// </summary>
        public override void UpdateEarly()
        {
            if (Game1.CurrentEvent == null)
                this.removeDisconnectedFarmers();
            this.updatePendingConnections();
            if (Game1.server != null)
                (Game1.server as CustomGameServer).receiveMessages();
            else if (Game1.client != null)
                Game1.client.receiveMessages();
            this.tickFarmerRoots();
            this.tickLocationRoots();
        }

        /// <summary>
        /// Creates a net outgoing message that is written specifically to call a void function when sent.
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="objectParametersType"></param>
        /// <param name="data"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public OutgoingMessage sendOutGoingMessageReturnVoid(string functionName, string objectParametersType, object data, Farmer source)
        {
            OutgoingMessage message = new OutgoingMessage((byte)20, source, makeDataArray(functionName, objectParametersType, data));
            return message;
        }

        public OutgoingMessage sendOutGoingMessageReturnVoid(string functionName, Type objectParametersType, object data, Farmer source)
        {
            OutgoingMessage message = new OutgoingMessage((byte)20, source, makeDataArray(functionName, objectParametersType.ToString(), data));
            return message;
        }


        public object[] makeDataArray(string functionName, string objectParametersType, object data)
        {
            DataInfo datainfo = new DataInfo(objectParametersType, data);
            object[] obj = new object[3]
            {
                functionName,
                typeof(DataInfo).ToString(),
                datainfo,
            };
            return obj;
        }

        /// <summary>
        /// Creates all of the necessary parameters for the outgoing message to be sent to the server/client on what to do and how to handle the data sent.
        /// This message written will attempt to access a function that doesn't return anything. Essentially null.
        /// </summary>
        /// <param name="uniqueID"></param>
        /// <param name="classType"></param>
        /// <param name="data"></param>
        public void sendModInfoReturnVoid(string uniqueID,Type classType,object data)
        {
            Farmer f = Game1.player;

            OutgoingMessage message =ModCore.multiplayer.sendOutGoingMessageReturnVoid(uniqueID, classType, data, f);

            ModCore.multiplayer.sendMessage(message);
        }

    }
}
