using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Omegasis.HappyBirthday.Framework;
using Omegasis.HappyBirthday.Framework.Configs;
using Omegasis.HappyBirthday.Framework.ContentPack;
using Omegasis.HappyBirthday.Framework.Gifts;
using StardewValley;
using static System.String;
using SObject = StardewValley.Object;

namespace Omegasis.HappyBirthday
{
    public class GiftManager
    {
        public ModConfig Config => HappyBirthdayModCore.Configs.modConfig;


        /// <summary>The next birthday gift the player will receive.</summary>
        public Item BirthdayGiftToReceive;


        public static Dictionary<string, List<GiftInformation>> NPCBirthdayGifts;
        public static Dictionary<string, List<GiftInformation>> SpouseBirthdayGifts;
        public static List<GiftInformation> DefaultBirthdayGifts;

        /// <summary>Construct an instance.</summary>
        public GiftManager()
        {
            //this.BirthdayGifts = new List<Item>();


            NPCBirthdayGifts = new Dictionary<string, List<GiftInformation>>();
            SpouseBirthdayGifts = new Dictionary<string, List<GiftInformation>>();
            DefaultBirthdayGifts = new List<GiftInformation>();


            this.registerGiftIDS();

            /*
            this.loadDefaultBirthdayGifts();
            this.loadVillagerBirthdayGifts();
            this.loadSpouseBirthdayGifts();
            */

            //this.createNPCBirthdayGifts();
            //this.createSpouseBirthdayGifts();


        }

        /// <summary>
        /// Reloads all of the birthday gifts from content packs.
        /// </summary>
        public virtual void reloadBirthdayGifts()
        {

            NPCBirthdayGifts.Clear();
            SpouseBirthdayGifts.Clear();
            DefaultBirthdayGifts.Clear();

            /*
            this.loadDefaultBirthdayGifts();
            this.loadVillagerBirthdayGifts();
            this.loadSpouseBirthdayGifts();
            */

            this.loadInGiftsFromContentPacks();
        }


        protected virtual void registerGiftIDS()
        {
            foreach (var v in GiftIDS.GetSDVObjects())
            {
                Item i = new StardewValley.Object((int)v, 1);
                string uniqueID = "StardewValley.Object." + Enum.GetName(typeof(GiftIDS.SDVObject), (int)v);
                HappyBirthdayModCore.Instance.Monitor.Log("Added gift with id: " + uniqueID);
                GiftIDS.RegisteredGifts.Add(uniqueID, i);
            }
            List<string> registeredGiftKeys = GiftIDS.RegisteredGifts.Keys.ToList();
            registeredGiftKeys.Sort();
            HappyBirthdayModCore.Instance.Helper.Data.WriteJsonFile<List<string>>(Path.Combine("ModAssets", "Gifts", "RegisteredGifts" + ".json"), GiftIDS.RegisteredGifts.Keys.ToList());
        }


