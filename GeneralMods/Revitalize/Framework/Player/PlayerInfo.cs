using Omegasis.Revitalize.Framework.Player.Managers;

namespace Omegasis.Revitalize.Framework.Player
{
    public class PlayerInfo
    {
        public MagicManager magicManager;

        public PlayerInfo()
        {
            this.magicManager = new MagicManager();
        }

        public void update()
        {
        }
    }
}
