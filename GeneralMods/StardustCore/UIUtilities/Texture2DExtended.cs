using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;

namespace StardustCore.UIUtilities
{
    public class Texture2DExtended
    {
        public string Name;

        [XmlIgnore]
        public Texture2D texture;
        public string path;
        public string modID;
        public string textureManagerId;


        public int Width
        {
            get
            {
                return this.texture.Width;
            }
        }
        public int Height
        {
            get
            {
                return this.texture.Height;
            }
        }

        /// <summary>Empty/null constructor.</summary>
        public Texture2DExtended()
        {
            this.Name = "";
            this.texture = null;
            this.path = "";
            this.modID = "";
            this.textureManagerId = "";
        }


        /// <summary>Construct an instance.</summary>
        /// <param name="path">The relative path to file on disk. See StardustCore.Utilities.getRelativePath(modname,path);
        public Texture2DExtended(IManifest manifest, string path, string TextureManagerId)
        {
            this.Name = Path.GetFileNameWithoutExtension(path);
            this.path = path;
            this.modID = manifest.UniqueID;
            this.textureManagerId = TextureManagerId;
            this.loadTexture();
        }

        public Texture2DExtended(string modID, string path, string TextureManagerId)
        {
            this.Name = Path.GetFileNameWithoutExtension(path);
            this.path = path;
            this.modID = modID;
            this.textureManagerId = TextureManagerId;
            this.loadTexture();
        }

        public Texture2DExtended(IContentPack content, string path, string TextureManagerId)
        {
            this.Name = Path.GetFileNameWithoutExtension(path);
            this.path = path;
            this.modID = content.Manifest.UniqueID;
            this.textureManagerId = TextureManagerId;
            this.loadTexture();


        }

        public Texture2DExtended Copy()
        {
            return new Texture2DExtended(this.modID, this.path,this.textureManagerId);
        }

        /// <summary>Returns the actual 2D texture held by this wrapper class.</summary>
        public virtual Texture2D getTexture()
        {
            if (this.texture != null)
            {
                return this.texture;
            }
            else
            {
                this.loadTexture();
                return this.texture;
            }
        }

        /// <summary>
        /// Sets the texture 2d for this extended texture.
        /// </summary>
        /// <param name="text"></param>
        public virtual void setTexture(Texture2D text)
        {
            this.texture = text;
        }

        /// <summary>
        /// Loads the texture if this texture is null;
        /// </summary>
        public virtual void loadTexture()
        {
            if (this.texture == null)
            {
                StardustCore.UIUtilities.TextureManager.GetTextureManager(this.modID, this.textureManagerId).loadTexture(this.path, this.Name, this);
            }
        }
    }
}