        //Migrating to the new content pack system, but will keep this code as reference in case I do need to go back for something, but I am doubtful...
        /*
        protected void loadDefaultBirthdayGifts()
        {

            if (File.Exists(Path.Combine(HappyBirthday.Instance.Helper.DirectoryPath, "ModAssets", "Gifts", "DefaultGifts" + ".json")))
            {
                DefaultBirthdayGifts = HappyBirthday.Instance.Helper.Data.ReadJsonFile<List<GiftInformation>>(Path.Combine("ModAssets", "Gifts", "DefaultGifts" + ".json"));
            }
            else
            {
                foreach (var v in GiftIDS.RegisteredGifts)
                {
                    //Seeds, minerals, and cooked dishes are all acceptable gifts.
                    if (v.Value.Category == -2)
                    {
                        if (v.Value.salePrice() <= 400)
                        {
                            //Add in possible minerals and gems as a 4 heart gift
                            //Exclude prismatic shards and diamonds because that is a bit much.
                            DefaultBirthdayGifts.Add(new GiftInformation(v.Key, 4, 1, 1));
                        }
                    }
                    if (v.Value.Category == -7)
                    {
                        //Add in all possible food dishes as a 6 heart value
                        DefaultBirthdayGifts.Add(new GiftInformation(v.Key, 6, 1, 1));
                    }
                    //Add in seeds as a 2 heart gift
                    if (v.Value.Category == -74)
                    {
                        int seedPrice = v.Value.salePrice();
                        if (seedPrice < 500)
                        {
                            int stackAmount = 0;
                            if (seedPrice < 100)
                            {
                                //Get 5 generic seeds
                                stackAmount = 5;
                            }
                            else
                            {
                                //Can get 2 rare seeds such as starfruit and well rare seeds.
                                stackAmount = 2;
                            }
                            if (v.Value.ParentSheetIndex == 499)
                            {
                                stackAmount = 1; //Prevent ancient fruit from giving more than 1 seed.
                            }

                            DefaultBirthdayGifts.Add(new GiftInformation(v.Key, 0, stackAmount, stackAmount));
                        }
                        else
                        {
                            //Dont add sapplings in as a gift.
                        }
                    }
                }
                HappyBirthday.Instance.Helper.Data.WriteJsonFile<List<GiftInformation>>(Path.Combine("ModAssets", "Gifts", "DefaultGifts" + ".json"), DefaultBirthdayGifts);
            }
        }

        /// <summary>Load birthday gift information from disk. Preferably from BirthdayGift.json in the mod's directory.</summary>
        protected void loadVillagerBirthdayGifts()
        {
            Directory.CreateDirectory(Path.Combine(HappyBirthday.Instance.Helper.DirectoryPath, "ModAssets", "Gifts"));
            string[] files = Directory.GetFiles(Path.Combine(HappyBirthday.Instance.Helper.DirectoryPath, "ModAssets", "Gifts"));
            foreach (string File in files)
            {
                try
                {
                    NPCBirthdayGifts.Add(Path.GetFileNameWithoutExtension(File), HappyBirthday.Instance.Helper.Data.ReadJsonFile<List<GiftInformation>>(Path.Combine("ModAssets", "Gifts", Path.GetFileNameWithoutExtension(File) + ".json")));
                    HappyBirthday.Instance.Monitor.Log("Loaded in gifts for npc: " + Path.GetFileNameWithoutExtension(File));
                }
                catch (Exception err)
                {

                }
            }
        }

        /// <summary>Used to load spouse birthday gifts from disk.</summary>
        protected void loadSpouseBirthdayGifts()
        {
            Directory.CreateDirectory(Path.Combine(HappyBirthday.Instance.Helper.DirectoryPath, "ModAssets", "Gifts", "Spouses"));
            string[] files = Directory.GetFiles(Path.Combine(HappyBirthday.Instance.Helper.DirectoryPath, "ModAssets", "Gifts", "Spouses"));
            foreach (string File in files)
            {
                SpouseBirthdayGifts.Add(Path.GetFileNameWithoutExtension(File), HappyBirthday.Instance.Helper.Data.ReadJsonFile<List<GiftInformation>>(Path.Combine("ModAssets", "Gifts", "Spouses", Path.GetFileNameWithoutExtension(File) + ".json")));
                HappyBirthday.Instance.Monitor.Log("Loaded in spouse gifts for npc: " + Path.GetFileNameWithoutExtension(File));
            }
        }
        */


