using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Omegasis.HappyBirthday.Framework.Constants;
using Omegasis.HappyBirthday.Framework.ContentPack;
using Omegasis.HappyBirthday.Framework.Events.EventPreconditions;
using Omegasis.HappyBirthday.Framework.Menus;
using Omegasis.StardustCore.Events;
using StardewModdingAPI.Events;
using StardewValley;

namespace Omegasis.HappyBirthday.Framework.Events
{
    public static class BirthdayEventUtilities
    {
        /// <summary>
        /// A flag used to gate specific dialogue parsing to ensure that the <see cref="HappyBirthdayEventHelper"/> can write the generic spouse data to a json string and parse it properly for when serving it to the game for events.
        /// </summary>
        public static bool NEED_TO_WRITE_DEFAULT_BIRTHDAY_EVENTS_TO_JSON;

        public static EventManager BirthdayEventManager;
        public static bool ShouldAskPlayerForBirthday;
        public static bool ShouldAskPlayerForFavoriteGift;

        public static void Player_Warped(object sender, WarpedEventArgs e)
        {
            StartEventAtLocationIfPossible(e.NewLocation);
        }

        public static void OnDayStarted()
        {
            StartEventAtLocationIfPossible(Game1.player.currentLocation);
        }

        public static void StartEventAtLocationIfPossible(GameLocation location)
        {
            BirthdayEventManager.startEventsAtLocationIfPossible();
        }

        public static void ClearEventsFromFarmer()
        {
            foreach (KeyValuePair<string, EventHelper> v in BirthdayEventManager.events)
                BirthdayEventManager.clearEventFromFarmer(v.Key);
        }

