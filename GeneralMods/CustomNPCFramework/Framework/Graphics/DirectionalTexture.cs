using CustomNPCFramework.Framework.Enums;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNPCFramework.Framework.Graphics
{
    /// <summary>
    /// A class that's used to hold textures for different directions.
    /// </summary>
    public class DirectionalTexture
    {
        /// <summary>
        /// The left texture for this group.
        /// </summary>
        public Texture2D leftTexture;
        /// <summary>
        /// The right texture for this group.
        /// </summary>
        public Texture2D rightTexture;
        
        /// <summary>
        /// The down textiure for this group.
        /// </summary>
        public Texture2D downTexture;
        /// <summary>
        /// The up texture for this group.
        /// </summary>
        public Texture2D upTexture;

        /// <summary>
        /// The current texture for this group.
        /// </summary>
        public Texture2D currentTexture;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="left">The left texture to use.</param>
        /// <param name="right">The right texture to use.</param>
        /// <param name="up">The up texture to use.</param>
        /// <param name="down">The down texture to use.</param>
        /// <param name="direction">The direction texture for the sprite to face.</param>
        public DirectionalTexture(Texture2D left, Texture2D right, Texture2D up, Texture2D down, Direction direction=Direction.down)
        {
            this.leftTexture = left;
            this.rightTexture = right;
            this.upTexture = up;
            this.downTexture = down;

            if (direction == Direction.left) this.currentTexture = leftTexture;
            if (direction == Direction.right) this.currentTexture = rightTexture;
            if (direction == Direction.up) this.currentTexture = upTexture;
            if (direction == Direction.down) this.currentTexture = downTexture;


        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="info">A list of name pairings to hold the info for the directional texture.</param>
        /// <param name="path">The path location of the textures to load.</param>
        /// <param name="direction">The direction the textures should be facing.</param>
        public DirectionalTexture(NamePairings info, string path, Direction direction = Direction.down)
        {
            this.leftTexture = Class1.ModHelper.Content.Load<Texture2D>(Class1.getShortenedDirectory(Path.Combine(path, info.leftString + ".png")).Remove(0, 1));
            this.rightTexture = Class1.ModHelper.Content.Load<Texture2D>(Class1.getShortenedDirectory(Path.Combine(path, info.rightString + ".png")).Remove(0, 1));
            this.upTexture = Class1.ModHelper.Content.Load<Texture2D>(Class1.getShortenedDirectory(Path.Combine(path, info.upString + ".png")).Remove(0, 1));
            this.downTexture = Class1.ModHelper.Content.Load<Texture2D>(Class1.getShortenedDirectory(Path.Combine(path, info.downString + ".png")).Remove(0, 1));

            if (direction == Direction.left) this.currentTexture = leftTexture;
            if (direction == Direction.right) this.currentTexture = rightTexture;
            if (direction == Direction.up) this.currentTexture = upTexture;
            if (direction == Direction.down) this.currentTexture = downTexture;
        }

        /// <summary>
        /// Sets the direction of this current texture to left.
        /// </summary>
        public void setLeft()
        {
            this.currentTexture = leftTexture;
        }

        /// <summary>
        /// Sets the direction of this current texture to up.
        /// </summary>
        public void setUp()
        {
            this.currentTexture = upTexture;
        }

        /// <summary>
        /// Sets the direction of this current texture to down.
        /// </summary>
        public void setDown()
        {
            this.currentTexture = downTexture;
        }

        /// <summary>
        /// Sets the direction of this current texture to right.
        /// </summary>
        public void setRight()
        {
            this.currentTexture = rightTexture;
        }

        /// <summary>
        /// Gets the texture from this texture group depending on the direction.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public virtual Texture2D getTextureFromDirection(Direction direction)
        {
            if (direction == Direction.left) return this.leftTexture;
            if (direction == Direction.right) return this.rightTexture;
            if (direction == Direction.up) return this.upTexture;
            if (direction == Direction.down) return this.downTexture;
            return null;
        }
    }
}
