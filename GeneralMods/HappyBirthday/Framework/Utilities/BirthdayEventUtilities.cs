using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Omegasis.HappyBirthday.Framework.Constants;
using Omegasis.HappyBirthday.Framework.EventPreconditions;
using Omegasis.HappyBirthday.Framework.Menus;
using StardewModdingAPI.Events;
using StardewValley;
using StardustCore.Events;

namespace Omegasis.HappyBirthday.Framework.Utilities
{
    public static class BirthdayEventUtilities
    {
        public static EventManager BirthdayEventManager;
        public static bool ShouldAskPlayerForBirthday;
        public static bool ShouldAskPlayerForFavoriteGift;

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

            if (e.NewLocation == Game1.getLocationFromName("Farm"))
            {
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.AskPlayerForBirthday);
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
            Omegasis.StardustCore.Compatibility.SpaceCore.SpaceCoreAPIUtil.RegisterCustomEventCommand("Omegasis.HappyBirthday.Events.ShowTranslatedMessage", showTranslatedMessage);

            BirthdayEventManager.addCustomEventLogic("Omegasis.HappyBirthday.Events.ShowBirthdaySelectionMenu", setShouldShowChooseBirthdayMenu);
            Omegasis.StardustCore.Compatibility.SpaceCore.SpaceCoreAPIUtil.RegisterCustomEventCommand("Omegasis.HappyBirthday.Events.ShowBirthdaySelectionMenu", setShouldShowChooseBirthdayMenu);
            BirthdayEventManager.addCustomEventLogic("Omegasis.HappyBirthday.Events.ShowFavoriteGiftSelectionMenu", setShouldShowChooseFavoriteGiftMenu);
            Omegasis.StardustCore.Compatibility.SpaceCore.SpaceCoreAPIUtil.RegisterCustomEventCommand("Omegasis.HappyBirthday.Events.ShowFavoriteGiftSelectionMenu", setShouldShowChooseFavoriteGiftMenu);


            BirthdayEventManager.addCustomEventLogic("Omegasis.HappyBirthday.Events.SpeakIfTodayIsPlayersBirthday", speakIfTodayIsPlayersBirthday);
            Omegasis.StardustCore.Compatibility.SpaceCore.SpaceCoreAPIUtil.RegisterCustomEventCommand("Omegasis.HappyBirthday.Events.SpeakIfTodayIsPlayersBirthday", speakIfTodayIsPlayersBirthday);

            BirthdayEventManager.addCustomEventLogic("Omegasis.HappyBirthday.Events.SkipNextCommand", skipNextCommand);
            Omegasis.StardustCore.Compatibility.SpaceCore.SpaceCoreAPIUtil.RegisterCustomEventCommand("Omegasis.HappyBirthday.Events.SkipNextCommand", skipNextCommand);

            BirthdayEventManager.addCustomEventLogic("Omegasis.HappyBirthday.Events.SpeakWithBirthdayMessageIncluded", speakWithBirthdayIncluded);
            Omegasis.StardustCore.Compatibility.SpaceCore.SpaceCoreAPIUtil.RegisterCustomEventCommand("Omegasis.HappyBirthday.Events.SpeakWithBirthdayMessageIncluded", speakWithBirthdayIncluded);
            BirthdayEventManager.addCustomEventLogic("Omegasis.HappyBirthday.Events.SpeakWithTranslatedMessage", speakWithTranslatedMessage);
            Omegasis.StardustCore.Compatibility.SpaceCore.SpaceCoreAPIUtil.RegisterCustomEventCommand("Omegasis.HappyBirthday.Events.SpeakWithTranslatedMessage", speakWithTranslatedMessage);



            BirthdayEventManager.eventPreconditionParsingMethods.Add(FarmerBirthdayPrecondition.EventPreconditionId, HappyBirthdayPreconditionParsingMethods.ParseFarmerBirthdayPrecondition);
            BirthdayEventManager.eventPreconditionParsingMethods.Add(SpouseBirthdayPrecondition.EventPreconditionId, HappyBirthdayPreconditionParsingMethods.ParseSpouseBirthdayPrecondition);

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
                BirthdayEvents.DatingBirthday_Sebastian(),

