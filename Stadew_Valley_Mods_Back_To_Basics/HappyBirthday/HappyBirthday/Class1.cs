using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewValley;
using System.IO;

namespace HappyBirthday
{
    public class Class1 :Mod
    {
        public List<string> npc_name_list;
        bool game_loaded;
        public string key_binding= "O";
        public List<Item> possible_birthday_gifts;
        public Item birthday_gift_to_receive;
        bool once;
        bool has_input_birthday;


        public string folder_name ="Player_Birthdays";
        public string birthdays_path;

        public static int player_birthday_date;
        public static string player_birthday_season;
        public override void Entry(params object[] objects)
        {
            StardewModdingAPI.Events.TimeEvents.DayOfMonthChanged += Day_Update;
            StardewModdingAPI.Events.GameEvents.UpdateTick += GameEvents_UpdateTick;
            StardewModdingAPI.Events.PlayerEvents.LoadedGame += PlayerEvents_LoadedGame;
            StardewModdingAPI.Events.ControlEvents.KeyPressed += ControlEvents_KeyPressed;
            npc_name_list = new List<string>();
            possible_birthday_gifts = new List<Item>();
            birthdays_path = Path.Combine(PathOnDisk, folder_name);
            if (!Directory.Exists(birthdays_path))
            {
                Directory.CreateDirectory(birthdays_path);
            }
        }

        public void ControlEvents_KeyPressed(object sender, StardewModdingAPI.Events.EventArgsKeyPressed e)
        {
            if (Game1.player == null) return;
            if (Game1.player.currentLocation == null) return;
            if (game_loaded == false) return;
            if (has_input_birthday == true) return;
            if (e.KeyPressed.ToString() == key_binding) //if the key is pressed, load my cusom save function
            {
                if (Game1.activeClickableMenu != null) return;
                 Game1.activeClickableMenu = new StardewValley.Menus.Birthday_Menu();
            }
            //DataLoader_Settings(); //update the key if players changed it while playing.
        }

        public void PlayerEvents_LoadedGame(object sender, StardewModdingAPI.Events.EventArgsLoadedGameChanged e)
        {
            game_loaded = true;
            DataLoader_Birthday();
            DataLoader_Settings();
        }

