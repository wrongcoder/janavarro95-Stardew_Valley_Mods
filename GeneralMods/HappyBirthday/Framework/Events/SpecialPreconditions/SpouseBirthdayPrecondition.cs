using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.HappyBirthday.Framework.Events.Preconditions;
using StardewValley;

namespace Omegasis.HappyBirthday.Framework.Events.SpecialPreconditions
{
    public class SpouseBirthdayPrecondition:EventPrecondition
    {


        public SpouseBirthdayPrecondition()
        {

        }

        public override bool meetsCondition()
        {
            if (Game1.player.getSpouse() == null) return false;
            else
            {
                NPC spouse = Game1.player.getSpouse();
                if (spouse.isBirthday(Game1.currentSeason, Game1.dayOfMonth)){
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
