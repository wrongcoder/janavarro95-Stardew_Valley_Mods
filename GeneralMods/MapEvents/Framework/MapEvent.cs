using MapEvents.Framework.FunctionEvents;
using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MapEvents.Framework.Delegates;

namespace MapEvents.Framework
{
    class MapEvent
    {
        /// <summary>
        /// Make an update function that runs associated functions to check which events need to be ran.
        /// make way to detect which button is clicked when on this tile.
        /// //MAKE A MAP EVENT MANAGER TO HOLD GAME LOCATIONS AND A LIST OF MAP EVENTS!!!!!! Dic<Loc.List<Events>>
        /// </summary>


        public Vector2 tilePosition;
        public GameLocation location;

        public PlayerEvents playerEvents;

        public bool playerOnTile;


        public MouseButtonEvents mouseButtonEvents;
        public MouseEntryLeaveEvent mouseEntryLeaveEvents;
        public bool mouseOnTile;

        

        public bool doesInteractionNeedToRun;
        public bool loopInteraction;

        /// <summary>
        /// A simple map event that doesn't do anything.
        /// </summary>
        /// <param name="Location"></param>
        /// <param name="Position"></param>
        public MapEvent(GameLocation Location,Vector2 Position)
        {
            this.location = Location;
            this.tilePosition = Position;
        }

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public MapEvent()
        {

        }


        /// <summary>
        /// A simple map function that runs when the player enters and leaves a tile. Set values to null for nothing to happen.
        /// </summary>
        /// <param name="Location">The game location where the event is. I.E Farm, Town, Mine etc.</param>
        /// <param name="position">The x,y position on the map the event is.</param>
        /// <param name="PlayerEvents">Handles various events that runs when the player enters/leaves a tile, etc.</param>
       
        public MapEvent(GameLocation Location,Vector2 position, PlayerEvents PlayerEvents)
        {
            this.location = Location;
            this.tilePosition = position;
            this.playerEvents = PlayerEvents;
        }

        /// <summary>
        /// A constructor that handles when the mouse leaves and enters a tile.
        /// </summary>
        /// <param name="Location">The game location where the event is.</param>
        /// <param name="Position">The x,y position of the tile at the game location.</param>
        /// <param name="mouseEvents">A class used to handle mouse entry/leave events.</param>
        public MapEvent(GameLocation Location, Vector2 Position, MouseEntryLeaveEvent mouseEvents)
        {
            this.location = Location;
            this.tilePosition = Position;
            this.mouseEntryLeaveEvents = mouseEvents;
        }

        /// <summary>
        /// A constructor that handles when the mouse leaves and enters a tile.
        /// </summary>
        /// <param name="Location">The game location where the event is.</param>
        /// <param name="Position">The x,y position of the tile at the game location.</param>
        /// <param name="mouseEvents">A class used to handle mouse click/scroll events.</param>
        public MapEvent(GameLocation Location, Vector2 Position, MouseButtonEvents mouseEvents)
        {
            this.location = Location;
            this.tilePosition = Position;
            this.mouseButtonEvents = mouseEvents;
        }

        /// <summary>
        /// A constructor encapsulating player, mouse button, and mouse entry events.
        /// </summary>
        /// <param name="Location">The game location for which the event is located. I.E Town, Farm, etc.</param>
        /// <param name="Position">The x,y cordinates for this event to be located at.</param>
        /// <param name="playerEvents">The events that occur associated with the player. I.E player entry, etc.</param>
        /// <param name="mouseButtonEvents">The events associated with clicking a mouse button while on this tile.</param>
        /// <param name="mouseEntryLeaveEvents">The events that occur when the mouse enters or leaves the same tile position as this event.</param>
        public MapEvent(GameLocation Location, Vector2 Position, PlayerEvents playerEvents, MouseButtonEvents mouseButtonEvents, MouseEntryLeaveEvent mouseEntryLeaveEvents)
        {
            this.location = Location;
            this.tilePosition = Position;
            this.playerEvents = playerEvents;
            this.mouseButtonEvents = mouseButtonEvents;
            this.mouseEntryLeaveEvents = mouseEntryLeaveEvents;
        }

