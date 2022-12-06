using Godot;
using SDVRevitalizeCreationUtility.Scripts.Constants.PathConstants;
using SDVRevitalizeCreationUtility.Scripts.Constants.PathConstants.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SdvRevitalizeCreationUtility.Scripts
{
    public class ButtonOpenFileDiaog : Button
    {

        public enum DirectoryToBeginFrom
        {
            NULL,
            DisplayStrings,
            BlueprintObjects,
            CraftingPath,
        }

        [Export]
        public DirectoryToBeginFrom startingDir = DirectoryToBeginFrom.NULL;

        [Export]
        public string TextFieldToFind;

        public override void _Pressed()
        {
            base._Pressed();
            SelectFileDialog dialog = NodeExtensions.GetChild<SelectFileDialog>(Game.Self, "SelectFileDialog");
            dialog.AddFilter("*.json");

            dialog.CurrentFile = ""; ;
            dialog.Mode = FileDialog.ModeEnum.SaveFile;
            dialog.Access = FileDialog.AccessEnum.Filesystem;

            dialog.textEdit = NodeExtensions.GetChild<TextEdit>(Game.Self, "ScrollContainer", "VBoxContainer", this.TextFieldToFind);

            if (this.startingDir == DirectoryToBeginFrom.BlueprintObjects)
            {
                dialog.CurrentDir = System.IO.Path.Combine(Game.GetRevitalizeBaseFolder(), ObjectsDataPaths.CraftingBlueprintsPath);
            }
            if (this.startingDir == DirectoryToBeginFrom.DisplayStrings)
            {
                dialog.CurrentDir = System.IO.Path.Combine(Game.GetRevitalizeEnglishContentPackFolder(), StringsPaths.ObjectDisplayStrings);
            }
            if (this.startingDir == DirectoryToBeginFrom.CraftingPath)
            {
                dialog.CurrentDir = System.IO.Path.Combine(Game.GetRevitalizeBaseFolder(), CraftingDataPaths.CraftingPath);
            }

            dialog.Popup_();
        }
    }
}
