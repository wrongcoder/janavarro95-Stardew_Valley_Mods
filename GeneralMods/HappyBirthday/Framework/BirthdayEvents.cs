using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.HappyBirthday.Framework.EventPreconditions;
using StardustCore.Events;
using StardustCore.Events.Preconditions;
using StardustCore.Events.Preconditions.TimeSpecific;
using StardewValley;
using Microsoft.Xna.Framework;
using StardustCore.Events.Preconditions.PlayerSpecific;

namespace Omegasis.HappyBirthday.Framework
{
    public class BirthdayEvents
    {

        /// <summary>
        /// Creates the junimo birthday party event.
        /// </summary>
        /// <returns></returns>
        public static EventHelper CommunityCenterJunimoBirthday()
        {
            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new FarmerBirthdayPrecondition());
            conditions.Add(new LocationPrecondition(Game1.getLocationFromName("CommunityCenter")));
            conditions.Add(new TimePrecondition(600, 2600));
            conditions.Add(new CanReadJunimo());
            conditions.Add(new StardustCore.Events.Preconditions.PlayerSpecific.JojaMember(false));
            conditions.Add(new CommunityCenterCompleted(false));
            //conditions.Add(new HasUnlockedCommunityCenter()); //Infered by the fact that you must enter the community center to trigger this event anyways.
            EventHelper e = new EventHelper("CommunityCenterBirthday", 19950, conditions, new EventStartData("playful", 32, 12, new EventStartData.FarmerData(32, 22, EventHelper.FacingDirection.Up), new List<EventStartData.NPCData>()));

            e.AddInJunimoActor("Juni", new Microsoft.Xna.Framework.Vector2(32, 10), StardustCore.IlluminateFramework.Colors.getRandomJunimoColor());
            e.AddInJunimoActor("Juni2", new Microsoft.Xna.Framework.Vector2(30, 11), StardustCore.IlluminateFramework.Colors.getRandomJunimoColor());
            e.AddInJunimoActor("Juni3", new Microsoft.Xna.Framework.Vector2(34, 11), StardustCore.IlluminateFramework.Colors.getRandomJunimoColor());
            e.AddInJunimoActor("Juni4", new Microsoft.Xna.Framework.Vector2(26, 11), StardustCore.IlluminateFramework.Colors.getRandomJunimoColor());
            e.AddInJunimoActor("Juni5", new Microsoft.Xna.Framework.Vector2(28, 11), StardustCore.IlluminateFramework.Colors.getRandomJunimoColor());
            e.AddInJunimoActor("Juni6Tank", new Vector2(38, 10), StardustCore.IlluminateFramework.Colors.getRandomJunimoColor());
            e.AddInJunimoActor("Juni7", new Vector2(27, 16), StardustCore.IlluminateFramework.Colors.getRandomJunimoColor());
            e.AddInJunimoActor("Juni8", new Vector2(40, 15), StardustCore.IlluminateFramework.Colors.getRandomJunimoColor());
            e.AddJunimoAdvanceMoveTiles(new StardustCore.Utilities.JunimoAdvanceMoveData("Juni6Tank", new List<Point>()
            {
                new Point(38,10),
                new Point(38,11),
                new Point(39,11),
                new Point(40,11),
                new Point(41,11),
                new Point(42,11),
                new Point(42,10),
                new Point(41,10),
                new Point(40,10),
                new Point(39,10),

            }, 60, 1, true)); ;

            e.FlipJunimoActor("Juni5", true);
            e.junimoFaceDirection("Juni4", EventHelper.FacingDirection.Right); //Make a junimo face right.
            e.junimoFaceDirection("Juni5", EventHelper.FacingDirection.Left);
            e.junimoFaceDirection("Juni7", EventHelper.FacingDirection.Down);
            e.animate("Juni", true, true, 250, new List<int>()
            {
                28,
                29,
                30,
                31
            });
            e.animate("Juni7", false, true, 250, new List<int>()
            {
                44,45,46,47
            });
            e.animate("Juni8", false, true, 250, new List<int>()
            {
                12,13,14,15
            });

