using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;
using System.Timers;
using StardewModdingAPI;

namespace Revitalize.Menus.Compatability
{
    class MenuCompatabilityBase : Revitalize.Menus.Compatability.CompatInterface
    {

        public static Point startingPositionIndex = new Point(0, 1);
        public static Point CurrentLocationIndex;
        public Dictionary<Point, Rectangle> componentList;
        public int width;
        public int height;

        public int maxX;
        public int minX;
        public int maxY;
        public int minY;


        public static int millisecondMoveDelay;
        public static System.Timers.Timer movementTimer;
        public static bool canMoveInMenu;

        public static void activateTimer() { 
             SetTimer();        
   }

    private static void SetTimer()
    {
        
            // Create a timer with a two second interval.
            if (canMoveInMenu == true)
            {
                movementTimer = new System.Timers.Timer(millisecondMoveDelay);
                // Hook up the Elapsed event for the timer. 
                movementTimer.Elapsed += OnTimedEvent;
                movementTimer.AutoReset = false;
                movementTimer.Enabled = true;
                canMoveInMenu = false;
            }
            else
            {
                return;
            }
    }

    private static void OnTimedEvent(System.Object source, ElapsedEventArgs e)
    {
            movementTimer.Enabled = false;
           // movementTimer.Dispose();
            canMoveInMenu = true;
    }

    public Dictionary<Point, Rectangle> ComponentList
        {
            get
            {
                return this.componentList;
                //  throw new NotImplementedException();
                //  return this.componentList;
            }

            set
            {
                // throw new NotImplementedException();
            }
        }

        public virtual void Compatability()
        {
           // throw new NotImplementedException();
        }

        

        public virtual void moveLeft()
        {
          //  throw new NotImplementedException();
        }

        public virtual void moveRight()
        {
            //throw new NotImplementedException();
        }

        public virtual void Update()
        {
           // throw new NotImplementedException();
        }

        public virtual void updateMouse(Point p)
        {
            
            if (p.X == 0 && p.Y == 0) p = startingPositionIndex;
                Game1.setMousePosition(p);
            
        }

        public virtual Point getComponentCenter(Rectangle r)
        {
            // throw new NotImplementedException();
            return new Point(r.X + r.Width / 2, r.Y + r.Height / 2);
        }

        public virtual void moveUp()
        {
          //  throw new NotImplementedException();
        }

        public virtual void moveDown()
        {
           // throw new NotImplementedException();
        }

        public virtual void resize()
        {
            
        }
    }
}
