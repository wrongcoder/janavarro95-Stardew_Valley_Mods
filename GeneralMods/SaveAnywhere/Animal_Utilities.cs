using System.IO;

namespace Omegasis.SaveAnywhere
{
    class Animal_Utilities
    {

        public static void save_animal_info()
        {
            Mod_Core.animal_path = Path.Combine(Mod_Core.player_path, "Animals");
            if (!Directory.Exists(Mod_Core.animal_path))
            {
                Directory.CreateDirectory(Mod_Core.animal_path);

            }
            Horse_Utility.Save_Horse_Info();
            Pet_Utilities.save_pet_info();
        }

        public static void load_animal_info()
        {
            Mod_Core.animal_path = Path.Combine(Mod_Core.player_path, "Animals");
            if (!Directory.Exists(Mod_Core.animal_path))
            {
                Directory.CreateDirectory(Mod_Core.animal_path);

            }
            Horse_Utility.Load_Horse_Info();
            Pet_Utilities.Load_pet_Info();
        }

    }
}
