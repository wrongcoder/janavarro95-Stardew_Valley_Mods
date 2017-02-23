using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Persistance
{
   public class PlayerVariables
    {
        public static int MaxMagic;
        public static int CurrentMagic;

        public PlayerVariables()
        {
            MaxMagic = 100;
            CurrentMagic = 100;

        }

        public static void initializePlayerVariables()
        {
            MaxMagic = 100;
            CurrentMagic = 100;

        }


        public static void savePlayerVariables()
        {


        }

        public static void loadPlayerVariables()
        {

        }

    }
}
