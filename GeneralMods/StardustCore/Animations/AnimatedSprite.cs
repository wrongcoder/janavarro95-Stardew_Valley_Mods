using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardustCore.Animations;

namespace StardustCore.Animations
{
    /// <summary>
    /// Deals with animated sprites.
    /// </summary>
    public class AnimatedSprite
    {
        /// <summary>
        /// The position of the sprite.
        /// </summary>
        public Vector2 position;
        /// <summary>
        /// The animation manager for the sprite.
        /// </summary>
        public AnimationManager animation;
        /// <summary>
        /// The name of the sprite.
        /// </summary>
        public string name;
        /// <summary>
        /// The draw color for the sprite.
        /// </summary>
        public Color color;

        /// <summary>
        /// Constructor.
        /// </summary>
        public AnimatedSprite()
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Name">The name of the sprite.</param>
        /// <param name="Position">The position of the sprite.</param>
        /// <param name="Animation">The animation manager for the sprite.</param>
        /// <param name="DrawColor">The draw color for the sprite.</param>
        public AnimatedSprite(string Name, Vector2 Position, AnimationManager Animation, Color DrawColor)
        {
            this.position = Position;
            this.name = Name;
            this.animation = Animation;
            this.color = DrawColor;
        }

        /// <summary>
        /// Updates the sprite's logic.
        /// </summary>
        /// <param name="Time"></param>
        public virtual void Update(GameTime Time)
        {

        }

        /// <summary>
        /// Draws the sprite to the screen.
        /// </summary>
        /// <param name="b"></param>
        public virtual void draw(SpriteBatch b)
        {
            this.draw(b, 1f, 0f);
        }

        /// <summary>
        /// Draws the sprite to the screen.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="scale"></param>
        /// <param name="depth"></param>
        public virtual void draw(SpriteBatch b, float scale, float depth)
        {
            this.animation.draw(b, this.position, this.color, scale, SpriteEffects.None, depth);
        }

        /// <summary>
        /// Draws the sprite to the screen.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="position"></param>
        /// <param name="scale"></param>
        /// <param name="depth"></param>
        public virtual void draw(SpriteBatch b,Vector2 position ,float scale, float depth)
        {
            this.animation.draw(b, position, this.color, scale, SpriteEffects.None, depth);
        }

        /// <summary>
        /// Draws the sprite to the screen.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="position"></param>
        /// <param name="scale"></param>
        /// <param name="depth"></param>
        public virtual void draw(SpriteBatch b, Vector2 position, Vector2 scale, float depth)
        {
            this.animation.draw(b, position, this.color, scale, SpriteEffects.None, depth);
        }


    }
}