        /*
        /// <summary>
        /// Creates the birthday gifts for npcs.
        /// </summary>
        protected void createNPCBirthdayGifts()
        {
            if (NPCBirthdayGifts.ContainsKey("Alex") == false)
            {
                NPCBirthdayGifts.Add("Alex", new List<GiftInformation>()
                {
                    new GiftInformation(GiftIDS.SDVObject.Egg,0,4,4),
                    new GiftInformation(GiftIDS.SDVObject.SalmonDinner,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.CompleteBreakfast,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.PurpleMushroom,0,1,1)

                });
            }

            if (NPCBirthdayGifts.ContainsKey("Elliott") == false)
            {
                NPCBirthdayGifts.Add("Elliott", new List<GiftInformation>()
                {
                    new GiftInformation(GiftIDS.SDVObject.CrabCakes,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.DuckFeather,0,1,1),
                });

                foreach (var v in GiftIDS.RegisteredGifts)
                {
                    if (v.Value.Category == -79)
                    {
                        NPCBirthdayGifts["Elliott"].Add(new GiftInformation(v.Key, 0, 20, 1, 1));
                    }
                }
            }


            if (NPCBirthdayGifts.ContainsKey("Harvey") == false)
            {
                NPCBirthdayGifts.Add("Harvey", new List<GiftInformation>()
                {
                    new GiftInformation(GiftIDS.SDVObject.MuscleRemedy,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.Coffee,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.SuperMeal,0,1,1),
                });
            }

            if (NPCBirthdayGifts.ContainsKey("Sam") == false)
            {
                NPCBirthdayGifts.Add("Sam", new List<GiftInformation>()
                {
                    new GiftInformation(GiftIDS.SDVObject.Pizza,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.JojaCola,0,6,6),
                    new GiftInformation(GiftIDS.SDVObject.CherryBomb,0,4,4),
                });
            }

            if (NPCBirthdayGifts.ContainsKey("Sebastian") == false)
            {
                NPCBirthdayGifts.Add("Sebastian", new List<GiftInformation>()
                {
                    new GiftInformation(GiftIDS.SDVObject.Quartz,0,2,2),
                    new GiftInformation(GiftIDS.SDVObject.BlueberryTart,234,1,1),
                    new GiftInformation(GiftIDS.SDVObject.CherryBomb,0,4,4),
                    new GiftInformation(GiftIDS.SDVObject.GhostCrystal,0,1,1),
                });
            }

            if (NPCBirthdayGifts.ContainsKey("Shane") == false)
            {
                NPCBirthdayGifts.Add("Shane", new List<GiftInformation>()
                {
                    new GiftInformation(GiftIDS.SDVObject.Pizza,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.Egg,0,6,6),
                    new GiftInformation(GiftIDS.SDVObject.JojaCola,0,6,6),
                    new GiftInformation(GiftIDS.SDVObject.Beer,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.Milk,0,2,2),
                });
            }


            if (NPCBirthdayGifts.ContainsKey("Abigail") == false)
            {
                NPCBirthdayGifts.Add("Abigail", new List<GiftInformation>()
                {
                    new GiftInformation(GiftIDS.SDVObject.Quartz,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.FireOpal,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.Malachite,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.IceCream,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.PurpleMushroom,0,1,1),
                });
            }

            if (NPCBirthdayGifts.ContainsKey("Emily") == false)
            {
                NPCBirthdayGifts.Add("Emily", new List<GiftInformation>()
                {
                    new GiftInformation(GiftIDS.SDVObject.Amethyst,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.Ruby,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.Topaz,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.Aquamarine,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.Cloth,0,1,1),
                });
            }

            if (NPCBirthdayGifts.ContainsKey("Haley") == false)
            {
                NPCBirthdayGifts.Add("Haley", new List<GiftInformation>());
                foreach (var v in GiftIDS.RegisteredGifts)
                {
                    //Haley gives flowers
                    if (v.Value.Category == -80)
                    {
                        NPCBirthdayGifts["Haley"].Add(new GiftInformation(v.Key, 0, 20, 1, 1));
                    }
                }
            }

            if (NPCBirthdayGifts.ContainsKey("Leah") == false)
            {
                NPCBirthdayGifts.Add("Leah", new List<GiftInformation>() {
                    new GiftInformation( GiftIDS.SDVObject.Salad,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.Wood,0,30,30),
                    new GiftInformation(GiftIDS.SDVObject.BlackberryCobbler,0,1,1)

                });
                foreach (var v in GiftIDS.RegisteredGifts)
                {
                    //Leah gives forged goods
                    if (v.Value.Category == -81)
                    {
                        NPCBirthdayGifts["Leah"].Add(new GiftInformation(v.Key, 0, 20, 1, 1));
                    }
                }
            }

            if (NPCBirthdayGifts.ContainsKey("Maru") == false)
            {
                NPCBirthdayGifts.Add("Maru", new List<GiftInformation>() {
                    new GiftInformation( GiftIDS.SDVObject.IronBar,0,3,3),
                    new GiftInformation(GiftIDS.SDVObject.GoldBar,0,3,3),
                    new GiftInformation(GiftIDS.SDVObject.CopperBar,0,5,5),
                    new GiftInformation(GiftIDS.SDVObject.BatteryPack,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.IridiumSprinkler,6,1,1)

                });
            }

            if (NPCBirthdayGifts.ContainsKey("Penny") == false)
            {
                NPCBirthdayGifts.Add("Penny", new List<GiftInformation>() {

                    new GiftInformation(GiftIDS.SDVObject.Hashbrowns,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.CrispyBass,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.VegetableMedley,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.MixedSeeds,0,5,5),
                    new GiftInformation(GiftIDS.SDVObject.Sunflower,0,1,1)
                });
            }


            if (NPCBirthdayGifts.ContainsKey("Caroline") == false)
            {
                NPCBirthdayGifts.Add("Caroline", new List<GiftInformation>() {

                    new GiftInformation(GiftIDS.SDVObject.GreenTea,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.TeaLeaves,0,3,3),
                    new GiftInformation(GiftIDS.SDVObject.SummerSpangle,0,1,1)
                });
            }

            if (NPCBirthdayGifts.ContainsKey("Clint") == false)
            {
                NPCBirthdayGifts.Add("Clint", new List<GiftInformation>() {

                    new GiftInformation(GiftIDS.SDVObject.CopperBar,0,5,5),
                    new GiftInformation(GiftIDS.SDVObject.IronBar,0,3,3),
                    new GiftInformation(GiftIDS.SDVObject.GoldBar,0,3,3),
                    new GiftInformation(GiftIDS.SDVObject.IridiumBar,4,1,1),
                    new GiftInformation(GiftIDS.SDVObject.Geode,0,5,5),
                    new GiftInformation(GiftIDS.SDVObject.FrozenGeode,0,3,3),
                    new GiftInformation(GiftIDS.SDVObject.MagmaGeode,0,2,2),
                    new GiftInformation(GiftIDS.SDVObject.OmniGeode,2,1,1),
                });
            }


            if (NPCBirthdayGifts.ContainsKey("Demetrius") == false)
            {
                NPCBirthdayGifts.Add("Demetrius", new List<GiftInformation>() {
                    new GiftInformation( GiftIDS.SDVObject.PurpleMushroom,0,2,2),
                    new GiftInformation( GiftIDS.SDVObject.RedMushroom,0,2,2),

                });
                foreach (var v in GiftIDS.RegisteredGifts)
                {
                    if (v.Value.Category == -79)
                    {
                        NPCBirthdayGifts["Demetrius"].Add(new GiftInformation(v.Key, 0, 20, 1, 1));
                    }
                }
            }

            if (NPCBirthdayGifts.ContainsKey("Dwarf") == false)
            {
                NPCBirthdayGifts.Add("Dwarf", new List<GiftInformation>() {
                    new GiftInformation( GiftIDS.SDVObject.CherryBomb,0,4,4),
                    new GiftInformation( GiftIDS.SDVObject.Bomb,0,2,2),
                    new GiftInformation( GiftIDS.SDVObject.MegaBomb,0,1,1)

                });
                foreach (var v in GiftIDS.RegisteredGifts)
                {
                    if (v.Value.Category == -2)
                    {
                        if (v.Value.salePrice() <= 400)
                        {
                            //Add in possible minerals and gems as a 4 heart gift
                            //Exclude prismatic shards and diamonds because that is a bit much.
                            NPCBirthdayGifts["Dwarf"].Add(new GiftInformation(v.Key, 0, 20, 1, 1));
                        }
                    }
                }
            }


            if (NPCBirthdayGifts.ContainsKey("Evelyn") == false)
            {
                NPCBirthdayGifts.Add("Evelyn", new List<GiftInformation>() {
                    new GiftInformation( GiftIDS.SDVObject.Cookie,0,3,3),

                });
            }

            if (NPCBirthdayGifts.ContainsKey("George") == false)
            {
                NPCBirthdayGifts.Add("George", new List<GiftInformation>() {
                    new GiftInformation( GiftIDS.SDVObject.CommonMushroom,0,1,1),
                    new GiftInformation( GiftIDS.SDVObject.FriedMushroom,0,1,1),
                    new GiftInformation( GiftIDS.SDVObject.PurpleMushroom,0,1,1),
                    new GiftInformation( GiftIDS.SDVObject.Morel,0,1,1),
                    new GiftInformation( GiftIDS.SDVObject.Truffle,0,1,1),
                    new GiftInformation( GiftIDS.SDVObject.SnowYam,0,1,1),

                });
            }


            if (NPCBirthdayGifts.ContainsKey("Gus") == false)
            {
                NPCBirthdayGifts.Add("Gus", new List<GiftInformation>());
                foreach (var v in GiftIDS.RegisteredGifts)
                {
                    if (v.Value.Category == -7)
                    {
                        if (v.Value.salePrice() >= 100)
                        {
                            //Gus will give a random food dish for the player's birthday that has some decent value to it.
                            NPCBirthdayGifts["Gus"].Add(new GiftInformation(v.Key, 0, 1, 1));
                        }

                    }
                }
            }


            if (NPCBirthdayGifts.ContainsKey("Jas") == false)
            {
                NPCBirthdayGifts.Add("Jas", new List<GiftInformation>());
                foreach (var v in GiftIDS.RegisteredGifts)
                {
                    //Jas gives flowers
                    if (v.Value.Category == -80)
                    {
                        NPCBirthdayGifts["Jas"].Add(new GiftInformation(v.Key, 0, 20, 1, 1));
                    }
                }
            }

            if (NPCBirthdayGifts.ContainsKey("Jodi") == false)
            {
                NPCBirthdayGifts.Add("Jodi", new List<GiftInformation>() {
                    new GiftInformation(GiftIDS.SDVObject.BlueberryTart,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.BlackberryCobbler,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.PumpkinPie,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.RhubarbPie,0,1,1)

                });
            }

            if (NPCBirthdayGifts.ContainsKey("Kent") == false)
            {
                NPCBirthdayGifts.Add("Kent", new List<GiftInformation>() {
                    new GiftInformation(GiftIDS.SDVObject.RoastedHazelnuts,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.FiddleheadRisotto,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.PineTar,0,3,3),
                    new GiftInformation(GiftIDS.SDVObject.BrownEgg,0,6,6)

                });
            }

            if (NPCBirthdayGifts.ContainsKey("Krobus") == false)
            {
                NPCBirthdayGifts.Add("Krobus", new List<GiftInformation>() {
                    new GiftInformation(GiftIDS.SDVObject.SolarEssence,0,5,5),
                    new GiftInformation(GiftIDS.SDVObject.VoidEssence,0,5,5),
                    new GiftInformation(GiftIDS.SDVObject.VoidEgg,4,1,1),

                });
            }


            if (NPCBirthdayGifts.ContainsKey("Lewis") == false)
            {
                NPCBirthdayGifts.Add("Lewis", new List<GiftInformation>() {
                    new GiftInformation(GiftIDS.SDVObject.GreenTea,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.PepperPoppers,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.VegetableMedley,0,1,1),

                });
            }

            if (NPCBirthdayGifts.ContainsKey("Linus") == false)
            {
                NPCBirthdayGifts.Add("Linus", new List<GiftInformation>());
                foreach (var v in GiftIDS.RegisteredGifts)
                {
                    //Linus gives forged goods
                    if (v.Value.Category == -81)
                    {
                        NPCBirthdayGifts["Linus"].Add(new GiftInformation(v.Key, 0, 20, 1, 1));
                    }
                }
            }

            if (NPCBirthdayGifts.ContainsKey("Marnie") == false)
            {
                NPCBirthdayGifts.Add("Marnie", new List<GiftInformation>());
                foreach (var v in GiftIDS.RegisteredGifts)
                {
                    //Marnie gives milk or cheese
                    if (v.Value.Category == -6)
                    {
                        NPCBirthdayGifts["Marnie"].Add(new GiftInformation(v.Key, 0, 20, 1, 1));
                    }
                    if (v.Value.Category == -26)
                    {
                        NPCBirthdayGifts["Marnie"].Add(new GiftInformation(v.Key, 0, 20, 1, 1));
                    }
                }
            }

            if (NPCBirthdayGifts.ContainsKey("Pam") == false)
            {
                NPCBirthdayGifts.Add("Pam", new List<GiftInformation>() {
                    new GiftInformation(GiftIDS.SDVObject.Mead,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.PaleAle,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.Beer,0,1,1),

                });
            }

            if (NPCBirthdayGifts.ContainsKey("Pierre") == false)
            {
                NPCBirthdayGifts.Add("Pierre", new List<GiftInformation>() {
                    new GiftInformation(GiftIDS.SDVObject.QualityFertilizer,0,5,5),
                    new GiftInformation(GiftIDS.SDVObject.BasicFertilizer,0,10,10),
                });

                foreach (var v in GiftIDS.RegisteredGifts)
                {
                    if (v.Value.Category == -74)
                    {
                        int seedPrice = v.Value.salePrice();
                        if (seedPrice < 500)
                        {
                            int stackAmount = 0;
                            if (seedPrice < 100)
                            {
                                //Get 5 generic seeds
                                stackAmount = 5;
                            }
                            else
                            {
                                //Can get 2 rare seeds such as starfruit and well rare seeds.
                                stackAmount = 2;
                            }
                            if (v.Value.ParentSheetIndex == 499)
                            {
                                stackAmount = 1; //Prevent ancient fruit from giving more than 1 seed.
                            }

                            NPCBirthdayGifts["Pierre"].Add(new GiftInformation(v.Key, 0, stackAmount, stackAmount));
                        }
                        else
                        {
                            //Dont add sapplings in as a gift.
                        }
                    }
                }
            }

            if (NPCBirthdayGifts.ContainsKey("Robin") == false)
            {
                NPCBirthdayGifts.Add("Robin", new List<GiftInformation>() {
                    new GiftInformation(GiftIDS.SDVObject.Wood,0,50,50),
                    new GiftInformation(GiftIDS.SDVObject.Stone,0,50,50),
                    new GiftInformation(GiftIDS.SDVObject.Hardwood,0,20,20),

                });
            }

            if (NPCBirthdayGifts.ContainsKey("Sandy") == false)
            {
                NPCBirthdayGifts.Add("Sandy", new List<GiftInformation>() {
                    new GiftInformation(GiftIDS.SDVObject.Starfruit,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.Milk,0,3,3)

                });
            }

            if (NPCBirthdayGifts.ContainsKey("Vincent") == false)
            {
                NPCBirthdayGifts.Add("Vincent", new List<GiftInformation>() {
                    new GiftInformation(GiftIDS.SDVObject.Snail,0,1,1),

                });
                foreach (var v in GiftIDS.RegisteredGifts)
                {
                    //Linus gives forged goods
                    if (v.Value.Category == -81)
                    {
                        NPCBirthdayGifts["Vincent"].Add(new GiftInformation(v.Key, 0, 20, 1, 1));
                    }
                }
            }

            if (NPCBirthdayGifts.ContainsKey("Willy") == false)
            {
                NPCBirthdayGifts.Add("Willy", new List<GiftInformation>() {
                    new GiftInformation(GiftIDS.SDVObject.Bait,0,50,50),
                });
                foreach (var v in GiftIDS.RegisteredGifts)
                {
                    //Linus gives forged goods
                    if (v.Value.Category == -4)
                    {
                        if (v.Value.salePrice() <= 500 && v.Value.salePrice() >= 150)
                        {
                            NPCBirthdayGifts["Willy"].Add(new GiftInformation(v.Key, 0, 20, 1, 1));
                        }
                    }
                }
            }

            if (NPCBirthdayGifts.ContainsKey("Wizard") == false)
            {
                NPCBirthdayGifts.Add("Wizard", new List<GiftInformation>() {
                    new GiftInformation(GiftIDS.SDVObject.SolarEssence,0,5,5),
                    new GiftInformation(GiftIDS.SDVObject.VoidEssence,0,5,5),
                    new GiftInformation(GiftIDS.SDVObject.FireQuartz,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.Obsidian,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.LifeElixir,0,1,1),
                });
            }

            foreach (var v in NPCBirthdayGifts)
            {
                HappyBirthday.Instance.Helper.Data.WriteJsonFile(Path.Combine("ModAssets", "Gifts", Path.GetFileNameWithoutExtension(v.Key) + ".json"), v.Value);
            }
        }

        protected void createSpouseBirthdayGifts()
        {
            if (SpouseBirthdayGifts.ContainsKey("Alex") == false)
            {
                SpouseBirthdayGifts.Add("Alex", new List<GiftInformation>()
                {
                    new GiftInformation(GiftIDS.SDVObject.Egg,0,4,4),
                    new GiftInformation(GiftIDS.SDVObject.SalmonDinner,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.CompleteBreakfast,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.PurpleMushroom,0,1,1)

                });
            }

            if (SpouseBirthdayGifts.ContainsKey("Elliott") == false)
            {
                SpouseBirthdayGifts.Add("Elliott", new List<GiftInformation>()
                {
                    new GiftInformation(GiftIDS.SDVObject.CrabCakes,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.DuckFeather,0,1,1),
                });

                foreach (var v in GiftIDS.RegisteredGifts)
                {
                    if (v.Value.Category == -79)
                    {
                        SpouseBirthdayGifts["Elliott"].Add(new GiftInformation(v.Key, 0, 20, 1, 1));
                    }
                }
            }


            if (SpouseBirthdayGifts.ContainsKey("Harvey") == false)
            {
                SpouseBirthdayGifts.Add("Harvey", new List<GiftInformation>()
                {
                    new GiftInformation(GiftIDS.SDVObject.MuscleRemedy,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.Coffee,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.SuperMeal,0,1,1),
                });
            }

            if (SpouseBirthdayGifts.ContainsKey("Sam") == false)
            {
                SpouseBirthdayGifts.Add("Sam", new List<GiftInformation>()
                {
                    new GiftInformation(GiftIDS.SDVObject.Pizza,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.JojaCola,0,6,6),
                    new GiftInformation(GiftIDS.SDVObject.CherryBomb,0,4,4),
                });
            }

            if (SpouseBirthdayGifts.ContainsKey("Sebastian") == false)
            {
                SpouseBirthdayGifts.Add("Sebastian", new List<GiftInformation>()
                {
                    new GiftInformation(GiftIDS.SDVObject.Quartz,0,2,2),
                    new GiftInformation(GiftIDS.SDVObject.BlueberryTart,234,1,1),
                    new GiftInformation(GiftIDS.SDVObject.CherryBomb,0,4,4),
                    new GiftInformation(GiftIDS.SDVObject.GhostCrystal,0,1,1),
                });
            }

            if (SpouseBirthdayGifts.ContainsKey("Shane") == false)
            {
                SpouseBirthdayGifts.Add("Shane", new List<GiftInformation>()
                {
                    new GiftInformation(GiftIDS.SDVObject.Pizza,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.Egg,0,6,6),
                    new GiftInformation(GiftIDS.SDVObject.JojaCola,0,6,6),
                    new GiftInformation(GiftIDS.SDVObject.Beer,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.Milk,0,2,2),
                });
            }


            if (SpouseBirthdayGifts.ContainsKey("Abigail") == false)
            {
                SpouseBirthdayGifts.Add("Abigail", new List<GiftInformation>()
                {
                    new GiftInformation(GiftIDS.SDVObject.Quartz,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.FireOpal,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.Malachite,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.IceCream,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.PurpleMushroom,0,1,1),
                });
            }

            if (SpouseBirthdayGifts.ContainsKey("Emily") == false)
            {
                SpouseBirthdayGifts.Add("Emily", new List<GiftInformation>()
                {
                    new GiftInformation(GiftIDS.SDVObject.Amethyst,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.Ruby,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.Topaz,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.Aquamarine,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.Cloth,0,1,1),
                });
            }

            if (SpouseBirthdayGifts.ContainsKey("Haley") == false)
            {
                SpouseBirthdayGifts.Add("Haley", new List<GiftInformation>());
                foreach (var v in GiftIDS.RegisteredGifts)
                {
                    //Haley gives flowers
                    if (v.Value.Category == -80)
                    {
                        SpouseBirthdayGifts["Haley"].Add(new GiftInformation(v.Key, 0, 20, 1, 1));
                    }
                }
            }

            if (SpouseBirthdayGifts.ContainsKey("Leah") == false)
            {
                SpouseBirthdayGifts.Add("Leah", new List<GiftInformation>() {
                    new GiftInformation( GiftIDS.SDVObject.Salad,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.Wood,0,30,30),
                    new GiftInformation(GiftIDS.SDVObject.BlackberryCobbler,0,1,1)

                });
                foreach (var v in GiftIDS.RegisteredGifts)
                {
                    //Leah gives forged goods
                    if (v.Value.Category == -81)
                    {
                        SpouseBirthdayGifts["Leah"].Add(new GiftInformation(v.Key, 0, 20, 1, 1));
                    }
                }
            }

            if (SpouseBirthdayGifts.ContainsKey("Maru") == false)
            {
                SpouseBirthdayGifts.Add("Maru", new List<GiftInformation>() {
                    new GiftInformation( GiftIDS.SDVObject.IronBar,0,3,3),
                    new GiftInformation(GiftIDS.SDVObject.GoldBar,0,3,3),
                    new GiftInformation(GiftIDS.SDVObject.CopperBar,0,5,5),
                    new GiftInformation(GiftIDS.SDVObject.BatteryPack,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.IridiumSprinkler,6,1,1)

                });
            }

            if (SpouseBirthdayGifts.ContainsKey("Penny") == false)
            {
                SpouseBirthdayGifts.Add("Penny", new List<GiftInformation>() {

                    new GiftInformation(GiftIDS.SDVObject.Hashbrowns,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.CrispyBass,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.VegetableMedley,0,1,1),
                    new GiftInformation(GiftIDS.SDVObject.MixedSeeds,0,5,5),
                    new GiftInformation(GiftIDS.SDVObject.Sunflower,0,1,1)
                });
            }


            foreach (var v in SpouseBirthdayGifts)
            {
                HappyBirthday.Instance.Helper.Data.WriteJsonFile(Path.Combine("ModAssets", "Gifts", "Spouses", Path.GetFileNameWithoutExtension(v.Key) + ".json"), v.Value);
            }
        }
        */

