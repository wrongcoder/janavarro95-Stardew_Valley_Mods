using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.Revitalize.Framework.Crafting.JsonContent
{
    /// <summary>
    /// Exception used to notify the user that an error has occured when loading a crfafting recipe from a json file from disk.
    /// </summary>
    public class InvalidJsonCraftingComponentException: Exception
    {
        public InvalidJsonCraftingComponentException()
        {

        }


        public InvalidJsonCraftingComponentException(string Message):base(Message)
        {
        }
    }
}
