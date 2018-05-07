using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkNightmare.Framework
{
    public enum MultiplayerPacket
    {
        ReadFarmerRoot,
        UnusedByte1,
        ReceivePlayerIntroduction,
        ReadActiveLocation,
        ReadWarp,
        UnsedByte5,
        ReadLocation,
        ReadSpritesAtLocation,
        WarpNPCS,
        UnusedByte9,
        ReceiveChatMessage,
        UnusedByte11,
        ReceiveWorldState,
        ReceiveTeamDelta,
        ReceiveNewDaySync,
        ReceiveChatInfoMessage,
        Unused16,
        ReceiveFarmerGainExperience,
        ParseServerToClientsMessage,
        PlayerDisconnected
    }
}