        public virtual void loadInGiftsFromContentPacks()
        {

            //Loads in all gifts across all content packs across all translations.
            foreach (HappyBirthdayContentPack contentPack in HappyBirthdayModCore.Instance.happyBirthdayContentPackManager.contentPacks.Values.SelectMany(contentPackList=>contentPackList))
            {
                HappyBirthdayModCore.Instance.Monitor.Log("Adding default gifts for content pack: " + contentPack.baseContentPack.Manifest.UniqueID);
                foreach (GiftInformation giftInfo in contentPack.getDefaultBirthdayGifts())
                {
                    DefaultBirthdayGifts.Add(giftInfo);
                }
                foreach (KeyValuePair<string,List<GiftInformation>> giftInfo in contentPack.npcBirthdayGifts)
                {
                    if (NPCBirthdayGifts.ContainsKey(giftInfo.Key))
                    {
                        HappyBirthdayModCore.Instance.Monitor.Log(string.Format("Adding npc {0} gifts for content pack: {1}", contentPack.baseContentPack.Manifest.UniqueID, giftInfo.Key));
                        NPCBirthdayGifts[giftInfo.Key].AddRange(giftInfo.Value);
                    }
                    else
                    {
                        HappyBirthdayModCore.Instance.Monitor.Log(string.Format("Adding npc {0} gifts for content pack: {1}", contentPack.baseContentPack.Manifest.UniqueID, giftInfo.Key));
                        NPCBirthdayGifts.Add(giftInfo.Key, giftInfo.Value);
                    }
                }
                foreach (KeyValuePair<string, List<GiftInformation>> giftInfo in contentPack.spouseBirthdayGifts)
                {
                    if (SpouseBirthdayGifts.ContainsKey(giftInfo.Key))
                    {
                        HappyBirthdayModCore.Instance.Monitor.Log(string.Format("Adding spouse {0} gifts for content pack: {1}", contentPack.baseContentPack.Manifest.UniqueID, giftInfo.Key));
                        SpouseBirthdayGifts[giftInfo.Key].AddRange(giftInfo.Value);
                    }
                    else
                    {
                        HappyBirthdayModCore.Instance.Monitor.Log(string.Format("Adding spouse {0} gifts for content pack: {1}", contentPack.baseContentPack.Manifest.UniqueID, giftInfo.Key));
                        SpouseBirthdayGifts.Add(giftInfo.Key, giftInfo.Value);
                    }
                }
            }

            List<string> registeredGiftKeys = GiftIDS.RegisteredGifts.Keys.ToList();
            registeredGiftKeys.Sort();
            HappyBirthdayModCore.Instance.Helper.Data.WriteJsonFile<List<string>>(Path.Combine("ModAssets", "Gifts", "RegisteredGifts" + ".json"),registeredGiftKeys );
        }