                //Set up birthday events
                BirthdayEvents.LewisAsksPlayerForBirthday()
        };
            string relativePath = Path.Combine("ModAssets", "Data", "Events");
            string abspath = Path.Combine(HappyBirthdayModCore.Instance.Helper.DirectoryPath, relativePath);
            if (!Directory.Exists(abspath))
            {
                Directory.CreateDirectory(abspath);
            }

            string[] files = Directory.GetFiles(abspath);
            foreach (string file in files)
            {
                EventHelper eventHelper = HappyBirthdayModCore.Instance.Helper.Data.ReadJsonFile<EventHelper>(Path.Combine(relativePath, Path.GetFileName(file)));
                eventHelper.parseEventPreconditionsFromPreconditionStrings(BirthdayEventManager);



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
                    HappyBirthdayModCore.Instance.Helper.Data.WriteJsonFile<EventHelper>(Path.Combine(relativePath, eventHelper.eventStringId + ".json"), eventHelper);

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
            showTranslatedMessage(Game1.CurrentEvent, splits);
        }

        public static void showTranslatedMessage(Event Event, GameLocation gameLocation, GameTime Time, string[] EventData)
        {
            showTranslatedMessage(Event, EventData);
        }

        public static void showTranslatedMessage(Event Event,string[] EventData)
        {
            string[] splits = EventData;

            string translationKey = splits[1];
            List<string> eventCommands = Game1.CurrentEvent.eventCommands.ToList();
            EventHelper helper = new EventHelper();
            helper.showMessage(GetEventString(translationKey));
            eventCommands.Insert(Game1.CurrentEvent.CurrentCommand + 1, helper.EventData);
            Event.eventCommands = eventCommands.ToArray();
            Event.CurrentCommand++;
        }


        public static void speakWithBirthdayIncluded(EventManager EventManager, string EventData)
        {
            string[] splits = EventData.Split(' ');
            speakWithBirthdayIncluded(Game1.CurrentEvent, splits);
        }

        public static void speakWithBirthdayIncluded(Event Event, GameLocation gameLocation, GameTime Time, string[] EventData)
        {
            speakWithBirthdayIncluded(Event, EventData);
        }

        public static void speakWithBirthdayIncluded(Event Event, string[] EventData)
        {
            string speakerName = EventData[1];
            //final message should be changed to string.format(GetEventString(),BirthdaySeason,BirthdayDay);
            string finalMessage = string.Format(GetEventString(EventData[2]), HappyBirthday.HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData.BirthdaySeason,
                    HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData.BirthdayDay);
            List<string> eventCommands = Game1.CurrentEvent.eventCommands.ToList();
            EventHelper helper = new EventHelper();
            helper.speak(speakerName,finalMessage);
            eventCommands.Insert(Game1.CurrentEvent.CurrentCommand + 1, helper.EventData);
            Event.eventCommands = eventCommands.ToArray();
            Event.CurrentCommand++;

        }


        public static void speakWithTranslatedMessage(EventManager EventManager, string EventData)
        {
            string[] splits = EventData.Split(' ');
            speakWithTranslatedMessage(Game1.CurrentEvent, splits);
        }

        public static void speakWithTranslatedMessage(Event Event, GameLocation gameLocation, GameTime Time, string[] EventData)
        {
            speakWithTranslatedMessage(Event, EventData);
        }

        public static void speakWithTranslatedMessage(Event Event, string[] EventData)
        {
            string speakerName = EventData[1];
            string finalMessage = GetEventString(EventData[2]);
            List<string> eventCommands = Game1.CurrentEvent.eventCommands.ToList();
            EventHelper helper = new EventHelper();
            helper.speak(speakerName, finalMessage);
            eventCommands.Insert(Game1.CurrentEvent.CurrentCommand + 1, helper.EventData);
            Event.eventCommands = eventCommands.ToArray();
            Event.CurrentCommand++;

        }

        public static void setShouldShowChooseBirthdayMenu(EventManager EventManager, string EventData)
        {
            ShouldAskPlayerForBirthday = true;
            OpenBirthdaySelectionMenu();
        }

        public static void setShouldShowChooseBirthdayMenu(Event Event, GameLocation gameLocation, GameTime Time, string[] EventData)
        {
            ShouldAskPlayerForBirthday = true;

            OpenBirthdaySelectionMenu();
        }

