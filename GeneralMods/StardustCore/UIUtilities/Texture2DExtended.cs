using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardustCore.UIUtilities
{
    public class Texture2DExtended
    {
        public string Name;
        public Texture2D texture;
        public string path;
        
        public Texture2DExtended(string path)
        {
            this.Name = Path.GetFileName(path);
            this.path = path;
            this.texture = StardustCore.ModCore.ModHelper.Content.Load<Texture2D>(path);
        }
        
        public Texture2DExtended Copy()
        {
            return new Texture2DExtended(this.path);
        } 
    }
}
