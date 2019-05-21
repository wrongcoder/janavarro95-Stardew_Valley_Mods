using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;

namespace Revitalize.Framework.Graphics
{
    public class TextureManager
    {
        public static Dictionary<string, Dictionary<string,TextureManager>> TextureManagers = new Dictionary<string,Dictionary<string,TextureManager>>();


        public Dictionary<string, Texture2DExtended> textures;

        public string name;

        public TextureManager(string Name)
        {
            this.name = Name;
            this.textures = new Dictionary<string, Texture2DExtended>();
        }

        public TextureManager(string Name,IContentPack ContentPack)
        {
            this.name = Name;
            this.textures = new Dictionary<string, Texture2DExtended>();
        }

        public void addTexture(string name, Texture2DExtended texture)
        {
            this.textures.Add(name, texture);
        }

        /// <summary>Returns a Texture2DExtended held by the manager.</summary>
        public Texture2DExtended getTexture(string name)
        {
            foreach (var v in this.textures)
            {
                if (v.Key == name)
                    return v.Value.Copy();
            }
            throw new Exception("Error, texture name not found!!!");
        }


        /// <summary>
        /// Content pack search.
        /// </summary>
        public void searchForTextures(IContentPack content)
        {
            string path = content.DirectoryPath;
            this.searchDirectories(path, "", content);
        }

        /// <summary>
        /// Non-Content pack search.
        /// </summary>
        public void searchForTextures()
        {
            string path = ModCore.ModHelper.DirectoryPath;
            this.searchDirectories(path, "", ModCore.Manifest);
        }

        private void searchDirectories(string path, string relativePath, IManifest manifest)
        {
            string[] dirs = Directory.GetDirectories(path);
            string[] files = Directory.GetFiles(path);

            string[] extensions = new string[2]
            {
                ".png",
                ".jpg"
            };

            foreach (string directory in dirs)
            {
                string combo = string.IsNullOrEmpty(relativePath) ? Path.GetFileName(directory) : Path.Combine(relativePath, Path.GetFileName(directory));
                this.searchDirectories(directory, combo, manifest);
            }

            foreach (string file in files)
            {
                if (extensions.Contains(Path.GetExtension(file)))
                {
                    this.processFoundTexture(file, relativePath);

                }
            }

        }
        private void searchDirectories(string path, string relativePath, IContentPack ContentPack)
        {
            string[] dirs = Directory.GetDirectories(path);
            string[] files = Directory.GetFiles(path);

            string[] extensions = new string[2]
            {
                ".png",
                ".jpg"
            };

            foreach (string directory in dirs)
            {
                string combo = string.IsNullOrEmpty(relativePath) ? Path.GetFileName(directory) : Path.Combine(relativePath, Path.GetFileName(directory));
                this.searchDirectories(directory, combo, ContentPack);
            }

            foreach (string file in files)
            {
                if (extensions.Contains(Path.GetExtension(file)))
                {
                    this.processFoundTexture(file, relativePath, ContentPack);

                }
            }

        }

        private void processFoundTexture(string file, string relativePath)
        {
            Texture2DExtended textureExtended = new Texture2DExtended(ModCore.ModHelper, ModCore.Manifest, Path.Combine(relativePath, Path.GetFileName(file)));

            ModCore.log("Found texture: " + textureExtended.Name);

            textureExtended.texture.Name = ModCore.Manifest.UniqueID+"_"+this.name + "_" + textureExtended.Name;

            this.addTexture(textureExtended.Name, textureExtended);
        }

        private void processFoundTexture(string file, string relativePath, IContentPack ContentPack)
        {
            Texture2DExtended textureExtended = new Texture2DExtended(ContentPack, Path.Combine(relativePath, Path.GetFileName(file)));

            textureExtended.texture.Name = ContentPack.Manifest.UniqueID + "_" + this.name + "_" + textureExtended.Name;
            ModCore.log("Found texture: " + textureExtended.Name);

            //this.addTexture(textureExtended.Name, textureExtended);
        }


        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        //                      Static Functions                 //
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

        public static void addTexture(IManifest ModManifest,string managerName, string textureName, Texture2DExtended Texture)
        {
            Texture.texture.Name = managerName + "." + textureName;
            TextureManagers[ModManifest.UniqueID][managerName].addTexture(textureName, Texture);
        }

        public static void AddTextureManager(IManifest ModManifest,string Name)
        {
            if (TextureManager.TextureManagers.ContainsKey(ModManifest.UniqueID)){
                TextureManagers[ModManifest.UniqueID].Add(Name, new TextureManager(Name));
            }
            else
            {
                TextureManager.TextureManagers.Add(ModManifest.UniqueID, new Dictionary<string, TextureManager>());
                TextureManagers[ModManifest.UniqueID].Add(Name, new TextureManager(Name));
            }
           
        }

        public static TextureManager GetTextureManager(IManifest Manifest,string Name)
        {
            return TextureManagers[Manifest.UniqueID][Name];
        }



    }
}
