using System.Collections.Generic;
using Omegasis.StardustCore.UIUtilities.SpriteFonts.Components;

namespace Omegasis.StardustCore.UIUtilities.SpriteFonts.CharacterSheets
{
    public class GenericCharacterSheets
    {
        public Dictionary<char, TexturedCharacter> CharacterAtlus;


        public GenericCharacterSheets()
        {

        }

        public GenericCharacterSheets(string Path)
        {

        }

        public virtual TexturedCharacter getTexturedCharacter(char c)
        {
            var original = this.CharacterAtlus[c];
            return TexturedCharacter.Copy(original);
        }

        public virtual GenericCharacterSheets create(string Path)
        {
            return new GenericCharacterSheets(Path);
        }
    }
}