        public void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            if (Game1.player == null) return;
            if (game_loaded == false) return;
            if (Game1.player.isMoving()==true && once == false)
            {
                Log.AsyncM("Is it my birthday? "+isplayersbirthday());
                if (isplayersbirthday() == true)
                {
                    foreach (var location in Game1.locations)
                    {
                        foreach (NPC npc in location.characters)
                        {
                            try
                            {
                                if (npc is StardewValley.Characters.Cat || npc is StardewValley.Characters.Child || npc is StardewValley.Characters.Dog || npc is StardewValley.Characters.Horse || npc is StardewValley.Characters.Junimo || npc is StardewValley.Characters.Pet) continue;
                                if (npc is StardewValley.Monsters.Bat || npc is StardewValley.Monsters.BigSlime || npc is StardewValley.Monsters.Bug || npc is StardewValley.Monsters.Cat || npc is StardewValley.Monsters.Crow || npc is StardewValley.Monsters.Duggy || npc is StardewValley.Monsters.DustSpirit || npc is StardewValley.Monsters.Fireball || npc is StardewValley.Monsters.Fly || npc is StardewValley.Monsters.Ghost || npc is StardewValley.Monsters.GoblinPeasant || npc is StardewValley.Monsters.GoblinWizard || npc is StardewValley.Monsters.GreenSlime || npc is StardewValley.Monsters.Grub || npc is StardewValley.Monsters.LavaCrab || npc is StardewValley.Monsters.MetalHead || npc is StardewValley.Monsters.Monster || npc is StardewValley.Monsters.Mummy || npc is StardewValley.Monsters.RockCrab || npc is StardewValley.Monsters.RockGolem || npc is StardewValley.Monsters.Serpent || npc is StardewValley.Monsters.ShadowBrute || npc is StardewValley.Monsters.ShadowGirl || npc is StardewValley.Monsters.ShadowGuy || npc is StardewValley.Monsters.ShadowShaman || npc is StardewValley.Monsters.Skeleton || npc is StardewValley.Monsters.SkeletonMage || npc is StardewValley.Monsters.SkeletonWarrior || npc is StardewValley.Monsters.Spiker || npc is StardewValley.Monsters.SquidKid) continue;
                                npc.CurrentDialogue.Push(new Dialogue(Game1.content.Load<Dictionary<string, string>>("Data\\FarmerBirthdayDialogue")[npc.name], npc));
                            }
                            // npc.setNewDialogue(Game1.content.Load<Dictionary<string, string>>("Data\\FarmerBirthdayDialogue")[npc.name], true, false);
                            catch
                            {
                                if (npc is StardewValley.Characters.Cat || npc is StardewValley.Characters.Child || npc is StardewValley.Characters.Dog || npc is StardewValley.Characters.Horse || npc is StardewValley.Characters.Junimo || npc is StardewValley.Characters.Pet) continue;
                                if (npc is StardewValley.Monsters.Bat || npc is StardewValley.Monsters.BigSlime || npc is StardewValley.Monsters.Bug || npc is StardewValley.Monsters.Cat || npc is StardewValley.Monsters.Crow || npc is StardewValley.Monsters.Duggy || npc is StardewValley.Monsters.DustSpirit || npc is StardewValley.Monsters.Fireball || npc is StardewValley.Monsters.Fly || npc is StardewValley.Monsters.Ghost || npc is StardewValley.Monsters.GoblinPeasant || npc is StardewValley.Monsters.GoblinWizard || npc is StardewValley.Monsters.GreenSlime || npc is StardewValley.Monsters.Grub || npc is StardewValley.Monsters.LavaCrab || npc is StardewValley.Monsters.MetalHead || npc is StardewValley.Monsters.Monster || npc is StardewValley.Monsters.Mummy || npc is StardewValley.Monsters.RockCrab || npc is StardewValley.Monsters.RockGolem || npc is StardewValley.Monsters.Serpent || npc is StardewValley.Monsters.ShadowBrute || npc is StardewValley.Monsters.ShadowGirl || npc is StardewValley.Monsters.ShadowGuy || npc is StardewValley.Monsters.ShadowShaman || npc is StardewValley.Monsters.Skeleton || npc is StardewValley.Monsters.SkeletonMage || npc is StardewValley.Monsters.SkeletonWarrior || npc is StardewValley.Monsters.Spiker || npc is StardewValley.Monsters.SquidKid) continue;
                                // npc.setNewDialogue("Happy birthday @!", true, false);
                                npc.CurrentDialogue.Push(new Dialogue("Happy Birthday @!", npc));
                            }
                        }
                    }
                    //end birthday check
                }
                once = true;
                if (player_birthday_season == "" || player_birthday_season == null || player_birthday_date == 0)
                {
                    Game1.activeClickableMenu = new StardewValley.Menus.Birthday_Menu();
                    once = false;
                }
                
                
              }
            if (Game1.currentSpeaker != null)
            {
                if (isplayersbirthday()==true)
                {

                    try
                    {
                        //Game1.currentSpeaker.setNewDialogue(Game1.content.Load<Dictionary<string, string>>("Data\\FarmerBirthdayDialogue")[Game1.currentSpeaker.name], true, false);
                        foreach (var ehh in npc_name_list)
                        {
                            if (ehh == Game1.currentSpeaker.name)
                            {
                                birthday_gift();
                                npc_name_list.Remove(Game1.currentSpeaker.name);
                            }
                        }
                    }
                    catch
                    {
                       // Game1.currentSpeaker.setNewDialogue("Happy birthday @!", true, false);
                        foreach (var ehh in npc_name_list)
                        {
                            if (ehh == Game1.currentSpeaker.name)
                            {
                                birthday_gift();
                                npc_name_list.Remove(Game1.currentSpeaker.name);
                            }
                        }
                    }
                    

                }
            }
            if (birthday_gift_to_receive != null && Game1.currentSpeaker==null)
            {
                Game1.player.addItemByMenuIfNecessaryElseHoldUp(birthday_gift_to_receive);
                birthday_gift_to_receive = null;
            }

            if (player_birthday_season != "" && player_birthday_season != null && player_birthday_date != 0)
            {
                if (has_input_birthday == false)
                {
                    MyWritter_Settings();
                    MyWritter_Birthday();
                    has_input_birthday = true;
                }
            }
        }

