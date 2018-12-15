using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Netcode;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Locations;
using StardewValley.Minigames;
using StardewValley.Network;

namespace StardustCore.NetCode
{
    public class GameServer : IGameServer
    {
        public List<Server> servers = new List<Server>();
        public List<Action> pendingGameAvailableActions = new List<Action>();

        public GameServer()
        {
            this.servers.Add(ModCore.multiplayer.InitServer((Server)new LidgrenServer((IGameServer)this)));
            if (Program.sdk.Networking == null)
                return;
            this.servers.Add(Program.sdk.Networking.CreateServer((IGameServer)this));
        }

        public int connectionsCount
        {
            get
            {
                return this.servers.Sum<Server>((Func<Server, int>)(s => s.connectionsCount));
            }
        }

        public string getInviteCode()
        {
            foreach (Server server in this.servers)
            {
                string inviteCode = server.getInviteCode();
                if (inviteCode != null)
                    return inviteCode;
            }
            return (string)null;
        }

        public string getUserName(long farmerId)
        {
            foreach (Server server in this.servers)
            {
                if (server.getUserName(farmerId) != null)
                    return server.getUserName(farmerId);
            }
            return (string)null;
        }

        protected void initialize()
        {
            foreach (Server server in this.servers)
                server.initialize();
            this.updateLobbyData();
        }

        public void setPrivacy(ServerPrivacy privacy)
        {
            foreach (Server server in this.servers)
                server.setPrivacy(privacy);
        }

        public void stopServer()
        {
            foreach (Server server in this.servers)
                server.stopServer();
        }

        public void receiveMessages()
        {
            foreach (Server server in this.servers)
            {
                if (server == null)
                {
                    ModCore.ModMonitor.Log("ERROR");
                    continue;
                }
                server.receiveMessages();
            }
            if (!this.isGameAvailable())
                return;
            foreach (Action gameAvailableAction in this.pendingGameAvailableActions)
                gameAvailableAction();
            this.pendingGameAvailableActions.Clear();
        }

        public void sendMessage(long peerId, OutgoingMessage message)
        {
            foreach (Server server in this.servers)
                server.sendMessage(peerId, message);
        }

        public bool canAcceptIPConnections()
        {
            return this.servers.Select<Server, bool>((Func<Server, bool>)(s => s.canAcceptIPConnections())).Aggregate<bool, bool>(false, (Func<bool, bool, bool>)((a, b) => a | b));
        }

        public bool canOfferInvite()
        {
            return this.servers.Select<Server, bool>((Func<Server, bool>)(s => s.canOfferInvite())).Aggregate<bool, bool>(false, (Func<bool, bool, bool>)((a, b) => a | b));
        }

        public void offerInvite()
        {
            foreach (Server server in this.servers)
            {
                if (server.canOfferInvite())
                    server.offerInvite();
            }
        }

        public bool connected()
        {
            foreach (Server server in this.servers)
            {
                if (!server.connected())
                    return false;
            }
            return true;
        }

        public void sendMessage(long peerId, byte messageType, Farmer sourceFarmer, params object[] data)
        {
            this.sendMessage(peerId, new OutgoingMessage(messageType, sourceFarmer, data));
        }

        public void sendMessages()
        {
            foreach (Farmer farmer in (IEnumerable<Farmer>)Game1.otherFarmers.Values)
            {
                foreach (OutgoingMessage message in (IEnumerable<OutgoingMessage>)farmer.messageQueue)
                    this.sendMessage(farmer.UniqueMultiplayerID, message);
                farmer.messageQueue.Clear();
            }
        }