        public static void InitializeBirthdayEventCommands()
        {
            //Dialogue commands.
            BirthdayEventManager.addCustomEventLogic("Omegasis.HappyBirthday.Events.ShowTranslatedMessage", BirthdayEventCommands.showTranslatedMessage);
            StardustCore.Compatibility.SpaceCore.SpaceCoreAPIUtil.RegisterCustomEventCommand("Omegasis.HappyBirthday.Events.ShowTranslatedMessage", BirthdayEventCommands.showTranslatedMessage);
            BirthdayEventManager.addCustomEventLogic("Omegasis.HappyBirthday.Events.SpeakWithBirthdayMessageIncluded", BirthdayEventCommands.speakWithBirthdayIncluded);
            StardustCore.Compatibility.SpaceCore.SpaceCoreAPIUtil.RegisterCustomEventCommand("Omegasis.HappyBirthday.Events.SpeakWithBirthdayMessageIncluded", BirthdayEventCommands.speakWithBirthdayIncluded);
            BirthdayEventManager.addCustomEventLogic("Omegasis.HappyBirthday.Events.SpeakWithTranslatedMessage", BirthdayEventCommands.speakWithTranslatedMessage);
            StardustCore.Compatibility.SpaceCore.SpaceCoreAPIUtil.RegisterCustomEventCommand("Omegasis.HappyBirthday.Events.SpeakWithTranslatedMessage", BirthdayEventCommands.speakWithTranslatedMessage);
            BirthdayEventManager.addCustomEventLogic("Omegasis.HappyBirthday.Events.SpeakIfTodayIsPlayersBirthday", BirthdayEventCommands.speakIfTodayIsPlayersBirthday);
            StardustCore.Compatibility.SpaceCore.SpaceCoreAPIUtil.RegisterCustomEventCommand("Omegasis.HappyBirthday.Events.SpeakIfTodayIsPlayersBirthday", BirthdayEventCommands.speakIfTodayIsPlayersBirthday);

            //Menu commands.
            BirthdayEventManager.addCustomEventLogic("Omegasis.HappyBirthday.Events.ShowBirthdaySelectionMenu", BirthdayEventCommands.setShouldShowChooseBirthdayMenu);
            StardustCore.Compatibility.SpaceCore.SpaceCoreAPIUtil.RegisterCustomEventCommand("Omegasis.HappyBirthday.Events.ShowBirthdaySelectionMenu", BirthdayEventCommands.setShouldShowChooseBirthdayMenu);
            BirthdayEventManager.addCustomEventLogic("Omegasis.HappyBirthday.Events.ShowFavoriteGiftSelectionMenu", BirthdayEventCommands.setShouldShowChooseFavoriteGiftMenu);
            StardustCore.Compatibility.SpaceCore.SpaceCoreAPIUtil.RegisterCustomEventCommand("Omegasis.HappyBirthday.Events.ShowFavoriteGiftSelectionMenu", BirthdayEventCommands.setShouldShowChooseFavoriteGiftMenu);

            //Utility Commands
            BirthdayEventManager.addCustomEventLogic("Omegasis.HappyBirthday.Events.SkipNextCommand", BirthdayEventCommands.skipNextCommand);
            StardustCore.Compatibility.SpaceCore.SpaceCoreAPIUtil.RegisterCustomEventCommand("Omegasis.HappyBirthday.Events.SkipNextCommand", BirthdayEventCommands.skipNextCommand);

            BirthdayEventManager.addCustomEventLogic("Omegasis.HappyBirthday.Events.GivePlayerFavoriteGift", BirthdayEventCommands.givePlayerFavoriteGift);
            StardustCore.Compatibility.SpaceCore.SpaceCoreAPIUtil.RegisterCustomEventCommand("Omegasis.HappyBirthday.Events.GivePlayerFavoriteGift", BirthdayEventCommands.givePlayerFavoriteGift);

            BirthdayEventManager.addCustomEventLogic("Omegasis.HappyBirthday.Events.MakeObjectsTemporarilyInvisible", BirthdayEventCommands.makeObjectsTemporarilyInvisible);
            StardustCore.Compatibility.SpaceCore.SpaceCoreAPIUtil.RegisterCustomEventCommand("Omegasis.HappyBirthday.Events.MakeObjectsTemporarilyInvisible", BirthdayEventCommands.makeObjectsTemporarilyInvisible);


            //Additional Preconditions
            BirthdayEventManager.eventPreconditionParsingMethods.Add(FarmerBirthdayPrecondition.EventPreconditionId, HappyBirthdayPreconditionParsingMethods.ParseFarmerBirthdayPrecondition);
            BirthdayEventManager.eventPreconditionParsingMethods.Add(SpouseBirthdayPrecondition.EventPreconditionId, HappyBirthdayPreconditionParsingMethods.ParseSpouseBirthdayPrecondition);
            BirthdayEventManager.eventPreconditionParsingMethods.Add(HasChosenBirthdayPrecondition.EventPreconditionId, HappyBirthdayPreconditionParsingMethods.ParseHasChosenBirthdayPrecondition);
            BirthdayEventManager.eventPreconditionParsingMethods.Add(HasChosenFavoriteGiftPrecondition.EventPreconditionId, HappyBirthdayPreconditionParsingMethods.ParseHasChosenFavoriteGiftPrecondition);
            BirthdayEventManager.eventPreconditionParsingMethods.Add(IsMarriedToPrecondition.EventPreconditionId, HappyBirthdayPreconditionParsingMethods.ParseIsMarriedToPrecondition);

            BirthdayEventManager.eventPreconditionParsingMethods.Add(IsMarriedPrecondition.EventPreconditionId, HappyBirthdayPreconditionParsingMethods.ParseIsMarriedPrecondition);

            BirthdayEventManager.eventPreconditionParsingMethods.Add(GameLocationIsHomePrecondition.EventPreconditionId, HappyBirthdayPreconditionParsingMethods.ParseGameLocationIsHomePrecondition);
            BirthdayEventManager.eventPreconditionParsingMethods.Add(FarmHouseLevelPrecondition.EventPreconditionId, HappyBirthdayPreconditionParsingMethods.ParseFarmHouseLevelPrecondition);
            BirthdayEventManager.eventPreconditionParsingMethods.Add(YearPrecondition.EventPreconditionId, HappyBirthdayPreconditionParsingMethods.ParseYearGreaterThanOrEqualToPrecondition);
            BirthdayEventManager.eventPreconditionParsingMethods.Add(VillagersHaveEnoughFriendshipBirthdayPrecondition.EventPreconditionId, HappyBirthdayPreconditionParsingMethods.ParseVillagersHaveEnoughFriendshipBirthdayPrecondition);
        }

