using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNPCFramework.Framework.ModularNPCS
{
    public class AnimatedSpriteExtended
    {
        public AnimatedSprite sprite;
        public string path;



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="path">Full path to asset.</param>
        /// <param name="currentFrame">Starting animation frame.</param>
        /// <param name="spriteWidth">Sprite width.</param>
        /// <param name="spriteHeight">Sprite height</param>
        public AnimatedSpriteExtended(string path,int currentFrame,int spriteWidth, int spriteHeight)
        {
            this.path = Class1.getRelativeDirectory(path);
            this.sprite=new AnimatedSprite(Class1.ModHelper.Content.Load<Texture2D>(this.path),currentFrame,spriteWidth,spriteHeight);
        }

        /// <summary>
        /// Reloads the asset from disk.
        /// </summary>
        public void reload()
        {
            this.sprite.Texture = Class1.ModHelper.Content.Load<Texture2D>(this.path);
        }
    }
}