        /// <summary>
        /// Gets the next birthday gift that would be received by the given npc.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Item getNextBirthdayGift(string name)
        {
            if (Game1.player.friendshipData.ContainsKey(name))
            {
                if (Game1.player.getSpouse() != null)
                {
                    if (Game1.player.getSpouse().Name.Equals(name))
                    {
                        //Get spouse gift here
                        Item gift = this.getSpouseBirthdayGift(name);
                        return gift;
                    }
                    else
                    {
                        Item gift = this.getNonSpouseBirthdayGift(name);
                        return gift;
                    }
                }
                else
                {
                    if (NPCBirthdayGifts.ContainsKey(name))
                    {
                        Item gift = this.getNonSpouseBirthdayGift(name);
                        return gift;
                    }
                    else
                    {
                        Item gift = this.getDefaultBirthdayGift(name);
                        return gift;

                    }
                }
            }
            else
            {
                if (NPCBirthdayGifts.ContainsKey(name))
                {

                    Item gift = this.getNonSpouseBirthdayGift(name);
                    return gift;
                }
                else
                {
                    Item gift = this.getDefaultBirthdayGift(name);
                    return gift;
                }
            }
        }

        /// <summary>
        /// Tries to get a default spouse birthday gift.
        /// </summary>
        /// <param name="name"></param>
        public Item getNonSpouseBirthdayGift(string name)
        {
            int heartLevel = Game1.player.getFriendshipHeartLevelForNPC(name);

            HappyBirthdayModCore.Instance.Monitor.Log("Get non-spouse gift for npc" + name);

            List<Item> possibleItems = new List<Item>();
            if (NPCBirthdayGifts.ContainsKey(name))
            {
                List<GiftInformation> npcPossibleGifts = NPCBirthdayGifts[name];

                if (npcPossibleGifts == null)
                {
                    HappyBirthdayModCore.Instance.Monitor.Log("NPC GIFTS ARE NULL: " + name);
                }

                foreach (GiftInformation info in npcPossibleGifts)
                {

                    if (info == null)
                    {

                        HappyBirthdayModCore.Instance.Monitor.Log("Gift info is null????: " + name);
                        continue;
                    }

                    if (info.minRequiredHearts <= heartLevel && heartLevel <= info.maxRequiredHearts)
                    {
                        possibleItems.Add(info.getOne());
                    }
                }

                Item gift;
                int index = StardewValley.Game1.random.Next(possibleItems.Count);
                gift = possibleItems[index].getOne();
                return gift;

            }
            else
            {
                Item gift = this.getDefaultBirthdayGift(name);
                return gift;
            }

        }


