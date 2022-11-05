using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Constants.Mail;
using Omegasis.Revitalize.Framework.Constants.PathConstants;
using Omegasis.Revitalize.Framework.ContentPacks;
using Omegasis.Revitalize.Framework.Utilities;
using Omegasis.Revitalize.Framework.Utilities.JsonContentLoading;
using Omegasis.Revitalize.Framework.World.WorldUtilities;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace Omegasis.Revitalize.Framework.World.Mail
{
    /// <summary>
    /// Deals with adding custom mail to the game.
    ///
    /// In order to add new mail contents to the game, you must do the following.
    /// 1. Create a new .json file with the mail contents.
    /// 2. Add the mail title constant string and the path to the mail file into the <see cref="mailTitles"/> dictionary.
    /// 3. Update the <see cref="tryToAddMailToMailbox"/> method as that will check for specific conditions 
    /// </summary>
    public class MailManager
    {

        public const string newLineInMessageCharacter = "^";
        public const string newPageForMessage = "\n";

        public MailManager()
        {


        }


        /// <summary>
        /// Tries to add mail to the Player's mailbox when certain events happen for the mod.
        /// </summary>
        public virtual void tryToAddMailToMailbox()
        {
            if (!this.hasOrWillPlayerReceivedThisMail(MailTitles.HayMakerAvailableForPurchase) && (RevitalizeModCore.SaveDataManager.shopSaveData.animalShopSaveData.getHasBuiltTier2OrHigherBarnOrCoop() || BuildingUtilities.HasBuiltTier2OrHigherBarnOrCoop()))
                Game1.mailbox.Add(MailTitles.HayMakerAvailableForPurchase);
            if (!this.hasOrWillPlayerReceivedThisMail(MailTitles.SiloRefillServiceAvailable) && Utility.numSilos() >= 1)
                Game1.mailbox.Add(MailTitles.SiloRefillServiceAvailable);

            if (!this.hasOrWillPlayerReceivedThisMail(MailTitles.AutomaticFarmingSystemAvailableForPurchase) && Game1.player.FarmingLevel >= 10)
                Game1.mailbox.Add(MailTitles.AutomaticFarmingSystemAvailableForPurchase);

            if (!this.hasOrWillPlayerReceivedThisMail(MailTitles.ElectricFurnaceCanBePurchased) && RevitalizeModCore.SaveDataManager.playerSaveData.hasObtainedBatteryPack)
                Game1.mailbox.Add(MailTitles.ElectricFurnaceCanBePurchased);
        }

        /// <summary>
        /// Checks to see if the player has received a given peice of mail.
        /// </summary>
        /// <param name="MailTitle">The title of the mail to check.</param>
        /// <returns></returns>
        public virtual bool hasOrWillPlayerReceivedThisMail(string MailTitle)
        {
            return Game1.player.mailReceived.Contains(MailTitle) || Game1.player.mailbox.Contains(MailTitle) || Game1.player.mailForTomorrow.Contains(MailTitle);
        }

        public virtual bool canEditAsset(IAssetInfo asset)
        {
            return asset.NameWithoutLocale.IsEquivalentTo("Data/mail");
        }

        public virtual void editMailAsset(IAssetData asset)
        {
            if (asset.NameWithoutLocale.IsEquivalentTo("Data/mail"))
            {
                IDictionary<string, string> data = asset.AsDictionary<string, string>().Data;
                foreach (MailInfo mail in this.getMailInfo().Values)
                    data[mail.mailTitle] = mail.message;
            }
        }

        /// <summary>
        /// Gets all possible mail added in by content packs.
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string,MailInfo> getMailInfo()
        {
            List<RevitalizeContentPack> contentPacks = RevitalizeModCore.ModContentManager.revitalizeContentPackManager.getContentPacksForCurrentLanguageCode();

            Dictionary<string, MailInfo> mails = new Dictionary<string, MailInfo>();

            foreach (RevitalizeContentPack contentPack in contentPacks)
            {
                foreach(KeyValuePair<string,MailInfo> letter in contentPack.mail)
                {
                    if (!mails.ContainsKey(letter.Key))
                    {
                        mails.Add(letter.Key, letter.Value);
                    }
                }
            }
            return mails;
        }

        /// <summary>
        /// Parses out the mail message for the menu.
        /// </summary>
        /// <param name="MailMessageText"></param>
        /// <returns></returns>
        public virtual string parseMailMessage(string MailMessageText)
        {
            return MailMessageText.Replace("@", Game1.player.name.Value);
        }

        /// <summary>
        /// Occurs when a new menu is opened.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="menuEventArgs"></param>
        public virtual void onNewMenuOpened(object sender, StardewModdingAPI.Events.MenuChangedEventArgs menuEventArgs)
        {
            if (menuEventArgs.NewMenu != null && menuEventArgs.NewMenu is LetterViewerMenu)
            {
                LetterViewerMenu letterViewerMenu = menuEventArgs.NewMenu as LetterViewerMenu;
                Dictionary<string, MailInfo> mail = this.getMailInfo();

                if (letterViewerMenu.isMail && mail.ContainsKey(letterViewerMenu.mailTitle))
                {
                    letterViewerMenu.mailMessage = this.parseMailMessage(mail[letterViewerMenu.mailTitle].message).Split("\n").ToList();
                }
            }
        }
    }
}
