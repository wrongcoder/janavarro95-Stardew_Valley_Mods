using Godot;
using System;
namespace SdvRevitalizeCreationUtility.Scripts
{
    public class BlueprintCreationSceneSaveButton : Button
    {
        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {

        }

        public override void _Pressed()
        {
            base._Pressed();

            //Display strings
            string displayName = NodeExtensions.GetChild<TextEdit>(Game.Self, "ScrollContainer", "VBoxContainer", "DisplayNameText").Text;
            string description = NodeExtensions.GetChild<TextEdit>(Game.Self, "ScrollContainer", "VBoxContainer", "DescriptionText").Text;
            string category = "StardewValley.Crafting";

            //Blueprint template params.
            string itemToDraw = NodeExtensions.GetChild<TextEdit>(Game.Self, "ScrollContainer", "VBoxContainer", "ItemToDrawIdText").Text;
            string recipesToUnlock = NodeExtensions.GetChild<TextEdit>(Game.Self, "ScrollContainer", "VBoxContainer", "RecipesToUnlockText").Text;
            string blueprintObjectId = NodeExtensions.GetChild<TextEdit>(Game.Self, "ScrollContainer", "VBoxContainer", "BlueprintObjectIdText").Text;

            string blueprintOutputPath = NodeExtensions.GetChild<TextEdit>(Game.Self, "ScrollContainer", "VBoxContainer", "BlueprintObjectFilePathText").Text;
            string displayStringOutputPath = NodeExtensions.GetChild<TextEdit>(Game.Self, "ScrollContainer", "VBoxContainer", "BlueprintDisplayStringFilePathText").Text;

            string recipeTabId = NodeExtensions.GetChild<TextEdit>(Game.Self, "ScrollContainer", "VBoxContainer", "RecipeTabIdText").Text;
            string[] recipeIdSplitArray = recipesToUnlock.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
            string recipeId = recipeIdSplitArray[recipeIdSplitArray.Length - 1].Replace("\"", "").Replace(" ", ""); //Get the recipe id and remove all unnecessary whitespace.
            string recipeInputs= NodeExtensions.GetChild<TextEdit>(Game.Self, "ScrollContainer", "VBoxContainer", "RecipeInputsText").Text;
            string recipeOutputs = NodeExtensions.GetChild<TextEdit>(Game.Self, "ScrollContainer", "VBoxContainer", "RecipeOutputsText").Text;
            string recipeOutputFilePath= NodeExtensions.GetChild<TextEdit>(Game.Self, "ScrollContainer", "VBoxContainer", "RecipeFilePathText").Text;

            string newItemDisplayName= NodeExtensions.GetChild<TextEdit>(Game.Self, "ScrollContainer", "VBoxContainer", "NewItemDisplayNameText").Text;
            string newItemDescription = NodeExtensions.GetChild<TextEdit>(Game.Self, "ScrollContainer", "VBoxContainer", "NewItemDescriptionText").Text;
            string newItemCategory = NodeExtensions.GetChild<TextEdit>(Game.Self, "ScrollContainer", "VBoxContainer", "NewItemCategoryText").Text;

            string newItemDisplayStringsOutputPath = NodeExtensions.GetChild<TextEdit>(Game.Self, "ScrollContainer", "VBoxContainer", "NewItemDisplayStringFilePathText").Text;

            //Write them to new json files.
            TemplateTransformerScript.WriteCraftingBlueprintFile(blueprintOutputPath, blueprintObjectId, recipesToUnlock, itemToDraw);
            TemplateTransformerScript.WriteDisplayStringsFile(displayStringOutputPath, blueprintObjectId, displayName, description, category);
            TemplateTransformerScript.WriteRecipeFileForBlueprintObject(recipeOutputFilePath, recipeTabId, recipeId, recipeInputs, recipeOutputs);
            TemplateTransformerScript.WriteDisplayStringsFile(newItemDisplayStringsOutputPath, itemToDraw, newItemDisplayName, newItemDescription, newItemCategory);

            //Need new item display strings?

            //TODO EXTRA: Need to also update Revitalize .cs files with the new fields for the blueprint items, recipe ids, and display item as well?
        }
    }
}
