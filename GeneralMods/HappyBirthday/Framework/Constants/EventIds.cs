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

        public static string JunimoCommunityCenterBirthday = CreateEventId("JunimoCommunityCenterBirthday");

        public static string BirthdayDatingPennyTrailer = CreateEventId("BirthdayParty_Dating_Penny_Trailer");
        public static string BirthdayDatingPennyHouse = CreateEventId("BirthdayParty_Dating_Penny_House");
        public static string BirthdayDatingMaru = CreateEventId("BirthdayParty_Dating_Maru");
        public static string BirthdayDatingLeah = CreateEventId("BirthdayParty_Dating_Leah");
        public static string BirthdayDatingAbigailSeedShop = CreateEventId("BirthdayParty_Dating_Abigail_Seedshop");
        public static string BirthdayDatingAbigailMines = CreateEventId("BirthdayParty_Dating_Abigail_Mines");
        public static string BirthdayDatingHaley = CreateEventId("BirthdayParty_Dating_Haley");
        public static string BirthdayDatingEmily = CreateEventId("BirthdayParty_Dating_Emily");


        public static string BirthdayDatingSebastian = CreateEventId("BirthdayParty_Dating_Sebastian");
        public static string BirthdayDatingHarvey = CreateEventId("BirthdayParty_Dating_Harvey");
        public static string BirthdayDatingSam = CreateEventId("BirthdayParty_Dating_Sam");
        public static string BirthdayDatingElliott = CreateEventId("BirthdayParty_Dating_Elliott");
        public static string BirthdayDatingShane = CreateEventId("BirthdayParty_Dating_Shane");
        public static string BirthdayDatingAlex = CreateEventId("BirthdayParty_Dating_Alex");


        public static string CreateEventId(string EventId)
        {
            return PREFIX + EventId;
        }

    }
}
