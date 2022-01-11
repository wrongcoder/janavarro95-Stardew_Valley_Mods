using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Utilities;
using Revitalize.Framework.Constants.Mail;
using Revitalize.Framework.World.WorldUtilities;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace Revitalize.Framework.World
{
    /// <summary>
    /// Deals with adding custom mail to the game.
    /// </summary>
    public class MailManager
    {

        public const string newLineInMessageCharacter= "^";
        public const string newPageForMessage = "\n";

        public Dictionary<string, string> mailTitles = new Dictionary<string, string>()
        {
            { MailTitles.HayMakerAvailableForPurchase ,Path.Combine("Content", "Strings", "Mail", "AnimalShopHayMakerCanBePurchased.json")}
        };

        public MailManager()
        {
        }

        public virtual void tryToAddMailToMailbox()
        {
            if(Game1.player.mailReceived.Contains(MailTitles.HayMakerAvailableForPurchase)==false && (ModCore.SaveDataManager.shopSaveData.animalShopSaveData.getHasBuiltTier2OrHigherBarnOrCoop()==true  || BuildingUtilities.HasBuiltTier2OrHigherBarnOrCoop() == true))
            {
                Game1.mailbox.Add(MailTitles.HayMakerAvailableForPurchase);
            }
        }

        public virtual bool canEditAsset(IAssetInfo asset)
        {
            return asset.AssetNameEquals("Data/mail");
        }

        public virtual void editMailAsset(IAssetData asset)
        {
            if (asset.AssetNameEquals("Data/mail"))
            {
                IDictionary<string, string> data = asset.AsDictionary<string, string>().Data;
                string mailContents = this.getMailContentsFromTitle(MailTitles.HayMakerAvailableForPurchase);
                data[MailTitles.HayMakerAvailableForPurchase] = mailContents;
            }
        }

        /// <summary>
        /// Gets the path to the mail asset from the title of the mail.
        /// </summary>
        /// <param name="mailTitle"></param>
        /// <returns></returns>
        public virtual string getMailPathFromTitle(string mailTitle)
        {
            if (this.mailTitles.ContainsKey(mailTitle) == false) return "";

            if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.en)
            {
                return this.mailTitles[mailTitle];
            }
            else
            {
                return this.mailTitles[mailTitle].Replace(".json", LocalizedContentManager.CurrentLanguageCode.ToString() + ".json");
            }
        }

        /// <summary>
        /// Loads the letter contents from a given mail title.
        /// </summary>
        /// <param name="mailTitle"></param>
        /// <returns></returns>
        public virtual string getMailContentsFromTitle(string mailTitle)
        {
            return JsonUtilities.LoadStringDictionaryFile(this.getMailPathFromTitle(MailTitles.HayMakerAvailableForPurchase)).Values.First();
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
            
            if (menuEventArgs.NewMenu != null)
            {
                if (menuEventArgs.NewMenu is LetterViewerMenu)
                {
                    LetterViewerMenu letterViewerMenu = (menuEventArgs.NewMenu as LetterViewerMenu);
                    if (letterViewerMenu.isMail)
                    {
                        if (this.mailTitles.ContainsKey(letterViewerMenu.mailTitle))
                        {
                            letterViewerMenu.mailMessage = this.parseMailMessage(this.getMailContentsFromTitle(letterViewerMenu.mailTitle)).Split("\n").ToList();
                            foreach(string s in letterViewerMenu.mailMessage)
                            {
                                ModCore.log(s);
                            }
                        }
                    }
                }
            }
            
        }


    }
}
