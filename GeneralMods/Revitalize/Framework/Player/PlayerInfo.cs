using Microsoft.Xna.Framework;
using Revitalize.Framework.Player.Managers;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Framework.Player
{
    public class PlayerInfo
    {
        public SittingInfo sittingInfo;

        public PlayerInfo()
        {
            sittingInfo = new SittingInfo();
        }


        public void update()
        {
            sittingInfo.update();
        }
    }
}
