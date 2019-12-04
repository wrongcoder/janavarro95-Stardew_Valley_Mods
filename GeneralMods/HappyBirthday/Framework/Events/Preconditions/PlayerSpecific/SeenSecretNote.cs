using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Omegasis.HappyBirthday.Framework.Events.Preconditions.PlayerSpecific
{
    public class SeenSecretNote:EventPrecondition
    {

        public int id;

        public SeenSecretNote()
        {

        }

        public SeenSecretNote(int ID)
        {
            this.id = ID;
        }

        public override string ToString()
        {
            return this.precondition_playerHasThisSecretNote();
        }

        /// <summary>
        /// Adds in the precondition that the player has this secret note.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string precondition_playerHasThisSecretNote()
        {
            StringBuilder b = new StringBuilder();
            b.Append("S ");
            b.Append(this.id.ToString());
            return b.ToString();
        }

        public override bool meetsCondition()
        {
            return Game1.player.secretNotesSeen.Contains(this.id);
        }
    }
}
