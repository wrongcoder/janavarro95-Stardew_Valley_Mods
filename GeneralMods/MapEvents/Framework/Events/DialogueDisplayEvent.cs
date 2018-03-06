using EventSystem.Framework.FunctionEvents;
using EventSystem.Framework.Information;
using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Framework.Events
{
    public class DialogueDisplayEvent :MapEvent
    {
        string dialogue;
        public DialogueDisplayEvent(string Name, GameLocation Location, Vector2 Position, PlayerEvents playerEvents, MouseButtonEvents MouseEvents,MouseEntryLeaveEvent EntryLeave, string Dialogue) : base(Name, Location, Position, playerEvents)
        {
            this.name = Name;
            this.location = Location;
            this.tilePosition = Position;
            this.playerEvents = playerEvents;
            this.mouseButtonEvents = MouseEvents;

            this.doesInteractionNeedToRun = true;
            this.dialogue = Dialogue;

            this.mouseEntryLeaveEvents = EntryLeave;
        }


        /// <summary>
        /// Occurs when the player enters the warp tile event position.
        /// </summary>s
        public override void OnPlayerEnter()
        {
            if (isPlayerOnTile() == true && this.doesInteractionNeedToRun == true)
            {
                this.doesInteractionNeedToRun = false;
                this.playerOnTile = true;
                if (this.playerEvents != null)
                {
                    if (this.playerEvents.onPlayerEnter != null) this.playerEvents.onPlayerEnter.run(); //used to run a function before the warp.
                }
                Game1.activeClickableMenu =new StardewValley.Menus.DialogueBox(this.dialogue);
                //Game1.drawDialogueBox(this.dialogue);     
            }
        }

        /// <summary>
        /// Runs when the player is not on the tile and resets player interaction.
        /// </summary>
        public override void OnPlayerLeave()
        {
            if (isPlayerOnTile() == false && this.playerOnTile == true)
            {
                this.playerOnTile = false;
                this.doesInteractionNeedToRun = true;
                if (this.playerEvents != null)
                {
                    if (this.playerEvents.onPlayerLeave != null) this.playerEvents.onPlayerLeave.run();
                }
            }
        }

        public override void OnLeftClick()
        {
            if (Game1.activeClickableMenu != null) return;
            if (this.mouseButtonEvents == null) return;
            if (this.mouseOnTile == false) return;
            if (this.location.isObjectAt((int)this.tilePosition.X*Game1.tileSize, (int)this.tilePosition.Y*Game1.tileSize)) return;
            if (this.mouseButtonEvents.onLeftClick != null) this.mouseButtonEvents.onLeftClick.run();
            Game1.activeClickableMenu = new StardewValley.Menus.DialogueBox(this.dialogue);
        }

        /// <summary>
        /// Used to update the event and check for interaction.
        /// </summary>
        public override void update()
        {
            this.clickEvent();
            this.OnMouseEnter();
            this.OnMouseLeave();
            this.OnPlayerEnter();
            this.OnPlayerLeave();
        }

    }
}
