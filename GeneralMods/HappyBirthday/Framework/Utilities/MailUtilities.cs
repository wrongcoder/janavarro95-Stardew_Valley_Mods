using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.HappyBirthday.Framework.Constants;
using StardewValley;
using StardustCore.Utilities;

namespace Omegasis.HappyBirthday.Framework.Utilities
{
    public static class MailUtilities
    {


        public static void EditMailAsset(StardewModdingAPI.IAssetData asset)
        {
            //TODO: This seems to be called every time the mail asset is opened, which isn't super efficient. Find a way to optimize this?

            IDictionary<string, string> data = asset.AsDictionary<string, string>().Data;
            data[MailKeys.MomBirthdayMessageKey] = GetMomsMailMessage();
            data[MailKeys.DadBirthdayMessageKey] = GetDadsMailMessage();

            foreach(string MailKey in MailKeys.GetAllNonBelatedMailKeysExcludingParents())
            {
                UpdateMailMessage(ref data, MailKey);
            }

            foreach(KeyValuePair<string,string> npcNameToMailKey in MailKeys.GetAllBelatedBirthdayMailKeys())
            {
                string npcName = npcNameToMailKey.Key;
                string mailKey = npcNameToMailKey.Value;

                Item gift= HappyBirthday.Instance.giftManager.getNextBirthdayGift(npcName);
                int itemParentSheetIndex = gift.parentSheetIndex;
                int stackSize = gift.Stack;
                string formattedMailItemString = GetItemMailStringFormat(itemParentSheetIndex, stackSize);

                UpdateMailMessage(ref data, mailKey, formattedMailItemString);
            }
        }

        /// <summary>
        /// Creates the mail message from dad.
        /// </summary>
        /// <returns></returns>
        public static string GetDadsMailMessage()
        {
            int moneyToGet = Game1.year==1?  HappyBirthday.Configs.mailConfig.dadBirthdayYear1MoneyGivenAmount: HappyBirthday.Configs.mailConfig.dadBirthdayMoneyGivenAmount;

            string formattedString = string.Format("%item money {0} %%",moneyToGet);

            return string.Format(HappyBirthday.Instance.translationInfo.getMailString(MailKeys.DadBirthdayMessageKey), formattedString);
        }

        /// <summary>
        /// Gets the proper mail string for getting items in the mail.
        /// </summary>
        /// <param name="ParentSheetIndex"></param>
        /// <param name="StackSize"></param>
        /// <returns></returns>
        public static string GetItemMailStringFormat(int ParentSheetIndex, int StackSize)
        {
            return string.Format("%item object {0} {1} %%", ParentSheetIndex, StackSize);
        }

        /// <summary>
        /// Creates the mail message from mom.
        /// </summary>
        /// <returns></returns>
        public static string GetMomsMailMessage()
        {
            int itemToGet = HappyBirthday.Configs.mailConfig.momBirthdayItemGive;
            int stackSizeToGet = HappyBirthday.Configs.mailConfig.momBirthdayItemGiveStackSize;
            string formattedString = GetItemMailStringFormat(itemToGet, stackSizeToGet);

            return string.Format(HappyBirthday.Instance.translationInfo.getMailString(MailKeys.MomBirthdayMessageKey), formattedString);
        }

        /// <summary>
        /// Gets a mail message from the list of loaded strings that are currently selected from the current content pack.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string GetMailMessage(string Key)
        {
            return HappyBirthday.Instance.translationInfo.getMailString(Key);
        }

        /// <summary>
        /// Removes all birthday mail that the player could have seen that was added by this mod.
        /// </summary>
        public static void RemoveAllBirthdayMail()
        {
            foreach(string MailKey in MailKeys.GetAllMailKeys())
            {
                RemoveBirthdayMailIfReceived(MailKey);
            }
        }

        /// <summary>
        /// Removes a piece of mail from the Player's list of seen mail with the given mail key.
        /// </summary>
        /// <param name="MailKey"></param>
        public static void RemoveBirthdayMailIfReceived(string MailKey)
        {
            if (Game1.player.mailReceived.Contains(MailKey))
            {
                Game1.player.mailReceived.Remove(MailKey);
            }
        }

        /// <summary>
        /// Updates a mail message with a given mail key.
        /// </summary>
        /// <param name="MailData"></param>
        /// <param name="MailKey"></param>
        /// <param name="FormattingArgs">The string args to be used in replacing the mail keys.</param>
        public static void UpdateMailMessage(ref IDictionary<string,string> MailData, string MailKey, params string[] FormattingArgs)
        {
            MailData[MailKey] = string.Format(GetMailMessage(MailKey),FormattingArgs);
        }

        /// <summary>
        /// Adds all of the birthday mail to the player's mailbox.
        /// </summary>
        public static void AddBirthdayMailToMailbox()
        {

            Game1.player.mailbox.Add(MailKeys.MomBirthdayMessageKey);
            Game1.player.mailbox.Add(MailKeys.DadBirthdayMessageKey);

            foreach(NPC npc in NPCUtilities.GetAllHumanNpcs())
            {
                string npcName = npc.Name;
                if (Game1.player.friendshipData.ContainsKey(npcName))
                {
                    if (Game1.player.friendshipData[npcName].IsDating())
                    {
                        string mailKey = "";
                        if (npcName.Equals("Abigail"))
                        {
                            if (Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth).ToLowerInvariant().Equals("wed") || Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth).ToLowerInvariant().Equals("wed."))
                            {
                                mailKey = MailKeys.CreateDatingPartyInvitationKey(npcName,"_Wednesday");
                            }
                            else
                            {
                                mailKey = MailKeys.CreateDatingPartyInvitationKey(npcName);
                            }
                        }
                        else
                        {
                            mailKey = MailKeys.CreateDatingPartyInvitationKey(npcName);
                        }
                        if (!string.IsNullOrEmpty(mailKey))
                        {
                            Game1.player.mailbox.Add(mailKey);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds the belated birthday mail to the player's mailbox.
        /// </summary>
        /// <param name="NpcsToReceieveMailFrom"></param>
        public static void AddBelatedBirthdayMailToMailbox(List<string> NpcsToReceieveMailFrom)
        {
            foreach (string npcName in NpcsToReceieveMailFrom)
            {
                if (NPCUtilities.ShouldGivePlayerBirthdayGift(npcName))
                {
                    Game1.addHUDMessage(new HUDMessage("Didn't get birthday gift from npc:" + npcName));
                    string mailKey = MailKeys.CreateBelatedBirthdayWishMailKey(npcName);
                    Game1.player.mailbox.Add(mailKey);
                }
            }
        }

    }
}
