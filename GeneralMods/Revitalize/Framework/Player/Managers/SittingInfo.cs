using Microsoft.Xna.Framework;
using StardewValley;

namespace Revitalize.Framework.Player.Managers
{
    // TODO:
    // - Make chair
    // - animate player better
    // - have it where when player is sitting on chair it is passable so it can't be destoryed underneath
    public class SittingInfo
    {
        /// <summary>If the player is currently sitting.</summary>
        public bool isSitting;

        /// <summary>How long a Farmer has sat (in milliseconds)</summary>
        private int elapsedTime;

        /// <summary>Gets how long the farmer has sat (in milliseconds).</summary>
        public int ElapsedTime => this.elapsedTime;

        /// <summary>Keeps trck of time elapsed.</summary>
        GameTime timer;
        
        /// <summary>How long a player has to sit to recover energy/health;</summary>
        public int SittingSpan { get; }


        /// <summary>Construct an instance.</summary>
        public SittingInfo()
        {
            this.timer = Game1.currentGameTime;
            this.SittingSpan = 10000;
        }

        /// <summary>Update the sitting info.</summary>
        public void update()
        {
            if (Game1.activeClickableMenu != null) return;

            if (Game1.player.isMoving())
            {
                this.isSitting = false;
                this.elapsedTime = 0;
            }
            if (this.isSitting && Game1.player.CanMove)
            {
                this.showSitting();
                if (this.timer == null) this.timer = Game1.currentGameTime;
                this.elapsedTime += this.timer.ElapsedGameTime.Milliseconds;
            }

            if (this.elapsedTime >= this.SittingSpan)
            {
                this.elapsedTime %= this.SittingSpan;
                Game1.player.health++;
                Game1.player.Stamina++;
            }
            ModCore.log(this.elapsedTime);

        }

        public void showSitting()
        {
            switch (Game1.player.FacingDirection)
            {
                case 0:
                    Game1.player.FarmerSprite.setCurrentSingleFrame(113);
                    break;
                case 1:
                    Game1.player.FarmerSprite.setCurrentSingleFrame(106);
                    break;
                case 2:
                    Game1.player.FarmerSprite.setCurrentSingleFrame(107);
                    break;
                case 3:
                    Game1.player.FarmerSprite.setCurrentSingleFrame(106);
                    break;
            }
        }

        public void sit(StardewValley.Object obj, Vector2 offset)
        {
            this.isSitting = true;
            Game1.player.Position = (obj.TileLocation * Game1.tileSize + offset);
            Game1.player.position.Y += Game1.tileSize / 2;
        }
    }
}
