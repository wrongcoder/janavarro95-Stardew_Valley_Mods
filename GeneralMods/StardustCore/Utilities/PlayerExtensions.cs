using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace StardustCore.Utilities
{
    public static class PlayerExtensions
    {
        public static bool CanReadJunimo(this Farmer Farmer)
        {
            return Farmer.mailReceived.Contains("canReadJunimoText");
        }


    }
}
