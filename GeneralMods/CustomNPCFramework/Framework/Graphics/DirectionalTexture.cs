using System.IO;
using CustomNPCFramework.Framework.Enums;
using StardewModdingAPI;
using StardustCore.UIUtilities;

namespace CustomNPCFramework.Framework.Graphics
{
    /// <summary>A class that's used to hold textures for different directions.</summary>
    public class DirectionalTexture
    {
        /// <summary>The left texture for this group.</summary>
        public Texture2DExtended leftTexture;

        /// <summary>The right texture for this group.</summary>
        public Texture2DExtended rightTexture;

        /// <summary>The down textiure for this group.</summary>
        public Texture2DExtended downTexture;

        /// <summary>The up texture for this group.</summary>
        public Texture2DExtended upTexture;

        /// <summary>The current texture for this group.</summary>
        public Texture2DExtended currentTexture;

        /// <summary>Construct an instance.</summary>
        /// <param name="left">The left texture to use.</param>
        /// <param name="right">The right texture to use.</param>
        /// <param name="up">The up texture to use.</param>
        /// <param name="down">The down texture to use.</param>
        /// <param name="direction">The direction texture for the sprite to face.</param>
        public DirectionalTexture(Texture2DExtended left, Texture2DExtended right, Texture2DExtended up, Texture2DExtended down, Direction direction = Direction.down)
        {
            this.leftTexture = left;
            this.rightTexture = right;
            this.upTexture = up;
            this.downTexture = down;

            switch (direction)
            {
                case Direction.left:
                    this.currentTexture = this.leftTexture;
                    break;

                case Direction.right:
                    this.currentTexture = this.rightTexture;
                    break;

                case Direction.up:
                    this.currentTexture = this.upTexture;
                    break;

                case Direction.down:
                    this.currentTexture = this.downTexture;
                    break;
            }
        }

        public DirectionalTexture(IModHelper helper, NamePairings info, string path, Direction direction = Direction.down)
        {
            string leftString = Class1.getShortenedDirectory(Path.Combine(path, info.leftString + ".png")).Remove(0, 1);
            string rightString = Class1.getShortenedDirectory(Path.Combine(path, info.rightString + ".png")).Remove(0, 1);
            string upString = Class1.getShortenedDirectory(Path.Combine(path, info.upString + ".png")).Remove(0, 1);
            string downString = Class1.getShortenedDirectory(Path.Combine(path, info.downString + ".png")).Remove(0, 1);

            this.leftTexture = new Texture2DExtended(helper, Class1.Manifest, leftString);
            this.rightTexture = new Texture2DExtended(helper, Class1.Manifest, rightString);
            this.upTexture = new Texture2DExtended(helper, Class1.Manifest, upString);
            this.downTexture = new Texture2DExtended(helper, Class1.Manifest, downString);

            switch (direction)
            {
                case Direction.left:
                    this.currentTexture = this.leftTexture;
                    break;

                case Direction.right:
                    this.currentTexture = this.rightTexture;
                    break;

                case Direction.up:
                    this.currentTexture = this.upTexture;
                    break;

                case Direction.down:
                    this.currentTexture = this.downTexture;
                    break;
            }
        }

        /// <summary>Sets the direction of this current texture to left.</summary>
        public void setLeft()
        {
            this.currentTexture = this.leftTexture;
        }

        /// <summary>Sets the direction of this current texture to up.</summary>
        public void setUp()
        {
            this.currentTexture = this.upTexture;
        }

        /// <summary>Sets the direction of this current texture to down.</summary>
        public void setDown()
        {
            this.currentTexture = this.downTexture;
        }

        /// <summary>Sets the direction of this current texture to right.</summary>
        public void setRight()
        {
            this.currentTexture = this.rightTexture;
        }

        /// <summary>Gets the texture from this texture group depending on the direction.</summary>
        /// <param name="direction">The facing direction.</param>
        public virtual Texture2DExtended getTextureFromDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.left:
                    return this.leftTexture;

                case Direction.right:
                    return this.rightTexture;

                case Direction.up:
                    return this.upTexture;

                case Direction.down:
                    return this.downTexture;

                default:
                    return null;
            }
        }
    }
}
