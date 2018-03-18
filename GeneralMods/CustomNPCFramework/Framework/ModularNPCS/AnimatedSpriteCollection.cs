using CustomNPCFramework.Framework.Enums;
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
    /// <summary>
    /// Used to hold all of the sprites for a single asset such as hair or bodies.
    /// </summary>
    public class AnimatedSpriteCollection
    {
        AnimatedSpriteExtended leftSprite;
        AnimatedSpriteExtended rightSprite;
        AnimatedSpriteExtended upSprite;
        AnimatedSpriteExtended downSprite;

        public AnimatedSpriteExtended currentSprite;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="LeftSprite">Left animated sprite for this piece.</param>
        /// <param name="RightSprite">Right animated sprite for this piece.</param>
        /// <param name="UpSprite">Up animated sprite for this piece.</param>
        /// <param name="DownSprite">Down animated sprite for this piece.</param>
        /// <param name="startingSpriteDirection"></param>
        public AnimatedSpriteCollection(AnimatedSpriteExtended LeftSprite,AnimatedSpriteExtended RightSprite,AnimatedSpriteExtended UpSprite,AnimatedSpriteExtended DownSprite,Direction startingSpriteDirection)
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

        public virtual void reload()
        {
            this.leftSprite.reload();
            this.rightSprite.reload();
            this.upSprite.reload();
            this.downSprite.reload();
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
            b.Draw(this.currentSprite.sprite.Texture, screenPosition, new Rectangle?(this.currentSprite.sprite.sourceRect), Color.White, 0.0f, Vector2.Zero, (float)Game1.pixelZoom, this.currentSprite.sprite.currentAnimation == null || !this.currentSprite.sprite.currentAnimation[this.currentSprite.sprite.currentAnimationIndex].flip ? SpriteEffects.None : SpriteEffects.FlipHorizontally, layerDepth);
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
            b.Draw(this.currentSprite.sprite.Texture, screenPosition, new Rectangle?(new Rectangle(this.currentSprite.sprite.sourceRect.X + xOffset, this.currentSprite.sprite.sourceRect.Y + yOffset, this.currentSprite.sprite.sourceRect.Width, this.currentSprite.sprite.sourceRect.Height)), c, rotation, characterSourceRectOffset ? new Vector2((float)(this.currentSprite.sprite.spriteWidth / 2), (float)((double)this.currentSprite.sprite.spriteHeight * 3.0 / 4.0)) : Vector2.Zero, scale, flip || this.currentSprite.sprite.currentAnimation != null && this.currentSprite.sprite.currentAnimation[this.currentSprite.sprite.currentAnimationIndex].flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth);
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
            //DEFINITELY FIX THIS PART. Something is wrong with how these two functions handle the drawing of my npc to the scene.
            //this.draw(b, position, layerDepth);
            b.Draw(this.currentSprite.sprite.Texture,position,this.currentSprite.sprite.sourceRect, color, 0.0f, origin,scale,effects,layerDepth);
            //b.Draw(this.Sprite.Texture, npc.getLocalPosition(Game1.viewport) + new Vector2((float)(this.sprite.spriteWidth * Game1.pixelZoom / 2), (float)(this.GetBoundingBox().Height / 2)) + (this.shakeTimer > 0 ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Microsoft.Xna.Framework.Rectangle?(this.Sprite.SourceRect), Color.White * alpha, this.rotation, new Vector2((float)(this.sprite.spriteWidth / 2), (float)((double)this.sprite.spriteHeight * 3.0 / 4.0)), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip || this.sprite.currentAnimation != null && this.sprite.currentAnimation[this.sprite.currentAnimationIndex].flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float)this.getStandingY() / 10000f));
        }


        /// <summary>
        /// Animate the current sprite. Theoreticlly works from index offset to how many frames
        /// </summary>
        /// <param name="intervalFromCharacter"></param>
        public void Animate(float intervalFromCharacter,bool loop=true)
        {
            //ANIMATE AND UPDATE SOURCE RECTANGLE NOT WORKING!!!! FIGURE THIS OUT!!!!
            //Class1.ModMonitor.Log("Current sprite frame:"+this.currentSprite.sprite.currentFrame.ToString());
            //Class1.ModMonitor.Log("Am I ignoring something??: " + this.currentSprite.sprite.ignoreSourceRectUpdates);
            //Class1.ModMonitor.Log("Current Sprite Source Rect:" + this.currentSprite.sprite.sourceRect);
            //Class1.ModMonitor.Log("Sprite width: " + this.currentSprite.sprite.spriteWidth);
            //Class1.ModMonitor.Log("Sprite width: " + this.currentSprite.sprite.spriteHeight);
            this.Animate(Game1.currentGameTime, 0,2, intervalFromCharacter,this.currentSprite.sprite,loop);
        }

        public virtual bool Animate(GameTime gameTime, int startFrame, int numberOfFrames, float interval, AnimatedSprite sprite, bool loop=true)
        {
            if (sprite.CurrentFrame >= startFrame + numberOfFrames + 1 || sprite.CurrentFrame < startFrame)
                sprite.CurrentFrame = startFrame + sprite.CurrentFrame % numberOfFrames;
            sprite.timer = sprite.timer + (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if ((double)sprite.timer > (double)interval)
            {
                sprite.CurrentFrame = sprite.CurrentFrame + 1;
                sprite.timer = 0.0f;
                if (sprite.CurrentFrame == startFrame + numberOfFrames + 1 || sprite.currentFrame * sprite.spriteWidth >= sprite.Texture.Width)
                {
                    if (loop)
                        sprite.CurrentFrame = startFrame;
                    sprite.UpdateSourceRect();
                    return true;
                }
            }
            this.UpdateSourceRect(sprite);
            return false;
        }

        public virtual void UpdateSourceRect(AnimatedSprite sprite)
        {
            if (sprite.ignoreSourceRectUpdates)
                return;
            sprite.sourceRect.X = sprite.CurrentFrame * sprite.spriteWidth;
            sprite.sourceRect.Y = 0;
            //sprite.SourceRect = new Rectangle(, 0, sprite.spriteWidth, sprite.spriteHeight);
        }

        /// <summary>
        /// Animate the current sprite. Theoreticlly works from index offset to how many frames
        /// </summary>
        /// <param name="intervalFromCharacter"></param>
        public void Animate(float intervalFromCharacter,int startFrame,int endFrame, bool loop)
        {
            this.currentSprite.sprite.loop = loop;
            this.currentSprite.sprite.Animate(Game1.currentGameTime, startFrame, endFrame, intervalFromCharacter);
        }
    }
}
