using System;
using StardewValley;
using StardewModdingAPI;
using System.IO;
using StardewValley.Menus;
using System.Collections.Generic;
using System.Linq;
using StardewValley.Objects;
using Microsoft.Xna.Framework;
using StardewValley.Tools;
/*
TO DO:
Items
furniture
swords -
hats -
boots   -
wallpapers -
carpets -
ring -
lamp-
craftables-
*/
namespace Custom_Shop_Mod_Redux
{
    public class Class1 : Mod
    {
      static  int j = 0;
     static   Dictionary<Item, int[]> list_price = new Dictionary<Item, int[]>();

        static string master_path;
        List<string> myoptions = new List<string>();
        string key_binding = "U";

        bool game_loaded = false;

        public override void Entry(params object[] objects)
        {
            //set up all of my events here
            StardewModdingAPI.Events.PlayerEvents.LoadedGame += PlayerEvents_LoadedGame;
            StardewModdingAPI.Events.ControlEvents.KeyPressed += ControlEvents_KeyPressed;
        }



        public void ControlEvents_KeyPressed(object sender, StardewModdingAPI.Events.EventArgsKeyPressed e)
        {
            

            if (e.KeyPressed.ToString() == key_binding) //if the key is pressed, load my cusom save function
            {
                
                    my_key_call();


            }
            //DataLoader_Settings(); //update the key if players changed it while playing.

        }

        public void PlayerEvents_LoadedGame(object sender, StardewModdingAPI.Events.EventArgsLoadedGameChanged e)
        {
            game_loaded = true;
            DataLoader_Settings();
            MyWritter_Settings();
        }
        



        void DataLoader_Settings()
        {
            //loads the data to the variables upon loading the game.
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(PathOnDisk, "Custom_Shop_Redux_Config");
            string mylocation2 = mylocation;
            string mylocation3 = mylocation2 + ".txt";
            master_path = PathOnDisk;

            Directory.CreateDirectory(Path.Combine(master_path, "Custom_Shops"));

            master_path = Path.Combine(master_path, "Custom_Shops");

           

            if (!File.Exists(mylocation3)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
                //  Console.WriteLine("Can't load custom save info since the file doesn't exist.");

                key_binding = "U";
                //  Log.Info("KEY TIME");
            }

            else
            {
                //        Console.WriteLine("HEY THERE IM LOADING DATA");
                string[] readtext = File.ReadAllLines(mylocation3);
                key_binding = Convert.ToString(readtext[3]);


                // Log.Info(key_binding);
                // Log.Info(Convert.ToString(readtext[3]));

            }
        }

        void MyWritter_Settings()
        {

            //write all of my info to a text file.
            string myname = StardewValley.Game1.player.name;

            string mylocation = Path.Combine(PathOnDisk, "Custom_Shop_Redux_Config");
            string mylocation2 = mylocation;
            string mylocation3 = mylocation2 + ".txt";

            string[] mystring3 = new string[20];
            if (!File.Exists(mylocation3))
            {
                Console.WriteLine("The custom character save info doesn't exist. It will be created when the custom saving method is run. Which is now.");

                mystring3[0] = "Config: Custom_Shop_Redux. Feel free to mess with these settings.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Key binding for saving anywhere. Press this key to save anywhere!";
                mystring3[3] = key_binding.ToString();



                File.WriteAllLines(mylocation3, mystring3);

            }

            else
            {

                //write out the info to a text file at the end of a day. This will run if it doesnt exist.
                Console.WriteLine("The custom character save info doesn't exist. It will be created when the custom saving method is run. Which is now.");

                mystring3[0] = "Config: Custom_Shop_Redux. Feel free to mess with these settings.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Key binding for saving anywhere. Press this key to save anywhere!";
                mystring3[3] = key_binding.ToString();



                File.WriteAllLines(mylocation3, mystring3);
            }
        }




