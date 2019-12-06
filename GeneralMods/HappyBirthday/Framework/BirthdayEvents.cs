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

namespace Omegasis.HappyBirthday.Framework
{
    public class BirthdayEvents
    {

        public static EventHelper CommunityCenterBirthday()
        {
            //TODO Junimos
            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new FarmerBirthdayPrecondition());
            conditions.Add(new LocationPrecondition(Game1.getLocationFromName("CommunityCenter")));
            conditions.Add(new TimePrecondition(600, 2600));
            EventHelper e = new EventHelper("CommunityCenterBirthday",19950, conditions, new EventStartData(EventStartData.MusicToPlayType.Continue, 32, 16, new EventStartData.FarmerData(32, 22, EventHelper.FacingDirection.Up),new List<EventStartData.NPCData>()));

            e.AddInJunimoActor("Juni", new Microsoft.Xna.Framework.Vector2(32, 14), Color.Blue);

            e.globalFadeIn();
            e.moveFarmerUp(6, EventHelper.FacingDirection.Up, false);
            e.ViewportLerpTileOffset(new Microsoft.Xna.Framework.Point(0,-6),60*6,true);

            e.moveActorLeft("Juni", 1, EventHelper.FacingDirection.Down, false);
            //e.addObjectToPlayersInventory(64, 22,true);

            //e.addTemporaryActor_NPC("Junimo", 16, 16, 32, 14, EventHelper.FacingDirection.Down, false);

            e.showMessage("Community center birthday here.");
            
            //Notes
            //Add a temporary actor (or sprite) to the screen.
            //Use the attachCharacterToTempSprite command to stitch them together?

            /*
             *
             *
             *                         else if (strArray[index].Equals("Junimo"))
                        {

                        }

            */

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
