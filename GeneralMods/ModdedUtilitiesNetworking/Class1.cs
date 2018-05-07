using Galaxy.Api;
using ModdedUtilitiesNetworking.Framework;
using ModdedUtilitiesNetworking.Framework.Clients;
using ModdedUtilitiesNetworking.Framework.Delegates;
using ModdedUtilitiesNetworking.Framework.Extentions;
using ModdedUtilitiesNetworking.Framework.Servers;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using static ModdedUtilitiesNetworking.Framework.Delegates.DelegateInfo;

namespace ModdedUtilitiesNetworking
{
    public class ModCore : Mod
    {
        public static CustomMultiplayer multiplayer;

        public static IModHelper helper;
        public static IMonitor monitor;
        public static IManifest manifest;
        bool multiplayerSet;

        public static Dictionary<string, ReadWriter> objectTypes = new Dictionary<string, ReadWriter>();
        public static Dictionary<string, voidFunc> possibleVoidFunctions = new Dictionary<string, voidFunc>();

        public static string displayMessageString = "Omegasis.ModdedUtilitiesNetworking.ModCore.displayMessage";

        public override void Entry(IModHelper helper)
        {
            helper = Helper;
            monitor = Monitor;
            manifest = ModManifest;

            StardewModdingAPI.Events.SaveEvents.AfterLoad += SaveEvents_AfterLoad;
            StardewModdingAPI.Events.ControlEvents.KeyPressed += ControlEvents_KeyPressed;
            multiplayerSet = false;
            multiplayer = new CustomMultiplayer();

            possibleVoidFunctions.Add(displayMessageString, new voidFunc(displayMessage));
            initializeBasicTypes();
            
        }


        private void ControlEvents_KeyPressed(object sender, StardewModdingAPI.Events.EventArgsKeyPressed e)
        {
            
                if (e.KeyPressed==Microsoft.Xna.Framework.Input.Keys.K)
                {
                    multiplayer.sendModInfoReturnVoid(displayMessageString, typeof(String), (object)"My love is like fire.");
                }
        }

        /// <summary>
        /// Don't change this. Sets appropriate clients and servers once the game has loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            if (Game1.server != null && multiplayerSet == false)
            {
                List<Server> servers = new List<Server>();
                if (Game1.server != null)
                {
                    ModCore.monitor.Log(Game1.server.GetType().ToString());
                    //var property = ModCore.helper.Reflection.GetProperty<List<Server>>(Game1.server, "servers", true);

                    Type type = Game1.server.GetType();
                    BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;

                    // get the field info
                    FieldInfo finfo = type.GetField("servers", bindingFlags);


                    var list = (List<Server>)finfo.GetValue(Game1.server);

                    foreach (var server in list)
                    {
                        servers.Add(server);
                    }
                }
                Game1.server.stopServer();
                Game1.server = new CustomGameServer();
                multiplayerSet = true;

                (Game1.server as CustomGameServer).startServer();

                ModCore.monitor.Log("Custom multiplayer binding success!");
            }

            if(Game1.client !=null && multiplayerSet == false)
            {
                if(Game1.client is LidgrenClient)
                {
                    
                    BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;

                    // get the field info
                    FieldInfo finfo = typeof(LidgrenClient).GetField("address", bindingFlags);


                    var address = (string)finfo.GetValue(Game1.client);

                    Game1.client.disconnect(true); //Disconnect old client
                    CustomLidgrenClient client = new CustomLidgrenClient(address);
                    Game1.client = client;
                    client.connect(); //Connect new client.
                    multiplayerSet = true;
                 
                }
                if(Game1.client is StardewValley.SDKs.GalaxyNetClient)
                {
                    BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;

                    // get the field info
                    FieldInfo finfo = typeof(StardewValley.SDKs.GalaxyNetClient).GetField("lobbyId", bindingFlags);


                    var galaxyID = (GalaxyID)finfo.GetValue(Game1.client);

                    Game1.client.disconnect(true); //Disconnect old client
                    CustomGalaxyClient client = new CustomGalaxyClient(galaxyID);
                    Game1.client = client;
                    client.connect(); //Connect new client.
                    multiplayerSet = true;
                }
            }
        }

        /// <summary>
        /// Static Debug function.
        /// </summary>
        /// <param name="param"></param>
        public static void displayMessage(object param)
        {
            string s =(string) param;
            monitor.Log(s);
        }

        /// <summary>
        /// Initialize basic supported types.
        /// </summary>
        public static void initializeBasicTypes()
        {

            objectTypes.Add(typeof(String).ToString(), new ReadWriter(new reader(BinaryReadWriteExtentions.ReadString), new writer(BinaryReadWriteExtentions.WriteString)));

        }

        /// <summary>
        /// Process all possible functions that can occur.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public static void processVoidFunction(string key,object obj)
        {
            foreach(var v in possibleVoidFunctions)
            {
                if (v.Key == key) v.Value.Invoke(obj);
            }
        }

        /// <summary>
        /// Process all possible data types.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object processTypes(BinaryReader msg,string key)
        {
            foreach(var v in objectTypes)
            {
                if (v.Key == key)
                {
                    object o= v.Value.read(msg);
                    return o;
                }
            }
            monitor.Log("Error: type not found: " + key, LogLevel.Error);
            return null;
        }

    }

 
}