            e.globalFadeIn();

            e.moveFarmerUp(10, EventHelper.FacingDirection.Up, true);

            e.junimoFaceDirection("Juni4", EventHelper.FacingDirection.Down);
            e.junimoFaceDirection("Juni5", EventHelper.FacingDirection.Down);
            e.RemoveJunimoAdvanceMove("Juni6Tank");
            e.junimoFaceDirection("Juni6Tank", EventHelper.FacingDirection.Down);
            e.junimoFaceDirection("Juni7", EventHelper.FacingDirection.Right);
            e.FlipJunimoActor("Juni8", true);
            e.junimoFaceDirection("Juni8", EventHelper.FacingDirection.Left);

            e.playSound("junimoMeep1");

            e.emoteFarmer_ExclamationMark();
            e.showMessage(HappyBirthday.Config.translationInfo.getTranslatedString("Event:JunimoBirthdayParty_0"));
            e.emoteFarmer_Heart();
            e.globalFadeOut(0.010);
            e.setViewportPosition(-100, -100);
            e.showMessage(HappyBirthday.Config.translationInfo.getTranslatedString("Event:JunimoBirthdayParty_1"));
            e.showMessage(HappyBirthday.Config.translationInfo.getTranslatedString("Event:PartyOver"));
            e.addObjectToPlayersInventory(220, 1, false);

            e.end();

            return e;
        }


        /// <summary>
        /// Birthday event for when the player is dating Penny.
        /// Status: Completed.
        /// </summary>
        /// <returns></returns>
        public static EventHelper DatingBirthday_Penny()
        {

            NPC penny = Game1.getCharacterFromName("Penny");
            NPC pam = Game1.getCharacterFromName("Pam");

            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new FarmerBirthdayPrecondition());
            conditions.Add(new LocationPrecondition(Game1.getLocationFromName("Trailer")));
            conditions.Add(new TimePrecondition(600, 2600));
            conditions.Add(new StardustCore.Events.Preconditions.NPCSpecific.DatingNPC(penny));

            //conditions.Add(new StardustCore.Events.Preconditions.NPCSpecific.DatingNPC(Game1.getCharacterFromName("Penny"));
            EventHelper e = new EventHelper("BirthdayDating:Penny", 19951, conditions, new EventStartData("playful", 12, 8, new EventStartData.FarmerData(12, 9, EventHelper.FacingDirection.Up), new List<EventStartData.NPCData>() {
                new EventStartData.NPCData(penny,12,7, EventHelper.FacingDirection.Up),
                new EventStartData.NPCData(pam,15,4, EventHelper.FacingDirection.Down)
            }));

            e.globalFadeIn();

            e.moveFarmerUp(1, EventHelper.FacingDirection.Up, false);

            e.actorFaceDirection("Penny", EventHelper.FacingDirection.Down);
            //starting = starting.Replace("@", Game1.player.Name);
            e.speak(penny, HappyBirthday.Config.translationInfo.getTranslatedString("Event:DatingPennyBirthday_Penny:0"));
            e.speak(pam, HappyBirthday.Config.translationInfo.getTranslatedString("Event:DatingPennyBirthday_Pam:0"));
            e.speak(penny, HappyBirthday.Config.translationInfo.getTranslatedString("Event:DatingPennyBirthday_Penny:1"));
            e.speak(pam, HappyBirthday.Config.translationInfo.getTranslatedString("Event:DatingPennyBirthday_Pam:1"));
            e.emote_Angry("Penny");
            e.speak(penny, HappyBirthday.Config.translationInfo.getTranslatedString("Event:DatingPennyBirthday_Penny:2")); //penny2
            e.speak(penny, HappyBirthday.Config.translationInfo.getTranslatedString("Event:DatingPennyBirthday_Penny:3")); //penny3

