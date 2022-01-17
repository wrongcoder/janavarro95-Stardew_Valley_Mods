using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Omegasis.HappyBirthday.Framework
{
    public class BirthdayManager
    {

        /// <summary>
        /// The birthday data for the player.
        /// </summary>
        public PlayerData playerBirthdayData;

        public Dictionary<long, PlayerData> othersBirthdays;

        public BirthdayManager()
        {
            this.othersBirthdays = new Dictionary<long, PlayerData>();
        }

        /// <summary>Set the player's birthday/</summary>
        /// <param name="season">The birthday season.</param>
        /// <param name="day">The birthday day.</param>
        public void setBirthday(string season, int day)
        {
            if (this.playerBirthdayData == null)
            {
                this.playerBirthdayData = new PlayerData();
            }
            this.playerBirthdayData.BirthdaySeason = season;
            this.playerBirthdayData.BirthdayDay = day;
        }

        /// <summary>Get whether today is the player's birthday.</summary>
        public bool isBirthday()
        {
            return
                this.playerBirthdayData.BirthdayDay == Game1.dayOfMonth
                && this.playerBirthdayData.BirthdaySeason.ToLower().Equals(Game1.currentSeason.ToLower());
        }

        /// <summary>
        /// Checks to see if the player has choosen their birthday.
        /// </summary>
        /// <returns></returns>
        public bool hasChosenBirthday()
        {
            if (this.playerBirthdayData == null) return false;
            return !string.IsNullOrEmpty(this.playerBirthdayData.BirthdaySeason) && this.playerBirthdayData.BirthdayDay != 0;
        }

        /// <summary>
        /// Checks to see if the player has chosen a favorite gift yet.
        /// </summary>
        /// <returns></returns>
        public bool hasChoosenFavoriteGift()
        {
            if (this.playerBirthdayData == null)
            {
                return false;
            }
            else
            {
                if (string.IsNullOrEmpty(this.playerBirthdayData.favoriteBirthdayGift))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public virtual void removeOtherPlayerBirthdayData(long MultiplayerId)
        {
            this.othersBirthdays.Remove(MultiplayerId);
        }

        public virtual void addOtherPlayerBirthdayData(KeyValuePair<long, PlayerData> birthdayData)
        {
            this.othersBirthdays.Add(birthdayData.Key,birthdayData.Value);
        }

        public virtual void updateOtherPlayerBirthdayData(KeyValuePair<long,PlayerData> birthdayData)
        {
            this.removeOtherPlayerBirthdayData(birthdayData.Key);
            this.addOtherPlayerBirthdayData(birthdayData);
        }
    }
}