        public void Day_Update(object sender, StardewModdingAPI.Events.EventArgsIntChanged e)
        {
           // foreach (var bleh in npc_name_list) npc_name_list.Remove(bleh);
            foreach (var location in Game1.locations)
            {
                foreach (NPC npc in location.characters)
                {
                    if (npc is StardewValley.Characters.Cat || npc is StardewValley.Characters.Child || npc is StardewValley.Characters.Dog || npc is StardewValley.Characters.Horse || npc is StardewValley.Characters.Junimo || npc is StardewValley.Characters.Pet) return;
                    if (npc is StardewValley.Monsters.Bat || npc is StardewValley.Monsters.BigSlime || npc is StardewValley.Monsters.Bug || npc is StardewValley.Monsters.Cat || npc is StardewValley.Monsters.Crow || npc is StardewValley.Monsters.Duggy || npc is StardewValley.Monsters.DustSpirit || npc is StardewValley.Monsters.Fireball || npc is StardewValley.Monsters.Fly || npc is StardewValley.Monsters.Ghost || npc is StardewValley.Monsters.GoblinPeasant || npc is StardewValley.Monsters.GoblinWizard || npc is StardewValley.Monsters.GreenSlime || npc is StardewValley.Monsters.Grub || npc is StardewValley.Monsters.LavaCrab || npc is StardewValley.Monsters.MetalHead || npc is StardewValley.Monsters.Monster || npc is StardewValley.Monsters.Mummy || npc is StardewValley.Monsters.RockCrab || npc is StardewValley.Monsters.RockGolem || npc is StardewValley.Monsters.Serpent || npc is StardewValley.Monsters.ShadowBrute || npc is StardewValley.Monsters.ShadowGirl || npc is StardewValley.Monsters.ShadowGuy || npc is StardewValley.Monsters.ShadowShaman || npc is StardewValley.Monsters.Skeleton || npc is StardewValley.Monsters.SkeletonMage || npc is StardewValley.Monsters.SkeletonWarrior || npc is StardewValley.Monsters.Spiker || npc is StardewValley.Monsters.SquidKid) return;
                    if (npc_name_list.Contains(npc.name)) continue;
                    npc_name_list.Add(npc.name);
                    //Log.AsyncM(npc.name);



                    }
                
                
            }
            if (isplayersbirthday() == true)
            {
               // Game1.mailbox.Enqueue("\n        Dear @,^  Happy birthday sweetheart. It's been amazing watching you grow into the kind, hard working person that I've always dreamed that you would become. I hope you continue to make many more fond memories with the ones you love. ^  Love, Mom ^ P.S. Here's a little something that I made for you. %item object 221 1 %");
               // Game1.mailbox.Enqueue("\n        Dear @,^  Happy birthday kiddo. It's been a little quiet around here on your birthday since you aren't around, but your mother and I know that you are making both your grandpa and us proud.  We both know that living on your own can be tough but we believe in you one hundred percent, just keep following your dreams.^  Love, Dad ^ P.S. Here's some spending money to help you out on the farm. Good luck! %item money 5000 5001 %");
            }

            if (Game1.player == null) return;
            if(has_input_birthday == true) MyWritter_Birthday();
            MyWritter_Settings();
            once = false;
        }

        public virtual void birthday_gift()
        {

            //grab 0~3 hearts  //Neutral
            //grab 4~6       //Good
            //grab 7~10       //Best
            Item farmers_birthday_gift;
            if (this.possible_birthday_gifts.Count > 0)
            {
                Random rnd = new Random();
                int r = rnd.Next(this.possible_birthday_gifts.Count);
                farmers_birthday_gift = this.possible_birthday_gifts.ElementAt(r);
                if (Game1.player.isInventoryFull() == true)
                {
                    Game1.createItemDebris(farmers_birthday_gift, Game1.player.getStandingPosition(), Game1.player.getDirection());
                }
                else {
                    birthday_gift_to_receive = farmers_birthday_gift;
                }
                return;
            }

            this.get_default_birthday_gifts();

            Random rnd2 = new Random();
            int r2 = rnd2.Next(this.possible_birthday_gifts.Count);
            farmers_birthday_gift = this.possible_birthday_gifts.ElementAt(r2);
            if (Game1.player.isInventoryFull() == true)
            {
                Game1.createItemDebris(farmers_birthday_gift, Game1.player.getStandingPosition(), Game1.player.getDirection());
            }
            else {
                birthday_gift_to_receive = farmers_birthday_gift;
                //Game1.player.addItemByMenuIfNecessaryElseHoldUp(farmers_birthday_gift);
            }
            this.possible_birthday_gifts.Clear();
            Log.AsyncO("IS THIS EVER WORKING????");
            return;
        }

