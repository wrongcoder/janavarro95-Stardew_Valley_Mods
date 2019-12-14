using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.NightOwl.Framework
{
    public class NightOwlAPI
    {

        /// <summary>
        /// Adds an event that triggers after the player has been warped to their pre-collapse position.
        /// </summary>
        /// <param name="ID">The id of the event.</param>
        /// <param name="Action">The code that triggers.</param>
        public void addPostWarpEvent(string ID,Func<bool> Action)
        {
            NightOwl.PostWarpCharacter.Add(ID, Action);
        }


        /// <summary>
        /// Removes an event that triggers when the player has been warped to their pre-collapse position.
        /// </summary>
        /// <param name="ID"></param>
        public void removePostWarpEvent(string ID)
        {
            if (NightOwl.PostWarpCharacter.ContainsKey(ID))
            {
                NightOwl.PostWarpCharacter.Remove(ID);
            }
        }

        /// <summary>
        /// Adds an event that triggers when the player has stayed up all night until 6:00 A.M.
        /// </summary>
        /// <param name="ID">The id of the event.</param>
        /// <param name="Action">The code that triggers.</param>
        public void addPlayerUpLateEvent(string ID, Func<bool> Action)
        {
            NightOwl.OnPlayerStayingUpLate.Add(ID, Action);
        }

        /// <summary>
        /// Removes an event that triggers when the player has stayed up all night.
        /// </summary>
        /// <param name="ID"></param>
        public void removePlayerUpLateEvent(string ID)
        {
            if (NightOwl.OnPlayerStayingUpLate.ContainsKey(ID))
            {
                NightOwl.OnPlayerStayingUpLate.Remove(ID);
            }
        }


    }
}
