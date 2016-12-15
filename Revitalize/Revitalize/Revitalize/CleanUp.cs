using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using Revitalize;
using Revitalize.Objects;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize
{
    public class CleanUp
    {
        public static string InvPath;

        public static string PlayerDataPath;
        public static string DataDirectoryPath;
        

        public static void createDirectories()
        {

            DataDirectoryPath = Path.Combine(Class1.path, "PlayerData");
            PlayerDataPath = Path.Combine(DataDirectoryPath, Game1.player.name);
            InvPath = Path.Combine(PlayerDataPath, "Inventory");

            if (!Directory.Exists(DataDirectoryPath))
            {
                Directory.CreateDirectory(DataDirectoryPath);
            }
            if (!Directory.Exists(PlayerDataPath))
            {
                Directory.CreateDirectory(PlayerDataPath);
            }
            if (!Directory.Exists(InvPath))
            {
                Directory.CreateDirectory(InvPath);
            }
        }

        public static void cleanUpInventory()
        {
            createDirectories();


            List<Item> removalList = new List<Item>();
            foreach(Item d in Game1.player.items)
            {
                try {
                    if (d == null) {
                        //Log.AsyncG("WTF");
                        continue;
                    }
                   // Log.AsyncC(d.GetType());
                }
                catch(Exception e)
                {

                }
                string s = Convert.ToString((d.GetType()));

                if (s.Contains("Decoration"))
                {
                    Serialize.WriteToJsonFile(Path.Combine(InvPath,d.Name+".json"),(Decoration)d);
                    removalList.Add(d);
                }

                if (s.Contains("Light"))
                {
                    Serialize.WriteToJsonFile(Path.Combine(InvPath, d.Name + ".json"), (Light)d);
                    removalList.Add(d);
                }

                if (s.Contains("shopObject"))
                {
                    Serialize.WriteToJsonFile(Path.Combine(InvPath, d.Name + ".json"), (shopObject)d);
                    removalList.Add(d);
                }

            }
            foreach(var i in removalList)
            {
                Game1.player.removeItemFromInventory(i);
            }
            removalList.Clear();

            Log.AsyncM("Done cleaning inventory!");
        }

        public static void restoreInventory()
        {
            createDirectories();
            ProcessDirectory(InvPath);
        }



        public static void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                ProcessFile(fileName);
                File.Delete(fileName);
            }
            
            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
                
        }

        // Insert logic for processing found files here.
        public static void ProcessFile(string path)
        {
            string[] ehh = File.ReadAllLines(path);
            string data = ehh[0];
            dynamic obj = JObject.Parse(data);


         //   Log.AsyncC(obj.thisType);

            string a = obj.thisType;
            string[] b = a.Split(',');
            string s = b.ElementAt(0);
         //   Log.AsyncC(s);

            if (Util.acceptedTypes.ContainsKey(s))
            {
              //  Log.AsyncC("FUUUUU");
                foreach (KeyValuePair<string, Util.del> pair in Util.acceptedTypes)
                {
                    if (pair.Key == s)
                    {
                  var  cObj=pair.Value.Invoke(data);
                        Log.AsyncC("NEED TO HANDLE PUTTING OBJECTS BACK INTO A LOCATION!!!!");
                        if (cObj.thisLocation == null)
                        {
                            Game1.player.addItemToInventory(cObj);
                            return;
                        }
                    }
                }
            }



            
        }

    }
}
