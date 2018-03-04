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
    public class DirectionalTexture
    {
        public Texture2D leftTexture;
        public Texture2D rightTexture;
        public Texture2D downTexture;
        public Texture2D upTexture;

        public Texture2D currentTexture;

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

        public DirectionalTexture(AssetInfo info, string path, Direction direction = Direction.down)
        {
            this.leftTexture = Class1.ModHelper.Content.Load<Texture2D>(Class1.getShortenedDirectory(Path.Combine(path, info.leftAssetName + ".png")).Remove(0, 1));
            this.rightTexture = Class1.ModHelper.Content.Load<Texture2D>(Class1.getShortenedDirectory(Path.Combine(path, info.rightAssetName + ".png")).Remove(0, 1));
            this.upTexture = Class1.ModHelper.Content.Load<Texture2D>(Class1.getShortenedDirectory(Path.Combine(path, info.upAssetName + ".png")).Remove(0, 1));
            this.downTexture = Class1.ModHelper.Content.Load<Texture2D>(Class1.getShortenedDirectory(Path.Combine(path, info.downAssetName + ".png")).Remove(0, 1));

            if (direction == Direction.left) this.currentTexture = leftTexture;
            if (direction == Direction.right) this.currentTexture = rightTexture;
            if (direction == Direction.up) this.currentTexture = upTexture;
            if (direction == Direction.down) this.currentTexture = downTexture;
        }

        public void setLeft()
        {
            this.currentTexture = leftTexture;
        }

        public void setUp()
        {
            this.currentTexture = upTexture;
        }

        public void setDown()
        {
            this.currentTexture = downTexture;
        }

        public void setRight()
        {
            this.currentTexture = rightTexture;
        }
    }
}
