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
        public Texture2D texture;
        public AssetInfo assetInfo;
        public string path;

        public int index;

        private int widthIndex;
        private int heightIndex;
        private int widthIndexMax;
        private int heightIndexMax;

        public Rectangle currentAsset;

        public AssetSheet(AssetInfo info,string path)
        {
            this.assetInfo = info;
            this.texture = Class1.ModHelper.Content.Load<Texture2D>(Class1.getShortenedDirectory(Path.Combine(path,info.name+".png")).Remove(0,1));
            this.path = Class1.getShortenedDirectory(Path.Combine(path, info.name + ".png"));
            this.widthIndexMax = this.texture.Width / (int)this.assetInfo.assetSize.X;
            this.heightIndexMax = this.texture.Width / (int)this.assetInfo.assetSize.Y;
            this.index = 0;
            if (this.assetInfo.randomizeUponLoad == false)
            {
                this.widthIndex = 0;
                this.heightIndex = 0;
            }
            else
            {
                getRandomAssetIndicies();
                setIndex();
            }
            this.currentAsset = new Rectangle(widthIndex * (int)this.assetInfo.assetSize.X, heightIndex * (int)this.assetInfo.assetSize.Y, (int)this.assetInfo.assetSize.X, (int)this.assetInfo.assetSize.Y);
        }

        public KeyValuePair<string, Texture2D> getPathTexturePair()
        {
            return new KeyValuePair<string, Texture2D>(this.path, this.texture);
        }

        /// <summary>
        /// Get the next graphic from the texture.
        /// </summary>
        public void getNext()
        {
            //If I can still iterate through my list but my width is maxed, increment height.
            if (this.widthIndex == this.widthIndexMax - 1 && this.heightIndex != this.heightIndexMax)
            {
                this.widthIndex -= 0;
                this.heightIndex++;
            }
            //If I reached the end of my image loop to 0;
            else if (this.heightIndex == this.heightIndexMax && this.widthIndex == this.widthIndexMax - 1)
            {
                this.heightIndex = 0;
                this.widthIndex = 0;
            }
            else
            {
                //If I can still iterate through my list do so.
                widthIndex++;
            }
            this.setIndex();
            this.setAsset();
        }

        /// <summary>
        /// Get the last graphic from my texture.
        /// </summary>
        public void getPrevious()
        {
            //If my width index is 0 and my height index isn't decrement my height index and set the width index to the far right.
            if (this.widthIndex == 0 && this.heightIndex != 0)
            {
                this.heightIndex--;
                this.widthIndex = this.widthIndexMax - 1;
            }
            //If both my height and width indicies are 0, loop to the bottom right of the texture.
            else if (this.widthIndex == 0 && this.heightIndex == 0)
            {
                this.widthIndex = this.widthIndexMax - 1;
                this.heightIndex = this.heightIndexMax - 1;
            }
            else
            {
                //Just decrement my width index by 1.
                this.widthIndex--;
            }
            this.setIndex();
            this.setAsset();
        }

        /// <summary>
        /// sets the current positioning for the rectangle index;
        /// </summary>
        private void setAsset()
        {
            this.currentAsset.X = widthIndex * (int)this.assetInfo.assetSize.X;
            this.currentAsset.Y = heightIndex * (int)this.assetInfo.assetSize.Y;
        }

        /// <summary>
        /// Used mainly for display purposes and length purposes.
        /// </summary>
        public void setIndex()
        {
            this.index = heightIndex * widthIndexMax + widthIndex;
        }

        /// <summary>
        /// Sets the asset index to a random value.
        /// </summary>
        public void getRandomAssetIndicies()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            this.widthIndex = r.Next(0, this.widthIndexMax);
            this.widthIndex = r.Next(0, this.heightIndexMax);
            setIndex();
            setAsset();
        }

        /// <summary>
        /// Used just to get a copy of this asset sheet.
        /// </summary>
        public void clone()
        {
            var asset = new AssetSheet(this.assetInfo,(string)this.path.Clone());
        }
    }
}
