using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.StardustCore.Animations;
using Omegasis.StardustCore.UIUtilities;

namespace Omegasis.Revitalize.Framework.Managers
{
    public class TextureManagers
    {
        private static bool HasLoadedTextureManagers;

        public static TextureManager HUD;

        public static TextureManager Items_Resources_Ore;
        public static TextureManager Items_Crafting;
        public static TextureManager Items_Farming;

        public static TextureManager Objects_Crafting;
        public static TextureManager Objects_Farming;
        public static TextureManager Objects_Furniture;
        public static TextureManager Objects_Machines;

        public static TextureManager Menus_Misc;
        public static TextureManager Menus_CraftingMenu;
        public static TextureManager Menus_EnergyMenu;
        public static TextureManager Menus_InventoryMenu;

        public static TextureManager Resources_Ore;

        public static TextureManager Resources_Misc;

        public static TextureManager Tools;

        /// <summary>
        /// Loads in textures to be used by the mod.
        /// </summary>
        public static void loadInTextures()
        {

            if (HasLoadedTextureManagers) return;

            //HUD
            HUD = InitializeTextureManager("Revitalize.HUD", Path.Combine("Content", "Graphics", "HUD"));

            //Items
            Items_Resources_Ore = InitializeTextureManager("Revitalize.Items.Resources.Ore", Path.Combine("Content", "Graphics", "Items", "Resources", "Ore"));
            Items_Crafting = InitializeTextureManager("Revitalize.Items.Crafting", Path.Combine("Content", "Graphics", "Items", "Crafting"));
            Items_Farming = InitializeTextureManager("Revitalize.Items.Farming", Path.Combine("Content", "Graphics", "Items", "Farming"));

            //World Objects
            Objects_Crafting = InitializeTextureManager("Revitalize.Objects.Crafting", Path.Combine("Content", "Graphics", "Objects", "Crafting"));
            Objects_Farming = InitializeTextureManager("Revitalize.Objects.Farming", Path.Combine("Content", "Graphics", "Objects", "Farming"));
            Objects_Furniture = InitializeTextureManager("Revitalize.Objects.Furniture", Path.Combine("Content", "Graphics", "Objects", "Furniture"));
            Objects_Machines = InitializeTextureManager("Revitalize.Objects.Machines", Path.Combine("Content", "Graphics", "Objects", "Machines"));

            //Menus
            Menus_Misc = InitializeTextureManager("Revitalize.Menus", Path.Combine("Content", "Graphics", "Menus", "Misc"));
            Menus_CraftingMenu = InitializeTextureManager("Revitalize.Menus.CraftingMenu", Path.Combine("Content", "Graphics", "Menus", "CraftingMenu"));
            Menus_EnergyMenu = InitializeTextureManager("Revitalize.Menus.EnergyMenu", Path.Combine("Content", "Graphics", "Menus", "EnergyMenu"));
            Menus_InventoryMenu = InitializeTextureManager("Revitalize.Menus.InventoryMenu", Path.Combine("Content", "Graphics", "Menus", "InventoryMenu"));

            //Resources
            Resources_Ore = InitializeTextureManager("Revitalize.Resources.Ore", Path.Combine("Content", "Graphics", "Objects", "Resources", "Ore"));
            Resources_Misc = InitializeTextureManager("Revitalize.Items.Resources.Misc", Path.Combine("Content", "Graphics", "Items", "Resources", "Misc"));

            //Tools
            Tools = InitializeTextureManager("Revitalize.Tools", Path.Combine("Content", "Graphics", "Items", "Tools"));

            HasLoadedTextureManagers = true;
        }

        private static TextureManager InitializeTextureManager(string TextureManagerId, string TextureManagerPathToSearch)
        {
            TextureManager.AddTextureManager(RevitalizeModCore.Instance.Helper.DirectoryPath, RevitalizeModCore.Instance.ModManifest, TextureManagerId);
            TextureManager textureManager = TextureManager.GetTextureManager(RevitalizeModCore.Instance.ModManifest, TextureManagerId);
            textureManager.searchForTextures(RevitalizeModCore.ModHelper, RevitalizeModCore.Manifest, TextureManagerPathToSearch);
            return textureManager;
        }




        public static AnimationManager createOreResourceAnimationManager(string TextureName)
        {
            return createOreResourceAnimationManager(TextureName, new Animation(0, 0, 16, 16));
        }

        public static AnimationManager createOreResourceAnimationManager(string TextureName, Animation DefaultAnimation)
        {
            return TextureManagers.Items_Resources_Ore.createAnimationManager(TextureName, DefaultAnimation);
        }

        public static AnimationManager createMiscResourceAnimationManager(string TextureName)
        {
            return createMiscResourceAnimationManager(TextureName, new Animation(0, 0, 16, 16));
        }

        public static AnimationManager createMiscResourceAnimationManager(string TextureName, Animation DefaultAnimation)
        {
            return TextureManagers.Resources_Misc.createAnimationManager(TextureName, DefaultAnimation);
        }

    }
}
