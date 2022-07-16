using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Constants.PathConstants;
using Omegasis.Revitalize.Framework.Constants.PathConstants.Data;
using Omegasis.Revitalize.Framework.Constants.PathConstants.Graphics;
using Omegasis.Revitalize.Framework.Crafting;
using Omegasis.Revitalize.Framework.Managers;
using Omegasis.Revitalize.Framework.Objects;
using Omegasis.Revitalize.Framework.Utilities;
using Omegasis.Revitalize.Framework.World;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using StardewModdingAPI;

namespace Omegasis.Revitalize.Framework.Content
{
    public class ModContentManager
    {
        public CraftingManager craftingManager;
        /// <summary>
        /// Keeps track of custom objects.
        /// </summary>
        public ObjectManager objectManager;
        public MailManager mailManager;

        public ModContentManager()
        {

        }


        public virtual void initializeModContent(IManifest modManifest)
        {
            this.createDirectories();

            this.createJsonDataTemplates();

            //Loads in textures to be used by the mod.
            TextureManagers.loadInTextures();

            //Loads in objects to be use by the mod.
            this.objectManager = new ObjectManager(modManifest);


            this.mailManager = new MailManager();

            this.craftingManager = new CraftingManager();
        }

        public virtual void loadContentOnGameLaunched()
        {
            //Load in all objects from disk.
            this.objectManager.loadItemsFromDisk();

            //Once all objects have been initialized, then we can add references to them for recipes and initialize all of the crafting recipes for the mod.
            this.craftingManager.initializeRecipeBooks();
        }

        private void createDirectories()
        {
            Directory.CreateDirectory(Path.Combine(RevitalizeModCore.ModHelper.DirectoryPath, "Configs"));

            Directory.CreateDirectory(Path.Combine(RevitalizeModCore.ModHelper.DirectoryPath, RelativePaths.ModAssetsFolder));
            Directory.CreateDirectory(Path.Combine(RevitalizeModCore.ModHelper.DirectoryPath, RelativePaths.Graphics_Folder));
            Directory.CreateDirectory(Path.Combine(RevitalizeModCore.ModHelper.DirectoryPath, ObjectsGraphicsPaths.Furniture, "Chairs"));
            Directory.CreateDirectory(Path.Combine(RevitalizeModCore.ModHelper.DirectoryPath, ObjectsGraphicsPaths.Furniture, "Lamps"));
            Directory.CreateDirectory(Path.Combine(RevitalizeModCore.ModHelper.DirectoryPath, ObjectsGraphicsPaths.Furniture, "Tables"));
        }

        /// <summary>
        /// Creates empty json templates of various mod classes to be able to add/edit json files that are then converted into mod content.
        /// </summary>
        protected virtual void createJsonDataTemplates()
        {
            Crafting.JsonContent.JsonCraftingComponent jsonCraftingComponent = new Crafting.JsonContent.JsonCraftingComponent();
            JsonUtilities.saveToRevitaliveModContentFolder(jsonCraftingComponent, CraftingRecipesDataPaths.CraftingRecipesTemplatesPath, "JsonCraftingComponentTemplate");

            JsonItemInformation jsonItemInformationFile = new JsonItemInformation();
            JsonUtilities.saveToRevitaliveModContentFolder(jsonItemInformationFile, ObjectsDataPaths.ObjectsDataTemplatesPath,"JsonItemInformationTemplate");
            JsonUtilities.saveToRevitaliveModContentFolder(jsonItemInformationFile, ItemsDataPaths.ItemsDataTemplatesPath, "JsonItemInformationTemplate");
        }





    }
}
