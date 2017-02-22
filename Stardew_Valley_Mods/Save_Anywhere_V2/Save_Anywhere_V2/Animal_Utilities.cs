using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Save_Anywhere_V2.Save_Utilities
{
    class Animal_Utilities
    {

        public static void save_animal_info()
        {
            Save_Anywhere_V2.Mod_Core.animal_path = Path.Combine(Save_Anywhere_V2.Mod_Core.player_path, "Animals");
            if (!Directory.Exists(Save_Anywhere_V2.Mod_Core.animal_path))
            {
                Directory.CreateDirectory(Save_Anywhere_V2.Mod_Core.animal_path);

            }
            Save_Anywhere_V2.Save_Utilities.Animals.Horse_Utility.Save_Horse_Info();
            Save_Anywhere_V2.Save_Utilities.Animals.Pet_Utilities.save_pet_info();
        }

        public static void load_animal_info()
        {
            Save_Anywhere_V2.Mod_Core.animal_path = Path.Combine(Save_Anywhere_V2.Mod_Core.player_path, "Animals");
            if (!Directory.Exists(Save_Anywhere_V2.Mod_Core.animal_path))
            {
                Directory.CreateDirectory(Save_Anywhere_V2.Mod_Core.animal_path);

            }
            Save_Anywhere_V2.Save_Utilities.Animals.Horse_Utility.Load_Horse_Info();
            Save_Anywhere_V2.Save_Utilities.Animals.Pet_Utilities.Load_pet_Info();
        }

    }
}
