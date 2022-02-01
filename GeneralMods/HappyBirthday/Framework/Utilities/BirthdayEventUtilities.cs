using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Omegasis.HappyBirthday.Framework.Constants;
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
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.JunimoCommunityCenterBirthday);
            }
            if (e.NewLocation == Game1.getLocationFromName("Trailer"))
            {
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.BirthdayDatingPennyTrailer);
            }
            if (e.NewLocation == Game1.getLocationFromName("Trailer_Big"))
            {
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.BirthdayDatingPennyHouse);
            }

            if (e.NewLocation == Game1.getLocationFromName("ScienceHouse"))
            {
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.BirthdayDatingMaru);
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.BirthdayDatingSebastian);
            }
            if (e.NewLocation == Game1.getLocationFromName("LeahHouse"))
            {
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.BirthdayDatingLeah);
            }
            if (e.NewLocation == Game1.getLocationFromName("SeedShop"))
            {
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.BirthdayDatingAbigailSeedShop);
            }
            if (e.NewLocation == Game1.getLocationFromName("Mine"))
            {
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.BirthdayDatingAbigailMines);
            }
            if (e.NewLocation == Game1.getLocationFromName("HaleyHouse"))
            {
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.BirthdayDatingEmily);
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.BirthdayDatingHaley);
            }
            if (e.NewLocation == Game1.getLocationFromName("HarveyRoom"))
            {
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.BirthdayDatingHarvey);
            }
            if (e.NewLocation == Game1.getLocationFromName("ElliottHouse"))
            {
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.BirthdayDatingElliott);
            }
            if (e.NewLocation == Game1.getLocationFromName("SamHouse"))
            {
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.BirthdayDatingSam);
            }
            if (e.NewLocation == Game1.getLocationFromName("JoshHouse"))
            {
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.BirthdayDatingAlex);
            }
            if (e.NewLocation == Game1.getLocationFromName("AnimalShop"))
            {
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.BirthdayDatingShane);
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
            BirthdayEventManager.addCustomEventLogic("Omegasis.HappyBirthday.Events.ShowTranslatedMessage", showTranslatedMessage);
            StardustCore.Compatibility.SpaceCore.SpaceCoreAPIUtil.RegisterCustomEventCommand("Omegasis.HappyBirthday.Events.ShowTranslatedMessage", showTranslatedMessage);

            List<EventHelper> defaultBirthdayEvents = new List<EventHelper>()
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
            string relativePath = Path.Combine("ModAssets", "Data", "Events");
            string abspath = Path.Combine(HappyBirthday.Instance.Helper.DirectoryPath, relativePath);
            if (!Directory.Exists(abspath))
            {
                Directory.CreateDirectory(abspath);
            }

            string[] files = Directory.GetFiles(abspath);
            foreach (string file in files)
            {
                EventHelper eventHelper = HappyBirthday.Instance.Helper.Data.ReadJsonFile<EventHelper>(Path.Combine(relativePath, Path.GetFileName(file)));

                if (eventHelper == null)
                {
                    continue;
                }

                if (BirthdayEventManager.events.ContainsKey(eventHelper.eventStringId))
                {
                    continue;
                }
                else
                {
                    BirthdayEventManager.addEvent(eventHelper);
                }

            }

            foreach (EventHelper eventHelper in defaultBirthdayEvents)
            {
                if (BirthdayEventManager.events.ContainsKey(eventHelper.eventStringId))
                {
                    continue;
                }
                else
                {
                    BirthdayEventManager.addEvent(eventHelper);
                    HappyBirthday.Instance.Helper.Data.WriteJsonFile<EventHelper>(Path.Combine(relativePath, eventHelper.eventStringId + ".json"), eventHelper);

                }
            }
        }

        public static void UpdateEventManager()
        {
            if (BirthdayEventManager != null)
            {
                BirthdayEventManager.update();
            }
        }

        public static void showTranslatedMessage(EventManager EventManager, string EventData)
        {
            string[] splits = EventData.Split(' ');
            string name = splits[0];
            string translationKey = splits[1];
            List<string> eventCommands = Game1.CurrentEvent.eventCommands.ToList();
            EventHelper helper = new EventHelper();
            helper.showMessage(GetEventString(translationKey));
            eventCommands.Insert(Game1.CurrentEvent.CurrentCommand + 1, helper.EventData);
            Game1.CurrentEvent.eventCommands = eventCommands.ToArray();
            Game1.CurrentEvent.CurrentCommand++;
        }

        public static void showTranslatedMessage(Event Event, GameLocation gameLocation, GameTime Time, string[] EventData)
        {
            string[] splits = EventData;

            foreach(string s in EventData)
            {
                HappyBirthday.Instance.Monitor.Log("Event data param: " + s);
            }

            string translationKey = splits[1];
            List<string> eventCommands = Game1.CurrentEvent.eventCommands.ToList();
            EventHelper helper = new EventHelper();
            helper.showMessage(GetEventString(translationKey));
            eventCommands.Insert(Game1.CurrentEvent.CurrentCommand + 1, helper.EventData);
            Event.eventCommands = eventCommands.ToArray();
            Event.CurrentCommand++;
        }


        public static void addTranslatedMessageToBeShown(EventHelper eventHelper,string MessageKey)
        {
            StringBuilder b = new StringBuilder();
            b.Append("Omegasis.HappyBirthday.Events.ShowTranslatedMessage ");
            b.Append(MessageKey);
            eventHelper.add(b);
        }

        public static string GetEventString(string Key)
        {
            return HappyBirthday.Instance.translationInfo.getEventString(Key);
        }

    }
}