        /// <summary>
        /// Tries to get a spouse birthday gift.
        /// </summary>
        /// <param name="name"></param>
        public Item getSpouseBirthdayGift(string name)
        {
            if (string.IsNullOrEmpty(HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData.favoriteBirthdayGift) == false)
            {
                if (GiftIDS.RegisteredGifts.ContainsKey(HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData.favoriteBirthdayGift))
                {
                    GiftInformation info = new GiftInformation(HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData.favoriteBirthdayGift, 0, 1, 1);
                    return info.getOne();
                }
            }

            int heartLevel = Game1.player.getFriendshipHeartLevelForNPC(name);


            List<Item> possibleItems = new List<Item>();
            if (SpouseBirthdayGifts.ContainsKey(name))
            {
                List<GiftInformation> npcPossibleGifts = SpouseBirthdayGifts[name];
                foreach (GiftInformation info in npcPossibleGifts)
                {
                    if (info.minRequiredHearts <= heartLevel && heartLevel <= info.maxRequiredHearts)
                    {
                        possibleItems.Add(info.getOne());
                    }
                }

                Item gift;
                int index = StardewValley.Game1.random.Next(possibleItems.Count);
                gift = possibleItems[index].getOne();
                return gift;
            }
            else
            {
                return this.getNonSpouseBirthdayGift(name);
            }

        }