      public  void my_key_call()
        {
            var modpath = new Custom_Shop_Mod_Redux.Class1();
            

            DirectoryInfo d = new DirectoryInfo(master_path);//Assuming Test is your Folder
            Log.Info(d);
          //  System.Threading.Thread.Sleep(20000);
            
            FileInfo[] Files = d.GetFiles("*.txt"); //Getting Text files
            string str = "";

            if (Files.Length == 0)
            {
                Log.Error("No shop .txt information is found. You should create one.");
                return;

            }
            foreach (FileInfo file in Files)
            {
                str = file.Name;
           //     Log.Info(str);
                
                myoptions.Add(str);
            }

            if (myoptions.Count == 0)
            {
                Log.Error("No shop .txt information is found. You should create one.");
                return;
            }

            StardewValley.Menus.ChooseFromListMenu mychoices = new StardewValley.Menus.ChooseFromListMenu(myoptions, new ChooseFromListMenu.actionOnChoosingListOption(shop_file_call), false);

            Game1.activeClickableMenu = mychoices;

       
            

          


        }
       


        static void shop_file_call(string s)
        {
            List<Item> list = new List<Item>();
            

            string mylocation = Path.Combine(master_path , s);
           
      ///      Log.Info(mylocation);
           

          string[] readtext = File.ReadAllLines(mylocation);
   
            var lineCount = File.ReadLines(mylocation).Count();


//          Log.Info(lineCount);

            if(s== "Custom_Shop_Redux_Config.txt")
            {
                Log.Info("Silly human. The config file is not a shop.");
                return;
            }

            int i = 4;

            int obj_id = 0;
            bool is_recipe;
            int price;
            int quality;

            string obj_type;

            obj_type = Convert.ToString(readtext[i]);
            i += 2;

           

            while (i < lineCount)
            {
                if (i >= lineCount) break;
                if (obj_type == "") break;
                if (readtext[i] == "") break;

                //read in a line for obj type here
                Log.Info(i);
                Log.Info(obj_type);
                if (obj_type == "item" || obj_type == "Item" || obj_type == "Object" || obj_type == "object")
                {

                    obj_id = Convert.ToInt16(readtext[i]);
                    i += 2;

                    Log.Info(i);
                    is_recipe = Convert.ToBoolean(readtext[i]);
                    i += 2;
                    Log.Info(i);
                    price = Convert.ToInt32(readtext[i]);
                    i += 2;
                    Log.Info(i);

                    quality = Convert.ToInt32(readtext[i]);
                    // if (quality > 2) quality = 0;


                    if (price == -1)
                    {
                        StardewValley.Object myobj = new StardewValley.Object(obj_id, int.MaxValue, is_recipe, price, quality);

                     //   Log.Info("MYPRICE");
                     //   Log.Info(myobj.salePrice());
                        price = myobj.salePrice();

                    }

                   
                    //   list.Add((Item)new StardewValley.Object(obj_id, int.MaxValue, is_recipe, price, quality));
                    list_price.Add((Item)new StardewValley.Object(obj_id, int.MaxValue, is_recipe, price, quality), new int[2] {price, int.MaxValue});
                    i += 3;
                    if (i >= lineCount) break;
                }
                if (obj_type == "Furniture" || obj_type == "furniture")
                {
                    obj_id = Convert.ToInt32(readtext[i]);
                    i += 2;
                    price = Convert.ToInt32(readtext[i]);

                    //  list.Add((Item)new Furniture(obj_id, Vector2.Zero)); //ADD FUNCTIONALITY TO SHOP FILES TO TEST IF FURNITURE OR NOT.
                    list_price.Add((Item)new Furniture(obj_id, Vector2.Zero), new int[2] { price, int.MaxValue });
                    i += 3;
                    if (i>= lineCount) break;
                    



                }

                if (obj_type == "Boots" || obj_type == "boots" || obj_type == "shoe" || obj_type == "Shoe")  //just incase someone forgets it's called boots and they type shoe.
                {
                    obj_id = Convert.ToInt32(readtext[i]);
                    i += 2;
                    price= Convert.ToInt32(readtext[i]);
                    list_price.Add((Item)new StardewValley.Objects.Boots(obj_id),new int[2] { price, int.MaxValue });
                    i += 3;
                    if (i >= lineCount) break;

                }

                if (obj_type == "WallPaper" || obj_type == "Wallpaper" || obj_type == "wallPaper" || obj_type == "wallpaper")  //just incase someone forgets it's called boots and they type shoe.
                {
                    if (i + 3 > lineCount) break;
                    obj_id = Convert.ToInt32(readtext[i]);
                    i += 2;
                    price = Convert.ToInt32(readtext[i]);
                    list_price.Add((Item)new StardewValley.Objects.Wallpaper(obj_id, false), new int[2] { price, int.MaxValue }); //add in support for wallpapers and carpets
                    i += 3;
                    if (i >= lineCount) break;

                }

                if (obj_type == "Carpet" || obj_type == "carpet" || obj_type == "Floor" || obj_type == "floor" || obj_type == "Rug" || obj_type == "rug")
                {
                    obj_id = Convert.ToInt32(readtext[i]);
                    i += 2;
                    price = Convert.ToInt32(readtext[i]);
                    list_price.Add((Item)new StardewValley.Objects.Wallpaper(obj_id, true), new int[2] { price, int.MaxValue }); //add in support for wallpapers and carpets
                    i += 3;
                    if (i >= lineCount) break;
                }

                if (obj_type == "Ring" || obj_type == "ring")
                {
                    obj_id = Convert.ToInt32(readtext[i]);
                    i += 2;
                    price = Convert.ToInt32(readtext[i]);
                    list_price.Add((Item)new StardewValley.Objects.Ring(obj_id), new int[2] { price, int.MaxValue });
                    i += 3;
                    if (i >= lineCount) break;
                }
                if (obj_type == "Lamp" || obj_type == "lamp" || obj_type == "Torch" || obj_type == "torch" || obj_type == "Craftable" || obj_type == "craftable" || obj_type == "BigCraftable" || obj_type == "bigcraftable")
                {
                    obj_id = Convert.ToInt32(readtext[i]);
                    i += 2;
                    price = Convert.ToInt32(readtext[i]);
                    list_price.Add((Item)new StardewValley.Torch(Vector2.Zero, obj_id, true), new int[2] { price, int.MaxValue });
                    i += 3;
                    if (i >= lineCount) break;
                }

                if (obj_type == "Sword" || obj_type == "sword" || obj_type == "Weapon" || obj_type == "weapon")
                {
                    obj_id = Convert.ToInt32(readtext[i]);
                    i += 2;
                    price = Convert.ToInt32(readtext[i]);
                    list_price.Add((Item)new MeleeWeapon(obj_id), new int[2] { price, int.MaxValue });

                    i += 3;
                    if (i >= lineCount) break;
                }

                if (obj_type == "Hat" || obj_type == "hat" || obj_type == "Hats" || obj_type == "hats")
                {
                    obj_id = Convert.ToInt32(readtext[i]);
                    i += 2;
                    price = Convert.ToInt32(readtext[i]);
                    //  list_price.Add((Item)new Hat(obj_id), new int[2] { price, int.MaxValue });
                   list_price.Add((Item)new Hat(obj_id), new int[2]
                   {
          price,
          int.MaxValue
  });
                    i += 3;
                    if (i >= lineCount) break;
                }


                //TODO:
                //add in support for colored objects
                //add in support for tools
                Log.Success(i);
                if (i >= lineCount) break;
                else {
                    obj_type = Convert.ToString(readtext[i]);
                    i += 2;
                }
            }


            //NEED TO TEST ALL DATA FILES TO SEE WHAT CAN AND CANT BE ADDED
            //list.Add((Item)new StardewValley.Objects.ColoredObject(475,300, Color.Aqua));
         //   Game1.activeClickableMenu = (IClickableMenu)new ShopMenu(list, 0, "Pierre");




            Game1.activeClickableMenu = (IClickableMenu)new ShopMenu(list_price, 0, "Pierre");

        }