            e.moveActorLeft("Penny", 3, EventHelper.FacingDirection.Up, true);
            e.moveFarmerRight(2, EventHelper.FacingDirection.Up, false);
            e.moveFarmerUp(3, EventHelper.FacingDirection.Down, false);
            e.moveActorRight("Penny", 5, EventHelper.FacingDirection.Up, true);
            e.moveActorUp("Penny", 1, EventHelper.FacingDirection.Up, true);
            e.speak(pam, HappyBirthday.Config.translationInfo.getTranslatedString("Event:DatingPennyBirthday_Pam:2")); //pam2
            e.speak(penny, HappyBirthday.Config.translationInfo.getTranslatedString("Event:DatingPennyBirthday_Penny:4"));//penny4

            e.emoteFarmer_Heart();
            e.emote_Heart("Penny");
            e.globalFadeOut(0.010);
            e.setViewportPosition(-100, -100);
            e.showMessage(HappyBirthday.Config.translationInfo.getTranslatedString("Event:DatingPennyBirthday_Finish:0")); //penny party finish 0
            e.showMessage(HappyBirthday.Config.translationInfo.getTranslatedString("Event:DatingPennyBirthday_Finish:1"));// penny party finish 1
            e.addObjectToPlayersInventory(220, 1, false);
            e.addObjectToPlayersInventory(346, 1, false);

            e.showMessage(HappyBirthday.Config.translationInfo.getTranslatedString("Event:PartyOver"));

            e.end();

