using CustomNPCFramework.Framework.NPCS;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNPCFramework.Framework.ModularNPCS
{
    public class Sprite
    {
        public AnimatedSprite sprite;
        public string relativePath;

        /// <summary>
        /// A class for handling portraits.
        /// </summary>
        /// <param name="path">The full path to the file.</param>
        public Sprite(string path)
        {
            this.relativePath = Class1.getRelativeDirectory(path);
            this.sprite = new AnimatedSprite(Class1.ModHelper.Content.Load<Texture2D>(this.relativePath));
        }

        /// <summary>
        /// Sets the npc's portrait to be this portrait texture.
        /// </summary>
        /// <param name="npc"></param>
        public void setCharacterSpriteFromThis(ExtendedNPC npc)
        {
            npc.Sprite = this.sprite;
        }

        /// <summary>
        /// Reloads the texture for the NPC portrait.
        /// </summary>
        public void reload()
        {
            this.sprite.Texture = Class1.ModHelper.Content.Load<Texture2D>(this.relativePath);
        }
    }
}
