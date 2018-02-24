using CustomNPCFramework.Framework.NPCS;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNPCFramework.Framework.ModularNPCS
{
    public class Portrait
    {
        public Texture2D portrait;
        public string relativePath;

        /// <summary>
        /// A class for handling portraits.
        /// </summary>
        /// <param name="path">The full path to the file.</param>
        public Portrait(string path)
        {
            this.relativePath =Class1.getRelativeDirectory(path);
            this.portrait=Class1.ModHelper.Content.Load<Texture2D>(path);
        }

        /// <summary>
        /// Sets the npc's portrait to be this portrait texture.
        /// </summary>
        /// <param name="npc"></param>
        public void setCharacterPortraitFromThis(ExtendedNPC npc)
        {
            npc.Portrait = this.portrait;
        }

        /// <summary>
        /// Reloads the texture for the NPC portrait.
        /// </summary>
        public void reload()
        {
            this.portrait = Class1.ModHelper.Content.Load<Texture2D>(this.relativePath);
        }
    }
}
