using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.Revitalize.Framework.Utilities
{
    public class TypeUtilities
    {
        public static bool IsSameOrSubclass(Type potentialBase, Type potentialDescendant)
        {
            return potentialDescendant.IsSubclassOf(potentialBase)
                   || potentialDescendant.Equals(potentialBase);
        }

        public static bool IsSameType(Type potentialBase, Type potentialDescendant)
        {
            return potentialDescendant.Equals(potentialBase);
        }

        public static bool IsSubclass(Type potentialBase, Type potentialDescendant)
        {
            return potentialDescendant.IsSubclassOf(potentialBase);
        }
    }
}