        /// <summary>
        /// Occurs when the player enters the same tile as this event. The function associated with this event is then ran.
        /// </summary>
        private void OnPlayerEnter()
        {
            this.playerOnTile = true;
            if (this.playerEvents.onPlayerEnter != null) this.playerEvents.onPlayerEnter.run();
        }

        /// <summary>
        /// Occurs when the player leaves the same tile that this event is on. The function associated with thie event is then ran.
        /// </summary>
        private void OnPlayerLeave()
        {
            this.playerOnTile = false;
            if (this.playerEvents.onPlayerLeave != null) this.playerEvents.onPlayerEnter.run();
        }

        /// <summary>
        /// Occurs when the player left clicks the same tile that this event is on.
        /// </summary>
        public void OnLeftClick()
        {
            if (this.mouseOnTile==false) return;
            if (this.mouseButtonEvents.onLeftClick != null) this.mouseButtonEvents.onLeftClick.run();
        }

        /// <summary>
        /// Occurs when the player right clicks the same tile that this event is on.
        /// </summary>
        public void OnRightClick()
        {
            if (this.mouseOnTile == false) return;
            if (this.mouseButtonEvents.onRightClick != null) this.mouseButtonEvents.onRightClick.run();
        }

        /// <summary>
        /// Occurs when the mouse tile position is the same as this event's x,y position.
        /// </summary>
        public void OnMouseEnter()
        {
            if (isMouseOnTile())
            {
                this.mouseOnTile = true;
                if (this.mouseEntryLeaveEvents.onMouseEnter != null) this.mouseEntryLeaveEvents.onMouseEnter.run();
            }
        }

        /// <summary>
        /// Occurs when the mouse tile position leaves the the same x,y position as this event.
        /// </summary>
        public void OnMouseLeave()
        {
            if (isMouseOnTile() == false && this.mouseOnTile == true)
            {
                this.mouseOnTile = false;
                if (this.mouseEntryLeaveEvents.onMouseLeave != null) this.mouseEntryLeaveEvents.onMouseLeave.run();
            }
        }

        /// <summary>
        /// Occurs when the mouse is on the same position as the tile AND the user scrolls the mouse wheel.
        /// </summary>
        public void OnMouseScroll()
        {
            if (isMouseOnTile() == false) return;
            if (this.mouseButtonEvents.onMouseScroll != null) this.mouseButtonEvents.onMouseScroll.run();
        }

        /// <summary>
        /// Checks if the player is on the same tile as this event.
        /// </summary>
        /// <returns></returns>
        public bool isPlayerOnTile()
        {
            if (Game1.player.getTileX() == this.tilePosition.X && Game1.player.getTileY() == this.tilePosition.Y) return true;
            else return false;
        }

        /// <summary>
        /// Checks if the player is on the same tile as the event and then runs the associated event.
        /// </summary>
        public void isPlayerOnTileRunEvent()
        {
            if (isPlayerOnTile() == true) OnPlayerEnter();
        }

        /// <summary>
        /// If the player recently entered the tile and the player is no longer on this tile then the player left the tile. If that is true then run the leaving function.
        /// </summary>
        public void didPlayerLeaveTileRunEvent()
        {
            if (this.playerOnTile == true && isPlayerOnTile() == false) this.OnPlayerLeave();
        }

        /// <summary>
        /// Checks if the mouse is on the tile.
        /// </summary>
        /// <returns></returns>
        public bool isMouseOnTile()
        {
            Vector2 mousePosition = new Vector2(Game1.getMouseX(), Game1.getMouseY());
            if (mousePosition.X == this.tilePosition.X && mousePosition.Y == this.tilePosition.Y) return true;
            return false;
        }

       

    }
}