        public virtual void get_default_birthday_gifts()
        {
            Dictionary<string, string> dictionary = null;
            try
            {
                dictionary = Game1.content.Load<Dictionary<string, string>>("Data\\PossibleBirthdayGifts");


                string text;
                dictionary.TryGetValue(Game1.currentSpeaker.name, out text);
                if (text != null)
                {
                    string[] array = text.Split(new char[]
                    {
                    '/'
                    });
                    //love
                    if (Game1.player.getFriendshipHeartLevelForNPC(Game1.currentSpeaker.name) >= 7)
                    {

                        string[] array2 = array[1].Split(new char[]
                        {
                    ' '
                        });
                        for (int i = 0; i < array2.Count<string>(); i += 2)
                        {
                            try
                            {
                                if (Convert.ToInt32(array2[i]) > 0) this.possible_birthday_gifts.Add((Item)new StardewValley.Object(Convert.ToInt32(array2[i]), Convert.ToInt32(array2[i + 1]), false, -1, 0));
                                else
                                {
                                    List<StardewValley.Object> some_object_list = StardewValley.PatchedUtilities.ObjectUtility.getAllObjectsAssociatedWithCategory(Convert.ToInt32(array2[i]));
                                    foreach (var obj in some_object_list)
                                    {
                                        StardewValley.Object new_obj = new StardewValley.Object(obj.parentSheetIndex, Convert.ToInt32(array2[i + 1]), false, -1, 0);
                                        this.possible_birthday_gifts.Add((Item)new_obj);
                                    }
                                }
                                // this.itemsRequired.Add(Convert.ToInt32(array2[i]), Convert.ToInt32(array2[i + 1]));
                            }
                            catch
                            {

                            }
                        }
                    }
                    //Like
                    if (Game1.player.getFriendshipHeartLevelForNPC(Game1.currentSpeaker.name) >= 4 && Game1.player.getFriendshipHeartLevelForNPC(Game1.currentSpeaker.name) <= 6)
                    {

                        string[] array3 = array[3].Split(new char[]
                    {
                    ' '
                    });
                        for (int i = 0; i < array3.Count<string>(); i += 2)
                        {
                            try
                            {


                                if (Convert.ToInt32(array3[i]) > 0) this.possible_birthday_gifts.Add((Item)new StardewValley.Object(Convert.ToInt32(array3[i]), Convert.ToInt32(array3[i + 1]), false, -1, 0));
                                else
                                {
                                    List<StardewValley.Object> some_object_list = StardewValley.PatchedUtilities.ObjectUtility.getAllObjectsAssociatedWithCategory(Convert.ToInt32(array3[i]));
                                    foreach (var obj in some_object_list)
                                    {
                                        StardewValley.Object new_obj = new StardewValley.Object(obj.parentSheetIndex, Convert.ToInt32(array3[i + 1]), false, -1, 0);
                                        this.possible_birthday_gifts.Add((Item)new_obj);
                                    }
                                }
                                // this.itemsRequired.Add(Convert.ToInt32(array2[i]), Convert.ToInt32(array2[i + 1]));
                            }
                            catch
                            {

                            }
                        }
                    }
                    //Neutral
                    if (Game1.player.getFriendshipHeartLevelForNPC(Game1.currentSpeaker.name) >= 0 && Game1.player.getFriendshipHeartLevelForNPC(Game1.currentSpeaker.name) <= 3)
                    {

                        string[] array4 = array[5].Split(new char[]
                   {
                    ' '
                   });

                        for (int i = 0; i < array4.Count<string>(); i += 2)
                        {
                            try
                            {
                                if (Convert.ToInt32(array4[i]) > 0) this.possible_birthday_gifts.Add((Item)new StardewValley.Object(Convert.ToInt32(array4[i]), Convert.ToInt32(array4[i + 1]), false, -1, 0));
                                else
                                {
                                    List<StardewValley.Object> some_object_list = StardewValley.PatchedUtilities.ObjectUtility.getAllObjectsAssociatedWithCategory(Convert.ToInt32(array4[i]));
                                    foreach (var obj in some_object_list)
                                    {
                                        StardewValley.Object new_obj = new StardewValley.Object(obj.parentSheetIndex, Convert.ToInt32(array4[i + 1]), false, -1, 0);
                                        this.possible_birthday_gifts.Add((Item)new_obj);
                                    }
                                }
                                // this.itemsRequired.Add(Convert.ToInt32(array2[i]), Convert.ToInt32(array2[i + 1]));
                            }
                            catch
                            {

                            }
                        }
                    }
                } //text !=null
                //grabs from //Data//PossibleBirthdayGifts
                if (Game1.player.getFriendshipHeartLevelForNPC(Game1.currentSpeaker.name) >= 7) getAllUniversalLovedItems(true);
                if (Game1.player.getFriendshipHeartLevelForNPC(Game1.currentSpeaker.name) >= 4 && Game1.player.getFriendshipHeartLevelForNPC(Game1.currentSpeaker.name) <= 6) getAllUniversalLikedItems(true);
                if (Game1.player.getFriendshipHeartLevelForNPC(Game1.currentSpeaker.name) >= 0 && Game1.player.getFriendshipHeartLevelForNPC(Game1.currentSpeaker.name) <= 3) getAllUniversalNeutralItems(true);
                return;


            }
            catch
            {
                //grabs from NPCGiftTastes               
                if (Game1.player.getFriendshipHeartLevelForNPC(Game1.currentSpeaker.name) >= 7)
                {
                    getAllUniversalLovedItems(false);
                    getAllSpecifiedLovedItems();
                }
                if (Game1.player.getFriendshipHeartLevelForNPC(Game1.currentSpeaker.name) >= 4 && Game1.player.getFriendshipHeartLevelForNPC(Game1.currentSpeaker.name) <= 6)
                {
                    getAllSpecifiedLikedItems();
                    getAllUniversalLikedItems(false);
                }
                if (Game1.player.getFriendshipHeartLevelForNPC(Game1.currentSpeaker.name) >= 0 && Game1.player.getFriendshipHeartLevelForNPC(Game1.currentSpeaker.name) <= 3)
                {
                    getAllUniversalNeutralItems(false);
                }
                return;
            }


            //TODO: Make different tiers of gifts depending on the friendship, and if it is the spouse.
            /*
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(198, 1));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(204, 1));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(220, 1));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(221, 1));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(223, 1));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(233, 1));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(234, 1));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(286, 5));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(368, 5));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(608, 1));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(612, 1));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(773, 1));
                */

        }
        public virtual void getAllUniversalNeutralItems(bool is_birthday_gift_list)
        {
            string text;
            if (is_birthday_gift_list == false)
            {
                Game1.NPCGiftTastes.TryGetValue("Universal_Neutral", out text);
                if (text != null)
                {

                    string[] array = text.Split(new char[]
                    {
                    ' '
                    });


                    for (int i = 0; i < array.Count<string>(); i++)
                    {
                        int parentSheetIndex = Convert.ToInt32(array[i]);
                        if (parentSheetIndex < 0)
                        {
                            List<StardewValley.Object> some_object_list = StardewValley.PatchedUtilities.ObjectUtility.getAllObjectsAssociatedWithCategory(Convert.ToInt32(array[i]));
                            foreach (var obj in some_object_list)
                            {
                                this.possible_birthday_gifts.Add((Item)obj);
                            }
                            continue;
                        }
                        else
                        {
                            this.possible_birthday_gifts.Add((Item)new StardewValley.Object(parentSheetIndex, 1, false, -1, 0));
                        }
                        //this.itemsRequired.Add(Convert.ToInt32(array[i]), Convert.ToInt32(array[i + 1]));
                    }


                    return;// new SytardewValley.Object(parentSheetIndex, 1, false, -1, 0);
                }
            }
            else
            {
                Dictionary<string, string> dictionary = Game1.content.Load<Dictionary<string, string>>("Data\\PossibleBirthdayGifts");
                string text2;
                dictionary.TryGetValue("Universal_Neutral_Gift", out text2);
                string[] array = text2.Split(new char[]
                                    {
                    ' '
                                    });


                for (int i = 0; i < array.Count<string>(); i += 2)
                {
                    int parentSheetIndex = Convert.ToInt32(array[i]);
                    if (parentSheetIndex < 0)
                    {
                        List<StardewValley.Object> some_object_list = StardewValley.PatchedUtilities.ObjectUtility.getAllObjectsAssociatedWithCategory(Convert.ToInt32(array[i]));
                        foreach (var obj in some_object_list)
                        {
                            StardewValley.Object new_obj = new StardewValley.Object(obj.parentSheetIndex, Convert.ToInt32(array[i + 1]), false, -1, 0);
                            this.possible_birthday_gifts.Add((Item)new_obj);
                        }
                        continue;
                    }
                    else
                    {
                        this.possible_birthday_gifts.Add((Item)new StardewValley.Object(parentSheetIndex, Convert.ToInt32(array[i + 1]), false, -1, 0));
                    }
                    //this.itemsRequired.Add(Convert.ToInt32(array[i]), Convert.ToInt32(array[i + 1]));
                }


                return;// new StardewValley.Object(parentSheetIndex, 1, false, -1, 0);
            }
            return;// null;
        }
        public virtual void getAllUniversalLikedItems(bool is_birthday_gift_list)
        {
            string text;
            if (is_birthday_gift_list == false)
            {
                Game1.NPCGiftTastes.TryGetValue("Universal_Like", out text);
                if (text != null)
                {

                    string[] array = text.Split(new char[]
                    {
                    ' '
                    });


                    for (int i = 0; i < array.Count<string>(); i++)
                    {
                        int parentSheetIndex = Convert.ToInt32(array[i]);
                        if (parentSheetIndex < 0)
                        {
                            List<StardewValley.Object> some_object_list = StardewValley.PatchedUtilities.ObjectUtility.getAllObjectsAssociatedWithCategory(Convert.ToInt32(array[i]));
                            foreach (var obj in some_object_list)
                            {
                                this.possible_birthday_gifts.Add((Item)obj);
                            }
                            continue;
                        }
                        else
                        {
                            this.possible_birthday_gifts.Add((Item)new StardewValley.Object(parentSheetIndex, 1, false, -1, 0));
                        }
                        //this.itemsRequired.Add(Convert.ToInt32(array[i]), Convert.ToInt32(array[i + 1]));
                    }


                    return;// new StardewValley.Object(parentSheetIndex, 1, false, -1, 0);
                }
            }
            else
            {
                Dictionary<string, string> dictionary = Game1.content.Load<Dictionary<string, string>>("Data\\PossibleBirthdayGifts");
                string text2;
                dictionary.TryGetValue("Universal_Like_Gift", out text2);
                string[] array = text2.Split(new char[]
                                    {
                    ' '
                                    });


                for (int i = 0; i < array.Count<string>(); i += 2)
                {
                    int parentSheetIndex = Convert.ToInt32(array[i]);
                    if (parentSheetIndex < 0)
                    {
                        List<StardewValley.Object> some_object_list = StardewValley.PatchedUtilities.ObjectUtility.getAllObjectsAssociatedWithCategory(Convert.ToInt32(array[i]));
                        foreach (var obj in some_object_list)
                        {
                            StardewValley.Object new_obj = new StardewValley.Object(obj.parentSheetIndex, Convert.ToInt32(array[i + 1]), false, -1, 0);
                            this.possible_birthday_gifts.Add((Item)new_obj);
                        }
                        continue;
                    }
                    else
                    {
                        this.possible_birthday_gifts.Add((Item)new StardewValley.Object(parentSheetIndex, Convert.ToInt32(array[i + 1]), false, -1, 0));
                    }
                    //this.itemsRequired.Add(Convert.ToInt32(array[i]), Convert.ToInt32(array[i + 1]));
                }


                return;// new StardewValley.Object(parentSheetIndex, 1, false, -1, 0);
            }
            return;// null;
        }
        public virtual void getAllUniversalLovedItems(bool is_birthday_gift_list)
        {
            string text;
            if (is_birthday_gift_list == false)
            {
                Game1.NPCGiftTastes.TryGetValue("Universal_Neutral", out text);
                if (text != null)
                {

                    string[] array = text.Split(new char[]
                    {
                    ' '
                    });


                    for (int i = 0; i < array.Count<string>(); i++)
                    {
                        int parentSheetIndex = Convert.ToInt32(array[i]);
                        if (parentSheetIndex < 0)
                        {
                            List<StardewValley.Object> some_object_list = StardewValley.PatchedUtilities.ObjectUtility.getAllObjectsAssociatedWithCategory(Convert.ToInt32(array[i]));
                            foreach (var obj in some_object_list)
                            {
                                this.possible_birthday_gifts.Add((Item)obj);
                            }
                            continue;
                        }
                        else
                        {
                            this.possible_birthday_gifts.Add((Item)new StardewValley.Object(parentSheetIndex, 1, false, -1, 0));
                        }
                        //this.itemsRequired.Add(Convert.ToInt32(array[i]), Convert.ToInt32(array[i + 1]));
                    }


                    return;// new StardewValley.Object(parentSheetIndex, 1, false, -1, 0);
                }
            }
            else
            {
                Dictionary<string, string> dictionary = Game1.content.Load<Dictionary<string, string>>("Data\\PossibleBirthdayGifts");
                string text2;
                dictionary.TryGetValue("Universal_Love_Gift", out text2);
                string[] array = text2.Split(new char[]
                                    {
                    ' '
                                    });


                for (int i = 0; i < array.Count<string>(); i += 2)
                {
                    int parentSheetIndex = Convert.ToInt32(array[i]);
                    if (parentSheetIndex < 0)
                    {
                        List<StardewValley.Object> some_object_list = StardewValley.PatchedUtilities.ObjectUtility.getAllObjectsAssociatedWithCategory(Convert.ToInt32(array[i]));
                        foreach (var obj in some_object_list)
                        {
                            StardewValley.Object new_obj = new StardewValley.Object(obj.parentSheetIndex, Convert.ToInt32(array[i + 1]), false, -1, 0);
                            this.possible_birthday_gifts.Add((Item)new_obj);
                        }
                        continue;
                    }
                    else
                    {
                        this.possible_birthday_gifts.Add((Item)new StardewValley.Object(parentSheetIndex, Convert.ToInt32(array[i + 1]), false, -1, 0));
                    }
                    //this.itemsRequired.Add(Convert.ToInt32(array[i]), Convert.ToInt32(array[i + 1]));
                }


                return;// new StardewValley.Object(parentSheetIndex, 1, false, -1, 0);
            }
            return;// null;
        }