        static void external_shop_file_call(string path, string fileName)
        {
            List<Item> list = new List<Item>();


            string mylocation = Path.Combine(path, fileName);

            ///      Log.Info(mylocation);


            string[] readtext = File.ReadAllLines(mylocation);

            var lineCount = File.ReadLines(mylocation).Count();


            //          Log.Info(lineCount);

            int i = 4;

            int obj_id = 0;
            bool is_recipe;
            int price;
            int quality;

            string obj_type;

            obj_type = Convert.ToString(readtext[i]);
            i += 2;



            while (i < lineCount)
            {
                if (i >= lineCount) break;
                if (obj_type == "") break;
                if (readtext[i] == "") break;

                //read in a line for obj type here
                Log.Info(i);
                Log.Info(obj_type);
                if (obj_type == "item" || obj_type == "Item" || obj_type == "Object" || obj_type == "object")
                {

                    obj_id = Convert.ToInt16(readtext[i]);
                    i += 2;

                    Log.Info(i);
                    is_recipe = Convert.ToBoolean(readtext[i]);
                    i += 2;
                    Log.Info(i);
                    price = Convert.ToInt32(readtext[i]);
                    i += 2;
                    Log.Info(i);

                    quality = Convert.ToInt32(readtext[i]);
                    // if (quality > 2) quality = 0;


                    if (price == -1)
                    {
                        StardewValley.Object myobj = new StardewValley.Object(obj_id, int.MaxValue, is_recipe, price, quality);

                        //   Log.Info("MYPRICE");
                        //   Log.Info(myobj.salePrice());
                        price = myobj.salePrice();

                    }


                    //   list.Add((Item)new StardewValley.Object(obj_id, int.MaxValue, is_recipe, price, quality));
                    list_price.Add((Item)new StardewValley.Object(obj_id, int.MaxValue, is_recipe, price, quality), new int[2] { price, int.MaxValue });
                    i += 3;
                    if (i >= lineCount) break;
                }
                if (obj_type == "Furniture" || obj_type == "furniture")
                {
                    obj_id = Convert.ToInt32(readtext[i]);
                    i += 2;
                    price = Convert.ToInt32(readtext[i]);

                    //  list.Add((Item)new Furniture(obj_id, Vector2.Zero)); //ADD FUNCTIONALITY TO SHOP FILES TO TEST IF FURNITURE OR NOT.
                    list_price.Add((Item)new Furniture(obj_id, Vector2.Zero), new int[2] { price, int.MaxValue });
                    i += 3;
                    if (i >= lineCount) break;




                }

                if (obj_type == "Boots" || obj_type == "boots" || obj_type == "shoe" || obj_type == "Shoe")  //just incase someone forgets it's called boots and they type shoe.
                {
                    obj_id = Convert.ToInt32(readtext[i]);
                    i += 2;
                    price = Convert.ToInt32(readtext[i]);
                    list_price.Add((Item)new StardewValley.Objects.Boots(obj_id), new int[2] { price, int.MaxValue });
                    i += 3;
                    if (i >= lineCount) break;

                }

                if (obj_type == "WallPaper" || obj_type == "Wallpaper" || obj_type == "wallPaper" || obj_type == "wallpaper")  //just incase someone forgets it's called boots and they type shoe.
                {
                    if (i + 3 > lineCount) break;
                    obj_id = Convert.ToInt32(readtext[i]);
                    i += 2;
                    price = Convert.ToInt32(readtext[i]);
                    list_price.Add((Item)new StardewValley.Objects.Wallpaper(obj_id, false), new int[2] { price, int.MaxValue }); //add in support for wallpapers and carpets
                    i += 3;
                    if (i >= lineCount) break;

                }

                if (obj_type == "Carpet" || obj_type == "carpet" || obj_type == "Floor" || obj_type == "floor" || obj_type == "Rug" || obj_type == "rug")
                {
                    obj_id = Convert.ToInt32(readtext[i]);
                    i += 2;
                    price = Convert.ToInt32(readtext[i]);
                    list_price.Add((Item)new StardewValley.Objects.Wallpaper(obj_id, true), new int[2] { price, int.MaxValue }); //add in support for wallpapers and carpets
                    i += 3;
                    if (i >= lineCount) break;
                }

                if (obj_type == "Ring" || obj_type == "ring")
                {
                    obj_id = Convert.ToInt32(readtext[i]);
                    i += 2;
                    price = Convert.ToInt32(readtext[i]);
                    list_price.Add((Item)new StardewValley.Objects.Ring(obj_id), new int[2] { price, int.MaxValue });
                    i += 3;
                    if (i >= lineCount) break;
                }
                if (obj_type == "Lamp" || obj_type == "lamp" || obj_type == "Torch" || obj_type == "torch" || obj_type == "Craftable" || obj_type == "craftable" || obj_type == "BigCraftable" || obj_type == "bigcraftable")
                {
                    obj_id = Convert.ToInt32(readtext[i]);
                    i += 2;
                    price = Convert.ToInt32(readtext[i]);
                    list_price.Add((Item)new StardewValley.Torch(Vector2.Zero, obj_id, true), new int[2] { price, int.MaxValue });
                    i += 3;
                    if (i >= lineCount) break;
                }

                if (obj_type == "Sword" || obj_type == "sword" || obj_type == "Weapon" || obj_type == "weapon")
                {
                    obj_id = Convert.ToInt32(readtext[i]);
                    i += 2;
                    price = Convert.ToInt32(readtext[i]);
                    list_price.Add((Item)new MeleeWeapon(obj_id), new int[2] { price, int.MaxValue });

                    i += 3;
                    if (i >= lineCount) break;
                }

                if (obj_type == "Hat" || obj_type == "hat" || obj_type == "Hats" || obj_type == "hats")
                {
                    obj_id = Convert.ToInt32(readtext[i]);
                    i += 2;
                    price = Convert.ToInt32(readtext[i]);
                    //  list_price.Add((Item)new Hat(obj_id), new int[2] { price, int.MaxValue });
                    list_price.Add((Item)new Hat(obj_id), new int[2]
                    {
          price,
          int.MaxValue
   });
                    i += 3;
                    if (i >= lineCount) break;
                }


                //TODO:
                //add in support for colored objects
                //add in support for tools
                Log.Success(i);
                if (i >= lineCount) break;
                else {
                    obj_type = Convert.ToString(readtext[i]);
                    i += 2;
                }
            }


            //NEED TO TEST ALL DATA FILES TO SEE WHAT CAN AND CANT BE ADDED
            //list.Add((Item)new StardewValley.Objects.ColoredObject(475,300, Color.Aqua));
            //   Game1.activeClickableMenu = (IClickableMenu)new ShopMenu(list, 0, "Pierre");




            Game1.activeClickableMenu = (IClickableMenu)new ShopMenu(list_price, 0, "Pierre");

        }


