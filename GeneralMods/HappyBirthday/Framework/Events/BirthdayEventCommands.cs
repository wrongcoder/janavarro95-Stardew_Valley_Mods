using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Omegasis.HappyBirthday.Framework.Menus;
using StardewValley;
using StardustCore.Events;

namespace Omegasis.HappyBirthday.Framework.Events
{
    public class BirthdayEventCommands
    {

        public static void showTranslatedMessage(EventManager EventManager, string EventData)
        {
            string[] splits = EventData.Split(' ');
            showTranslatedMessage(Game1.CurrentEvent, splits);
        }

        public static void showTranslatedMessage(Event Event, GameLocation gameLocation, GameTime Time, string[] EventData)
        {
            showTranslatedMessage(Event, EventData);
        }

        public static void showTranslatedMessage(Event Event, string[] EventData)
        {
            string[] splits = EventData;

            string translationKey = splits[1];
            List<string> eventCommands = Game1.CurrentEvent.eventCommands.ToList();
            EventHelper helper = new EventHelper();
            helper.showMessage(BirthdayEventUtilities.GetEventString(translationKey));
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
            string finalMessage = string.Format(BirthdayEventUtilities.GetEventString(EventData[2]), HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData.BirthdaySeason,
                    HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData.BirthdayDay);
            List<string> eventCommands = Game1.CurrentEvent.eventCommands.ToList();
            EventHelper helper = new EventHelper();
            helper.speak(speakerName, finalMessage);
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
            string finalMessage = BirthdayEventUtilities.GetEventString(EventData[2]);
            List<string> eventCommands = Game1.CurrentEvent.eventCommands.ToList();
            EventHelper helper = new EventHelper();
            helper.speak(speakerName, finalMessage);
            eventCommands.Insert(Game1.CurrentEvent.CurrentCommand + 1, helper.EventData);
            Event.eventCommands = eventCommands.ToArray();
            Event.CurrentCommand++;

        }

        public static void setShouldShowChooseBirthdayMenu(EventManager EventManager, string EventData)
        {
            BirthdayEventUtilities.ShouldAskPlayerForBirthday = true;
            OpenBirthdaySelectionMenu();
        }

        public static void setShouldShowChooseBirthdayMenu(Event Event, GameLocation gameLocation, GameTime Time, string[] EventData)
        {
            BirthdayEventUtilities.ShouldAskPlayerForBirthday = true;

            OpenBirthdaySelectionMenu();
        }

        public static void OpenBirthdaySelectionMenu()
        {

            if (!HappyBirthdayModCore.Instance.birthdayManager.hasChosenBirthday() && Game1.activeClickableMenu == null && BirthdayEventUtilities.ShouldAskPlayerForBirthday)
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
            BirthdayEventUtilities.ShouldAskPlayerForFavoriteGift = true;
            OpenGiftSelectionMenu();
        }

        public static void setShouldShowChooseFavoriteGiftMenu(Event Event, GameLocation gameLocation, GameTime Time, string[] EventData)
        {
            BirthdayEventUtilities.ShouldAskPlayerForFavoriteGift = true;
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
            if (HappyBirthdayModCore.Instance.birthdayManager.isBirthday())
                CurrentEvent.CurrentCommand++;
            else
                CurrentEvent.CurrentCommand += 3;
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
            Event.CurrentCommand += 2;
        }



        /// <summary>
        /// Skips the next event command for the game. Necessary for branching.
        /// </summary>
        /// <param name="EventManager"></param>
        /// <param name="data"></param>
        public static void givePlayerFavoriteGift(EventManager EventManager, string data)
        {

            givePlayerFavoriteGift(Game1.CurrentEvent);
        }

        /// <summary>
        /// Skips the next event command for the game. Necessary for branching.
        /// </summary>
        /// <param name="Event"></param>
        /// <param name="gameLocation"></param>
        /// <param name="Time"></param>
        /// <param name="EventData"></param>
        public static void givePlayerFavoriteGift(Event Event, GameLocation gameLocation, GameTime Time, string[] EventData)
        {
            givePlayerFavoriteGift(Event);
        }

        /// <summary>
        /// Skips the next event command for the game. Necessary for branching.
        /// </summary>
        /// <param name="Event"></param>
        public static void givePlayerFavoriteGift(Event Event)
        {

            Item gift = HappyBirthdayModCore.Instance.giftManager.getSpouseBirthdayGift(Game1.player.spouse);
            Game1.player.addItemByMenuIfNecessaryElseHoldUp(gift);

        }

    }
}
