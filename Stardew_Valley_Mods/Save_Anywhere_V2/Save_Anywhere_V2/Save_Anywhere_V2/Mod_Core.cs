using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using SerializerUtils;
using StardewModdingAPI.Events;
using System.Reflection;
using System.Globalization;
using System.IO;
using StardewValley.Characters;
using Newtonsoft.Json;

namespace Save_Anywhere_V2
{
    public class Mod_Core : StardewModdingAPI.Mod
    {

        public static string mod_path;
        public static string player_path;
        public static string animal_path;
        public static string npc_path;
        public static bool npc_warp;
        public static int checking_time;
        public static bool once;
        public static bool new_day;
        Dictionary<string, string> npc_key_value_pair;

        public static string Error_Path;
        string location_name;
        string npc_name;

        public override void Entry(params object[] objects)
        {
            StardewModdingAPI.Events.ControlEvents.KeyPressed += KeyPressed_Save_Load_Menu;
            StardewModdingAPI.Events.PlayerEvents.LoadedGame += PlayerEvents_LoadedGame;
            StardewModdingAPI.Events.GameEvents.UpdateTick += Warp_Check;
            StardewModdingAPI.Events.GameEvents.UpdateTick += ShippingCheck;
            StardewModdingAPI.Events.TimeEvents.TimeOfDayChanged += NPC_scheduel_update;
            StardewModdingAPI.Events.TimeEvents.DayOfMonthChanged += TimeEvents_DayOfMonthChanged;
            StardewModdingAPI.Events.TimeEvents.OnNewDay += TimeEvents_OnNewDay;
            Command.RegisterCommand("include_types", "Includes types to serialize").CommandFired += Command_IncludeTypes;
            mod_path = PathOnDisk;
            Error_Path = Path.Combine(mod_path, "Error_Logs");
            npc_key_value_pair = new Dictionary<string, string>();
        }

        private void TimeEvents_OnNewDay(object sender, EventArgsNewDay e)
        {
            try
            {
                //Log.Info("Day of Month Changed");
                new_day = true;
                string name = Game1.player.name;
                Save_Anywhere_V2.Mod_Core.player_path = Path.Combine(Save_Anywhere_V2.Mod_Core.mod_path, "Save_Data", name);
                if (Directory.Exists(player_path))
                {
                    Directory.Delete(player_path, true);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                    serializer.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    serializer.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All;
                    serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
                    serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    using (StreamWriter sw = new StreamWriter(Path.Combine(Error_Path, "Mod_State.json")))
                    {
                        using (Newtonsoft.Json.JsonWriter writer2 = new Newtonsoft.Json.JsonTextWriter(sw))
                        {
                            serializer.Serialize(writer2, this, typeof(Save_Anywhere_V2.Mod_Core));
                        }
                    }
                }
                catch (Exception exc)
                {
                    Log.Info(exc);
                }
                Stardew_Omegasis_Utilities.Mod.Error_Handling.Log_Error(new List<string>(), ex);
            }
        }

