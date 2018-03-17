using CustomNPCFramework.Framework.Enums;
using CustomNPCFramework.Framework.Graphics.TextureGroups;
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
        public TextureGroups.TextureGroup textures;

        public AssetInfo assetInfo;
        public string path;

        public int index;

        public Rectangle currentAsset;

        public AssetSheet(AssetInfo info,string path,Direction direction=Direction.down)
        {
            this.assetInfo = info;
            this.textures = new TextureGroup(info,path,direction);
            try
            {
                this.path = Class1.getShortenedDirectory(path);
            }
            catch(Exception err)
            {
                this.path = path;
            }
            this.index = 0;
        }

        public virtual KeyValuePair<string, Texture2D> getPathTexturePair()
        {
            return new KeyValuePair<string, Texture2D>(this.path, this.textures.currentTexture.currentTexture);
        }


        /// <summary>
        /// Used just to get a copy of this asset sheet.
        /// </summary>
        public virtual AssetSheet clone()
        {
            var asset = new AssetSheet(this.assetInfo,(string)this.path.Clone());
            return asset;
        }


        public virtual void setLeft()
        {
            this.textures.setLeft();
        }

        public virtual void setUp()
        {
            this.textures.setUp();
        }

        public virtual void setDown()
        {
            this.textures.setDown();
        }

        public virtual void setRight()
        {
            this.textures.setRight();
        }

        public virtual Texture2D getCurrentSpriteTexture()
        {
            return this.textures.currentTexture.currentTexture;
        }

        public virtual Texture2D getTexture(Direction direction,AnimationType type)
        {
            return this.textures.getTextureFromAnimation(type).getTextureFromDirection(direction);
        }
    }
}
