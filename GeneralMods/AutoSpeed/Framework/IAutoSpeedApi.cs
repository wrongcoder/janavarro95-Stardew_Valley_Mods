using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.AutoSpeed.Framework
{
    /// <summary>
    /// Interface used to interface AutoSpeed's API class. 
    /// </summary>
    public interface IAutoSpeedAPI
    {
        void addSpeedBoost(string ID, int Amount);
        /// <summary>
        /// Removes an added speed boost by passing in the unique key.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Amount"></param>
        void remvoveSpeedBoost(string ID, int Amount);
    }
}
