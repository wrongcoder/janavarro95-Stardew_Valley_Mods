
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;

namespace Omegasis.HappyBirthday.Framework.Events.Preconditions.PlayerSpecific
{
    public class PlayerOnThisTile:EventPrecondition
    {
        public int x;
        public int y;

        public PlayerOnThisTile()
        {

        }

        public PlayerOnThisTile(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public PlayerOnThisTile(Vector2 vec)
        {
            this.x = (int)vec.X;
            this.y = (int)vec.Y;
        }

        public PlayerOnThisTile(Point Point)
        {
            this.x = Point.X;
            this.y = Point.Y;
        }

        public override string ToString()
        {
            return this.precondition_playerOnThisTile();
        }

        /// <summary>
        /// Creates the precondition that the player must be standing on this tile.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public string precondition_playerOnThisTile()
        {
            StringBuilder b = new StringBuilder();
            b.Append("a ");
            b.Append(this.x.ToString());
            b.Append(" ");
            b.Append(this.y.ToString());
            return b.ToString();
        }


        public override bool meetsCondition()
        {
            return (int)Game1.player.getTileLocation().X == this.x && (int)Game1.player.getTileLocation().Y == this.y;
        }

    }
}
