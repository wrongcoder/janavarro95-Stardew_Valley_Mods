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
        private void sendMessage(OutgoingMessage message)
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
                    if (Game1.client is CustomLidgrenClient)
                    {
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
        /// Creates a net outgoing message that is written specifically to call a void function when sent. USed to specifiy types and specific ways to handle them.
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="objectParametersType"></param>
        /// <param name="data"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public OutgoingMessage sendOutGoingMessageReturnVoid(string functionName, string objectParametersType, object data, Farmer source,Enums.MessageTypes.messageTypes sendingInfo)
        {
            byte bite = new byte();
            if (sendingInfo == Enums.MessageTypes.messageTypes.SendOneWay) bite = Enums.MessageTypes.SendOneWay;
            if (sendingInfo == Enums.MessageTypes.messageTypes.SendToAll) bite = Enums.MessageTypes.SendToAll;
            OutgoingMessage message = new OutgoingMessage(bite, source, makeDataArray(functionName, objectParametersType, data));
            return message;
        }

        public OutgoingMessage sendOutGoingMessageReturnVoid(string functionName, Type objectParametersType, object data, Farmer source, Enums.MessageTypes.messageTypes sendingInfo)
        {
            byte bite=new byte();
            if (sendingInfo == Enums.MessageTypes.messageTypes.SendOneWay) bite = Enums.MessageTypes.SendOneWay;
            if (sendingInfo == Enums.MessageTypes.messageTypes.SendToAll) bite = Enums.MessageTypes.SendToAll;
            OutgoingMessage message = new OutgoingMessage(bite, source, makeDataArray(functionName, objectParametersType.ToString(), data));
            return message;
        }

        public OutgoingMessage sendOutGoingMessageReturnVoid(string functionName, string objectParametersType, object data, Farmer source, Enums.MessageTypes.messageTypes sendingInfo, Farmer recipient)
        {
            byte bite = new byte();
            if (sendingInfo == Enums.MessageTypes.messageTypes.SendOneWay) bite = Enums.MessageTypes.SendOneWay;
            if (sendingInfo == Enums.MessageTypes.messageTypes.SendToAll) bite = Enums.MessageTypes.SendToAll;
            if (sendingInfo == Enums.MessageTypes.messageTypes.SendToSpecific) bite = Enums.MessageTypes.SendToSpecific;
            OutgoingMessage message = new OutgoingMessage(bite, source, makeDataArray(functionName, objectParametersType, data,recipient));
            return message;
        }

        public OutgoingMessage sendOutGoingMessageReturnVoid(string functionName, Type objectParametersType, object data, Farmer source, Enums.MessageTypes.messageTypes sendingInfo, Farmer recipient)
        {
            byte bite = new byte();
            if (sendingInfo == Enums.MessageTypes.messageTypes.SendOneWay) bite = Enums.MessageTypes.SendOneWay;
            if (sendingInfo == Enums.MessageTypes.messageTypes.SendToAll) bite = Enums.MessageTypes.SendToAll;
            if (sendingInfo == Enums.MessageTypes.messageTypes.SendToSpecific) bite = Enums.MessageTypes.SendToSpecific;
            OutgoingMessage message = new OutgoingMessage(bite, source, makeDataArray(functionName, objectParametersType.ToString(), data, recipient));
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

        public object[] makeDataArray(string functionName, string objectParametersType, object data, Farmer recipient)
        {
            DataInfo datainfo = new DataInfo(objectParametersType, data,recipient);
            object[] obj = new object[3]
            {
                functionName,
                typeof(DataInfo).ToString(),
                datainfo,
            };
            return obj;
        }

        public object[] makeDataArray(string functionName, string objectParametersType, object data, long recipient)
        {
            DataInfo datainfo = new DataInfo(objectParametersType, data, recipient.ToString());
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
        public void sendMessage(string uniqueID, Type classType, object data, Enums.MessageTypes.messageTypes sendingInfo, Farmer recipient = null)
        {
            Farmer f = Game1.player;

            if ((sendingInfo == Enums.MessageTypes.messageTypes.SendOneWay || sendingInfo == Enums.MessageTypes.messageTypes.SendToAll))
            {
                OutgoingMessage message = ModCore.multiplayer.sendOutGoingMessageReturnVoid(uniqueID, classType, data, f, sendingInfo);
                ModCore.multiplayer.sendMessage(message);
                return;
            }

            if (sendingInfo == Enums.MessageTypes.messageTypes.SendToSpecific && recipient != null)
            {
                OutgoingMessage message = ModCore.multiplayer.sendOutGoingMessageReturnVoid(uniqueID, classType, data, f, sendingInfo, recipient);
                ModCore.multiplayer.sendMessage(message);
                return;
            }

            if (sendingInfo == Enums.MessageTypes.messageTypes.SendToSpecific && recipient == null)
            {
                ModCore.monitor.Log("ERROR: Attempted to send a target specific message to a NULL recipient");
                return;
            }
        }


        /// <summary>
        /// A way to send mod info across the net.
        /// </summary>
        /// <param name="uniqueID"></param>
        /// <param name="classType"></param>
        /// <param name="data"></param>
        /// <param name="sendingInfo"></param>
        /// <param name="recipient"></param>
        public void sendMessage(string uniqueID, string classType, object data, Enums.MessageTypes.messageTypes sendingInfo, Farmer recipient=null)
        {
            Farmer f = Game1.player;

            if ((sendingInfo == Enums.MessageTypes.messageTypes.SendOneWay || sendingInfo == Enums.MessageTypes.messageTypes.SendToAll))
            {
                OutgoingMessage message = ModCore.multiplayer.sendOutGoingMessageReturnVoid(uniqueID, classType, data, f, sendingInfo);
                ModCore.multiplayer.sendMessage(message);
                return;
            }

            if (sendingInfo == Enums.MessageTypes.messageTypes.SendToSpecific && recipient!=null)
            {
                OutgoingMessage message = ModCore.multiplayer.sendOutGoingMessageReturnVoid(uniqueID, classType, data, f, sendingInfo,recipient);
                ModCore.multiplayer.sendMessage(message);
                return;
            }

            if (sendingInfo == Enums.MessageTypes.messageTypes.SendToSpecific && recipient == null)
            {
                ModCore.monitor.Log("ERROR: Attempted to send a target specific message to a NULL recipient");
                return;
            }
        }


        /// <summary>
        /// Get's the server host farmer.
        /// </summary>
        /// <returns></returns>
        public static Farmer getServerHost()
        {
            return Game1.serverHost.Value;
        }

        /// <summary>
        /// Get's the farmer in the player one slot also known as player 1.
        /// </summary>
        /// <returns></returns>
        public static Farmer getPlayerOne()
        {
            return getServerHost();
        }

        /// <summary>
        /// Get's the farmer in the player two slot for the server.
        /// </summary>
        /// <returns></returns>
        public static Farmer getPlayerTwo()
        {
            return Game1.otherFarmers.ElementAt(0).Value;
        }

        /// <summary>
        /// Get's the farmer in the player three slot for the server.
        /// </summary>
        /// <returns></returns>
        public static Farmer getPlayerThree()
        {
            return Game1.otherFarmers.ElementAt(1).Value;
        }

        /// <summary>
        /// Get's the farmer in the player four slot for the server.
        /// </summary>
        /// <returns></returns>
        public static Farmer getPlayerFour()
        {
            return Game1.otherFarmers.ElementAt(2).Value;
        }


        /// <summary>
        /// Gets all farmers that are not the current player.
        /// </summary>
        /// <returns></returns>
        public static List<Farmer> getAllFarmersExceptThisOne()
        {
            Farmer player = Game1.player;

            Farmer player1 = getPlayerOne();
            Farmer player2 = getPlayerTwo();
            Farmer player3 = getPlayerThree();
            Farmer player4 = getPlayerFour();

            List<Farmer> otherFarmers=new List<Farmer>();

            if (player1 != null)
            {
                if (player != player1) otherFarmers.Add(player1);
            }
            if (player2 != null)
            {
                if (player != player2) otherFarmers.Add(player2);
            }
            if (player3 != null)
            {
                if (player != player3) otherFarmers.Add(player3);
            }
            if (player4 != null)
            {
                if (player != player4) otherFarmers.Add(player4);
            }
                return otherFarmers;
        }


    }
}
