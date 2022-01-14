using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Omegasis.HappyBirthday.Framework.Utilities
{
    public static class MailUtilities
    {

        public static void EditMailAsset(StardewModdingAPI.IAssetData asset)
        {
            IDictionary<string, string> data = asset.AsDictionary<string, string>().Data;
            data["BirthdayMom"] = GetMomsMailMessage();
            data["BirthdayDad"] = GetDadsMailMessage();
            data["BirthdayJunimos"] = HappyBirthday.Instance.translationInfo.getMailString("BirthdayJunimos");
            data["BirthdayDatingPenny"] = HappyBirthday.Instance.translationInfo.getMailString("BirthdayDatingPenny");
            data["BirthdayDatingMaru"] = HappyBirthday.Instance.translationInfo.getMailString("BirthdayDatingMaru");
            data["BirthdayDatingSebastian"] = HappyBirthday.Instance.translationInfo.getMailString("BirthdayDatingSebastian");
            data["BirthdayDatingLeah"] = HappyBirthday.Instance.translationInfo.getMailString("BirthdayDatingLeah");
            data["BirthdayDatingAbigail"] = HappyBirthday.Instance.translationInfo.getMailString("BirthdayDatingAbigail");
            data["BirthdayDatingAbigail_Wednesday"] = HappyBirthday.Instance.translationInfo.getMailString("BirthdayDatingAbigail_Wednesday");
            data["BirthdayDatingEmily"] = HappyBirthday.Instance.translationInfo.getMailString("BirthdayDatingEmily");
            data["BirthdayDatingHaley"] = HappyBirthday.Instance.translationInfo.getMailString("BirthdayDatingHaley");
            data["BirthdayDatingHarvey"] = HappyBirthday.Instance.translationInfo.getMailString("BirthdayDatingHarvey");
            data["BirthdayDatingElliott"] = HappyBirthday.Instance.translationInfo.getMailString("BirthdayDatingElliott");
            data["BirthdayDatingSam"] = HappyBirthday.Instance.translationInfo.getMailString("BirthdayDatingSam");
            data["BirthdayDatingAlex"] = HappyBirthday.Instance.translationInfo.getMailString("BirthdayDatingAlex");
            data["BirthdayDatingShane"] = HappyBirthday.Instance.translationInfo.getMailString("BirthdayDatingShane");

        }

        public static string GetDadsMailMessage()
        {
            int moneyToGet = Game1.year==1?  HappyBirthday.Configs.mailConfig.dadBirthdayYear1MoneyGivenAmount: HappyBirthday.Configs.mailConfig.dadBirthdayMoneyGivenAmount;

            string formattedString = string.Format("%item money {0} %%",moneyToGet);

            return string.Format(HappyBirthday.Instance.translationInfo.getMailString("BirthdayDad"), formattedString);
        }

        public static string GetMomsMailMessage()
        {
            int itemToGet = HappyBirthday.Configs.mailConfig.momBirthdayItemGive;
            int stackSizeToGet = HappyBirthday.Configs.mailConfig.momBirthdayItemGiveStackSize;
            string formattedString = string.Format("%item object {0} {1} %%",itemToGet,stackSizeToGet);

            return string.Format(HappyBirthday.Instance.translationInfo.getMailString("BirthdayMom"), formattedString);

        }

        public static void RemoveAllBirthdayMail()
        {
            //Non character npc birthday mail.
            RemoveBirthdayMailIfReceived("BirthdayMom");
            RemoveBirthdayMailIfReceived("BirthdayDad");
            RemoveBirthdayMailIfReceived("BirthdayJunimos");

            //Dating candidates birthday mail.
            RemoveBirthdayMailIfReceived("BirthdayDatingPenny");
            RemoveBirthdayMailIfReceived("BirthdayDatingMaru");
            RemoveBirthdayMailIfReceived("BirthdayDatingLeah");
            RemoveBirthdayMailIfReceived("BirthdayDatingAbigail");
            RemoveBirthdayMailIfReceived("BirthdayDatingAbigail_Wednesday");
            RemoveBirthdayMailIfReceived("BirthdayDatingEmily");
            RemoveBirthdayMailIfReceived("BirthdayDatingHaley");

            RemoveBirthdayMailIfReceived("BirthdayDatingSebastian");
            RemoveBirthdayMailIfReceived("BirthdayDatingHarvey");
            RemoveBirthdayMailIfReceived("BirthdayDatingElliott");
            RemoveBirthdayMailIfReceived("BirthdayDatingSam");
            RemoveBirthdayMailIfReceived("BirthdayDatingAlex");
            RemoveBirthdayMailIfReceived("BirthdayDatingShane");


        }

        public static void RemoveBirthdayMailIfReceived(string MailKey)
        {
            if (Game1.player.mailReceived.Contains(MailKey))
            {
                Game1.player.mailReceived.Remove(MailKey);
            }
        }

    }
}
