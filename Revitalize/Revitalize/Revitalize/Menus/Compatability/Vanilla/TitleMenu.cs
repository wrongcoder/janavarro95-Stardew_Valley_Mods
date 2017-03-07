using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley.Menus;

namespace Revitalize.Menus.Compatability.Vanilla
{
    class TitleMenu:Revitalize.Menus.Compatability.MenuCompatabilityBase
    {
        public TitleMenu()
        {
            minY = 1;
            maxY = 2;
            minX = 1;
            maxX = 5;
            canMoveInMenu = true;
            this.width = Game1.viewport.Width;
            this.height = Game1.viewport.Height;

            componentList = new Dictionary<Point, Rectangle>();

            CurrentLocationIndex =startingPositionIndex= new Point(2, 2);

            
           
            componentList.Add(new Point(2, 2), new Rectangle(width / 2 - 333 - 48, height - 174 - 24, 222, 174));//play
            componentList.Add(new Point(3, 2), new Rectangle(this.width / 2 - 111 - 24, this.height - 174 - 24, 222, 174));//load
            componentList.Add(new Point(4, 2), new Rectangle(this.width / 2 + 111, this.height - 174 - 24, 222, 174));//exit
            componentList.Add(new Point(5, 2), new Rectangle(this.width + -66 - 48, this.height - 75 - 24, 66, 75)); //about
            //int end = componentList.Count+1;
            
            //full screen button
            for (int i = 4; i <= 5; i++)
            {
                componentList.Add(new Point(i, 1), new Rectangle(Game1.viewport.Width - 9 * Game1.pixelZoom - Game1.tileSize / 4, Game1.tileSize / 4, 9 * Game1.pixelZoom, 9 * Game1.pixelZoom));
            }
            //MUTE BUTTON
            componentList.Add(new Point(1, 1),new Rectangle(Game1.tileSize / 4, Game1.tileSize / 4, 9 * Game1.pixelZoom, 9 * Game1.pixelZoom));
            componentList.Add(new Point(2, 1), new Rectangle(Game1.tileSize / 4, Game1.tileSize / 4, 9 * Game1.pixelZoom, 9 * Game1.pixelZoom));
            componentList.Add(new Point(3, 1), new Rectangle(Game1.tileSize / 4, Game1.tileSize / 4, 9 * Game1.pixelZoom, 9 * Game1.pixelZoom));
            //add in menu secrets here


            CompatabilityManager.characterCustomizer = false;
            Menus.Compatability.MenuCompatabilityBase.millisecondMoveDelay = 100;
            //Log.AsyncC("WTF");
        }


        public override void Compatability()
        {
            // Get the current gamepad state.
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);

//            Log.AsyncG("DOES THIS WORK???");

            if ((double)currentState.ThumbSticks.Left.X < 0 || currentState.IsButtonDown(Buttons.LeftThumbstickLeft))
            {
               // Log.AsyncC(currentState.ThumbSticks.Left);
                moveLeft();
            }
            if ((double)currentState.ThumbSticks.Left.X > 0 || currentState.IsButtonDown(Buttons.LeftThumbstickRight))
            {
               // Log.AsyncC(currentState.ThumbSticks.Left);
                moveRight();
            }

            if ((double)currentState.ThumbSticks.Left.Y < 0 || currentState.IsButtonDown(Buttons.LeftThumbstickRight))
            {
                // Log.AsyncC(currentState.ThumbSticks.Left);
                moveDown();
            }

            if ((double)currentState.ThumbSticks.Left.Y > 0 || currentState.IsButtonDown(Buttons.LeftThumbstickRight))
            {
                // Log.AsyncC(currentState.ThumbSticks.Left);
                moveUp();
            }




            Update();
        }

     
        //same code as movement but no change
        public override void Update()
        {
          
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);
            if (currentState.Buttons.A ==ButtonState.Pressed && CurrentLocationIndex.X==2 && CurrentLocationIndex.Y==2)
            {
               
                Class1.compatabilityMenu = null;
                CompatabilityManager.characterCustomizer = true;

               // Log.AsyncC("A pressed");
                return;
            }

            if (currentState.Buttons.A == ButtonState.Pressed && CurrentLocationIndex.X == 3 && CurrentLocationIndex.Y == 2)
            {

                Class1.compatabilityMenu = null;
                CompatabilityManager.loadMenu = true;

                // Log.AsyncC("A pressed");
                return;
            }

            if (currentState.Buttons.A == ButtonState.Pressed && CurrentLocationIndex.X == 5 && CurrentLocationIndex.Y == 2)
            {

                Class1.compatabilityMenu = null;
                CompatabilityManager.aboutMenu = true;

                // Log.AsyncC("A pressed");
                return;
            }

            Rectangle p;
          

            componentList.TryGetValue(CurrentLocationIndex, out p);
            
            
            updateMouse(getComponentCenter(p));
        }

        public override void moveLeft()
        {
            if (canMoveInMenu == false) return;
            activateTimer();
            CurrentLocationIndex.X--;

            Rectangle p;
            if (CurrentLocationIndex.X <= minX)
            {
                CurrentLocationIndex.X = minX;
            }
            //  Log.AsyncC("CRY");
            componentList.TryGetValue(CurrentLocationIndex, out p);



            updateMouse(getComponentCenter(p));

        }
        public override void moveRight()
        {
           
            if (canMoveInMenu == false) return;
            activateTimer();
            CurrentLocationIndex.X++;
           
            Rectangle p;

            if (CurrentLocationIndex.X >= maxX)
            {
                CurrentLocationIndex.X = maxX;
            }

       

          //  Log.AsyncC("CRY");
            componentList.TryGetValue(CurrentLocationIndex, out p);
            updateMouse(getComponentCenter(p));


        }

        public override void moveDown()
        {
            if (canMoveInMenu == false) return;
            activateTimer();
            CurrentLocationIndex.Y++;

            Rectangle p;
            if (CurrentLocationIndex.Y > maxY)
            {
                CurrentLocationIndex.Y = maxY;
            }
            //  Log.AsyncC("CRY");
            componentList.TryGetValue(CurrentLocationIndex, out p);


            updateMouse(getComponentCenter(p));
        }

        public override void moveUp()
        {
            if (canMoveInMenu == false) return;
            activateTimer();
            CurrentLocationIndex.Y--;

            Rectangle p;

            //  Log.AsyncC("CRY");
         componentList.TryGetValue(CurrentLocationIndex, out p);

            if (CurrentLocationIndex.Y < minY)
            {
                CurrentLocationIndex.Y = minY;
            }

            updateMouse(getComponentCenter(p));
        }

        public override void resize()
        {
            Class1.compatabilityMenu = new TitleMenu();
        }

    }
}
