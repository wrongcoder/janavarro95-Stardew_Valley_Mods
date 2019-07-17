using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Revitalize.Framework.Minigame.SeasideScrambleMinigame
{
    public class SSCCamera
    {

        public xTile.Dimensions.Rectangle viewport;

        public SSCCamera()
        {
            this.viewport = new xTile.Dimensions.Rectangle(StardewValley.Game1.viewport);
        }

        public void snapToPosition(Vector2 position)
        {
            this.viewport.Location = new xTile.Dimensions.Location((int)position.X, (int)position.Y);
        }

        public void centerOnPosition(Vector2 position)
        {
            this.viewport.Location = new xTile.Dimensions.Location((int)position.X - (int)(SeasideScramble.self.camera.viewport.Width / 2), (int)position.Y - (int)(SeasideScramble.self.camera.viewport.Height / 2));
        }

        public void update(GameTime time)
        {

        }
    }
}