        public static void OpenBirthdaySelectionMenu()
        {

            if (!HappyBirthdayModCore.Instance.birthdayManager.hasChosenBirthday() && Game1.activeClickableMenu == null && BirthdayEventUtilities.ShouldAskPlayerForBirthday)
            {
                if (HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData != null)
                {
                    Game1.activeClickableMenu = new BirthdayMenu(HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData.BirthdaySeason, HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData.BirthdayDay, HappyBirthdayModCore.Instance.birthdayManager.setBirthday);
                    HappyBirthdayModCore.Instance.birthdayManager.setCheckedForBirthday(false);
                }
                else
                {
                    HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData = new PlayerData();
                    Game1.activeClickableMenu = new BirthdayMenu("", 0, HappyBirthdayModCore.Instance.birthdayManager.setBirthday);
                    HappyBirthdayModCore.Instance.birthdayManager.setCheckedForBirthday(false);
                }
            }
        }

        public static void OpenGiftSelectionMenu()
        {
            HappyBirthdayModCore.Instance.Monitor.Log("OpenGiftSelectorMenuIs running!");

            if (HappyBirthdayModCore.Instance.birthdayManager.hasChosenBirthday() && Game1.activeClickableMenu == null && HappyBirthdayModCore.Instance.birthdayManager.hasChoosenFavoriteGift() == false && BirthdayEventUtilities.ShouldAskPlayerForFavoriteGift)
            {
                Game1.activeClickableMenu = new FavoriteGiftMenu();
                HappyBirthdayModCore.Instance.birthdayManager.setCheckedForBirthday(false);
                return;
            }
        }



        public static void setShouldShowChooseFavoriteGiftMenu(EventManager EventManager, string EventData)
        {
            ShouldAskPlayerForFavoriteGift = true;
            OpenGiftSelectionMenu();
        }

        public static void setShouldShowChooseFavoriteGiftMenu(Event Event, GameLocation gameLocation, GameTime Time, string[] EventData)
        {
            ShouldAskPlayerForFavoriteGift = true;
            OpenGiftSelectionMenu();
        }


        public static void speakIfTodayIsPlayersBirthday(EventManager EventManager, string EventData)
        {

            speakIfTodayIsPlayersBirthday(Game1.CurrentEvent);
        }

        public static void speakIfTodayIsPlayersBirthday(Event Event, GameLocation gameLocation, GameTime Time, string[] EventData)
        {
            speakIfTodayIsPlayersBirthday(Event);
        }

        /// <summary>
        /// Speak a given dialogue if it is the player's birthday or not.
        /// </summary>
        /// <param name="CurrentEvent"></param>
        public static void speakIfTodayIsPlayersBirthday(Event CurrentEvent)
        {
            if (HappyBirthday.HappyBirthdayModCore.Instance.birthdayManager.isBirthday())
            {
                CurrentEvent.CurrentCommand++;
                //goes to true
            }
            else
            {
                CurrentEvent.CurrentCommand += 3;
                //Progresses to the false condition.
            }
        }

        /// <summary>
        /// Progresses the game's current event command by 1
        /// </summary>
        /// <param name="EventManager"></param>
        /// <param name="data"></param>
        public static void nextCommand(EventManager EventManager, string data)
        {

            nextCommand(Game1.CurrentEvent);
        }

        /// <summary>
        /// Progresses the game's current event command by 1
        /// </summary>
        /// <param name="Event"></param>
        /// <param name="gameLocation"></param>
        /// <param name="Time"></param>
        /// <param name="EventData"></param>
        public static void nextCommand(Event Event, GameLocation gameLocation, GameTime Time, string[] EventData)
        {
            nextCommand(Event);
        }

        /// <summary>
        /// Progresses the game's current event command by 1
        /// </summary>
        /// <param name="Event"></param>
        public static void nextCommand(Event Event)
        {
            Event.CurrentCommand++;
        }


        /// <summary>
        /// Skips the next event command for the game. Necessary for branching.
        /// </summary>
        /// <param name="EventManager"></param>
        /// <param name="data"></param>
        public static void skipNextCommand(EventManager EventManager, string data)
        {

            skipNextCommand(Game1.CurrentEvent);
        }

        /// <summary>
        /// Skips the next event command for the game. Necessary for branching.
        /// </summary>
        /// <param name="Event"></param>
        /// <param name="gameLocation"></param>
        /// <param name="Time"></param>
        /// <param name="EventData"></param>
        public static void skipNextCommand(Event Event, GameLocation gameLocation, GameTime Time, string[] EventData)
        {
            skipNextCommand(Event);
        }