        /// <summary>
        /// Tries to get a default birthday gift.
        /// </summary>
        /// <param name="name"></param>
        public Item getDefaultBirthdayGift(string name)
        {
            int heartLevel = Game1.player.getFriendshipHeartLevelForNPC(name);

            List<Item> possibleItems = new List<Item>();

            List<GiftInformation> npcPossibleGifts = DefaultBirthdayGifts;
            foreach (GiftInformation info in npcPossibleGifts)
            {
                if (info.minRequiredHearts <= heartLevel && heartLevel <= info.maxRequiredHearts)
                {
                    possibleItems.Add(info.getOne());
                }
            }

            Item gift;
            int index = StardewValley.Game1.random.Next(possibleItems.Count);
            gift = possibleItems[index].getOne();
            return gift;

        }

        /// <summary>
        /// Actually sets the next birthday gift to receieve or drops it on the ground for the player to pick up afterwards.
        /// </summary>
        /// <param name="gift"></param>
        public virtual void setNextBirthdayGift(Item gift)
        {
            if (Game1.player.isInventoryFull())
                Game1.createItemDebris(gift, Game1.player.getStandingPosition(), Game1.player.getDirection());
            else
                this.BirthdayGiftToReceive = gift;
        }

        /// <summary>Set the next birthday gift the player will receive.</summary>
        /// <param name="name">The villager's name who's giving the gift.</param>
        /// <remarks>This returns gifts based on the speaker's heart level towards the player: neutral for 3-4, good for 5-6, and best for 7-10.</remarks>
        public void setNextBirthdayGift(string name)
        {
            Item gift = this.getNextBirthdayGift(name);
            this.setNextBirthdayGift(gift);
        }
    }

}
