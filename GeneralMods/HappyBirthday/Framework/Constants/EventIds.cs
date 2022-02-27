using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.HappyBirthday.Framework.Constants
{
    public class EventIds
    {
        public static string PREFIX = "HappyBirthday_Events_";

        //Dating bachelorette birthdays.
        public static string BirthdayDatingAbigailSeedShop = CreateEventId("BirthdayParty_Dating_Abigail_Seedshop");
        public static string BirthdayDatingAbigailMines = CreateEventId("BirthdayParty_Dating_Abigail_Mines");
        public static string BirthdayDatingEmily = CreateEventId("BirthdayParty_Dating_Emily");
        public static string BirthdayDatingHaley = CreateEventId("BirthdayParty_Dating_Haley");
        public static string BirthdayDatingLeah = CreateEventId("BirthdayParty_Dating_Leah");
        public static string BirthdayDatingMaru = CreateEventId("BirthdayParty_Dating_Maru");
        public static string BirthdayDatingPennyTrailer = CreateEventId("BirthdayParty_Dating_Penny_Trailer");
        public static string BirthdayDatingPennyHouse = CreateEventId("BirthdayParty_Dating_Penny_House");

        //Dating bacehelor birthdays.
        public static string BirthdayDatingAlex = CreateEventId("BirthdayParty_Dating_Alex");
        public static string BirthdayDatingHarvey = CreateEventId("BirthdayParty_Dating_Harvey");
        public static string BirthdayDatingElliott = CreateEventId("BirthdayParty_Dating_Elliott");
        public static string BirthdayDatingSebastian = CreateEventId("BirthdayParty_Dating_Sebastian");
        public static string BirthdayDatingSam = CreateEventId("BirthdayParty_Dating_Sam");
        public static string BirthdayDatingShane = CreateEventId("BirthdayParty_Dating_Shane");


        //Birthday setup events.
        public static string AskPlayerForBirthday = CreateEventId("AskPlayerForBirthday");


        public static string AskPlayerForFavoriteGift_Farmhouse_1_Abigail = CreateEventId("AskPlayerForFavoriteGift_Farmhouse_1_Abigail");
        public static string AskPlayerForFavoriteGift_Farmhouse_1_Emily = CreateEventId("AskPlayerForFavoriteGift_Farmhouse_1_Emily");
        public static string AskPlayerForFavoriteGift_Farmhouse_1_Haley = CreateEventId("AskPlayerForFavoriteGift_Farmhouse_1_Haley");
        public static string AskPlayerForFavoriteGift_Farmhouse_1_Leah = CreateEventId("AskPlayerForFavoriteGift_Farmhouse_1_Leah");
        public static string AskPlayerForFavoriteGift_Farmhouse_1_Penny = CreateEventId("AskPlayerForFavoriteGift_Farmhouse_1_Penny");
        public static string AskPlayerForFavoriteGift_Farmhouse_1_Maru = CreateEventId("AskPlayerForFavoriteGift_Farmhouse_1_Maru");
        public static string AskPlayerForFavoriteGift_Farmhouse_1_Alex = CreateEventId("AskPlayerForFavoriteGift_Farmhouse_1_Alex");
        public static string AskPlayerForFavoriteGift_Farmhouse_1_Harvey = CreateEventId("AskPlayerForFavoriteGift_Farmhouse_1_Harvey");
        public static string AskPlayerForFavoriteGift_Farmhouse_1_Elliott = CreateEventId("AskPlayerForFavoriteGift_Farmhouse_1_Elliott");
        public static string AskPlayerForFavoriteGift_Farmhouse_1_Sebastian = CreateEventId("AskPlayerForFavoriteGift_Farmhouse_1_Sebastian");
        public static string AskPlayerForFavoriteGift_Farmhouse_1_Sam = CreateEventId("AskPlayerForFavoriteGift_Farmhouse_1_Sam");
        public static string AskPlayerForFavoriteGift_Farmhouse_1_Shane = CreateEventId("AskPlayerForFavoriteGift_Farmhouse_1_Shane");


        public static string AskPlayerForFavoriteGift_Farmhouse_2_Abigail = CreateEventId("AskPlayerForFavoriteGift_Farmhouse_2_Abigail");
        public static string AskPlayerForFavoriteGift_Farmhouse_2_Emily = CreateEventId("AskPlayerForFavoriteGift_Farmhouse_2_Emily");
        public static string AskPlayerForFavoriteGift_Farmhouse_2_Haley = CreateEventId("AskPlayerForFavoriteGift_Farmhouse_2_Haley");
        public static string AskPlayerForFavoriteGift_Farmhouse_2_Leah = CreateEventId("AskPlayerForFavoriteGift_Farmhouse_2_Leah");
        public static string AskPlayerForFavoriteGift_Farmhouse_2_Penny = CreateEventId("AskPlayerForFavoriteGift_Farmhouse_2_Penny");
        public static string AskPlayerForFavoriteGift_Farmhouse_2_Maru = CreateEventId("AskPlayerForFavoriteGift_Farmhouse_2_Maru");
        public static string AskPlayerForFavoriteGift_Farmhouse_2_Alex = CreateEventId("AskPlayerForFavoriteGift_Farmhouse_2_Alex");
        public static string AskPlayerForFavoriteGift_Farmhouse_2_Harvey = CreateEventId("AskPlayerForFavoriteGift_Farmhouse_2_Harvey");
        public static string AskPlayerForFavoriteGift_Farmhouse_2_Elliott = CreateEventId("AskPlayerForFavoriteGift_Farmhouse_2_Elliott");
        public static string AskPlayerForFavoriteGift_Farmhouse_2_Sebastian = CreateEventId("AskPlayerForFavoriteGift_Farmhouse_2_Sebastian");
        public static string AskPlayerForFavoriteGift_Farmhouse_2_Sam = CreateEventId("AskPlayerForFavoriteGift_Farmhouse_2_Sam");
        public static string AskPlayerForFavoriteGift_Farmhouse_2_Shane = CreateEventId("AskPlayerForFavoriteGift_Farmhouse_2_Shane");



        //Npc birthday events.
        public static string JojaMartBirthday = CreateEventId("JojaMartBirthday");
        public static string JunimoCommunityCenterBirthday = CreateEventId("JunimoCommunityCenterBirthday");

        public static string SaloonBirthdayParty_Year1 = CreateEventId("SaloonBirthdayParty_Year1");
        public static string SaloonBirthdayParty_Year2 = CreateEventId("SaloonBirthdayParty_Year2");


        //Married birthday events.
        public static string Married_AbigailBirthdayParty_Farmhouse_1 = CreateEventId("Married_BirthdayParty_Abigail_Farmhouse_1");
        public static string Married_AbigailBirthdayParty_Farmhouse_2 = CreateEventId("Married_BirthdayParty_Abigail_Farmhouse_2");
        public static string Married_EmilyBirthdayParty_Farmhouse_1 = CreateEventId("Married_BirthdayParty_Emily_Farmhouse_1");
        public static string Married_EmilyBirthdayParty_Farmhouse_2 = CreateEventId("Married_BirthdayParty_Emily_Farmhouse_2");
        public static string Married_HaleyBirthdayParty_Farmhouse_1 = CreateEventId("Married_BirthdayParty_Haley_Farmhouse_1");
        public static string Married_HaleyBirthdayParty_Farmhouse_2 = CreateEventId("Married_BirthdayParty_Haley_Farmhouse_2");
        public static string Married_LeahBirthdayParty_Farmhouse_1 = CreateEventId("Married_BirthdayParty_Leah_Farmhouse_1");
        public static string Married_LeahBirthdayParty_Farmhouse_2 = CreateEventId("Married_BirthdayParty_Leah_Farmhouse_2");
        public static string Married_MaruBirthdayParty_Farmhouse_1 = CreateEventId("Married_BirthdayParty_Maru_Farmhouse_1");
        public static string Married_MaruBirthdayParty_Farmhouse_2 = CreateEventId("Married_BirthdayParty_Maru_Farmhouse_2");
        public static string Married_PennyBirthdayParty_Farmhouse_1 = CreateEventId("Married_BirthdayParty_Emily_Farmhouse_1");
        public static string Married_PennyBirthdayParty_Farmhouse_2 = CreateEventId("Married_BirthdayParty_Penny_Farmhouse_2");
        public static string Married_AlexBirthdayParty_Farmhouse_1 = CreateEventId("Married_BirthdayParty_Alex_Farmhouse_1");
        public static string Married_AlexBirthdayParty_Farmhouse_2 = CreateEventId("Married_BirthdayParty_Alex_Farmhouse_2");
        public static string Married_HarveyBirthdayParty_Farmhouse_1 = CreateEventId("Married_BirthdayParty_Harvey_Farmhouse_1");
        public static string Married_HarveyBirthdayParty_Farmhouse_2 = CreateEventId("Married_BirthdayParty_Harvey_Farmhouse_2");
        public static string Married_ElliottBirthdayParty_Farmhouse_1 = CreateEventId("Married_BirthdayParty_Elliott_Farmhouse_1");
        public static string Married_ElliottBirthdayParty_Farmhouse_2 = CreateEventId("Married_BirthdayParty_Elliott_Farmhouse_2");
        public static string Married_SebastianBirthdayParty_Farmhouse_1 = CreateEventId("Married_BirthdayParty_Sebastian_Farmhouse_1");
        public static string Married_SebastianBirthdayParty_Farmhouse_2 = CreateEventId("Married_BirthdayParty_Sebastian_Farmhouse_2");
        public static string Married_SamBirthdayParty_Farmhouse_1 = CreateEventId("Married_BirthdayParty_Sam_Farmhouse_1");
        public static string Married_SamBirthdayParty_Farmhouse_2 = CreateEventId("Married_BirthdayParty_Sam_Farmhouse_2");
        public static string Married_ShaneBirthdayParty_Farmhouse_1 = CreateEventId("Married_BirthdayParty_Shane_Farmhouse_1");
        public static string Married_ShaneBirthdayParty_Farmhouse_2 = CreateEventId("Married_BirthdayParty_Shane_Farmhouse_2");



        public static string CreateEventId(string EventId)
        {
            return PREFIX + EventId;
        }

    }
}
