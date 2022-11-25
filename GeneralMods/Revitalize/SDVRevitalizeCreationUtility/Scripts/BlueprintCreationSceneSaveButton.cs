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

            //Write them to new json files.
            TemplateTransformerScript.WriteCraftingBlueprintFile(blueprintOutputPath, blueprintObjectId, recipesToUnlock, itemToDraw);
            TemplateTransformerScript.WriteDisplayStringsFile(displayStringOutputPath, blueprintObjectId, displayName, description, category);

            //TODO EXTRA: Need to also update Revitalize .cs files with the new fields for the blueprint items, recipe ids, and display item as well?
        }

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        //  public override void _Process(float delta)
        //  {
        //      
        //  }
    }
}
