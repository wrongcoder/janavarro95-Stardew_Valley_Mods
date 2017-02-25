using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;

namespace Revitalize.Aesthetics
{
    


    public class WeatherDebrisPlus
    {
       


        public Rectangle sourceRect;

        public bool blowing;
        private Vector2 position;
        private int which;
        private float dx;
        private float dy;
        private int animationIntervalOffset;

        public WeatherDebrisPlus(Vector2 position,Rectangle SourceRect, int animationOffset, int which, float rotationVelocity, float dx, float dy) 
        {
            this.position = position;
            this.which = which;
            this.dx = dx;
            this.dy = dy;
            sourceRect = SourceRect;
            animationIntervalOffset = animationOffset;
        }

        public WeatherDebrisPlus(Vector2 position, Rectangle SourceRect, int animationOffset, int which, float rotationVelocity, float dx, float dy,bool yup)
        {
            this.position = position;
            this.which = which;
            this.dx = dx;
            this.dy = dy;
          //  sourceRect = SourceRect;
            animationIntervalOffset = animationOffset;

            Log.AsyncC(this.dx);
            Log.AsyncC(this.dy);

            switch (which)
            {
                case 0:
                    this.sourceRect = new Rectangle(352, 1184, 16, 16);
                    this.animationIntervalOffset = (Game1.random.Next(25) - 12) * 2;
                    return;
                case 1:
                    this.sourceRect = new Rectangle(352, 1200, 16, 16);
                    this.animationIntervalOffset = (Game1.random.Next(25) - 12) * 2;
                    return;
                case 2:
                    this.sourceRect = new Rectangle(352, 1216, 16, 16);
                    this.animationIntervalOffset = (Game1.random.Next(25) - 12) * 2;
                    return;
                case 3:
                    this.sourceRect = new Rectangle(391 + 4 * Game1.random.Next(5), 1236, 4, 4);
                    return;
                case 4:
                    this.sourceRect = new Rectangle(338, 400, 8, 8);
                    return;

                default:
                    return;
            }
        }

        public new void update()
        {
            this.update(false);
        }

        public new void update(bool slow)
        {
            this.position.X = this.position.X + (this.dx + (slow ? 0f : WeatherDebris.globalWind));
            this.position.Y = this.position.Y + (this.dy - (slow ? 0f : -0.5f));
            if (this.dy < 0f && !this.blowing)
            {
                this.dy += 0.01f;
            }
            if (!Game1.fadeToBlack && Game1.fadeToBlackAlpha <= 0f)
            {
                if (this.position.X < (float)(-(float)Game1.tileSize - Game1.tileSize / 4))
                {
                    this.position.X = (float)Game1.viewport.Width;
                    this.position.Y = (float)Game1.random.Next(0, Game1.viewport.Height - Game1.tileSize);
                }
                if (this.position.Y > (float)(Game1.viewport.Height + Game1.tileSize / 4))
                {
                    this.position.X = (float)Game1.random.Next(0, Game1.viewport.Width);
                    this.position.Y = (float)(-(float)Game1.tileSize);
                    this.dy = (float)Game1.random.Next(-15, 10) / (slow ? ((Game1.random.NextDouble() < 0.1) ? 5f : 200f) : 50f);
                    this.dx = (float)Game1.random.Next(-10, 0) / (slow ? 200f : 50f);
                }
                else if (this.position.Y < (float)(-(float)Game1.tileSize))
                {
                    this.position.Y = (float)Game1.viewport.Height;
                    this.position.X = (float)Game1.random.Next(0, Game1.viewport.Width);
                }
            }
            if (this.blowing)
            {
                this.dy -= 0.01f;
                if (Game1.random.NextDouble() < 0.006 || this.dy < -2f)
                {
                    this.blowing = false;
                }
            }
            else if (!slow && Game1.random.NextDouble() < 0.001 && Game1.currentSeason != null && (Game1.currentSeason.Equals("spring") || Game1.currentSeason.Equals("summer")))
            {
                this.blowing = true;
            }


            
        }

        public void draw(SpriteBatch b)
        {
            b.Draw(Game1.mouseCursors, this.position, new Rectangle?(this.sourceRect), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1E-06f);

        }
    }
}
