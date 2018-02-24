using CustomNPCFramework.Framework.NPCS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNPCFramework.Framework.ModularNPCS
{
    public class AnimatedSpriteCollection
    {
        AnimatedSprite leftSprite;
        AnimatedSprite rightSprite;
        AnimatedSprite upSprite;
        AnimatedSprite downSprite;

        public AnimatedSprite currentSprite;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="LeftSprite">Left animated sprite for this piece.</param>
        /// <param name="RightSprite">Right animated sprite for this piece.</param>
        /// <param name="UpSprite">Up animated sprite for this piece.</param>
        /// <param name="DownSprite">Down animated sprite for this piece.</param>
        /// <param name="startingSpriteDirection"></param>
        public AnimatedSpriteCollection(AnimatedSprite LeftSprite,AnimatedSprite RightSprite,AnimatedSprite UpSprite,AnimatedSprite DownSprite,Direction startingSpriteDirection)
        {
            this.leftSprite = LeftSprite;
            this.rightSprite = RightSprite;
            this.upSprite = UpSprite;
            this.downSprite = DownSprite;
            if (startingSpriteDirection == Direction.down)
            {
                setDown();
            }
            if (startingSpriteDirection == Direction.left)
            {
                setLeft();
            }
            if (startingSpriteDirection == Direction.right)
            {
                setRight();
            }
            if (startingSpriteDirection == Direction.up)
            {
                setUp();
            }
        }

        /// <summary>
        /// Sets the current 
        /// </summary>
        public void setLeft()
        {
            this.currentSprite = leftSprite;
        }

        public void setRight()
        {
            this.currentSprite = rightSprite;
        }

        public void setDown()
        {
            this.currentSprite = downSprite;
        }

        public void setUp()
        {
            this.currentSprite = upSprite;
        }

        /// <summary>
        /// Used to draw the sprite to the screen.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="screenPosition"></param>
        /// <param name="layerDepth"></param>
        public void draw(SpriteBatch b, Vector2 screenPosition, float layerDepth)
        {
            b.Draw(this.currentSprite.Texture, screenPosition, new Rectangle?(this.currentSprite.sourceRect), Color.White, 0.0f, Vector2.Zero, (float)Game1.pixelZoom, this.currentSprite.currentAnimation == null || !this.currentSprite.currentAnimation[this.currentSprite.currentAnimationIndex].flip ? SpriteEffects.None : SpriteEffects.FlipHorizontally, layerDepth);
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
        public void draw(SpriteBatch b, Vector2 screenPosition, float layerDepth, int xOffset, int yOffset, Color c, bool flip = false, float scale = 1f, float rotation = 0.0f, bool characterSourceRectOffset = false)
        {
            b.Draw(this.currentSprite.Texture, screenPosition, new Rectangle?(new Rectangle(this.currentSprite.sourceRect.X + xOffset, this.currentSprite.sourceRect.Y + yOffset, this.currentSprite.sourceRect.Width, this.currentSprite.sourceRect.Height)), c, rotation, characterSourceRectOffset ? new Vector2((float)(this.currentSprite.spriteWidth / 2), (float)((double)this.currentSprite.spriteHeight * 3.0 / 4.0)) : Vector2.Zero, scale, flip || this.currentSprite.currentAnimation != null && this.currentSprite.currentAnimation[this.currentSprite.currentAnimationIndex].flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth);
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
        public void draw(SpriteBatch b, ExtendedNPC npc, Vector2 position, Rectangle sourceRectangle,Color color, float alpha,Vector2 origin,float scale,SpriteEffects effects,float layerDepth)
        {
            b.Draw(this.currentSprite.Texture,position,sourceRectangle, color* alpha, npc.rotation, origin,scale,effects,layerDepth);
            //b.Draw(this.Sprite.Texture, npc.getLocalPosition(Game1.viewport) + new Vector2((float)(this.sprite.spriteWidth * Game1.pixelZoom / 2), (float)(this.GetBoundingBox().Height / 2)) + (this.shakeTimer > 0 ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Microsoft.Xna.Framework.Rectangle?(this.Sprite.SourceRect), Color.White * alpha, this.rotation, new Vector2((float)(this.sprite.spriteWidth / 2), (float)((double)this.sprite.spriteHeight * 3.0 / 4.0)), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip || this.sprite.currentAnimation != null && this.sprite.currentAnimation[this.sprite.currentAnimationIndex].flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float)this.getStandingY() / 10000f));
        }

        /// <summary>
        /// Animate the current sprite. Theoreticlly works from index offset to how many frames
        /// </summary>
        /// <param name="intervalFromCharacter"></param>
        public void Animate(float intervalFromCharacter)
        {
            this.currentSprite.Animate(Game1.currentGameTime, 0, 3, intervalFromCharacter);
        }
    }
}