        public void startServer()
        {
            Console.WriteLine("Starting server. Protocol version: " + ModCore.multiplayer.protocolVersion);
            this.initialize();
            if ((NetFieldBase<Farmer, NetRef<Farmer>>)Game1.serverHost == (NetRef<Farmer>)null)
                Game1.serverHost = new NetFarmerRoot();
            Game1.serverHost.Value = Game1.player;
            Game1.serverHost.MarkClean();
            Game1.serverHost.Clock.InterpolationTicks = ModCore.multiplayer.defaultInterpolationTicks;
            if ((NetFieldBase<IWorldState, NetRef<IWorldState>>)Game1.netWorldState == (NetRef<IWorldState>)null)
                Game1.netWorldState = new NetRoot<IWorldState>((IWorldState)new NetWorldState());
            Game1.netWorldState.Clock.InterpolationTicks = 0;
            Game1.netWorldState.Value.UpdateFromGame1();
        }

        public void sendServerIntroduction(long peer)
        {
            this.sendLocation(peer, (GameLocation)Game1.getFarm());
            this.sendLocation(peer, Game1.getLocationFromName("FarmHouse"));
            ModCore.SerializationManager.cleanUpInventory();
            ModCore.SerializationManager.cleanUpWorld();
            ModCore.SerializationManager.cleanUpStorageContainers();
            this.sendMessage(peer, new OutgoingMessage((byte)1, Game1.serverHost.Value, new object[3]
            {
                (object)ModCore.multiplayer.writeObjectFullBytes<Farmer>((NetRoot<Farmer>)Game1.serverHost, new long?(peer)),
                (object)ModCore.multiplayer.writeObjectFullBytes<FarmerTeam>(Game1.player.teamRoot, new long?(peer)),
                (object)ModCore.multiplayer.writeObjectFullBytes<IWorldState>(Game1.netWorldState, new long?(peer))
            }));
            foreach (KeyValuePair<long, NetRoot<Farmer>> root in Game1.otherFarmers.Roots)
            {
                if (root.Key != Game1.player.UniqueMultiplayerID && root.Key != peer)
                    this.sendMessage(peer, new OutgoingMessage((byte)2, root.Value.Value, new object[2]
                    {
                        (object)this.getUserName(root.Value.Value.UniqueMultiplayerID),
                        (object)ModCore.multiplayer.writeObjectFullBytes<Farmer>(root.Value, new long?(peer))
                    }));
            }
            ModCore.SerializationManager.restoreAllModObjects(ModCore.SerializationManager.trackedObjectList);
        }

        public void playerDisconnected(long disconnectee)
        {
            Farmer sourceFarmer = (Farmer)null;
            Game1.otherFarmers.TryGetValue(disconnectee, out sourceFarmer);
            ModCore.multiplayer.playerDisconnected(disconnectee);
            if (sourceFarmer == null)
                return;
            OutgoingMessage message = new OutgoingMessage((byte)19, sourceFarmer, new object[0]);
            foreach (long key in (IEnumerable<long>)Game1.otherFarmers.Keys)
            {
                if (key != disconnectee)
                    this.sendMessage(key, message);
            }
        }

        public bool isGameAvailable()
        {
            /*
            bool flag1 = Game1.currentMinigame is Intro || Game1.Date.DayOfMonth == 0;
            bool flag2 = Game1.CurrentEvent != null && Game1.CurrentEvent.isWedding;
            bool flag3 = Game1.newDaySync != null && !Game1.newDaySync.hasFinished();
            //bool flag4 = Game1.player.team.buildingLock.IsLocked();
            if (!Game1.isFestival() && !flag2 && (!flag1 && !flag3))
                return !flag4;
            return false;
            *
            */
            return true;
        }

        public bool whenGameAvailable(Action action)
        {
            if (this.isGameAvailable())
            {
                action();
                return true;
            }
            this.pendingGameAvailableActions.Add(action);
            return false;
        }

        private void rejectFarmhandRequest(string userID, NetFarmerRoot farmer, Action<OutgoingMessage> sendMessage)
        {
            this.sendAvailableFarmhands(userID, sendMessage);
            Console.WriteLine("Rejected request for farmhand " + (farmer.Value != null ? farmer.Value.UniqueMultiplayerID.ToString() : "???"));
        }