            return e;
        }

        /// <summary>
        /// Birthday event for when the player is dating Maru.
        /// Finished.
        /// </summary>
        /// <returns></returns>
        public static EventHelper DatingBirthday_Maru()
        {
            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new FarmerBirthdayPrecondition());
            conditions.Add(new LocationPrecondition(Game1.getLocationFromName("ScienceHouse")));
            conditions.Add(new TimePrecondition(600, 2600));

            NPC maru = Game1.getCharacterFromName("Maru");
            NPC sebastian = Game1.getCharacterFromName("Sebastian");
            NPC robin = Game1.getCharacterFromName("Robin");
            NPC demetrius = Game1.getCharacterFromName("Demetrius");

            conditions.Add(new StardustCore.Events.Preconditions.NPCSpecific.DatingNPC(maru));

            EventHelper e = new EventHelper("BirthdayDating:Maru", 19952, conditions, new EventStartData("playful", 28, 12, new EventStartData.FarmerData(23, 12, EventHelper.FacingDirection.Right), new List<EventStartData.NPCData>() {
                new EventStartData.NPCData(maru,27,11, EventHelper.FacingDirection.Down),
                new EventStartData.NPCData(sebastian,26,13, EventHelper.FacingDirection.Up),
                new EventStartData.NPCData(robin,28,9, EventHelper.FacingDirection.Up),
                new EventStartData.NPCData(demetrius,30,11, EventHelper.FacingDirection.Left)
            }));
            e.globalFadeIn();

            e.moveFarmerRight(3, EventHelper.FacingDirection.Right, true);
            e.npcFaceDirection(maru, EventHelper.FacingDirection.Left);
            e.npcFaceDirection(demetrius, EventHelper.FacingDirection.Left);
            //Seb is already facing up.
            e.npcFaceDirection(robin, EventHelper.FacingDirection.Down);

            //Dialogue goes here.
            //Seriously improve dialogue lines. Maru is probably the NPC I know the least about.
            e.speak(maru, GetTranslatedString("Event:DatingMaruBirthday_Maru:0")); //maru 0
            e.speak(demetrius, GetTranslatedString("Event:DatingMaruBirthday_Demetrius:0")); //demetrius 0
            e.speak(maru, GetTranslatedString("Event:DatingMaruBirthday_Maru:1"));//Maru 1 //Spoiler she doesn't.
            e.speak(sebastian, GetTranslatedString("Event:DatingMaruBirthday_Sebastian:0")); //sebastian 0
            e.speak(robin, GetTranslatedString("Event:DatingMaruBirthday_Robin:0")); //robin 0
            e.speak(demetrius, GetTranslatedString("Event:DatingMaruBirthday_Demetrius:1")); //demetrius 1
            e.emote_ExclamationMark("Robin");
            e.npcFaceDirection(robin, EventHelper.FacingDirection.Up);
            e.speak(robin, GetTranslatedString("Event:DatingMaruBirthday_Robin:1")); //robin 1
            e.npcFaceDirection(robin, EventHelper.FacingDirection.Down);
            e.moveActorDown("Robin", 1, EventHelper.FacingDirection.Down, false);
            e.addObject(27, 12, 220);

            e.speak(maru, GetTranslatedString("Event:DatingMaruBirthday_Maru:2")); //maru 2
            e.emoteFarmer_Thinking();
            e.speak(sebastian, GetTranslatedString("Event:DatingMaruBirthday_Sebastian:1")); //Sebastian 1
            e.speak(maru, GetTranslatedString("Event:DatingMaruBirthday_Maru:3")); //maru 3

            //Event finish commands.
            e.emoteFarmer_Heart();
            e.emote_Heart("Maru");
            e.globalFadeOut(0.010);
            e.setViewportPosition(-100, -100);
            e.showMessage(HappyBirthday.Config.translationInfo.getTranslatedString("Event:DatingMaruBirthday_Finish:0")); //maru party finish 0
            e.showMessage(HappyBirthday.Config.translationInfo.getTranslatedString("Event:DatingMaruBirthday_Finish:1")); //maru party finish 0
            e.addObjectToPlayersInventory(220, 1, false);

            e.showMessage(HappyBirthday.Config.translationInfo.getTranslatedString("Event:PartyOver"));
            e.end();
            return e;
        }

        /// <summary>
        /// Birthday event for when the player is dating Leah.
        /// Finished.
        /// </summary>
        /// <returns></returns>
        public static EventHelper DatingBirthday_Leah()
        {
            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new FarmerBirthdayPrecondition());
            conditions.Add(new LocationPrecondition(Game1.getLocationFromName("LeahHouse")));
            conditions.Add(new TimePrecondition(600, 2600));

            NPC leah = Game1.getCharacterFromName("Leah");

            conditions.Add(new StardustCore.Events.Preconditions.NPCSpecific.DatingNPC(leah));

            EventHelper e = new EventHelper("BirthdayDating:Leah", 19953, conditions, new EventStartData("playful", 12, 7, new EventStartData.FarmerData(7, 9, EventHelper.FacingDirection.Up), new List<EventStartData.NPCData>() {
                new EventStartData.NPCData(leah,14,11, EventHelper.FacingDirection.Left),
            }));
            e.addObject(11, 11, 220);
            e.globalFadeIn();
            e.moveFarmerUp(2, EventHelper.FacingDirection.Up, false);
            e.moveFarmerRight(5, EventHelper.FacingDirection.Down, false);
            e.npcFaceDirection(leah, EventHelper.FacingDirection.Up);
            e.speak(leah, GetTranslatedString("Event:DatingLeahBirthday_Leah:0")); //0
            e.moveFarmerDown(2, EventHelper.FacingDirection.Down, false);
            e.moveFarmerRight(1, EventHelper.FacingDirection.Down, false);
            e.moveFarmerDown(1, EventHelper.FacingDirection.Down, false);
            e.speak(leah, GetTranslatedString("Event:DatingLeahBirthday_Leah:1")); //1
            e.emoteFarmer_Happy();
            e.speak(leah, GetTranslatedString("Event:DatingLeahBirthday_Leah:2"));//2
            e.speak(leah, GetTranslatedString("Event:DatingLeahBirthday_Leah:3"));//3
            e.speak(leah, GetTranslatedString("Event:DatingLeahBirthday_Leah:4"));//4


            e.emoteFarmer_Heart();
            e.emote_Heart("Leah");
            e.globalFadeOut(0.010);
            e.setViewportPosition(-100, -100);
            e.showMessage(HappyBirthday.Config.translationInfo.getTranslatedString("Event:DatingLeahBirthday_Finish:0")); //maru party finish 0
            e.showMessage(HappyBirthday.Config.translationInfo.getTranslatedString("Event:DatingLeahBirthday_Finish:1")); //maru party finish 0
            e.addObjectToPlayersInventory(220, 1, false);
            e.showMessage(HappyBirthday.Config.translationInfo.getTranslatedString("Event:PartyOver"));
            e.end();
            return e;
        }

        /// <summary>
        /// Birthday event for when the player is dating Abigail.
        /// Finished.
        /// </summary>
        /// <returns></returns>
        public static EventHelper DatingBirthday_Abigail()
        {
            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new FarmerBirthdayPrecondition());
            conditions.Add(new LocationPrecondition(Game1.getLocationFromName("SeedShop")));
            conditions.Add(new TimePrecondition(600, 2600));

            NPC abigail = Game1.getCharacterFromName("Abigail");
            NPC pierre = Game1.getCharacterFromName("Pierre");
            NPC caroline = Game1.getCharacterFromName("Caroline");

            conditions.Add(new StardustCore.Events.Preconditions.NPCSpecific.DatingNPC(abigail));

            EventHelper e = new EventHelper("BirthdayDating:Abigail", 19953, conditions, new EventStartData("playful", 35, 7, new EventStartData.FarmerData(31, 11, EventHelper.FacingDirection.Up), new List<EventStartData.NPCData>() {
                new EventStartData.NPCData(abigail,36,9, EventHelper.FacingDirection.Left),
                new EventStartData.NPCData(pierre,33,6, EventHelper.FacingDirection.Down),
                new EventStartData.NPCData(caroline,35,5, EventHelper.FacingDirection.Up),
            }));
            e.globalFadeIn();

            //Dialogue here.
            e.moveFarmerUp(2, EventHelper.FacingDirection.Right, false);
            e.moveFarmerRight(4, EventHelper.FacingDirection.Right, false);

            e.speak(abigail, GetTranslatedString("Event:DatingAbigailBirthday_Abigail:0")); //abi 0

            e.npcFaceDirection(caroline, EventHelper.FacingDirection.Down);

            e.speak(pierre, GetTranslatedString("Event:DatingAbigailBirthday_Pierre:0")); //pie 0
            e.speak(caroline, GetTranslatedString("Event:DatingAbigailBirthday_Caroline:0")); //car 0
            e.addObject(35, 5, 220);
            e.speak(abigail, GetTranslatedString("Event:DatingAbigailBirthday_Abigail:1")); //abi 1
            e.speak(pierre, GetTranslatedString("Event:DatingAbigailBirthday_Pierre:1")); //pie 1
            e.speak(caroline, GetTranslatedString("Event:DatingAbigailBirthday_Caroline:1")); //car 1
            e.speak(caroline, GetTranslatedString("Event:DatingAbigailBirthday_Caroline:2")); //car 2
            e.speak(abigail, GetTranslatedString("Event:DatingAbigailBirthday_Abigail:2")); //abi 2
            e.emoteFarmer_Thinking();
            e.speak(abigail, GetTranslatedString("Event:DatingAbigailBirthday_Abigail:3"));//abi 3
            e.speak(abigail, GetTranslatedString("Event:DatingAbigailBirthday_Abigail:4"));///abi 4

            e.emoteFarmer_Heart();
            e.emote_Heart("Abigail");
            e.globalFadeOut(0.010);
            e.setViewportPosition(-100, -100);
            e.showMessage(HappyBirthday.Config.translationInfo.getTranslatedString("Event:DatingAbigailBirthday_Finish:0")); //abi party finish 0
            e.showMessage(HappyBirthday.Config.translationInfo.getTranslatedString("Event:DatingAbigailBirthday_Finish:1")); //abi party finish 0
            e.addObjectToPlayersInventory(220, 1, false);
            e.showMessage(HappyBirthday.Config.translationInfo.getTranslatedString("Event:PartyOver"));
            e.end();
            return e;

        }


        
        public static EventHelper DatingBirthday_Emily()
        {
            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new FarmerBirthdayPrecondition());
            conditions.Add(new LocationPrecondition(Game1.getLocationFromName("HaleyHouse")));
            conditions.Add(new TimePrecondition(600, 2600));

            NPC emily = Game1.getCharacterFromName("Emily");

            conditions.Add(new StardustCore.Events.Preconditions.NPCSpecific.DatingNPC(emily));

            EventHelper e = new EventHelper("BirthdayDating:Emily", 19953, conditions, new EventStartData("playful", 20, 18, new EventStartData.FarmerData(11, 20, EventHelper.FacingDirection.Right), new List<EventStartData.NPCData>() {
                new EventStartData.NPCData(emily,20,17, EventHelper.FacingDirection.Down),
            }));
            e.globalFadeIn();

            //Dialogue here.
            e.moveFarmerRight(9, EventHelper.FacingDirection.Up, false);

            e.speak(emily, GetTranslatedString("Event:DatingEmilyBirthday_Emily:0")); //emi 0
            e.speak(emily, GetTranslatedString("Event:DatingEmilyBirthday_Emily:1")); //emi 0
            e.emoteFarmer_Happy();
            e.speak(emily, GetTranslatedString("Event:DatingEmilyBirthday_Emily:2")); //emi 0
            e.speak(emily, GetTranslatedString("Event:DatingEmilyBirthday_Emily:3")); //emi 0
            e.speak(emily, GetTranslatedString("Event:DatingEmilyBirthday_Emily:4")); //emi 0
            e.emoteFarmer_Thinking();
            e.speak(emily, GetTranslatedString("Event:DatingEmilyBirthday_Emily:5")); //emi 0


            e.emoteFarmer_Heart();
            e.emote_Heart("Emily");
            e.globalFadeOut(0.010);
            e.setViewportPosition(-100, -100);
            e.showMessage(HappyBirthday.Config.translationInfo.getTranslatedString("Event:DatingEmilyBirthday_Finish:0")); //abi party finish 0
            e.showMessage(HappyBirthday.Config.translationInfo.getTranslatedString("Event:DatingEmilyBirthday_Finish:1")); //abi party finish 0
            e.addObjectToPlayersInventory(220, 1, false);
            e.showMessage(HappyBirthday.Config.translationInfo.getTranslatedString("Event:PartyOver"));
            e.end();
            return e;
        }

        /*
        public static EventHelper DatingBirthday_Haley()
        {

        }
        public static EventHelper DatingBirthday_Sam()
        {
        
        }
        */
        /// <summary>
        /// Event that occurs when the player is dating Sebastian.
        /// Status: Finished.
        /// </summary>
        /// <returns></returns>
        public static EventHelper DatingBirthday_Sebastian()
        {
            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new FarmerBirthdayPrecondition());
            conditions.Add(new LocationPrecondition(Game1.getLocationFromName("ScienceHouse")));
            conditions.Add(new TimePrecondition(600, 2600));

            NPC maru = Game1.getCharacterFromName("Maru");
            NPC sebastian = Game1.getCharacterFromName("Sebastian");
            NPC robin = Game1.getCharacterFromName("Robin");
            NPC demetrius = Game1.getCharacterFromName("Demetrius");

            conditions.Add(new StardustCore.Events.Preconditions.NPCSpecific.DatingNPC(sebastian));

            EventHelper e = new EventHelper("BirthdayDating:Sebastian", 19952, conditions, new EventStartData("playful", 28, 12, new EventStartData.FarmerData(23, 12, EventHelper.FacingDirection.Right), new List<EventStartData.NPCData>() {
                new EventStartData.NPCData(maru,27,11, EventHelper.FacingDirection.Down),
                new EventStartData.NPCData(sebastian,26,13, EventHelper.FacingDirection.Up),
                new EventStartData.NPCData(robin,28,9, EventHelper.FacingDirection.Up),
                new EventStartData.NPCData(demetrius,30,11, EventHelper.FacingDirection.Left)
            }));
            e.globalFadeIn();

            e.moveFarmerRight(3, EventHelper.FacingDirection.Right, true);
            e.npcFaceDirection(maru, EventHelper.FacingDirection.Left);
            e.npcFaceDirection(demetrius, EventHelper.FacingDirection.Left);
            //Seb is already facing up.
            e.npcFaceDirection(robin, EventHelper.FacingDirection.Down);

            //Dialogue goes here.
            //Seriously improve dialogue lines. Maru is probably the NPC I know the least about.
            e.speak(sebastian, GetTranslatedString("Event:DatingSebastianBirthday_Sebastian:0")); //sebastian 0
            e.speak(robin, GetTranslatedString("Event:DatingSebastianBirthday_Robin:0")); //maru 0
            e.speak(maru, GetTranslatedString("Event:DatingSebastianBirthday_Maru:0"));//Maru 0
            e.speak(robin, GetTranslatedString("Event:DatingSebastianBirthday_Robin:1")); //robin 0
            e.speak(demetrius, GetTranslatedString("Event:DatingSebastianBirthday_Demetrius:0")); //demetrius 0
            e.speak(sebastian, GetTranslatedString("Event:DatingSebastianBirthday_Sebastian:1")); //Sebastian 1
            e.emote_ExclamationMark("Robin");
            e.npcFaceDirection(robin, EventHelper.FacingDirection.Up);
            e.speak(robin, GetTranslatedString("Event:DatingSebastianBirthday_Robin:2")); //robin 1
            e.npcFaceDirection(robin, EventHelper.FacingDirection.Down);
            e.moveActorDown("Robin", 1, EventHelper.FacingDirection.Down, false);
            e.addObject(27, 12, 220);
            e.speak(demetrius, GetTranslatedString("Event:DatingSebastianBirthday_Demetrius:1")); //maru 2
            e.emoteFarmer_Thinking();
            e.speak(maru, GetTranslatedString("Event:DatingSebastianBirthday_Maru:1")); //maru 3
            e.speak(sebastian, GetTranslatedString("Event:DatingSebastianBirthday_Sebastian:2")); //Sebastian 1

            //Event finish commands.
            e.emoteFarmer_Heart();
            e.emote_Heart("Sebastian");
            e.globalFadeOut(0.010);
            e.setViewportPosition(-100, -100);
            e.showMessage(HappyBirthday.Config.translationInfo.getTranslatedString("Event:DatingSebastianBirthday_Finish:0")); //maru party finish 0
            e.showMessage(HappyBirthday.Config.translationInfo.getTranslatedString("Event:DatingSebastianBirthday_Finish:1")); //maru party finish 0
            e.addObjectToPlayersInventory(220, 1, false);

            e.showMessage(HappyBirthday.Config.translationInfo.getTranslatedString("Event:PartyOver"));
            e.end();
            return e;
        }

        /*
            public static EventHelper DatingBirthday_Elliott()
            {

            }
            public static EventHelper DatingBirthday_Shane()
            {

            }
            public static EventHelper DatingBirthday_Harvey()
            {

            }

            public static EventHelper DatingBirthday_Alex()
            {

            }


            public static EventHelper MarriedBirthday()
            {

            }


            public static EventHelper CommunityBirthday()
            {

            }
            
            public static EventHelper JojaBirthday()
            {

            }
            */

        public static string GetTranslatedString(string Key)
        {
            return HappyBirthday.Config.translationInfo.getTranslatedString(Key);
        }

    }
}
