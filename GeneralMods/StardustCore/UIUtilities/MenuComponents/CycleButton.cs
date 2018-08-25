using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using StardustCore.UIUtilities.MenuComponents.Delegates.Functionality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardustCore.UIUtilities.MenuComponents
{
    public class CycleButton :Button
    {

        public List<Button> buttons;
        public int buttonIndex;


        public CycleButton(Rectangle bounds, List<Button> buttons, Rectangle SourceRect, float scale) : base(bounds, buttons.ElementAt(0).animationManager.getExtendedTexture(), SourceRect, scale)
        {
            this.buttons = buttons;
        }

        public CycleButton(string Name, string displayText, Rectangle bounds, List<Button> buttons, Rectangle SourceRect, float scale, Animations.Animation defaultAnimation, Color DrawColor, Color TextColor, ButtonFunctionality buttonFunctionality, bool AnimationEnabled, List<KeyValuePair<ClickableTextureComponent, ExtraTextureDrawOrder>> extraTexture) : base(Name, bounds, buttons.ElementAt(0).animationManager.getExtendedTexture(), displayText, SourceRect, scale, defaultAnimation, DrawColor, TextColor, buttonFunctionality, AnimationEnabled, extraTexture)
        {
            this.buttons = buttons;
        }

        public CycleButton(string Name, string displayText, Rectangle bounds, List<Button> buttons, Rectangle SourceRect, float scale, Animations.Animation defaultAnimation, Color DrawColor, Color TextColor, ButtonFunctionality buttonFunctionality, bool AnimationEnabled, Dictionary<string, List<Animations.Animation>> animationsToPlay, string startingKey, int startingAnimationFrame, List<KeyValuePair<ClickableTextureComponent, ExtraTextureDrawOrder>> extraTexture) : base(Name, bounds, buttons.ElementAt(0).animationManager.getExtendedTexture(), displayText, SourceRect, scale, defaultAnimation, animationsToPlay, startingKey, DrawColor, TextColor, buttonFunctionality, startingAnimationFrame, AnimationEnabled, extraTexture)
        {
            this.buttons = buttons;
        }

        public override void onLeftClick()
        {

            //cycle button to next button and loop around if necessary.

            base.onLeftClick();
        }


        //CHANGE ALL DRAW FUNCTIONS TO DRAW THE CURRENT BUTTON TEXTURE.

        public override void draw(SpriteBatch b, Color color)
        {
            base.draw(b, color);
        }

        public override void draw(SpriteBatch b, Color color, Vector2 offset)
        {
            base.draw(b, color, offset);
        }


        public override void draw(SpriteBatch b, Color color, Vector2 offset, float layerDepth)
        {
            base.draw(b, color, offset, layerDepth);
        }

    }
}
