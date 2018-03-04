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
    /// <summary>
    /// Used to handle warp events on the map.
    /// </summary>
    public class WarpEvent :MapEvent
    {
        WarpInformation warpInfo;

        /// <summary>
        /// Constructor for handling warp events.
        /// </summary>
        /// <param name="Name">The name of the event.</param>
        /// <param name="Location">The game location that this event is located at.</param>
        /// <param name="Position">The x,y tile position of the event.</param>
        /// <param name="playerEvents">The events to occur when the player enters the warp tile before the warp.</param>
        /// <param name="WarpInfo">The information for warping the farmer.</param>
        public WarpEvent(string Name, GameLocation Location, Vector2 Position, PlayerEvents playerEvents,WarpInformation WarpInfo) : base(Name, Location, Position, playerEvents)
        {
            this.name = Name;
            this.location = Location;
            this.tilePosition = Position;
            this.playerEvents = playerEvents;
            this.warpInfo = WarpInfo;

            this.doesInteractionNeedToRun = true;
        }

        /// <summary>
        /// Occurs when the player enters the warp tile event position.
        /// </summary>
        public override void OnPlayerEnter()
        {
            if (isPlayerOnTile() == true&& this.doesInteractionNeedToRun==true)
            {
                this.doesInteractionNeedToRun = false;
                this.playerOnTile = true;
                if (this.playerEvents.onPlayerEnter != null) this.playerEvents.onPlayerEnter.run(); //used to run a function before the warp.
                Game1.warpFarmer(Game1.getLocationFromName(this.warpInfo.targetMapName),this.warpInfo.targetX,this.warpInfo.targetY,this.warpInfo.facingDirection,this.warpInfo.isStructure);
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
                if (this.playerEvents.onPlayerLeave != null) this.playerEvents.onPlayerLeave.run();
            }    
        }

        /// <summary>
        /// Used to update the event and check for interaction.
        /// </summary>
        public override void update()
        {
            this.OnPlayerEnter();
            this.OnPlayerLeave();
        }


    }
}