        private IEnumerable<Cabin> cabins()
        {
            if (Game1.getFarm() != null)
            {
                foreach (Building building in Game1.getFarm().buildings)
                {
                    if ((int)((NetFieldBase<int, NetInt>)building.daysOfConstructionLeft) <= 0 && building.indoors.Value is Cabin)
                        yield return building.indoors.Value as Cabin;
                }
            }
        }

        private bool authCheck(string userID, Farmer farmhand)
        {
            if (!Game1.options.enableFarmhandCreation && !(bool)((NetFieldBase<bool, NetBool>)farmhand.isCustomized))
                return false;
            if (!(userID == "") && !(farmhand.userID.Value == ""))
                return farmhand.userID.Value == userID;
            return true;
        }

        private Cabin findCabin(Farmer farmhand)
        {
            foreach (Cabin cabin in this.cabins())
            {
                if (cabin.getFarmhand().Value.UniqueMultiplayerID == farmhand.UniqueMultiplayerID)
                    return cabin;
            }
            return (Cabin)null;
        }

        private Farmer findOriginalFarmhand(Farmer farmhand)
        {
            return this.findCabin(farmhand)?.getFarmhand().Value;
        }

        public void checkFarmhandRequest(string userID, NetFarmerRoot farmer, Action<OutgoingMessage> sendMessage, Action approve)
        {
            if (farmer.Value == null)
            {
                this.rejectFarmhandRequest(userID, farmer, sendMessage);
            }
            else
            {
                long id = farmer.Value.UniqueMultiplayerID;
                if (this.whenGameAvailable((Action)(() =>
                {
                    Farmer originalFarmhand = this.findOriginalFarmhand(farmer.Value);
                    if (originalFarmhand == null)
                    {
                        Console.WriteLine("Rejected request for farmhand " + (object)id + ": doesn't exist");
                        this.rejectFarmhandRequest(userID, farmer, sendMessage);
                    }
                    else if (!this.authCheck(userID, originalFarmhand))
                    {
                        Console.WriteLine("Rejected request for farmhand " + (object)id + ": authorization failure");
                        this.rejectFarmhandRequest(userID, farmer, sendMessage);
                    }
                    else if (Game1.otherFarmers.ContainsKey(id) && !ModCore.multiplayer.isDisconnecting(id) || Game1.serverHost.Value.UniqueMultiplayerID == id)
                    {
                        Console.WriteLine("Rejected request for farmhand " + (object)id + ": already in use");
                        this.rejectFarmhandRequest(userID, farmer, sendMessage);
                    }
                    else if (this.findCabin(farmer.Value).isInventoryOpen())
                    {
                        Console.WriteLine("Rejected request for farmhand " + (object)id + ": inventory in use");
                        this.rejectFarmhandRequest(userID, farmer, sendMessage);
                    }
                    else
                    {
                        Console.WriteLine("Approved request for farmhand " + (object)id);
                        approve();
                        ModCore.multiplayer.addPlayer(farmer);
                        ModCore.multiplayer.broadcastPlayerIntroduction(farmer);
                        this.sendServerIntroduction(id);
                        this.updateLobbyData();
                    }
                })))
                    return;
                Console.WriteLine("Postponing request for farmhand " + (object)id);
                sendMessage(new OutgoingMessage((byte)11, Game1.player, new object[1]
                {
                    (object)"Strings\\UI:Client_WaitForHostAvailability"
                }));
            }
        }

