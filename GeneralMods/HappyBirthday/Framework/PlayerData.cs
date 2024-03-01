using System;
using System.Collections.Generic;

namespace Omegasis.HappyBirthday.Framework
{
    /// <summary>The data for the current player.</summary>
    public class PlayerData
    {
        public string PlayersName;

        public long PlayerUniqueMultiplayerId;

        /// <summary>The player's current birthday day.</summary>
        public int BirthdayDay;

        /// <summary>The player's current birthday season.</summary>
        public string BirthdaySeason;

        public HashSet<string> potentialFavoriteGifts  = new HashSet<string>();
    }
}
