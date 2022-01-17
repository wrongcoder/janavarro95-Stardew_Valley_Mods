using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI.Events;
using StardewValley;
using StardustCore.Events;

namespace Omegasis.HappyBirthday.Framework.Utilities
{
    public static class BirthdayEventUtilities
    {
        public static EventManager BirthdayEventManager;

        public static void Player_Warped(object sender, WarpedEventArgs e)
        {
            if (e.NewLocation == Game1.getLocationFromName("CommunityCenter"))
            {
                BirthdayEventManager.startEventAtLocationIfPossible("CommunityCenterBirthday");
            }
            if (e.NewLocation == Game1.getLocationFromName("Trailer"))
            {
                BirthdayEventManager.startEventAtLocationIfPossible("BirthdayDating:Penny");
            }
            if (e.NewLocation == Game1.getLocationFromName("Trailer_Big"))
            {
                BirthdayEventManager.startEventAtLocationIfPossible("BirthdayDating:Penny_BigHome");
            }

            if (e.NewLocation == Game1.getLocationFromName("ScienceHouse"))
            {
                BirthdayEventManager.startEventAtLocationIfPossible("BirthdayDating:Maru");
                BirthdayEventManager.startEventAtLocationIfPossible("BirthdayDating:Sebastian");
            }
            if (e.NewLocation == Game1.getLocationFromName("LeahHouse"))
            {
                BirthdayEventManager.startEventAtLocationIfPossible("BirthdayDating:Leah");
            }
            if (e.NewLocation == Game1.getLocationFromName("SeedShop"))
            {
                BirthdayEventManager.startEventAtLocationIfPossible("BirthdayDating:Abigail");
            }
            if (e.NewLocation == Game1.getLocationFromName("Mine"))
            {
                BirthdayEventManager.startEventAtLocationIfPossible("BirthdayDating:Abigail_Mine");
            }
            if (e.NewLocation == Game1.getLocationFromName("HaleyHouse"))
            {
                BirthdayEventManager.startEventAtLocationIfPossible("BirthdayDating:Emily");
                BirthdayEventManager.startEventAtLocationIfPossible("BirthdayDating:Haley");
            }
            if (e.NewLocation == Game1.getLocationFromName("HarveyRoom"))
            {
                BirthdayEventManager.startEventAtLocationIfPossible("BirthdayDating:Harvey");
            }
            if (e.NewLocation == Game1.getLocationFromName("ElliottHouse"))
            {
                BirthdayEventManager.startEventAtLocationIfPossible("BirthdayDating:Elliott");
            }
            if (e.NewLocation == Game1.getLocationFromName("SamHouse"))
            {
                BirthdayEventManager.startEventAtLocationIfPossible("BirthdayDating:Sam");
            }
            if (e.NewLocation == Game1.getLocationFromName("JoshHouse"))
            {
                BirthdayEventManager.startEventAtLocationIfPossible("BirthdayDating:Alex");
            }
            if (e.NewLocation == Game1.getLocationFromName("AnimalShop"))
            {
                BirthdayEventManager.startEventAtLocationIfPossible("BirthdayDating:Shane");
            }

        }

        public static void ClearEventsFromFarmer()
        {
            foreach (KeyValuePair<string, EventHelper> v in BirthdayEventManager.events)
            {
                BirthdayEventManager.clearEventFromFarmer(v.Key);
            }
        }

        public static void InitializeBirthdayEvents()
        {

            List<EventHelper> events = new List<EventHelper>()
            {
                //Villager/Npc birthday events.
                BirthdayEvents.CommunityCenterJunimoBirthday(),

                //Female dating candidate birthday events.
                BirthdayEvents.DatingBirthday_Penny(),
                BirthdayEvents.DatingBirthday_Penny_BigHome(),
                BirthdayEvents.DatingBirthday_Maru(),
                BirthdayEvents.DatingBirthday_Leah(),
                BirthdayEvents.DatingBirthday_Abigail_Seedshop(),
                BirthdayEvents.DatingBirthday_Abigail_Mine(),
                BirthdayEvents.DatingBirthday_Emily(),
                BirthdayEvents.DatingBirthday_Haley(),

                //Male dating candidate birthday events.
                BirthdayEvents.DatingBirthday_Harvey(),
                BirthdayEvents.DatingBirthday_Elliott(),
                BirthdayEvents.DatingBirthday_Sam(),
                BirthdayEvents.DatingBirthday_Alex(),
                BirthdayEvents.DatingBirthday_Shane(),
                BirthdayEvents.DatingBirthday_Sebastian()
        };

            foreach(EventHelper eventHelper in events)
            {
                BirthdayEventManager.addEvent(eventHelper);
            }
        }

        public static void UpdateEventManager()
        {
            if (BirthdayEventManager != null)
            {
                BirthdayEventManager.update();
            }
        }
    }
}
