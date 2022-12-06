using Godot;
using System;
namespace SdvRevitalizeCreationUtility.Scripts
{
    /// <summary>
    /// Class to populate a dropdown button with the options for potential code files to generate ids for.
    /// </summary>
    public class IdFileSelectionButton : OptionButton
    {
        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";
        [Export]
        public string relativePathForFileGeneration = "";

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            foreach (string option in string.IsNullOrEmpty(this.relativePathForFileGeneration) ? CodeGeneration.GenerateListOfCodeReferencesForIds() : CodeGeneration.GenerateListOfCodeReferencesForIds(this.relativePathForFileGeneration))
            {
                this.AddItem(option);
            }
        }

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        //  public override void _Process(float delta)
        //  {
        //      
        //  }
    }
}
