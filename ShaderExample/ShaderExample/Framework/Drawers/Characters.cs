using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShaderExample.Framework.Drawers
{
    public class Characters
    {
        /// <summary>
        /// Draw the farmer.
        /// </summary>
        /// <param name="f"></param>
        public static void drawFarmer()
        {
            //Game1.spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            Class1.SetInstanceField(typeof(SpriteBatch), Game1.spriteBatch, ShaderExample.Class1.effect, "customEffect");
            Class1.effect.CurrentTechnique.Passes[0].Apply();
            Game1.player.currentLocation.draw(Game1.spriteBatch);
            //Game1.spriteBatch.End();
        }

        public static void drawCharacters()
        {
            //Game1.spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            Class1.SetInstanceField(typeof(SpriteBatch), Game1.spriteBatch, ShaderExample.Class1.effect, "customEffect");
            Class1.effect.CurrentTechnique.Passes[0].Apply();
            foreach(var character in Game1.player.currentLocation.characters)
            {
                character.draw(Game1.spriteBatch);
            }
            //Game1.spriteBatch.End();
        }

        

    }
}
