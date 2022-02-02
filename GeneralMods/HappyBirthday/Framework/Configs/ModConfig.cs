using System.IO;
using StardewModdingAPI;

namespace Omegasis.HappyBirthday.Framework.Configs
{
    /// <summary>The mod configuration.</summary>
    public class ModConfig
    {
        /// <summary>The key which shows the menu.</summary>
        public SButton KeyBinding { get; set; } = SButton.O;

        /// <summary>The minimum amount of friendship needed to get a happy birthday greeting from an npc.</summary>
        public int minimumFriendshipLevelForBirthdayWish = 2;

        public bool defaultToEnglishTranslation;
        

        /// <summary>Construct an instance.</summary>
        public ModConfig()
        {
            this.defaultToEnglishTranslation = true;
        }


        /// <summary>
        /// Initializes the config for the blacksmith shop prices.
        /// </summary>
        /// <returns></returns>
        public static ModConfig InitializeConfig()
        {
            if (HappyBirthdayModCore.Configs.doesConfigExist("ModConfig.json"))
            {
                return HappyBirthdayModCore.Configs.ReadConfig<ModConfig>("ModConfig.json");
            }
            else
            {
                ModConfig Config = new ModConfig();
                HappyBirthdayModCore.Configs.WriteConfig("ModConfig.json", Config);
                return Config;
            }
        }
    }
}
