using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.HappyBirthday.Framework.Utilities;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Monsters;

namespace Omegasis.HappyBirthday.Framework
{
    public class BirthdayManager
    {

        /// <summary>
        /// The birthday data for the player.
        /// </summary>
        public PlayerData playerBirthdayData;

        /// <summary>
        /// The birthdays for other players.
        /// </summary>
        public Dictionary<long, PlayerData> othersBirthdays;

        /// <summary>Whether we've already checked for and (if applicable) set up the player's birthday today.</summary>
        private bool checkedForBirthday;

        /// <summary>The queue of villagers who haven't given a gift yet.</summary>
        public Dictionary<string, VillagerInfo> villagerQueue;

        public BirthdayManager()
        {
            this.othersBirthdays = new Dictionary<long, PlayerData>();
            this.villagerQueue = new Dictionary<string, VillagerInfo>();
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
            this.othersBirthdays.Add(birthdayData.Key, birthdayData.Value);
        }

        public virtual void updateOtherPlayerBirthdayData(KeyValuePair<long, PlayerData> birthdayData)
        {
            this.removeOtherPlayerBirthdayData(birthdayData.Key);
            this.addOtherPlayerBirthdayData(birthdayData);
        }

        /// <summary>Reset the queue of villager names.</summary>
        public void ResetVillagerQueue()
        {

            if (this.villagerQueue.Count > 0)
            {
                foreach (string npcName in this.villagerQueue.Keys)
                {
                    if (NPCUtilities.ShouldGivePlayerBirthdayGift(npcName))
                    {
                        Game1.addHUDMessage(new HUDMessage("Didn't get birthday gift from npc:" + npcName));
                    }
                }
            }

            this.villagerQueue.Clear();

            foreach (GameLocation location in Game1.locations)
            {
                foreach (NPC npc in location.characters)
                {
                    if (npc is Child || npc is Horse || npc is Junimo || npc is Monster || npc is Pet)
                        continue;
                    if (this.villagerQueue.ContainsKey(npc.Name))
                        continue;
                    this.villagerQueue.Add(npc.Name, new VillagerInfo());
                }
            }
        }

        /// <summary>
        /// Sets up the players birthday.
        /// </summary>
        public virtual void setUpPlayersBirthday()
        {
            if (this.isBirthday())
            {
                string starMessage = HappyBirthday.Instance.translationInfo.getTranslatedContentPackString("Happy Birthday: Star Message");
                Messages.ShowStarMessage(starMessage);
                MultiplayerUtilities.SendBirthdayMessageToOtherPlayers();

                MailUtilities.AddBirthdayMailToMailbox();

                foreach (NPC npc in NPCUtilities.GetAllNpcs())
                {
                    if (npc is Child || npc is Horse || npc is Junimo || npc is Monster || npc is Pet)
                        continue;
                    string message = HappyBirthday.Instance.birthdayMessages.getBirthdayMessage(npc.Name);
                    Dialogue d = new Dialogue(message, npc);
                    npc.CurrentDialogue.Push(d);
                    if (npc.CurrentDialogue.ElementAt(0) != d) npc.setNewDialogue(message);
                }
            }
        }

        /// <summary>
        /// Sees if the mod has checked for the players birthday already or not.
        /// </summary>
        /// <returns></returns>
        public virtual bool hasCheckedForBirthday()
        {
            return this.checkedForBirthday;
        }

        /// <summary>
        /// Checks to see if the mod has checked for if the player's birthday is set.
        /// </summary>
        /// <param name="Value"></param>
        public virtual void setCheckedForBirthday(bool Value)
        {
            this.checkedForBirthday = Value;
        }


        /// <summary>
        /// Checks to see if a character is in the villager queue.
        /// </summary>
        /// <param name="NpcName"></param>
        /// <returns></returns>
        public virtual bool isVillagerInQueue(string NpcName)
        {
            return this.villagerQueue.ContainsKey(NpcName);
        }

        /// <summary>
        /// Checks to see if a villager in the villager queue has given a birthday wish.
        /// </summary>
        /// <param name="NpcName">The name of the npc.</param>
        /// <returns></returns>
        public virtual bool hasGivenBirthdayWish(string NpcName)
        {
            if (this.isVillagerInQueue(NpcName) == false) return false;
            return this.villagerQueue[NpcName].hasGivenBirthdayWish;
        }

        /// <summary>
        /// Checks to see if a villager in the villager queue has given a birthday gift.
        /// </summary>
        /// <param name="NpcName">The name of the npc.</param>
        /// <returns></returns>
        public virtual bool hasGivenBirthdayGift(string NpcName)
        {
            if (this.isVillagerInQueue(NpcName) == false) return false;
            return this.villagerQueue[NpcName].hasGivenBirthdayGift;
        }
    }
}