        private void TimeEvents_DayOfMonthChanged(object sender, EventArgsIntChanged e)
        {
            try
            {
                //new_day = true;
                // Log.Info("Day of Month Changed");
                npc_key_value_pair.Clear();
                foreach (var loc in Game1.locations)
                {
                    location_name = loc.name;
                    foreach (var character in loc.characters)
                    {
                        npc_name = character.name;
                        npc_key_value_pair.Add(character.name, get_key_value(character));
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                    serializer.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    serializer.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All;
                    serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
                    serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    using (StreamWriter sw = new StreamWriter(Path.Combine(Error_Path, "Mod_State.json")))
                    {
                        using (Newtonsoft.Json.JsonWriter writer2 = new Newtonsoft.Json.JsonTextWriter(sw))
                        {
                            serializer.Serialize(writer2, this, typeof(Save_Anywhere_V2.Mod_Core));
                        }
                    }
                }
                catch (Exception exc)
                {
                    Log.Info(exc);
                }
                Stardew_Omegasis_Utilities.Mod.Error_Handling.Log_Error(new List<string>(), ex);
            }
        }

        private void NPC_scheduel_update(object sender, EventArgs e)
        {
            try
            {
                //if (once == true) return;
                //FieldInfo field = typeof(NPC).GetField("scheduleTimeToTry", BindingFlags.NonPublic | BindingFlags.Instance);
                // MethodInfo dynMethod = typeof(NPC).GetMethod("prepareToDisembarkOnNewSchedulePath",BindingFlags.NonPublic | BindingFlags.Instance);
                MethodInfo dynMethod2 = typeof(NPC).GetMethod("pathfindToNextScheduleLocation", BindingFlags.NonPublic | BindingFlags.Instance);

                if (npc_warp == false) return;
                if (new_day == true) return;
                List<StardewValley.Characters.Child> child_list = new List<StardewValley.Characters.Child>();
                child_list = StardewValley.Game1.player.getChildren();
                foreach (var loc in Game1.locations)
                {
                    location_name = loc.name;
                    foreach (var npc in loc.characters)
                    {
                        npc_name = npc.name;
                        if (npc.DirectionsToNewLocation != null) continue;
                        if (npc.isMoving() == true) continue;
                        if (npc.Schedule == null) continue;
                        foreach (var child_name in child_list)
                        {
                            if (npc.name == child_name.name) continue;
                        }
                        if (npc.name == Game1.player.getPetName()) continue;
                        Horse horse = StardewValley.Utility.findHorse();
                        if (horse == null) continue;
                        if (npc.name == horse.name) continue;
                        // Log.Info("THIS IS MY NPC" + npc.name);
                        //  Log.AsyncC("NO SCHEDULE FOUND FOR " + npc.name);


                        //  npc.checkSchedule(Game1.timeOfDay);
                        SchedulePathDescription schedulePathDescription;

                        //int myint = (int)field.GetValue(npc);
                        /*
                        npc.Schedule.TryGetValue(Game1.timeOfDay, out schedulePathDescription);
                        int i = 0;
                        int pseudo_time=0;

                        while (schedulePathDescription == null)
                        {
                            i += 10;
                            pseudo_time = Game1.timeOfDay - i;
                            if (pseudo_time <= 600) { break; }
                            npc.Schedule.TryGetValue(pseudo_time, out schedulePathDescription);
                            checking_time = pseudo_time;
                        }
                        // npc.directionsToNewLocation = schedulePathDescription;
                        // npc.prepareToDisembarkOnNewSchedulePath();

                        //field.SetValue(npc, 9999999);

                        npc.DirectionsToNewLocation = schedulePathDescription;
                       */
                        //////////////////////////////////////////
                        // Log.Info("Does this break here 1");
                        Dictionary<string, string> dictionary;
                        string key_value = "";
                        try
                        {
                            dictionary = Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + npc.name);
                        }
                        catch (Exception ex)
                        {
                            // dictionary = new Dictionary<string, string>();//(Dictionary<int, SchedulePathDescription>)null;
                            continue;
                        }
                        // Log.Info("Does this break here 2");
                        //////////////////////
                        string value;
                        string end_map;
                        int x;
                        int y;
                        int end_dir;
                        string behavior;
                        string message;
                        npc_key_value_pair.TryGetValue(npc.name, out key_value);
                        if (key_value == "" || key_value == null) continue;
                        dictionary.TryGetValue(key_value, out value);
                        // Log.Info("Does this break here 3");
                        string[] valueArray1 = value.Split('/');
                        int count1 = 0;
                        foreach (var josh in valueArray1)
                        {
                            string[] valueArray2 = valueArray1[count1].Split(' ');

                            if (Convert.ToInt32(valueArray2.ElementAt(0)) > Game1.timeOfDay) break;
                            end_map = Convert.ToString(valueArray2.ElementAt(1));
                            x = Convert.ToInt32(valueArray2.ElementAt(2));
                            y = Convert.ToInt32(valueArray2.ElementAt(3));
                            end_dir = Convert.ToInt32(valueArray2.ElementAt(4));
                            schedulePathDescription = (SchedulePathDescription)dynMethod2.Invoke(npc, new object[] { npc.currentLocation.name, npc.getTileX(), npc.getTileY(), end_map, x, y, end_dir, null, null });
                            count1++;


                            if (schedulePathDescription == null) continue;
                            //  Log.Info("This works 2");
                            // Utility.getGameLocationOfCharacter(npc);
                            //  Log.Info("This works 3");

                            npc.DirectionsToNewLocation = schedulePathDescription;
                            npc.controller = new PathFindController(npc.DirectionsToNewLocation.route, (Character)npc, Utility.getGameLocationOfCharacter(npc))
                            {
                                finalFacingDirection = npc.DirectionsToNewLocation.facingDirection,
                                endBehaviorFunction = null//npc.getRouteEndBehaviorFunction(npc.DirectionsToNewLocation.endOfRouteBehavior, npc.DirectionsToNewLocation.endOfRouteMessage)
                            };

                        }
                    }
                }
                once = true;
            }
            catch (Exception ex)
            {
                try
                {
                    Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                    serializer.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    serializer.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All;
                    serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
                    serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    using (StreamWriter sw = new StreamWriter(Path.Combine(Error_Path, "Mod_State.json")))
                    {
                        using (Newtonsoft.Json.JsonWriter writer2 = new Newtonsoft.Json.JsonTextWriter(sw))
                        {
                            serializer.Serialize(writer2, this, typeof(Save_Anywhere_V2.Mod_Core));
                        }
                    }
                }
                catch (Exception exc)
                {
                    Log.Info(exc);
                }
                Stardew_Omegasis_Utilities.Mod.Error_Handling.Log_Error(new List<string>(), ex);
            }
        }