        public virtual void getAllSpecifiedLikedItems()
        {
            string text;
            Game1.NPCGiftTastes.TryGetValue(Game1.currentSpeaker.name, out text);
            if (text != null)
            {

                string[] array = text.Split(new char[]
                {
                    '/'
                });

                string[] array2 = array[3].Split(new char[]
                {
                    ' '
                });

                for (int i = 0; i < array2.Count<string>(); i++)
                {
                    int parentSheetIndex = Convert.ToInt32(array2[i]);
                    if (parentSheetIndex < 0)
                    {
                        List<StardewValley.Object> some_object_list = StardewValley.PatchedUtilities.ObjectUtility.getAllObjectsAssociatedWithCategory(Convert.ToInt32(array2[i]));
                        foreach (var obj in some_object_list)
                        {
                            this.possible_birthday_gifts.Add((Item)obj);
                        }
                        continue;
                    }
                    else
                    {
                        this.possible_birthday_gifts.Add((Item)new StardewValley.Object(parentSheetIndex, 1, false, -1, 0));
                    }
                    //this.itemsRequired.Add(Convert.ToInt32(array[i]), Convert.ToInt32(array[i + 1]));
                }


                return;// new Object(parentSheetIndex, 1, false, -1, 0);
            }
            return;// null;
        }
        public virtual void getAllSpecifiedLovedItems()
        {
            string text;
            Game1.NPCGiftTastes.TryGetValue(Game1.currentSpeaker.name, out text);
            if (text != null)
            {

                string[] array = text.Split(new char[]
                {
                    '/'
                });

                string[] array2 = array[1].Split(new char[]
                {
                    ' '
                });

                for (int i = 0; i < array2.Count<string>(); i++)
                {
                    int parentSheetIndex = Convert.ToInt32(array2[i]);
                    if (parentSheetIndex < 0)
                    {
                        List<StardewValley.Object> some_object_list = StardewValley.PatchedUtilities.ObjectUtility.getAllObjectsAssociatedWithCategory(Convert.ToInt32(array2[i]));
                        foreach (var obj in some_object_list)
                        {
                            this.possible_birthday_gifts.Add((Item)obj);
                        }
                        continue;
                    }
                    else
                    {
                        this.possible_birthday_gifts.Add((Item)new StardewValley.Object(parentSheetIndex, 1, false, -1, 0));
                    }
                    //this.itemsRequired.Add(Convert.ToInt32(array[i]), Convert.ToInt32(array[i + 1]));
                }


                return;// new StardewValley.Object(parentSheetIndex, 1, false, -1, 0);
            }
            return;// null;
        }

