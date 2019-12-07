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
            EventHelper e = new EventHelper("CommunityCenterBirthday",19950, conditions, new EventStartData("playful", 32, 12, new EventStartData.FarmerData(32, 22, EventHelper.FacingDirection.Up),new List<EventStartData.NPCData>()));

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
            e.FlipJunimoActor("Juni8",true);
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

        
        public static EventHelper DatingBirthday_Penny()
        {
            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new FarmerBirthdayPrecondition());
            conditions.Add(new LocationPrecondition(Game1.getLocationFromName("Trailer")));
            conditions.Add(new TimePrecondition(600, 2600));

            NPC penny = Game1.getCharacterFromName("Penny");
            NPC pam = Game1.getCharacterFromName("Pam");

            //conditions.Add(new StardustCore.Events.Preconditions.NPCSpecific.DatingNPC(Game1.getCharacterFromName("Penny"));
            EventHelper e = new EventHelper("BirthdayDating:Penny", 19951, conditions, new EventStartData("playful", 12, 8, new EventStartData.FarmerData(12, 9, EventHelper.FacingDirection.Up), new List<EventStartData.NPCData>() {
                new EventStartData.NPCData(penny,12,7, EventHelper.FacingDirection.Up),
                new EventStartData.NPCData(pam,15,4, EventHelper.FacingDirection.Down)
            }));

            e.globalFadeIn();

            e.moveFarmerUp(1, EventHelper.FacingDirection.Up, false);

            e.actorFaceDirection("Penny", EventHelper.FacingDirection.Down);
            string starting = "Oh, @ you are here just in time!$h";
            //starting = starting.Replace("@", Game1.player.Name);
            e.speak(penny, starting);
            e.speak(pam, "Come on in kid. The party has just begun!$h");
            e.speak(penny, "I thought it would be nice if we threw you a small party. Granted it's not much but I hope you like it. $l");
            e.speak(pam, "Here, pull up a seat and have a beer to celebrate!");
            e.emote_Angry("Penny");
            e.speak(penny, "Mom!$a");
            e.speak(penny, "*sigh* Well make yourself at home. I'll get the cake out.");

            e.moveActorLeft("Penny", 3, EventHelper.FacingDirection.Up, true);
            e.moveFarmerRight(2, EventHelper.FacingDirection.Up, false);
            e.moveFarmerUp(3, EventHelper.FacingDirection.Down, false);
            e.moveActorRight("Penny", 5, EventHelper.FacingDirection.Up, true);
            e.moveActorUp("Penny", 1, EventHelper.FacingDirection.Up, true);
            e.speak(pam, "Alright, cheers kid! Happy birthday and here is to another great year! $h");
            e.speak(penny, "Happy birthday @. Here is hoping we get to spend many more birthdays together. $l");

            e.emoteFarmer_Heart();
            e.emote_Heart("Penny");
            e.globalFadeOut(0.010);
            e.setViewportPosition(-100, -100);
            e.showMessage("It was nice celebrating my birthday with Pam and Penny.");
            e.showMessage("Looks like there was some leftover cake and beer too!");
            e.addObjectToPlayersInventory(220, 1, false);
            e.addObjectToPlayersInventory(346, 1, false);

            e.showMessage(HappyBirthday.Config.translationInfo.getTranslatedString("Event:PartyOver"));

            e.end();

            return e;
        }
        /*
        public static EventHelper DatingBirthday_Maru()
        {

        }
        public static EventHelper DatingBirthday_Leah()
        {

        }
        public static EventHelper DatingBirthday_Abigail()
        {

        }
        public static EventHelper DatingBirthday_Emily()
        {

        }
        public static EventHelper DatingBirthday_Haley()
        {

        }
        public static EventHelper DatingBirthday_Sam()
        {

        }
        public static EventHelper DatingBirthday_Sebastian()
        {

        }
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

        public static EventHelper Birthday_Krobus()
        {

        }


        public static EventHelper MarriedBirthday_NoKids()
        {

        }

        public static EventHelper MarriedBirthday_OneKids()
        {

        }
        public static EventHelper MarriedBirthday_TwoKids()
        {

        }
        */
        }
    }