        public void sendAvailableFarmhands(string userID, Action<OutgoingMessage> sendMessage)
        {
            List<NetRef<Farmer>> netRefList = new List<NetRef<Farmer>>();
            Game1.getFarm();
            foreach (Cabin cabin in this.cabins())
            {
                NetRef<Farmer> farmhand = cabin.getFarmhand();
                if ((!farmhand.Value.isActive() || ModCore.multiplayer.isDisconnecting(farmhand.Value.UniqueMultiplayerID)) && (this.authCheck(userID, farmhand.Value) && !cabin.isInventoryOpen()))
                    netRefList.Add(farmhand);
            }
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter((Stream)memoryStream))
                {
                    writer.Write(Game1.year);
                    writer.Write(Utility.getSeasonNumber(Game1.currentSeason));
                    writer.Write(Game1.dayOfMonth);
                    writer.Write((byte)netRefList.Count);
                    foreach (NetRef<Farmer> netRef in netRefList)
                    {
                        try
                        {
                            netRef.Serializer = SaveGame.farmerSerializer;
                            netRef.WriteFull(writer);
                        }
                        finally
                        {
                            netRef.Serializer = (XmlSerializer)null;
                        }
                    }
                    memoryStream.Seek(0L, SeekOrigin.Begin);
                    sendMessage(new OutgoingMessage((byte)9, Game1.player, new object[1]
                    {
                        (object)memoryStream.ToArray()
                    }));
                }
            }
        }

        private void sendLocation(long peer, GameLocation location)
        {
            this.sendMessage(peer, (byte)3, Game1.serverHost.Value, (object)ModCore.multiplayer.writeObjectFullBytes<GameLocation>(ModCore.multiplayer.locationRoot(location), new long?(peer)));
        }

        private void warpFarmer(Farmer farmer, short x, short y, string name, bool isStructure)
        {
            GameLocation locationFromName = Game1.getLocationFromName(name, isStructure);
            locationFromName.hostSetup();
            farmer.currentLocation = locationFromName;
            farmer.Position = new Vector2((float)((int)x * 64), (float)((int)y * 64 - (farmer.Sprite.getHeight() - 32) + 16));
            this.sendLocation(farmer.UniqueMultiplayerID, locationFromName);
        }

        public void processIncomingMessage(IncomingMessage message)
        {
            switch (message.MessageType)
            {
                case 2:
                    message.Reader.ReadString();
                    ModCore.multiplayer.processIncomingMessage(message);
                    break;
                case 5:
                    this.warpFarmer(message.SourceFarmer, message.Reader.ReadInt16(), message.Reader.ReadInt16(), message.Reader.ReadString(), message.Reader.ReadByte() == (byte)1);
                    break;
                default:
                    ModCore.multiplayer.processIncomingMessage(message);
                    break;
            }
            if (!ModCore.multiplayer.isClientBroadcastType(message.MessageType))
                return;
            this.rebroadcastClientMessage(message);
        }

        private void rebroadcastClientMessage(IncomingMessage message)
        {
            OutgoingMessage message1 = new OutgoingMessage(message);
            foreach (long key in (IEnumerable<long>)Game1.otherFarmers.Keys)
            {
                if (key != message.FarmerID)
                    this.sendMessage(key, message1);
            }
        }

        private void setLobbyData(string key, string value)
        {
            foreach (Server server in this.servers)
                server.setLobbyData(key, value);
        }

        private bool unclaimedFarmhandsExist()
        {
            foreach (Cabin cabin in this.cabins())
            {
                if (cabin.farmhand.Value == null || cabin.farmhand.Value.userID.Value == "")
                    return true;
            }
            return false;
        }

        public void updateLobbyData()
        {
            this.setLobbyData("farmName", Game1.player.farmName.Value);
            this.setLobbyData("farmType", Convert.ToString(Game1.whichFarm));
            this.setLobbyData("date", Convert.ToString(new WorldDate(Game1.year, Game1.currentSeason, Game1.dayOfMonth).TotalDays));
            this.setLobbyData("farmhands", string.Join(",", Game1.getAllFarmhands().Select<Farmer, string>((Func<Farmer, string>)(farmhand => farmhand.userID.Value)).Where<string>((Func<string, bool>)(user => user != ""))));
            this.setLobbyData("newFarmhands", Convert.ToString(Game1.options.enableFarmhandCreation && this.unclaimedFarmhandsExist()));
        }
    }
}
