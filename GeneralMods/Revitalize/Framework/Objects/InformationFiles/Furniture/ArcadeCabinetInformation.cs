using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley.Minigames;

namespace Revitalize.Framework.Objects.InformationFiles.Furniture
{
    public class ArcadeCabinetInformation
    {

        public IMinigame minigame;
        public bool freezeState;

        public ArcadeCabinetInformation()
        {
            this.minigame = null;
            this.freezeState = false;
        }
        public ArcadeCabinetInformation(IMinigame Minigame, bool FreezeState)
        {
            this.minigame = Minigame;
            this.freezeState = FreezeState;
        }
    }
}
