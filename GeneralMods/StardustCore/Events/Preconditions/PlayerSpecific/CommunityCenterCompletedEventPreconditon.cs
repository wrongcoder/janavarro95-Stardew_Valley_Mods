using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Omegasis.StardustCore.Events.Preconditions.PlayerSpecific
{
    public class CommunityCenterCompletedEventPreconditon: EventPrecondition
    {
        public const string EventPreconditionId = "StardustCore.Events.Preconditions.Player.CommunityCenterCompleted";

        /// <summary>
        /// False means the community center doesn't need to be completed.
        /// </summary>
        public bool communityCenterNeedsToBeCompleted;

        public CommunityCenterCompletedEventPreconditon()
        {

        }

        public CommunityCenterCompletedEventPreconditon(bool NeedsToBeCompleted)
        {
            this.communityCenterNeedsToBeCompleted = NeedsToBeCompleted;
        }

        public override string ToString()
        {
            return EventPreconditionId + " " +this.communityCenterNeedsToBeCompleted;
        }

        public override bool meetsCondition()
        {
            return this.communityCenterNeedsToBeCompleted == Game1.player.hasCompletedCommunityCenter();
        }

    }
}
