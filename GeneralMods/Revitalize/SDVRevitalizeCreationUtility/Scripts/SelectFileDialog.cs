using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SdvRevitalizeCreationUtility.Scripts
{
    public class SelectFileDialog:FileDialog
    {
        public TextEdit textEdit;
        string textToSet;

        public override void _Ready()
        {
            this.Connect("file_selected", this, "_on_SelectFileDialog_file_selected");
            this.Connect("dir_selected", this, "_on_SelectFileDialog_dir_selected");
            this.Connect("confirmed", this, "_on_SelectFileDialog_confirmed");
        }

        public virtual void _on_SelectFileDialog_dir_selected(string input)
        {
            if (this.Filters.Length == 1)
            {
                string extension = System.IO.Path.GetExtension(input);
                if (string.IsNullOrEmpty(extension))
                {
                    this.textToSet = input + ".json";
                }

                if (!System.IO.Path.GetExtension(input).Equals(this.Filters[0].Replace("*","")))
                {
                    throw new Exception("Bad file path extension!");
                }
            }
            this.textToSet = input;
        }

        public virtual void _on_SelectFileDialog_file_selected(string input)
        {
            this.textToSet = input;
        }

        public virtual void _on_SelectFileDialog_confirmed()
        {
            this.textEdit.Text = this.textToSet;
        }
    }
}
