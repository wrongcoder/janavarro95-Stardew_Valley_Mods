using CustomNPCFramework.Framework.NPCS;
using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNPCFramework.Framework.Utilities
{
    public class NPCTracker
    {
        /// <summary>
        /// A list used to keep track of the npcs.
        /// </summary>
        public List<ExtendedNPC> moddedNPCS;

        /// <summary>
        /// Constructor.
        /// </summary>
        public NPCTracker()
        {
            this.moddedNPCS = new List<ExtendedNPC>();
        }

        /// <summary>
        /// Use this to add a new npc into the game.
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="npc"></param>
        public void addNewNPCToLocation(GameLocation loc,ExtendedNPC npc)
        {
            this.moddedNPCS.Add(npc);
            npc.defaultLocation = loc;
            npc.currentLocation = loc;
            loc.addCharacter(npc);
        }

        public void addNewNPCToLocation(GameLocation loc, ExtendedNPC npc, Vector2 tilePosition)
        {
            this.moddedNPCS.Add(npc);
            npc.defaultLocation = loc;
            npc.currentLocation = loc;
            npc.position = tilePosition*Game1.tileSize;
            loc.addCharacter(npc);
        }

        /// <summary>
        /// Use this simply to remove a single npc from a location.
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="npc"></param>
        public void removeCharacterFromLocation(GameLocation loc, ExtendedNPC npc)
        {
            loc.characters.Remove(npc);
        }
        
        /// <summary>
        /// Use this to completly remove and npc from the game as it is removed from the location and is no longer tracked.
        /// </summary>
        /// <param name="npc"></param>
        public void removeFromLocationAndTrackingList(ExtendedNPC npc)
        {
            if (npc.currentLocation != null)
            {
                npc.currentLocation.characters.Remove(npc);
            }
            this.moddedNPCS.Remove(npc);
        }

        /// <summary>
        /// Use this to clean up all of the npcs before the game is saved.
        /// </summary>
        public void cleanUpBeforeSave()
        {
            foreach(ExtendedNPC npc in this.moddedNPCS)
            {
                //npc.currentLocation.characters.Remove(npc);
                //Game1.removeThisCharacterFromAllLocations(npc);
                Game1.removeCharacterFromItsLocation(npc.name);
                Class1.ModMonitor.Log("Removed an npc!");
                //Do some saving code here.
            }
            
        }

        /// <summary>
        /// Use this to load in all of the npcs again after saving.
        /// </summary>
        public void afterSave()
        {
            foreach(ExtendedNPC npc in this.moddedNPCS)
            {
                npc.defaultLocation.addCharacter(npc);
            }
        }
        
    }
}