        private string get_key_value(NPC npc)
        {
            try
            {
                Dictionary<string, string> dictionary;
                string key_value = "";
                try
                {
                    dictionary = Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + npc.name);
                }
                catch (Exception ex)
                {
                    dictionary = new Dictionary<string, string>();//(Dictionary<int, SchedulePathDescription>)null;
                }
                if (dictionary.ContainsKey(Game1.currentSeason + "_" + Convert.ToString(Game1.dayOfMonth)))
                    key_value = Game1.currentSeason + "_" + Convert.ToString(Game1.dayOfMonth);
                for (int index = !Game1.player.friendships.ContainsKey(npc.name) ? -1 : Game1.player.friendships[npc.name][0] / 250; index > 0; --index)
                {
                    if (dictionary.ContainsKey(Convert.ToString(Game1.dayOfMonth) + "_" + Convert.ToString(index)))
                        key_value = Convert.ToString(Game1.dayOfMonth) + "_" + Convert.ToString(index);
                }
                if (dictionary.ContainsKey(string.Empty + (object)Game1.dayOfMonth))
                    key_value = string.Empty + (object)Game1.dayOfMonth;
                if (npc.name.Equals("Pam") && Game1.player.mailReceived.Contains("ccVault"))
                    key_value = "bus";
                if (Game1.isRaining)
                {
                    if (Game1.random.NextDouble() < 0.5 && dictionary.ContainsKey("rain2"))
                        key_value = "rain2";
                    if (dictionary.ContainsKey("rain"))
                        key_value = "rain";
                }
                List<string> list = new List<string>()
      {
        Game1.currentSeason,
        Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)
      };
                int num1 = !Game1.player.friendships.ContainsKey(npc.name) ? -1 : Game1.player.friendships[npc.name][0] / 250;
                while (num1 > 0)
                {
                    list.Add(string.Empty + (object)num1);
                    if (dictionary.ContainsKey(string.Join("_", (IEnumerable<string>)list)))
                        key_value = string.Join("_", (IEnumerable<string>)list);
                    --num1;
                    list.RemoveAt(Enumerable.Count<string>((IEnumerable<string>)list) - 1);
                }
                if (dictionary.ContainsKey(string.Join("_", (IEnumerable<string>)list)))
                    key_value = string.Join("_", (IEnumerable<string>)list);
                if (dictionary.ContainsKey(Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)))
                    key_value = Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth);
                if (dictionary.ContainsKey(Game1.currentSeason))
                    key_value = Game1.currentSeason;
                if (dictionary.ContainsKey("spring_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)))
                    key_value = "spring_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth);
                list.RemoveAt(Enumerable.Count<string>((IEnumerable<string>)list) - 1);
                list.Add("spring");
                int num2 = !Game1.player.friendships.ContainsKey(npc.name) ? -1 : Game1.player.friendships[npc.name][0] / 250;
                while (num2 > 0)
                {
                    list.Add(string.Empty + (object)num2);
                    if (dictionary.ContainsKey(string.Join("_", (IEnumerable<string>)list)))
                        key_value = string.Join("_", (IEnumerable<string>)list);
                    --num2;
                    list.RemoveAt(Enumerable.Count<string>((IEnumerable<string>)list) - 1);
                }
                if (dictionary.ContainsKey("spring"))
                    key_value = "spring";
                return key_value;
            }
            catch (Exception ex)
            {
                try
                {
                    Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                    serializer.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    serializer.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All;
                    serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
                    serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    using (StreamWriter sw = new StreamWriter(Path.Combine(Error_Path, "Mod_State.json")))
                    {
                        using (Newtonsoft.Json.JsonWriter writer2 = new Newtonsoft.Json.JsonTextWriter(sw))
                        {
                            serializer.Serialize(writer2, this, typeof(Save_Anywhere_V2.Mod_Core));
                        }
                    }
                }
                catch (Exception exc)
                {
                    Log.Info(exc);
                }
                Stardew_Omegasis_Utilities.Mod.Error_Handling.Log_Error(new List<string>(), ex);
                return null;
            }
        }
        private void ShippingCheck(object sender, EventArgs e)
        {
            try
            {
                if (Game1.activeClickableMenu != null) return;
                Save_Anywhere_V2.Save_Utilities.GameUtilities.shipping_check();
            }
            catch (Exception ex)
            {
                try
                {
                    Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                    serializer.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    serializer.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All;
                    serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
                    serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    using (StreamWriter sw = new StreamWriter(Path.Combine(Error_Path, "Mod_State.json")))
                    {
                        using (Newtonsoft.Json.JsonWriter writer2 = new Newtonsoft.Json.JsonTextWriter(sw))
                        {
                            serializer.Serialize(writer2, this, typeof(Save_Anywhere_V2.Mod_Core));
                        }
                    }
                }
                catch (Exception exc)
                {
                    Log.Info(exc);
                }
                Stardew_Omegasis_Utilities.Mod.Error_Handling.Log_Error(new List<string>(), ex);
            }
        }

        private void Warp_Check(object sender, EventArgs e)
        {
            try
            {
                string name = StardewValley.Game1.player.name;
                Save_Anywhere_V2.Mod_Core.player_path = Path.Combine(Save_Anywhere_V2.Mod_Core.mod_path, "Save_Data", name);
                if (!Directory.Exists(Save_Anywhere_V2.Mod_Core.player_path))
                {
                    return;
                }
                if (Save_Anywhere_V2.Save_Utilities.Player_Utilities.has_player_warped_yet == false && Game1.player.isMoving() == true)
                {
                    Save_Anywhere_V2.Save_Utilities.Player_Utilities.warp_player();
                    Save_Anywhere_V2.Save_Utilities.Animal_Utilities.load_animal_info();
                    Save_Anywhere_V2.Save_Utilities.NPC_Utilities.Load_NPC_Info();
                    Save_Anywhere_V2.Save_Utilities.Player_Utilities.has_player_warped_yet = true;

                }
            }
            catch (Exception ex)
            {
                try
                {
                    Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                    serializer.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    serializer.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All;
                    serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
                    serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    using (StreamWriter sw = new StreamWriter(Path.Combine(Error_Path, "Mod_State.json")))
                    {
                        using (Newtonsoft.Json.JsonWriter writer2 = new Newtonsoft.Json.JsonTextWriter(sw))
                        {
                            serializer.Serialize(writer2, this, typeof(Save_Anywhere_V2.Mod_Core));
                        }
                    }
                }
                catch (Exception exc)
                {
                    Log.Info(exc);
                }
                Stardew_Omegasis_Utilities.Mod.Error_Handling.Log_Error(new List<string>(), ex);
            }
        }

        private void PlayerEvents_LoadedGame(object sender, EventArgsLoadedGameChanged e)
        {
            try
            {
                Save_Anywhere_V2.Save_Utilities.Player_Utilities.load_player_info();
                Save_Anywhere_V2.Save_Utilities.Config_Utilities.DataLoader_Settings();
                Save_Anywhere_V2.Save_Utilities.Config_Utilities.MyWritter_Settings();
                //
            }
            catch (Exception ex)
            {
                try
                {
                    Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                    serializer.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    serializer.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All;
                    serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
                    serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    using (StreamWriter sw = new StreamWriter(Path.Combine(Error_Path, "Mod_State.json")))
                    {
                        using (Newtonsoft.Json.JsonWriter writer2 = new Newtonsoft.Json.JsonTextWriter(sw))
                        {
                            serializer.Serialize(writer2, this, typeof(Save_Anywhere_V2.Mod_Core));
                        }
                    }
                }
                catch (Exception exc)
                {
                    Log.Info(exc);
                }
                Stardew_Omegasis_Utilities.Mod.Error_Handling.Log_Error(new List<string>(), ex);
            }
        }

        private static void Command_IncludeTypes(object sender, EventArgsCommand e)
        {
            SerializerUtility.AddType(typeof(StardewValley.Characters.Junimo)); //Adds a type to SaveGame.serializer
                                                                                // SerializerUtility.AddFarmerType(typeof(/*Class2NameHere*/)); //Adds a type to SaveGame.farmerSerializer
        }

        public void KeyPressed_Save_Load_Menu(object sender, StardewModdingAPI.Events.EventArgsKeyPressed e)
        {
            try
            {
                if (e.KeyPressed.ToString() == Save_Anywhere_V2.Save_Utilities.Config_Utilities.key_binding) //if the key is pressed, load my cusom save function
                {
                    if (Game1.activeClickableMenu != null) return;
                    try
                    {
                        Save_Anywhere_V2.Save_Utilities.GameUtilities.save_game();
                    }
                    catch (Exception exc)
                    {
                        Log.Info(exc);
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                    serializer.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    serializer.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All;
                    serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
                    serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    using (StreamWriter sw = new StreamWriter(Path.Combine(Error_Path, "Mod_State.json")))
                    {
                        using (Newtonsoft.Json.JsonWriter writer2 = new Newtonsoft.Json.JsonTextWriter(sw))
                        {
                            serializer.Serialize(writer2, this, typeof(Save_Anywhere_V2.Mod_Core));
                        }
                    }
                }
                catch (Exception exc)
                {
                    Log.Info(exc);
                }
                Stardew_Omegasis_Utilities.Mod.Error_Handling.Log_Error(new List<string>(), ex);
            }

        }
    }
}
