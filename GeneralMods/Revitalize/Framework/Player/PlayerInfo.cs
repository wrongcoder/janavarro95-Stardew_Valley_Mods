using Revitalize.Framework.Player.Managers;

namespace Revitalize.Framework.Player
{
    public class PlayerInfo
    {
        public SittingInfo sittingInfo;

        public PlayerInfo()
        {
            this.sittingInfo = new SittingInfo();
        }

        public void update()
        {
            this.sittingInfo.update();
        }
    }
}
