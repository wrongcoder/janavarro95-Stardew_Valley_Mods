using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.Revitalize.Framework.World.Objects.Items.Utilities
{
    /// <summary>
    /// Used when trying to get an object that doesnt't exist or is registered inside of the mod's object manager.
    /// </summary>
    public class InvalidObjectManagerItemException:Exception
    {

        public InvalidObjectManagerItemException():base()
        {

        }

        public InvalidObjectManagerItemException(string Message) : base(Message)
        {

        }
    }
}