        public virtual bool isplayersbirthday()
        {
            if (player_birthday_date.Equals(Game1.dayOfMonth) && player_birthday_season.Equals(Game1.currentSeason)) return true;
            else return false;
        }
        
        void DataLoader_Settings()
        {
            //loads the data to the variables upon loading the game.
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(PathOnDisk, "HappyBirthday_Config");
            string mylocation2 = mylocation;
            string mylocation3 = mylocation2 + ".txt";
            if (!File.Exists(mylocation3)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
                //  Console.WriteLine("Can't load custom save info since the file doesn't exist.");

                key_binding = "O";
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

            string mylocation = Path.Combine(PathOnDisk, "HappyBirthday_Config");
            string mylocation2 = mylocation;
            string mylocation3 = mylocation2 + ".txt";

            string[] mystring3 = new string[20];
            if (!File.Exists(mylocation3))
            {
                Log.Info("HappyBirthday: The HappyBirthday Config doesn't exist. Creating it now.");

                mystring3[0] = "Config: HappyBirthday Info. Feel free to mess with these settings.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Key binding for opening the birthday menu. Press this key to do so.";
                mystring3[3] = key_binding.ToString();



                File.WriteAllLines(mylocation3, mystring3);

            }

            else
            {

                //write out the info to a text file at the end of a day. This will run if it doesnt exist.

                mystring3[0] = "Config: HappyBirthday Info. Feel free to mess with these settings.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Key binding for opening the birthday menu. Press this key to do so.";
                mystring3[3] = key_binding.ToString();

                File.WriteAllLines(mylocation3, mystring3);
            }
        }



        void DataLoader_Birthday()
        {
            //loads the data to the variables upon loading the game.
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(birthdays_path, "HappyBirthday_");
            string mylocation2 = mylocation+myname;
            string mylocation3 = mylocation2 + ".txt";
            if (!File.Exists(mylocation3)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
                //  Console.WriteLine("Can't load custom save info since the file doesn't exist.");

                
                //  Log.Info("KEY TIME");
            }

            else
            {
                //        Console.WriteLine("HEY THERE IM LOADING DATA");
                string[] readtext = File.ReadAllLines(mylocation3);
                player_birthday_season = Convert.ToString(readtext[3]);
                player_birthday_date = Convert.ToInt32(readtext[5]);

                // Log.Info(key_binding);
                // Log.Info(Convert.ToString(readtext[3]));

            }
        }

        void MyWritter_Birthday()
        {
         
            //write all of my info to a text file.
            string myname = StardewValley.Game1.player.name;
            
            string mylocation = Path.Combine(birthdays_path, "HappyBirthday_");
            string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation2 + ".txt";
           
            string[] mystring3 = new string[20];
            if (!File.Exists(mylocation3))
            {
                Log.Info("HappyBirthday: The HappyBirthday Player Info doesn't exist. Creating it now.");

                mystring3[0] = "Player Info: Modifying these values could be considered cheating or an exploit. Edit at your own risk.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Player's Birthday Season";
                mystring3[3] = player_birthday_season;
                mystring3[4] = "Player's Birthday Date";
                mystring3[5] = player_birthday_date.ToString();

                File.WriteAllLines(mylocation3, mystring3);

            }

            else
            {

                //write out the info to a text file at the end of a day. This will run if it doesnt exist.

                mystring3[0] = "Player Info: Modifying these values could be considered cheating or an exploit. Edit at your own risk.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Player's Birthday Season";
                mystring3[3] = player_birthday_season.ToString();
                mystring3[4] = "Player's Birthday Date";
                mystring3[5] = player_birthday_date.ToString();

                File.WriteAllLines(mylocation3, mystring3);
            }
        }
        

    }
}
