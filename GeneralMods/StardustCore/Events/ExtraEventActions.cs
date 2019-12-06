using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;

namespace StardustCore.Events
{
    /// <summary>
    /// Contains functions that are used to parse event data and do additional things.
    /// </summary>
    public class ExtraEventActions
    {

        private static Point OldViewportPosition;
        private static bool StartedLerp;
        private static int CurrentViewportLerpAmount;

        /// <summary>
        /// Adds the item from Game1.ObjectInformation to the player's inventory from the given event string.
        /// </summary>
        /// <param name="EventData"></param>
        public static void addObjectToPlayerInventory(EventManager EventManager,string EventData)
        {
            string[] splits = EventData.Split(' ');
            string name = splits[0];
            int parentSheetIndex =Convert.ToInt32(splits[1]);
            int amount = Convert.ToInt32(splits[2]);
            bool makeActiveObject = Convert.ToBoolean(splits[3]);
            StardewValley.Object obj = new StardewValley.Object(parentSheetIndex, amount);
            Game1.player.addItemToInventoryBool(obj,makeActiveObject);
            Game1.CurrentEvent.CurrentCommand++;
        }


        /// <summary>
        /// Lerp the camera to a specified position.
        /// </summary>
        /// <param name="EventManager"></param>
        /// <param name="EventData"></param>
        public static void ViewportLerp(EventManager EventManager,string EventData)
        {
            string[] splits = EventData.Split(' ');
            string name = splits[0];

            int xEndPosition =Convert.ToInt32(splits[1]);
            int yEndPosition = Convert.ToInt32(splits[2]);
            int frames = Convert.ToInt32(splits[3]);
            bool concurrent = Convert.ToBoolean(splits[4]);
            if (concurrent)
            {
                if (EventManager.concurrentEventActions.ContainsKey(EventData)==false)
                {
                    EventManager.addConcurrentEvent(EventData, ViewportLerp);
                    ++Game1.CurrentEvent.CurrentCommand; //I've been told ++<int> is more efficient than <int>++;
                }
            }

            if (StartedLerp==false)
            {
                OldViewportPosition = new Point(Game1.viewport.Location.X,Game1.viewport.Location.Y);
                StartedLerp = true;
            }

            ++CurrentViewportLerpAmount;
            if (CurrentViewportLerpAmount >= frames)
            {
                Vector2 currentLerp2 = Vector2.Lerp(new Vector2(OldViewportPosition.X, OldViewportPosition.Y), new Vector2(OldViewportPosition.X+xEndPosition, OldViewportPosition.Y+yEndPosition), (float)((float)CurrentViewportLerpAmount/(float)frames));
                Game1.viewport.Location = new xTile.Dimensions.Location((int)currentLerp2.X, (int)currentLerp2.Y);

                OldViewportPosition = new Point(0, 0);
                CurrentViewportLerpAmount = 0;
                StartedLerp = false;
                if(concurrent==false)++Game1.CurrentEvent.CurrentCommand; //I've been told ++<int> is more efficient than <int>++;
                else
                {
                    EventManager.finishConcurrentEvent(EventData);
                }
                return;
            }
            Vector2 currentLerp = Vector2.Lerp(new Vector2(OldViewportPosition.X, OldViewportPosition.Y), new Vector2(OldViewportPosition.X + xEndPosition, OldViewportPosition.Y + yEndPosition), (float)((float)CurrentViewportLerpAmount/(float)frames));
            Game1.viewport.Location = new xTile.Dimensions.Location((int)currentLerp.X, (int)currentLerp.Y);
        }
    }
}