        //example of a shop that I don't use.
        public static List<Item> myshop()
        {
            List<Item> list = new List<Item>();
            list.Add((Item)new StardewValley.Object(478, int.MaxValue, false, -1, 0)); //int parentsheet index OR object_ID/int.MaxValue, bool is recipe, price, quality 
            list.Add((Item)new StardewValley.Object(486, int.MaxValue, false, -1, 0));
            list.Add((Item)new StardewValley.Object(494, int.MaxValue, false, -1, 0)); //Might be able to manipulate this code to give me recipes!!!!
            list.Add((Item)new StardewValley.Object(495, int.MaxValue, false, 800, 0));  //price is *2 of value shown. -1 means inherit default value
            switch (Game1.dayOfMonth % 7)
            {
                case 0:
                    list.Add((Item)new StardewValley.Object(233, int.MaxValue, false, -1, 0));
                    break;
                case 1:
                    list.Add((Item)new StardewValley.Object(88, int.MaxValue, false, -1, 0));
                    break;
                case 2:
                    list.Add((Item)new StardewValley.Object(90, int.MaxValue, false, -1, 0));
                    break;
                case 3:
                    list.Add((Item)new StardewValley.Object(749, int.MaxValue, false, 500, 0));
                    break;
                case 4:
                    list.Add((Item)new StardewValley.Object(466, int.MaxValue, false, -1, 0));
                    break;
                case 5:
                    list.Add((Item)new StardewValley.Object(340, int.MaxValue, false, -1, 0));
                    break;
                case 6:
                    list.Add((Item)new StardewValley.Object(371, int.MaxValue, false, 100, 0));
                    break;
            }
            return list;
        }


    }
}
//end class