using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSoundManager
{
    public class Class1 :Mod
    {
        public static IModHelper ModHelper;

        public override void Entry(IModHelper helper)
        {
            ModHelper = helper;       
            
        }

    }
}
