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
        public static EventHelper CommunityCenterBirthday()
        {
            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new FarmerBirthdayPrecondition());
            conditions.Add(new LocationPrecondition(Game1.getLocationFromName("CommunityCenter")));
            conditions.Add(new TimePrecondition(600, 2600));
            conditions.Add(new CanReadJunimo());
            conditions.Add(new StardustCore.Events.Preconditions.PlayerSpecific.JojaMember(false));
            //conditions.Add(new HasUnlockedCommunityCenter()); //Infered by the fact that you must enter the community center to trigger this event anyways.
            EventHelper e = new EventHelper("CommunityCenterBirthday",19950, conditions, new EventStartData(EventStartData.MusicToPlayType.Continue, 32, 16, new EventStartData.FarmerData(32, 22, EventHelper.FacingDirection.Up),new List<EventStartData.NPCData>()));

            e.AddInJunimoActor("Juni", new Microsoft.Xna.Framework.Vector2(32, 14), StardustCore.IlluminateFramework.Colors.getRandomJunimoColor());
            e.AddInJunimoActor("Juni2", new Microsoft.Xna.Framework.Vector2(30, 15), StardustCore.IlluminateFramework.Colors.getRandomJunimoColor());
            e.AddInJunimoActor("Juni3", new Microsoft.Xna.Framework.Vector2(34, 15), StardustCore.IlluminateFramework.Colors.getRandomJunimoColor());

            e.globalFadeIn();
            e.moveFarmerUp(6, EventHelper.FacingDirection.Up, true);

            //e.moveActorLeft("Juni", 1, EventHelper.FacingDirection.Down, false);
            e.animate("Juni", true, true, 250, new List<int>()
            {
                28,
                29,
                30,
                31
            });

            //

            e.playSound("junimoMeep1");
            //
            e.emoteFarmer_ExclamationMark();
            e.showMessage("It looks like the junimos wanted to throw you a party!"); //TODO get this from translated strings. NOT hard coded.
            e.emoteFarmer_Heart();
            e.globalFadeOut(0.010);
            e.setViewportPosition(-100, -100);
            e.showMessage("It looks like there was some cake left over too!");//TODO get this from translated strings. NOT hard coded.
            e.showMessage("That was a fun party. Back to work!");//TODO get this from translated strings. NOT hard coded.
            e.addObjectToPlayersInventory(220, 1, false);

            e.end();

            return e;
        }

        /*
        public static EventHelper DatingBirthday_Penny()
        {

        }
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
