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
        /// Make constructor that takes all parameters for a function.
        /// Make an update function that runs associated functions to check which events need to be ran.
        /// 
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
        /// Occurs when the player enters the same tile as this event. The function associated with this event is then ran.
        /// </summary>
        public void OnPlayerEnter()
        {
            this.playerOnTile = true;
            if (this.playerEvents.onPlayerEnter != null) this.playerEvents.onPlayerEnter.run();
        }

        /// <summary>
        /// Occurs when the player leaves the same tile that this event is on. The function associated with thie event is then ran.
        /// </summary>
        public void OnPlayerLeave()
        {
            this.playerOnTile = false;
            if (this.playerEvents.onPlayerLeave != null) this.playerEvents.onPlayerEnter.run();
        }

        /// <summary>
        /// Occurs when the player left clicks the same tile that this event is on.
        /// </summary>
        public void OnLeftClick()
        {
            if (this.mouseButtonEvents.onLeftClick != null) this.mouseButtonEvents.onLeftClick.run();
        }

        /// <summary>
        /// Occurs when the player right clicks the same tile that this event is on.
        /// </summary>
        public void OnRightClick()
        {
            if (this.mouseButtonEvents.onRightClick != null) this.mouseButtonEvents.onRightClick.run();
        }

        /// <summary>
        /// Occurs when the mouse tile position is the same as this event's x,y position.
        /// </summary>
        public void OnMouseEnter()
        {
            this.mouseOnTile = true;
            if (this.mouseEntryLeaveEvents.onMouseEnter != null) this.mouseEntryLeaveEvents.onMouseEnter.run();
        }

        /// <summary>
        /// Occurs when the mouse tile position leaves the the same x,y position as this event.
        /// </summary>
        public void OnMouseLeave()
        {
            this.mouseOnTile = false;
            if (this.mouseEntryLeaveEvents.onMouseLeave != null) this.mouseEntryLeaveEvents.onMouseLeave.run();
        }

        /// <summary>
        /// Occurs when the mouse is on the same position as the tile AND the user scrolls the mouse wheel.
        /// </summary>
        public void OnMouseScroll()
        {
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

    }
}
