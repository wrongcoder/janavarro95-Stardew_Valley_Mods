using CustomNPCFramework.Framework.NPCS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNPCFramework.Framework.ModularNPCS
{
    public class CharacterAnimationBase
    {

        public CharacterAnimationBase()
        {
        }

        public virtual void setLeft()
        {
        }
        public virtual void setRight()
        {

        }
        public virtual void setUp()
        {

        }
        public virtual void setDown()
        {

        }

        public virtual void reload()
        {
        }

        public virtual void Animate(float animationInterval)
        {

        }


        public virtual void Animate(float animationInterval, bool loop=true)
        {

        }

        /// <summary>
        /// Used to draw the sprite to the screen.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="screenPosition"></param>
        /// <param name="layerDepth"></param>
        public virtual void draw(SpriteBatch b, Vector2 screenPosition, float layerDepth)
        {
          
        }



        /// <summary>
        /// Used to draw the sprite to the screen.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="screenPosition"></param>
        /// <param name="layerDepth"></param>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        /// <param name="c"></param>
        /// <param name="flip"></param>
        /// <param name="scale"></param>
        /// <param name="rotation"></param>
        /// <param name="characterSourceRectOffset"></param>
        public virtual void draw(SpriteBatch b, Vector2 screenPosition, float layerDepth, int xOffset, int yOffset, Color c, bool flip = false, float scale = 1f, float rotation = 0.0f, bool characterSourceRectOffset = false)
        {
           
        }

        /// <summary>
        /// A very verbose asset drawer.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="npc"></param>
        /// <param name="position"></param>
        /// <param name="sourceRectangle"></param>
        /// <param name="color"></param>
        /// <param name="alpha"></param>
        /// <param name="origin"></param>
        /// <param name="scale"></param>
        /// <param name="effects"></param>
        /// <param name="layerDepth"></param>
        public virtual void draw(SpriteBatch b, ExtendedNPC npc, Vector2 position, Rectangle sourceRectangle, Color color, float alpha, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
           
        }

    }
}