        /// <summary>
        /// Skips the next event command for the game. Necessary for branching.
        /// </summary>
        /// <param name="Event"></param>
        public static void skipNextCommand(Event Event)
        {
            Event.CurrentCommand+=2;
        }

        /// <summary>
        /// Adds the event command to show the translated message.
        /// </summary>
        /// <param name="eventHelper"></param>
        /// <param name="MessageKey"></param>
        public static void addTranslatedMessageToBeShown(EventHelper eventHelper,string MessageKey)
        {
            StringBuilder b = new StringBuilder();
            b.Append("Omegasis.HappyBirthday.Events.ShowTranslatedMessage ");
            b.Append(MessageKey);
            eventHelper.addEventData(b);
        }

        /// <summary>
        /// Adds the event command to ask the player for their birthday.
        /// </summary>
        /// <param name="eventHelper"></param>
        public static void addAskForBirthday(EventHelper eventHelper)
        {
            StringBuilder b = new StringBuilder();
            b.Append("Omegasis.HappyBirthday.Events.ShowBirthdaySelectionMenu");
            eventHelper.addEventData(b);
        }

        /// <summary>
        /// Adds the event command to ask the player for their favorite gift.
        /// </summary>
        /// <param name="eventHelper"></param>
        public static void addAskForFavoriteGift(EventHelper eventHelper)
        {
            StringBuilder b = new StringBuilder();
            b.Append("Omegasis.HappyBirthday.Events.ShowFavoriteGiftSelectionMenu");
            eventHelper.addEventData(b);
        }

        /// <summary>
        /// Adds the event command to show specific dialogue if it is the players birthday. Otherwise, show an alternative message.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="SpeakerName"></param>
        /// <param name="PreconditionString"></param>
        /// <param name="TrueMessage"></param>
        /// <param name="FalseMessage"></param>
        public static void speakIfTodayIsPlayersBirthday(EventHelper helper, string SpeakerName ,string TrueMessageKey, string FalseMessageKey)
        {
            StringBuilder b = new StringBuilder();
            b.Append("Omegasis.HappyBirthday.Events.SpeakIfTodayIsPlayersBirthday");
            helper.addEventData(b);
            BirthdayEventUtilities.speakWithTranslatedMessage(helper, SpeakerName, TrueMessageKey);
            skipNextCommand(helper);
            speakWithBirthdayIncluded(helper,SpeakerName, FalseMessageKey); //TODO: CHange this to GetEventString()


        }

        /// <summary>
        /// Adds the event command to skip the next command in the current event. Used for event branching.
        /// </summary>
        /// <param name="eventHelper"></param>
        public static void skipNextCommand(EventHelper eventHelper)
        {
            StringBuilder b = new StringBuilder();
            b.Append("Omegasis.HappyBirthday.Events.SkipNextCommand");
            eventHelper.addEventData(b);
        }

        public static void speakWithBirthdayIncluded(EventHelper eventHelper, string SpeakerName, string OriginalMessageKey)
        {
            StringBuilder b = new StringBuilder();
            b.Append("Omegasis.HappyBirthday.Events.SpeakWithBirthdayMessageIncluded");
            b.Append(" ");
            b.Append(SpeakerName);
            b.Append(" ");
            b.Append(OriginalMessageKey);
            eventHelper.addEventData(b);
        }

        /// <summary>
        /// Creates the command to get a specific translation string and have a given npc speak that returned string.
        /// </summary>
        /// <param name="eventHelper"></param>
        /// <param name="SpeakerName"></param>
        /// <param name="MessageKey"></param>
        public static void speakWithTranslatedMessage(EventHelper eventHelper, string SpeakerName, string MessageKey)
        {
            StringBuilder b = new StringBuilder();
            b.Append("Omegasis.HappyBirthday.Events.SpeakWithTranslatedMessage");
            b.Append(" ");
            b.Append(SpeakerName);
            b.Append(" ");
            b.Append(MessageKey);
            eventHelper.addEventData(b);
        }


        /// <summary>
        /// Gets a string to be displayed during the event.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string GetEventString(string Key)
        {



            return HappyBirthdayModCore.Instance.translationInfo.getEventString(Key);
        }

    }
}
