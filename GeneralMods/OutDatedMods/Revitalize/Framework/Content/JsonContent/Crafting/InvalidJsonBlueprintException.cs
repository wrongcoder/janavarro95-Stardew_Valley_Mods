using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.Revitalize.Framework.Content.JsonContent.Crafting
{
    /// <summary>
    /// Exception used to notify users that an error has occured when loading a JsonBlueprint from disk.
    /// </summary>
    public class InvalidJsonBlueprintException:Exception
    {

        public InvalidJsonBlueprintException() {


        }

        public InvalidJsonBlueprintException(string message) : base(message) { }
    }
}
