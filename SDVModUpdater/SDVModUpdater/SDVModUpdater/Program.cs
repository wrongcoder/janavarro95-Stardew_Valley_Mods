using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDVModUpdater
{

    //TODO: Add in a DATE/Time Variable and input for more mod info stuff
    class Program
    {
       public static List<string> SupportedMods;

        static void Main(string[] args)
        {
            SupportedMods = new List<string>();
            SupportedMods.Add("C:/Users/owner/Documents/Visual Studio 2015/Projects/github/StardewValleyMods/GeneralMods/AutoSpeed");
            SupportedMods.Add("C:/Users/owner/Documents/Visual Studio 2015/Projects/github/StardewValleyMods/GeneralMods/BillboardAnywhere");
            SupportedMods.Add("C:/Users/owner/Documents/Visual Studio 2015/Projects/github/StardewValleyMods/GeneralMods/BuildEndurance");
            SupportedMods.Add("C:/Users/owner/Documents/Visual Studio 2015/Projects/github/StardewValleyMods/GeneralMods/BuildHealth");
            SupportedMods.Add("C:/Users/owner/Documents/Visual Studio 2015/Projects/github/StardewValleyMods/GeneralMods/BuyBackCollectables");
            SupportedMods.Add("C:/Users/owner/Documents/Visual Studio 2015/Projects/github/StardewValleyMods/GeneralMods/CurrentLocationPopUp");
            SupportedMods.Add("C:/Users/owner/Documents/Visual Studio 2015/Projects/github/StardewValleyMods/GeneralMods/DailyQuestAnywhere");
            SupportedMods.Add("C:/Users/owner/Documents/Visual Studio 2015/Projects/github/StardewValleyMods/GeneralMods/Fall28SnowDay");
            SupportedMods.Add("C:/Users/owner/Documents/Visual Studio 2015/Projects/github/StardewValleyMods/GeneralMods/HappyBirthday");
            SupportedMods.Add("C:/Users/owner/Documents/Visual Studio 2015/Projects/github/StardewValleyMods/GeneralMods/MoreRain");
            SupportedMods.Add("C:/Users/owner/Documents/Visual Studio 2015/Projects/github/StardewValleyMods/GeneralMods/MuseumRearranger");
            SupportedMods.Add("C:/Users/owner/Documents/Visual Studio 2015/Projects/github/StardewValleyMods/GeneralMods/NightOwl");
            SupportedMods.Add("C:/Users/owner/Documents/Visual Studio 2015/Projects/github/StardewValleyMods/GeneralMods/NoMorePets");
            SupportedMods.Add("C:/Users/owner/Documents/Visual Studio 2015/Projects/github/StardewValleyMods/GeneralMods/SaveAnywhere");
            SupportedMods.Add("C:/Users/owner/Documents/Visual Studio 2015/Projects/github/StardewValleyMods/GeneralMods/SaveBackup");
            SupportedMods.Add("C:/Users/owner/Documents/Visual Studio 2015/Projects/github/StardewValleyMods/GeneralMods/StardewSymphony");
            SupportedMods.Add("C:/Users/owner/Documents/Visual Studio 2015/Projects/github/StardewValleyMods/GeneralMods/TimeFreeze");

            SupportedMods.Add("C:/Users/owner/Documents/Visual Studio 2015/Projects/github/StardewValleyMods/OmegasisCore/OmegasisCore");
            SupportedMods.Add("C:/Users/owner/Documents/Visual Studio 2015/Projects/github/StardewValleyMods/Revitalize/Revitalize/Revitalize");
            foreach (var v in SupportedMods)
            {
                string ModVersion="";
                string APIVersion="";
                string ModName = "";

                string ModMajorVersion="";
                string ModMinorVersion="";
                string ModPatchVersion="";
                string ModBuildVersion="";

                string APIMajorVersion = "";
                string APIMinorVersion = "";
                string APIPatchVersion = "";

                if (Directory.Exists(v))
                {
                    ModName = getModName(v);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Processing data for mod: " + ModName);
                    Console.ResetColor();

                    string modInfoFile = (Path.Combine(v, "ModInfo.txt"));
                    string ManifestFile= (Path.Combine(v, "Manifest.json"));
                    if (!File.Exists(modInfoFile))
                    {
                        createModInfoFile(modInfoFile,ModName,ManifestFile);
                    }
                    else
                    {
                        //File Exists. 
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(ModName + ": ModInfo already exists.");
                        readModInfoFile(modInfoFile,ModName);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Edit the ModInfo file?");

                        string input = Console.ReadLine();
                        bool exit = false;
                        while (exit == false)
                        {
                            
                            if (input == "y" || input == "Y")
                            {
                                File.Delete(modInfoFile);
                                createModInfoFile(modInfoFile,ModName,ManifestFile);
                                exit = true;
                            }
                            else if (input == "n" || input == "N")
                            {
                                exit = true;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("ERROR: Invaild input. Input either Y/y/N/n to continue.");
                                Console.ForegroundColor = ConsoleColor.White;
                                input=Console.ReadLine();
                            }
                        }
                        //Output some stuff to the console here and prompt to update the mod info file.
                    }
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Done Writing ModInfo.txt file for: "+ModName);

                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid mod project directory @ " + v+"\nPlease make sure the directory is a valid one.");
                }
                //Console.WriteLine(v);
                Console.ResetColor();
            }
            Console.WriteLine("Done Processing all Mods. Please press any key to now compile all of the mods.");
            Console.ReadKey();

            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "C:/Users/owner/Documents/Visual Studio 2015/Projects/github/StardewValleyMods/ModUpdater.bat";
            proc.StartInfo.WorkingDirectory = "C:/Users/owner/Documents/Visual Studio 2015/Projects/github/StardewValleyMods";
            proc.Start();
            Console.WriteLine("All done! Press any key to close this.");
            Console.ReadKey();
        }

        public static string getModName(string pathName)
        {
            string modName = "";

            string[] s = pathName.Split('/');

            modName = s[s.Length - 1];
            //Console.WriteLine(modName);
            return modName;

        }

        public static string validateNumber(string Input)
        {
            bool doesItWork = false;
            string input = Input;
            while (doesItWork == false)
            {
                try
                {
                    int i = Convert.ToInt32(input);
                    doesItWork = true;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: Input is not a number. Please input a number and try again.");
                    doesItWork = false;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    input=Console.ReadLine();
                    Console.ResetColor();
                }           
            }
            return input;
        }

        public static void createModInfoFile(string path, string modName, string ManifestFile)
        {
            //File.Create(modInfoFile);

            string ModVersion = "";
            string APIVersion = "";
            string ModName = "";

            string ModMajorVersion = "";
            string ModMinorVersion = "";
            string ModPatchVersion = "";
            string ModBuildVersion = "";

            string APIMajorVersion = "";
            string APIMinorVersion = "";
            string APIPatchVersion = "";

            string DateTimeUpdated= DateTime.Now.ToString();
            


            ModName = modName;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Creating ModInfo file for " + ModName);

            Console.WriteLine("Please input Mod Major Version info");
            Console.ForegroundColor = ConsoleColor.Cyan;
            string modInput = Console.ReadLine();
            ModMajorVersion = validateNumber(modInput);
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("Please input Mod Minor Version info");
            Console.ForegroundColor = ConsoleColor.Cyan;
            modInput = Console.ReadLine();
            ModMinorVersion = validateNumber(modInput);
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("Please input Mod Patch Version info");
            Console.ForegroundColor = ConsoleColor.Cyan;
            modInput = Console.ReadLine();
            ModPatchVersion = validateNumber(modInput);
            Console.ForegroundColor = ConsoleColor.Yellow;


            ModVersion = ModMajorVersion + "." + ModMinorVersion + "." + ModPatchVersion + "." + ModBuildVersion;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Mod Version Updated to: " + ModVersion);
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("Please put in compatible SMAPI Major Version");
            Console.ForegroundColor = ConsoleColor.Cyan;
            string apiInput = Console.ReadLine();

            APIMajorVersion = validateNumber(apiInput);

            Console.WriteLine("Please put in compatible SMAPI Minor Version");
            Console.ForegroundColor = ConsoleColor.Cyan;
            apiInput = Console.ReadLine();

            APIMinorVersion = validateNumber(apiInput);

            Console.WriteLine("Please put in compatible SMAPI Patch Version");
            Console.ForegroundColor = ConsoleColor.Cyan;
            apiInput = Console.ReadLine();

            APIPatchVersion = validateNumber(apiInput);

            APIVersion = APIMajorVersion + "." + APIMinorVersion + "." + APIPatchVersion;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("API Version Updated to: " + APIVersion);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Mod updated at: " + DateTimeUpdated);
            File.WriteAllLines(path, new string[] { ModName + ":", "ModVersion:", ModVersion.ToString(), "SMAPI Compatibilty Version:", APIVersion.ToString(),"Date/Time Updated",DateTimeUpdated});
            Console.WriteLine("Successfully created ModInfo.txt file for mod " + ModName);

            if (File.Exists(ManifestFile))
            {

                string json = File.ReadAllText(ManifestFile);
                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                jsonObj["Version"]["MajorVersion"] = ModMajorVersion;
                jsonObj["Version"]["MinorVersion"] = ModMajorVersion;
                jsonObj["Version"]["PatchVersion"] = ModMajorVersion;
                jsonObj["Version"]["Build"] = "";
                jsonObj["UniqueID"] = ModName;
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(ManifestFile, output);

                string[] q = File.ReadAllLines(ManifestFile);
                foreach (var r in q)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(r);
                }

            }
        }

        public static void readModInfoFile(string path,string modName)
        {
            string[] s = File.ReadAllLines(path);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Previous Mod Version Number: " + s[2]);
            Console.WriteLine("Previous API Version Number: " + s[4]);
            Console.WriteLine("Mod info created/updated on: " + s[6]);
        }
    }
}