        public static void InitializeBirthdayEvents()
        {
            /*
            NEED_TO_WRITE_DEFAULT_BIRTHDAY_EVENTS_TO_JSON = true;

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
                BirthdayEvents.LewisAsksPlayerForBirthday(),

                BirthdayEvents.JojaBirthday(),
                BirthdayEvents.SaloonBirthday_Year1(),
                BirthdayEvents.SaloonBirthday_Year2(),

                //Married birthday events.
                BirthdayEvents.SpouseAsksPlayerForFavoriteGift_farmhouseLevel1(),
                BirthdayEvents.SpouseAsksPlayerForFavoriteGift_farmhouseLevel2(),
                BirthdayEvents.MarriedBirthday_farmhouseLevel1(),
                BirthdayEvents.MarriedBirthday_farmhouseLevel2(),

        };
            */



            /*
            string relativePath = Path.Combine("ModAssets", "Data", "Events");
            string abspath = Path.Combine(HappyBirthdayModCore.Instance.Helper.DirectoryPath, relativePath);
            if (!Directory.Exists(abspath))
                Directory.CreateDirectory(abspath);

            string[] files = Directory.GetFiles(abspath);
            foreach (string file in files)
            {
                string pathToFile = Path.Combine(relativePath, Path.GetFileName(file));
                //Exclude non json files and directories that may exist here due to Vortex or some sort of mistakes.
                if (!pathToFile.EndsWith(".json")) continue;

                HappyBirthdayEventHelper eventHelper = HappyBirthdayModCore.Instance.Helper.Data.ReadJsonFile<HappyBirthdayEventHelper>(pathToFile);

                eventHelper.parseEventPreconditionsFromPreconditionStrings(BirthdayEventManager);



                if (eventHelper == null)
                    continue;

                if (BirthdayEventManager.events.ContainsKey(eventHelper.eventStringId))
                    continue;
                else
                    BirthdayEventManager.addEvent(eventHelper);

            }

            foreach (EventHelper eventHelper in defaultBirthdayEvents)
            {
                if (BirthdayEventManager.events.ContainsKey(eventHelper.eventStringId))
                {

                    //auto update/replace outdated events.
                    if (eventHelper.version > BirthdayEventManager.events[eventHelper.eventStringId].version)
                    {
                        BirthdayEventManager.events[eventHelper.eventStringId] = eventHelper;
                        HappyBirthdayModCore.Instance.Helper.Data.WriteJsonFile(Path.Combine(relativePath, eventHelper.eventStringId + ".json"), eventHelper);
                        HappyBirthdayModCore.Instance.Monitor.Log(string.Format("Updating birthday event {0} to version {1}", eventHelper.eventStringId, eventHelper.version));
                    }
                    continue;
                }
                else
                {
                    BirthdayEventManager.addEvent(eventHelper);
                    HappyBirthdayModCore.Instance.Helper.Data.WriteJsonFile(Path.Combine(relativePath, eventHelper.eventStringId + ".json"), eventHelper);

                }
            }
            NEED_TO_WRITE_DEFAULT_BIRTHDAY_EVENTS_TO_JSON = false;
            */
            foreach(HappyBirthdayContentPack contentPack in HappyBirthdayModCore.Instance.happyBirthdayContentPackManager.getHappyBirthdayContentPacksForCurrentLanguageCode())
            {
                contentPack.loadBirthdayEvents();
            }
        }

        public static void UpdateEventManager()
        {
            if (BirthdayEventManager != null)
                BirthdayEventManager.update();
        }




        /// <summary>
        /// Gets a string to be displayed during the event.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string GetEventString(string Key)
        {
            string eventString = HappyBirthdayModCore.Instance.translationInfo.getEventString(Key);
            return ReplaceSpecialDialogueTokens(eventString);
        }

        public static string ReplaceSpecialDialogueTokens(string originalString)
        {
            string modifiedString = originalString;
            modifiedString = modifiedString.Replace("{AffectionateSpouseWord}", HappyBirthdayModCore.Instance.birthdayMessages.getAffectionateSpouseWord());
            modifiedString = modifiedString.Replace("{TimeOfDay}", HappyBirthdayModCore.Instance.birthdayMessages.getTimeOfDayString());
            modifiedString = modifiedString.Replace("@", Game1.player.Name);
            return modifiedString;
        }

    }
}
