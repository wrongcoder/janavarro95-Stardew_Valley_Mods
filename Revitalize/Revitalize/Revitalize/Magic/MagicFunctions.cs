using Revitalize.Objects;
using Revitalize.Persistance;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Magic
{
   public class MagicFunctions
    {
        public static void castMagic()
        {
         if(Game1.player.ActiveObject as Spell!=null) (Game1.player.ActiveObject as Spell).castMagic();
        }

        public static void showRedMessage(Spell s)
        {
            if (Class1.mouseAction == true) return; //necessary for non repeating functions/spells
           const int baseCost = 5;
            //calculate all costs then factor in player proficiency
            int cost =(int) (  (( ((baseCost) + s.spellCostModifierInt) * s.spellCostModifierPercent) * (1f - PlayerVariables.MagicProficiency))-PlayerVariables.MagicLevel); //+s.extraCostInt-s.spellSaveInt * (1f- PlayerVariables.MagicProficiency) / s.extraCostPercent* s.spellSavePercent;
            Game1.showRedMessage("MAGIC WORKS");
            MagicMonitor.consumeMagic(cost);
        }


    }
}
