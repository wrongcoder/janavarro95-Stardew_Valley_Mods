using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardustCore.UIUtilities.MenuComponents.ComponentsV2.Buttons
{
    public class AnimatedButton
    {
        public Animations.AnimatedSprite sprite;

        private Rectangle defaultBounds;
        public Rectangle bounds
        {
            get
            {
                return new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)(this.defaultBounds.Width * this.scale), (int)(this.defaultBounds.Height * this.scale));
            }
        }
        public float scale;

        public string label;
        public string name;
        public string hoverText;

        /// <summary>
        /// The position of the bounding box.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return this.sprite.position;
            }
            set
            {
                this.sprite.position = value;
                this.defaultBounds.X = (int)this.sprite.position.X;
                this.defaultBounds.Y = (int)this.sprite.position.Y;
            }
        }

        public AnimatedButton(Animations.AnimatedSprite Sprite, Rectangle DefaultBounds, float Scale)
        {

            this.sprite = Sprite;
            this.scale = Scale;
            this.defaultBounds = DefaultBounds;
            this.label = "";
            this.name = "";
            this.hoverText = "";
        }

        public void update(GameTime time)
        {

        }

        public void draw(SpriteBatch b)
        {
            this.sprite.draw(b);
        }

        public void draw(SpriteBatch b, float Depth)
        {
            this.sprite.draw(b, this.scale, Depth);
        }

        public bool containsPoint(int x, int y)
        {
            return this.bounds.Contains(x, y);
        }

        public bool receiveLeftClick(int x, int y)
        {
            return this.containsPoint(x, y);
        }
        public bool receiveRightClick(int x, int y)
        {
            return this.containsPoint(x, y);
        }
        public bool receiveHover(int x, int y)
        {
            return this.containsPoint(x, y);
        }
    }
}
