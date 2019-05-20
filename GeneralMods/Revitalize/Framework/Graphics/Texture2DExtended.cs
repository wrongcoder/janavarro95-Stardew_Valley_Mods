using System.IO;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;

namespace Revitalize.Framework.Graphics
{
    public class Texture2DExtended
    {
        public string Name;
        public Texture2D texture;
        public string path;
        public string modID;
        public ContentSource source;
        private readonly IModHelper helper;
        private readonly IContentPack contentPack;

        /// <summary>Empty/null constructor.</summary>
        public Texture2DExtended()
        {
            this.Name = "";
            this.texture = null;
            this.path = "";
            this.helper = null;
            this.modID = "";
        }

        public Texture2DExtended(Texture2D Texture)
        {
            this.Name = Texture.Name;
            this.texture = Texture;
            this.path = "";
            this.helper = null;
            this.modID = "";
            this.contentPack = null;
        }


        /// <summary>Construct an instance.</summary>
        /// <param name="path">The relative path to file on disk. See StardustCore.Utilities.getRelativePath(modname,path);
        public Texture2DExtended(IModHelper helper, IManifest manifest, string path, ContentSource contentSource = ContentSource.ModFolder)
        {
            this.Name = Path.GetFileNameWithoutExtension(path);
            this.path = path;
            this.texture = helper.Content.Load<Texture2D>(path, contentSource);
            this.helper = helper;
            this.modID = manifest.UniqueID;
            this.source = contentSource;
            this.contentPack = null;
        }

        public Texture2DExtended(IModHelper helper, string modID, string path, ContentSource contentSource = ContentSource.ModFolder)
        {
            this.Name = Path.GetFileNameWithoutExtension(path);
            this.path = path;
            this.texture = helper.Content.Load<Texture2D>(path, contentSource);
            this.helper = helper;
            this.modID = modID;
            this.source = contentSource;
            this.contentPack = null;
        }

        public Texture2DExtended(IContentPack ContentPack, IManifest manifest, string path)
        {
            this.Name = Path.GetFileNameWithoutExtension(path);
            this.path = path;
            this.texture = ContentPack.LoadAsset<Texture2D>(path);
            this.helper = null;
            this.modID = manifest.UniqueID;
            this.source = ContentSource.ModFolder;
            this.contentPack = null;
        }
        public Texture2DExtended(IContentPack ContentPack, string modID, string path)
        {
            this.Name = Path.GetFileNameWithoutExtension(path);
            this.path = path;
            this.texture = ContentPack.LoadAsset<Texture2D>(path);
            this.helper = null;
            this.modID = modID;
            this.source = ContentSource.ModFolder;
            this.contentPack = null;
        }

        public Texture2DExtended Copy()
        {
            //return this;
            if (this.helper != null)
            {
                return new Texture2DExtended(this.helper, this.modID, this.path);
            }
            else if(this.contentPack!=null)
            {
                return new Texture2DExtended(this.contentPack,this.modID,this.path);
            }
            else
            {
                return null;
            }
        }

        public IModHelper getHelper()
        {
            return this.helper;
        }

        /// <summary>Returns the actual 2D texture held by this wrapper class.</summary>
        public Texture2D getTexture()
        {
            return this.texture;
        }

        public void setTexure(Texture2D text)
        {
            this.texture = text;
        }
    }
}
