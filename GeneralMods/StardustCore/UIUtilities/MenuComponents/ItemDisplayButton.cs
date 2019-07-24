using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace StardustCore.UIUtilities.MenuComponents
{
    /// <summary>
    /// A simple menu component for displaying SDV Items as well as being able to click them.
    /// </summary>
    public class ItemDisplayButton
    {

        private Vector2 position;
        public StardewValley.Item item;
        public Rectangle boundingBox;
        public float scale;
        public bool drawStackNumber;
        public Color drawColor;
        public StardustCore.Animations.AnimatedSprite background;

        /// <summary>
        /// The position of the button on screen.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;
                this.boundingBox.X =(int)this.position.X;
                this.boundingBox.Y =(int)this.position.Y;
            }
        }

        public ItemDisplayButton()
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="I">The itme to display.</param>
        /// <param name="Position">The position of the item.</param>
        /// <param name="BoundingBox">The bounding box for the item.</param>
        /// <param name="Scale"></param>
        /// <param name="DrawStackNumber"></param>
        /// <param name="DrawColor"></param>
        public ItemDisplayButton(Item I,StardustCore.Animations.AnimatedSprite Background,Vector2 Position, Rectangle BoundingBox, float Scale, bool DrawStackNumber, Color DrawColor)
        {
            this.item = I;
            this.boundingBox = BoundingBox;
            this.Position = Position;
            this.scale = Scale;
            this.drawStackNumber = DrawStackNumber;
            this.drawColor = DrawColor;
            this.background = Background;
        }

        public void update(GameTime time)
        {

        }

        /// <summary>
        /// A simple draw function.
        /// </summary>
        /// <param name="b"></param>
        public void draw(SpriteBatch b)
        {
            this.background.draw(b);
            if(this.item!=null)this.item.drawInMenu(b, this.position, this.scale);
        }

        /// <summary>
        /// The full draw function for drawing this component to the screen.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="Depth"></param>
        /// <param name="Alpha"></param>
        /// <param name="DrawShadow"></param>
        public void draw(SpriteBatch b,float Depth, float Alpha,bool DrawShadow)
        {
            this.background.draw(b, this.scale, Depth);
            if(this.item!=null)this.item.drawInMenu(b, this.position, 1f,Alpha,Depth,this.drawStackNumber,this.drawColor,DrawShadow);
        }

        public bool receiveLeftClick(int x, int y)
        {
            return this.boundingBox.Contains(new Point(x, y));
        }

        public bool receiveRightClick(int x, int y)
        {
            return this.boundingBox.Contains(new Point(x, y));
        }

        public bool Contains(int x, int y)
        {
            return this.boundingBox.Contains(new Point(x, y));
        }
    }
}
