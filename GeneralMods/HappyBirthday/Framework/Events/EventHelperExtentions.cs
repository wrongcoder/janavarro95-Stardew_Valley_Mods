using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using StardustCore.Events;

namespace Omegasis.HappyBirthday.Framework.Events
{
    public static class EventHelperExtentions
    {
        /// <summary>
        /// Adds the event command to show the translated message.
        /// </summary>
        /// <param name="eventHelper"></param>
        /// <param name="MessageKey"></param>
        public static void addTranslatedMessageToBeShown(this EventHelper eventHelper, string MessageKey)
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
        public static void addAskForBirthday(this EventHelper eventHelper)
        {
            StringBuilder b = new StringBuilder();
            b.Append("Omegasis.HappyBirthday.Events.ShowBirthdaySelectionMenu");
            eventHelper.addEventData(b);
        }

        /// <summary>
        /// Adds the event command to ask the player for their favorite gift.
        /// </summary>
        /// <param name="eventHelper"></param>
        public static void addAskForFavoriteGift(this EventHelper eventHelper)
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
        public static void speakIfTodayIsPlayersBirthday(this EventHelper helper, string SpeakerName, string TrueMessageKey, string FalseMessageKey)
        {
            StringBuilder b = new StringBuilder();
            b.Append("Omegasis.HappyBirthday.Events.SpeakIfTodayIsPlayersBirthday");
            helper.addEventData(b);
            speakWithTranslatedMessage(helper, SpeakerName, TrueMessageKey);
            skipNextCommand(helper);
            speakWithBirthdayIncluded(helper, SpeakerName, FalseMessageKey); //TODO: CHange this to GetEventString()


        }

        /// <summary>
        /// Adds the event command to skip the next command in the current event. Used for event branching.
        /// </summary>
        /// <param name="eventHelper"></param>
        public static void skipNextCommand(this EventHelper eventHelper)
        {
            StringBuilder b = new StringBuilder();
            b.Append("Omegasis.HappyBirthday.Events.SkipNextCommand");
            eventHelper.addEventData(b);
        }

        public static void speakWithBirthdayIncluded(this EventHelper eventHelper, string SpeakerName, string OriginalMessageKey)
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
        public static void speakWithTranslatedMessage(this EventHelper eventHelper, string SpeakerName, string MessageKey)
        {
            StringBuilder b = new StringBuilder();
            b.Append("Omegasis.HappyBirthday.Events.SpeakWithTranslatedMessage");
            b.Append(" ");
            b.Append(SpeakerName);
            b.Append(" ");
            b.Append(MessageKey);
            eventHelper.addEventData(b);
        }

        public static void givePlayerFavoriteGift(this EventHelper eventHelper)
        {
            StringBuilder b = new StringBuilder();
            b.Append("Omegasis.HappyBirthday.Events.GivePlayerFavoriteGift");
            eventHelper.addEventData(b);
        }

        /// <summary>
        /// Creates the command to get a specific translation string and have a given npc speak that returned string.
        /// </summary>
        /// <param name="eventHelper"></param>
        /// <param name="SpeakerName"></param>
        /// <param name="MessageKey"></param>
        public static void speakWithTranslatedMessage(this EventHelper eventHelper, NPC Speaker, string MessageKey)
        {
            speakWithTranslatedMessage(eventHelper, Speaker.Name, MessageKey);
        }


    }
}
