using CustomNPCFramework.Framework.Enums;
using Microsoft.Xna.Framework;
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
    /// Used to handle loading different textures and handling opperations on those textures.
    /// </summary>
    public class AssetSheet
    {
        public DirectionalTexture texture;

        public AssetInfo assetInfo;
        public string path;

        public int index;

        public Rectangle currentAsset;

        public AssetSheet(AssetInfo info,string path,Direction direction=Direction.down)
        {
            this.assetInfo = info;

            this.texture = new DirectionalTexture(info, path, direction);
           
            this.path = Class1.getShortenedDirectory(path);
            this.index = 0;
            
        }

        public KeyValuePair<string, Texture2D> getPathTexturePair()
        {
            return new KeyValuePair<string, Texture2D>(this.path, this.texture.currentTexture);
        }


        /// <summary>
        /// Used just to get a copy of this asset sheet.
        /// </summary>
        public AssetSheet clone()
        {
            var asset = new AssetSheet(this.assetInfo,(string)this.path.Clone());
            return asset;
        }


        public void setLeft()
        {
            this.texture.setLeft();
        }

        public void setUp()
        {
            this.texture.setUp();
        }

        public void setDown()
        {
            this.texture.setDown();
        }

        public void setRight()
        {
            this.texture.setRight();
        }
    }
}
