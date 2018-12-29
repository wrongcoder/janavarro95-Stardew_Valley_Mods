using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Framework.Player.Managers
{
    /// <summary>
    /// TODO:
    /// Make chair
    /// animate player better
    /// have it where when player is sitting on chair it is passable so it can't be destoryed underneath
    /// </summary>
    public class SittingInfo
    {
        /// <summary>
        /// If the player is currently sitting.
        /// </summary>
        public bool isSitting;

        /// <summary>
        /// How long a Farmer has sat (in milliseconds)
        /// </summary>
        private int elapsedTime;

        /// <summary>
        /// Gets how long the farmer has sat (in milliseconds).
        /// </summary>
        public int ElapsedTime
        {
            get
            {
                return elapsedTime;
            }
        }

        /// <summary>
        /// Keeps trck of time elapsed.
        /// </summary>
        GameTime timer;


        /// <summary>
        /// The default amount of time a player has to sit to recover some energy/health
        /// </summary>
        private int _sittingSpan;
        /// <summary>
        /// A modified version of how long a player has to sit to recover energy/health;
        /// </summary>
        public int SittingSpan
        {
            get
            {
                return _sittingSpan;
            }
        }



        /// <summary>
        /// Constructor.
        /// </summary>
        public SittingInfo()
        {
            timer = Game1.currentGameTime;
            this._sittingSpan = 10000; 
        }

        /// <summary>
        /// Update the sitting info.
        /// </summary>
        public void update()
        {
            if (Game1.activeClickableMenu != null) return;

            if (Game1.player.isMoving())
            {
                isSitting = false;
                elapsedTime = 0;
            }
            if (isSitting && Game1.player.CanMove)
            {
                showSitting();
                if (timer == null) timer = Game1.currentGameTime;
                elapsedTime += timer.ElapsedGameTime.Milliseconds;
            }

            if (elapsedTime >= SittingSpan)
            {
                elapsedTime %= SittingSpan;
                Game1.player.health++;
                Game1.player.Stamina++;
            }
            Revitalize.ModCore.log(elapsedTime);

        }

        public void showSitting()
        {
            switch (Game1.player.FacingDirection)
            {
                case 0:
                    Game1.player.FarmerSprite.setCurrentSingleFrame(113, (short)32000, false, false);
                    break;
                case 1:
                    Game1.player.FarmerSprite.setCurrentSingleFrame(106, (short)32000, false, false);
                    break;
                case 2:
                    Game1.player.FarmerSprite.setCurrentSingleFrame(107, (short)32000, false, false);
                    break;
                case 3:
                    Game1.player.FarmerSprite.setCurrentSingleFrame(106, (short)32000, false, true);
                    break;
            }
        }

        public void sit(StardewValley.Object obj,Vector2 offset)
        {
            this.isSitting = true;
            Game1.player.Position = (obj.TileLocation * Game1.tileSize + offset);
            Game1.player.position.Y += Game1.tileSize/2;
        }

    }
}
