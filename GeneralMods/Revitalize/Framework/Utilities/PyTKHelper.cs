using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PyTK.CustomElementHandler;

namespace Revitalize.Framework.Utilities
{
    public class PyTKHelper
    {

        public static CustomObjectData CreateOBJData(string id, Texture2D Texture, Type type, Color color, bool bigCraftable = true)
        {
            return new PyTK.CustomElementHandler.CustomObjectData(id, "2048/0/-300/Crafting -9/Play '2048 by Platonymous' at home!/true/true/0/2048", Texture, color, 0, bigCraftable, type, null);
        }

    }
}
