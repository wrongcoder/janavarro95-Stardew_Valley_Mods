using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNPCFramework.Framework.ModularNPCS
{
    /// <summary>
    /// Used as a wrapper for the AnimatedSprite class.
    /// </summary>
    public class AnimatedSpriteExtended
    {
        /// <summary>
        /// The actual sprite of the object.
        /// </summary>
        public AnimatedSprite sprite;
        /// <summary>
        /// The path to the texture to load the sprite from.
        /// </summary>
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
        /// Constructor.
        /// </summary>
        /// <param name="texture">The texture for the sprite.</param>
        /// <param name="texture">Path used for retaining texture location on disk.</param>
        /// <param name="currentFrame">Starting animation frame.</param>
        /// <param name="spriteWidth">Sprite width.</param>
        /// <param name="spriteHeight">Sprite height</param>
        public AnimatedSpriteExtended(Texture2D texture,string path ,int currentFrame, int spriteWidth, int spriteHeight)
        {
            this.path = Class1.getRelativeDirectory(path);
            this.sprite = new AnimatedSprite(texture, currentFrame, spriteWidth, spriteHeight);
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
