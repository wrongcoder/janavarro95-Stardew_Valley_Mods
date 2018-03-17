using CustomNPCFramework.Framework.ModularNPCS.CharacterAnimationBases;
using CustomNPCFramework.Framework.NPCS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNPCFramework.Framework.ModularNPCS.ModularRenderers
{
    public class BasicRenderer
    {
        public Dictionary<string, StandardCharacterAnimation> animationList;
        public StandardCharacterAnimation currentAnimation;

        public BasicRenderer(StandardCharacterAnimation standingAnimation,StandardCharacterAnimation walkingAnimation, StandardCharacterAnimation swimmingAnimation)
        {
            animationList = new Dictionary<string, StandardCharacterAnimation>();
            animationList.Add(AnimationKeys.standingKey, standingAnimation);
            animationList.Add(AnimationKeys.walkingKey, walkingAnimation);
            animationList.Add(AnimationKeys.swimmingKey, swimmingAnimation);
            setAnimation(AnimationKeys.standingKey);
        }

        /// <summary>
        /// Sets the animation associated with the key name; If it fails the npc will just default to standing.
        /// </summary>
        /// <param name="key"></param>
        public virtual void setAnimation(string key)
        {
            this.currentAnimation = animationList[key];
            if (this.currentAnimation == null) this.setAnimation(AnimationKeys.standingKey);
        }

        public virtual void setDirection(int facingDirection)
        {
            if (facingDirection == 0) setUp();
            if (facingDirection == 1) setRight();
            if (facingDirection == 2) setDown();
            if (facingDirection == 2) setLeft();
        }

        public virtual void setLeft()
        {
            this.currentAnimation.setLeft();
        }

        public virtual void setRight()
        {
            this.currentAnimation.setRight();
        }

        public virtual void setUp()
        {
            this.currentAnimation.setUp();
        }

        public virtual void setDown()
        {
            this.currentAnimation.setDown();
        }

        public virtual void reloadSprites()
        {
            foreach(var v in this.animationList)
            {
                v.Value.reload();
            }
        }

        /// <summary>
        /// Used to draw the sprite to the screen.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="screenPosition"></param>
        /// <param name="layerDepth"></param>
        public virtual void draw(SpriteBatch b, Vector2 screenPosition, float layerDepth)
        {
            this.currentAnimation.draw(b, screenPosition, layerDepth);
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
            this.currentAnimation.draw(b, screenPosition, layerDepth, xOffset, yOffset, c, flip, scale, rotation, characterSourceRectOffset);
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
            this.currentAnimation.draw(b, npc, position, sourceRectangle, color, alpha, origin, scale, effects, layerDepth);
        }


        public virtual void Animate(float interval)
        {
            this.currentAnimation.Animate(interval);
        }


        /// <summary>
        /// Wrapper for a draw function that accepts rectangles to be null.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="extendedNPC"></param>
        /// <param name="vector21"></param>
        /// <param name="v1"></param>
        /// <param name="white"></param>
        /// <param name="rotation"></param>
        /// <param name="vector22"></param>
        /// <param name="v2"></param>
        /// <param name="spriteEffects"></param>
        /// <param name="v3"></param>
        public virtual void draw(SpriteBatch b, ExtendedNPC extendedNPC, Vector2 vector21, Rectangle? v1, Color white, float rotation, Vector2 vector22, float v2, SpriteEffects spriteEffects, float v3)
        {
            this.draw(b, extendedNPC, vector21, new Rectangle(v1.Value.X,v1.Value.Y,v1.Value.Width,v1.Value.Height), white, rotation, vector22, v2, spriteEffects, v3);
        }
    }
}
