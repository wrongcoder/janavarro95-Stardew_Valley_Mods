using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Revitalize.Framework.Objects;
using StardewValley;

namespace Revitalize.Framework.Utilities
{
    public class MultiplayerUtilities
    {
        public static Dictionary<GameLocation, Dictionary<Vector2, List<Item>>> ItemsToRestore = new Dictionary<GameLocation, Dictionary<Vector2, List<Item>>>();
        public static Dictionary<long, List<Item>> PlayerItemsToRestore = new Dictionary<long, List<Item>>();

        public static int onlineFarmers;
        public static bool needToRestore;
        public static bool HasLoadedIn;

        public static bool ShouldRestoreObjects
        {
            get
            {
                return HasLoadedIn && Game1.player.isMoving() && needToRestore == true;
            }
        }

        public static void OnPlayerConnect(object sender, StardewModdingAPI.Events.PeerContextReceivedEventArgs e)
        {
            if (Game1.IsServer)
            {
                needToRestore = true;
                foreach (Farmer f in Game1.getAllFarmers())
                {
                    if (PlayerItemsToRestore.ContainsKey(f.UniqueMultiplayerID))
                    {
                        //Do nothing.
                    }
                    else
                    {
                        PlayerItemsToRestore.Add(f.UniqueMultiplayerID, new List<Item>());
                    }

                    foreach (Item I in f.Items)
                    {
                        if (I == null) continue;

                        if (I is CustomObject)
                        {
                            //(I as CustomObject).InitNetFields();
                            PlayerItemsToRestore[f.UniqueMultiplayerID].Add(I);
                        }
                    }
                    foreach (Item I in PlayerItemsToRestore[f.UniqueMultiplayerID])
                    {
                        f.removeItemFromInventory(I);
                    }
                }
                ModCore.log("Clear items because someone showed up.");
            }

        }

        public static void OnPlayerDisconnect(object sender, StardewModdingAPI.Events.PeerDisconnectedEventArgs e)
        {
            if (Game1.IsServer)
            {
                foreach (Farmer f in Game1.getAllFarmers())
                {
                    if (PlayerItemsToRestore.ContainsKey(f.UniqueMultiplayerID))
                    {
                        //Do nothing.
                    }
                    else
                    {
                        PlayerItemsToRestore.Add(f.UniqueMultiplayerID, new List<Item>());
                    }

                    foreach (Item I in f.Items)
                    {
                        if (I == null) continue;
                        if (I is CustomObject)
                        {
                            //(I as CustomObject).InitNetFields();
                            PlayerItemsToRestore[f.UniqueMultiplayerID].Add(I);
                            /*
                            if (I is MultiTiledObject)
                            {
                                foreach (CustomObject c in (I as MultiTiledObject).objects.Values)
                                {
                                    //c.InitNetFields();
                                }
                            }
                            */
                        }
                    }
                    foreach (Item I in PlayerItemsToRestore[f.UniqueMultiplayerID])
                    {
                        f.removeItemFromInventory(I);
                    }
                }
                ModCore.log("Clear items because someone disconnected.");
            }
        }

        public static void GameLoop_OneSecondUpdateTicked(object sender, StardewModdingAPI.Events.UpdateTickedEventArgs e)
        {
            if (ShouldRestoreObjects)
            {
                ModCore.ModHelper.Multiplayer.SendMessage("Revitalize.RestoreModObjectsAfterConnect", "Revitalize.RestoreModObjectsAfterConnect", new string[] { ModCore.Manifest.UniqueID });
                needToRestore = false;
            }
        }

        public static void ModMessageReceived(object sender, StardewModdingAPI.Events.ModMessageReceivedEventArgs e)
        {
            if (e.Type == "Revitalize.RestoreModObjectsAfterConnect")
            {
                RestoreModObjects();
                needToRestore = false;
            }
            if(e.Type == "Revitalize.RequestCustomObjects")
            {
                SendCustomObjects();
            }
            if(e.Type== "Revitalize.SendCustomObjects")
            {
                Dictionary<Guid, CustomObject> additionalObjects =ModCore.Serializer.Deserialize<Dictionary<Guid,CustomObject>>(e.ReadAs<string>());
                foreach(KeyValuePair<Guid,CustomObject> pair in additionalObjects)
                {
                    if (ModCore.CustomObjects.ContainsKey(pair.Key))
                    {
                        ModCore.CustomObjects[pair.Key] = pair.Value;
                    }
                    else
                    {
                        ModCore.CustomObjects.Add(pair.Key, pair.Value);
                    }
                }
            }

            if(e.Type== "Revitalize.RequestCustomObjectGUID")
            {
                Guid requested = Guid.Parse(e.ReadAs<string>());
                //if specific guid exists send that.
                if (ModCore.CustomObjects.ContainsKey(requested))
                {
                    if (ModCore.CustomObjects[requested].info != null)
                    {
                        KeyValuePair<Guid, CustomObject> pair = new KeyValuePair<Guid, CustomObject>(requested, ModCore.CustomObjects[requested]);
                        SendCustomObject(pair);
                    }
                }
                else
                {
                    //If it doesn't exist send the whole dictionary.
                    SendCustomObjects();
                }
            }
            if(e.Type== "Revitalize.SendCustomObjectGUID")
            {
                KeyValuePair<Guid,CustomObject> requested = ModCore.Serializer.DeserializeFromJSONString<KeyValuePair<Guid,CustomObject>>(e.ReadAs<string>());
                if (ModCore.CustomObjects.ContainsKey(requested.Key))
                {
                    ModCore.CustomObjects[requested.Key] = requested.Value;
                }
                else
                {
                    ModCore.CustomObjects.Add(requested.Key, requested.Value);
                }
            }
        }

        public static void RestoreModObjects()
        {
            foreach (KeyValuePair<long, List<Item>> items in PlayerItemsToRestore)
            {
                Farmer f = Game1.getFarmer(items.Key);
                foreach (Item I in items.Value)
                {
                    f.addItemToInventoryBool(I, false);
                }
                items.Value.Clear();
            }
            ModCore.log("Restore mod objects!");
            needToRestore = false;
        }

        public static void SendCustomObjects()
        {
            ModCore.ModHelper.Multiplayer.SendMessage("Revitalize.SendCustomObjects", ModCore.Serializer.ToJSONString(ModCore.CustomObjects), new string[] { ModCore.Manifest.UniqueID });
        }

        public static void SendRequestForCustomObjects()
        {
            ModCore.ModHelper.Multiplayer.SendMessage("Revitalize.RequestCustomObjects", "Revitalize.RequestCustomObjects", new string[] { ModCore.Manifest.UniqueID });
        }

        public static void SendRequestForSpecificGUID(Guid guid)
        {
            ModCore.ModHelper.Multiplayer.SendMessage("Revitalize.RequestCustomObjectGUID", guid.ToString(), new string[] { ModCore.Manifest.UniqueID });
        }

        public static void SendCustomObject(KeyValuePair<Guid,CustomObject> Pair)
        {
            string s = ModCore.Serializer.ToJSONString(Pair);
            ModCore.ModHelper.Multiplayer.SendMessage("Revitalize.SendCustomObjectGUID", s , new string[] { ModCore.Manifest.UniqueID });
        }

    }
}
