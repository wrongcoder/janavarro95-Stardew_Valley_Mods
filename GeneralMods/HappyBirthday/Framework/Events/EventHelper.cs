using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Omegasis.HappyBirthday.Framework.Events.Preconditions;
using Omegasis.HappyBirthday.Framework.Events.Preconditions.TimeSpecific;
using StardewValley;

namespace Omegasis.HappyBirthday.Framework.Events
{
    /// <summary>
    /// Helps in creating events based in code for stardew valley.
    /// https://stardewvalleywiki.com/Modding:Event_data
    /// </summary>
    public class EventHelper
    {

        /// <summary>
        /// Nexus user id for Omegasis (aka me).
        /// </summary>
        protected int nexusUserId = 32171640;

        /// <summary>
        /// Wraps SDV facing direction.
        /// </summary>
        public enum FacingDirection
        {
            Up,
            Right,
            Down,
            Left
        }

        public enum Layers
        {
            Back,
            Paths,
            Buildings,
            Front,
            AlwaysFront
        }


        protected StringBuilder eventData;
        protected StringBuilder eventPreconditionData;
        protected List<EventPrecondition> eventPreconditions;
        protected int eventID;

        public EventHelper()
        {
            this.eventData = new StringBuilder();
            this.eventPreconditionData = new StringBuilder();
            this.eventPreconditions = new List<EventPrecondition>();
        }

        public EventHelper(int ID,LocationPrecondition Location ,TimePrecondition Time, EventDayExclusionPrecondition NotTheseDays, EventStartData StartData)
        {
            this.eventData = new StringBuilder();
            this.eventPreconditionData = new StringBuilder();
            this.eventPreconditions = new List<EventPrecondition>();
            this.eventID = ID;
            this.add(Location);
            this.add(Time);
            this.add(NotTheseDays);
            this.add(StartData.ToString());
        }

        public EventHelper(List<EventPrecondition> Conditions, EventStartData StartData)
        {
            this.eventData = new StringBuilder();
            this.eventPreconditions = new List<EventPrecondition>();
            this.eventPreconditionData = new StringBuilder();
            foreach (var v in Conditions)
            {
                this.add(v);
            }
            this.add(StartData.ToString());
        }

        /// <summary>
        /// Adds in the event precondition data to the string builder and appends seperators as necessary.
        /// </summary>
        /// <param name="Data"></param>
        public virtual void add(EventPrecondition Data)
        {
            this.eventPreconditions.Add(Data);
            if (this.eventPreconditionData.Length > 0)
            {
                this.eventPreconditionData.Append(this.getSeperator());
            }
            this.eventPreconditionData.Append(Data.ToString());
        }

        /// <summary>
        /// Adds in the data to the event data.Aka what happens during the event.
        /// </summary>
        /// <param name="Data"></param>
        protected virtual void add(string Data)
        {

            if (this.eventData.Length > 0)
            {
                this.eventData.Append(this.getSeperator());
            }
            this.eventData.Append(Data);
        }

        /// <summary>
        /// Adds in the data to the event data. Aka what happens during the event.
        /// </summary>
        /// <param name="Builder"></param>
        protected virtual void add(StringBuilder Builder)
        {
            this.add(Builder.ToString());
        }


        /// <summary>
        /// Converts the direction to enum.
        /// </summary>
        /// <param name="Dir"></param>
        /// <returns></returns>
        protected virtual int getFacingDirectionNumber(FacingDirection Dir)
        {
            return (int)Dir;
        }

        /// <summary>
        /// Gets the layer string from the Layer enum.
        /// </summary>
        /// <param name="Layer"></param>
        /// <returns></returns>
        protected virtual string getLayerName(Layers Layer)
        {
            if (Layer == Layers.AlwaysFront) return "AlwaysFront";
            if (Layer == Layers.Back) return "Back";
            if (Layer == Layers.Buildings) return "Buildings";
            if (Layer == Layers.Front) return "Front";
            if (Layer == Layers.Paths) return "Paths";
            return "";
        }

        /// <summary>
        /// Gets the even parsing seperator.
        /// </summary>
        /// <returns></returns>
        protected virtual string getSeperator()
        {
            return "/";
        }

        /// <summary>
        /// Gets the starting event numbers based off of my nexus user id.
        /// </summary>
        /// <returns></returns>
        protected virtual string getUniqueEventStartID()
        {
            string s = this.nexusUserId.ToString();
            return s.Substring(0, 4);
        }

        protected virtual string getEventID()
        {
            return this.getUniqueEventStartID() + this.eventID.ToString();
        }

        /// <summary>
        /// Checks to ensure I don't create a id value that is too big for nexus.
        /// </summary>
        /// <param name="IDToCheck"></param>
        /// <returns></returns>
        protected virtual bool isIdValid(int IDToCheck)
        {
            if (IDToCheck > 2147483647 || IDToCheck < 0) return false;
            else return true;
        }

        /// <summary>
        /// Checks to ensure I don't create a id value that is too big for nexus.
        /// </summary>
        /// <param name="IDToCheck"></param>
        /// <returns></returns>
        protected virtual bool isIdValid(string IDToCheck)
        {
            if (Convert.ToInt32(IDToCheck) > 2147483647 || Convert.ToInt32(IDToCheck) < 0) return false;
            else return true;
        }

        protected virtual string getEventString()
        {
            return this.eventData.ToString();
        }

        protected virtual StardewValley.Event getEvent(Farmer PlayerActor = null)
        {
            return new StardewValley.Event(this.getEventString(), Convert.ToInt32(this.getEventID()), PlayerActor);
        }

        /// <summary>
        /// Checks to see if all of the event preconditions have been met and starts the event if so.
        /// </summary>
        protected virtual void startEventAtLocationifPossible()
        {
            if (this.canEventOccur())
            {
                //Game1.player.currentLocation.currentEvent = this.getEvent();
                Game1.player.currentLocation.startEvent(this.getEvent());
            }
        }

        //~~~~~~~~~~~~~~~~//
        //   Validation   //
        //~~~~~~~~~~~~~~~~//

            /// <summary>
            /// Checks to see if the event can occur.
            /// </summary>
            /// <returns></returns>
        protected virtual bool canEventOccur()
        {
            foreach(EventPrecondition eve in this.eventPreconditions)
            {
                if (eve.meetsCondition() == false) return false;
            }

            return true;
        }

        //~~~~~~~~~~~~~~~~//
        //      Actions   //
        //~~~~~~~~~~~~~~~~//

        /// <summary>
        /// Adds an object at the specified tile from the TileSheets\Craftables.png sprite sheet
        /// </summary>
        /// <param name="xTile"></param>
        /// <param name="yTile"></param>
        /// <param name="ID"></param>
        protected virtual void addBigProp(int xTile, int yTile, int ID)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addBigProp ");
            b.Append(xTile.ToString());
            b.Append(" ");
            b.Append(yTile.ToString());
            b.Append(" ");
            b.Append(ID.ToString());
            this.add(b);
        }

        /// <summary>
        /// Starts an active dialogue event with the given ID and a length of 4 days.
        /// </summary>
        /// <param name="ID"></param>
        protected virtual void addConversationTopic(string ID)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addBigProp ");
            b.Append(ID);
            this.add(b);
        }

        /// <summary>
        /// Adds the specified cooking recipe to the player.
        /// </summary>
        /// <param name="Recipe"></param>
        protected virtual void addCookingRecipe(string Recipe)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addCookingRecipe ");
            b.Append(Recipe);
            this.add(b);
        }

        /// <summary>
        /// Adds the specified crafting recipe to the player.
        /// </summary>
        /// <param name="Recipe"></param>
        protected virtual void addCraftingRecipe(string Recipe)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addCraftingRecipe ");
            b.Append(Recipe);
            this.add(b);
        }

        /// <summary>
        /// Add a non-solid prop from the current festival texture. Default solid width/height is 1. Default display height is solid height.
        /// </summary>
        protected virtual void addFloorProp(int PropIndex, int XTile, int YTile, int SolidWidth, int SolidHeight, int DisplayHeight)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addFloorProp ");
            b.Append(PropIndex.ToString());
            b.Append(" ");
            b.Append(XTile.ToString());
            b.Append(" ");
            b.Append(YTile.ToString());
            b.Append(" ");
            b.Append(SolidWidth.ToString());
            b.Append(" ");
            b.Append(SolidHeight.ToString());
            b.Append(" ");
            b.Append(DisplayHeight.ToString());
            this.add(b);
        }

        /// <summary>
        /// Adds a glowing temporary sprite at the specified tile from the Maps\springobjects.png sprite sheet. A light radius of 0 just places the sprite.
        /// </summary>
        /// <param name="ItemID"></param>
        /// <param name="XPosition"></param>
        /// <param name="YPosition"></param>
        /// <param name="LightRadius"></param>
        protected virtual void addLantern(int ItemID, int XPosition, int YPosition, float LightRadius)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addLantern ");
            b.Append(ItemID.ToString());
            b.Append(" ");
            b.Append(XPosition.ToString());
            b.Append(" ");
            b.Append(YPosition.ToString());
            b.Append(" ");
            b.Append(LightRadius.ToString());
            this.add(b);
        }

        /// <summary>
        /// 	Set a letter as received.
        /// </summary>
        /// <param name="ID"></param>
        protected virtual void addMailReceived(string ID)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addMailReceived ");
            b.Append(ID);
            this.add(b);
        }

        /// <summary>
        /// Adds a temporary sprite at the specified tile from the Maps\springobjects.png sprite sheet.
        /// </summary>
        /// <param name="XTile"></param>
        /// <param name="YTile"></param>
        /// <param name="ParentSheetIndex"></param>
        /// <param name="Layer"></param>
        protected virtual void addObject(int XTile, int YTile, int ParentSheetIndex, string Layer)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addObject ");
            b.Append(XTile.ToString());
            b.Append(" ");
            b.Append(YTile.ToString());
            b.Append(" ");
            b.Append(Layer);
            this.add(b);
        }

        /// <summary>
        /// Adds a temporary sprite at the specified tile from the Maps\springobjects.png sprite sheet.
        /// </summary>
        /// <param name="XTile"></param>
        /// <param name="YTile"></param>
        /// <param name="ParentSheetIndex"></param>
        /// <param name="Layer"></param>
        protected virtual void addObject(int XTile, int YTile, int ParentSheetIndex, Layers Layer)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addObject ");
            b.Append(XTile.ToString());
            b.Append(" ");
            b.Append(YTile.ToString());
            b.Append(" ");
            b.Append(this.getLayerName(Layer));
            this.add(b);
        }

        /// <summary>
        /// Add a solid prop from the current festival texture. Default solid width/height is 1. Default display height is solid height.
        /// </summary>
        /// <param name="Index"></param>
        /// <param name="XTile"></param>
        /// <param name="YTile"></param>
        /// <param name="SolidWidth"></param>
        /// <param name="SolidHeight"></param>
        /// <param name="DisplayHeight"></param>
        protected virtual void addProp(int Index, int XTile, int YTile, int SolidWidth, int SolidHeight, int DisplayHeight)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addProp ");
            b.Append(Index.ToString());
            b.Append(" ");
            b.Append(XTile.ToString());
            b.Append(" ");
            b.Append(YTile.ToString());
            b.Append(" ");
            b.Append(SolidWidth.ToString());
            b.Append(" ");
            b.Append(SolidHeight.ToString());
            b.Append(" ");
            b.Append(DisplayHeight.ToString());
            this.add(b);
        }

        /// <summary>
        /// Add the specified quest to the quest log.
        /// </summary>
        /// <param name="QuestID"></param>
        public virtual void addQuest(int QuestID)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addQuest ");
            b.Append(QuestID.ToString());
            this.add(b);
        }

        /// <summary>
        /// Add a temporary actor. 'breather' is boolean. The category determines where the texture will be loaded from, default is Character. Animal name only applies to animal.
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="SpriteWidth"></param>
        /// <param name="SpriteHeight"></param>
        /// <param name="TileX"></param>
        /// <param name="TileY"></param>
        /// <param name="Direction"></param>
        /// <param name="Breather"></param>
        public virtual void addTemporaryActor_NPC(NPC npc, int SpriteWidth, int SpriteHeight, int TileX, int TileY, FacingDirection Direction, bool Breather)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addTemporaryActor ");
            b.Append(npc.Name);
            b.Append(" ");
            b.Append(SpriteWidth.ToString());
            b.Append(" ");
            b.Append(SpriteHeight.ToString());
            b.Append(" ");
            b.Append(TileX.ToString());
            b.Append(" ");
            b.Append(TileY.ToString());
            b.Append(" ");
            b.Append(Direction);
            b.Append(" ");
            b.Append("Character");
            this.add(b);
        }

        /// <summary>
        /// Add a temporary actor. 'breather' is boolean. The category determines where the texture will be loaded from, default is Character. Animal name only applies to animal.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="SpriteWidth"></param>
        /// <param name="SpriteHeight"></param>
        /// <param name="TileX"></param>
        /// <param name="TileY"></param>
        /// <param name="Direction"></param>
        /// <param name="Breather"></param>
        /// <param name="AnimalName"></param>
        public virtual void addTemporaryActor_Animal(string character, int SpriteWidth, int SpriteHeight, int TileX, int TileY, FacingDirection Direction, bool Breather,string AnimalName)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addTemporaryActor ");
            b.Append(character);
            b.Append(" ");
            b.Append(SpriteWidth.ToString());
            b.Append(" ");
            b.Append(SpriteHeight.ToString());
            b.Append(" ");
            b.Append(TileX.ToString());
            b.Append(" ");
            b.Append(TileY.ToString());
            b.Append(" ");
            b.Append(Direction);
            b.Append(" ");
            b.Append("Animal");
            b.Append(" ");
            b.Append(AnimalName);
            this.add(b);
        }

        /// <summary>
        /// Add a temporary actor. 'breather' is boolean. The category determines where the texture will be loaded from, default is Character. Animal name only applies to animal.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="SpriteWidth"></param>
        /// <param name="SpriteHeight"></param>
        /// <param name="TileX"></param>
        /// <param name="TileY"></param>
        /// <param name="Direction"></param>
        /// <param name="Breather"></param>
        public virtual void addTemporaryActor_Monster(string character, int SpriteWidth, int SpriteHeight, int TileX, int TileY, FacingDirection Direction, bool Breather)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addTemporaryActor ");
            b.Append(character);
            b.Append(" ");
            b.Append(SpriteWidth.ToString());
            b.Append(" ");
            b.Append(SpriteHeight.ToString());
            b.Append(" ");
            b.Append(TileX.ToString());
            b.Append(" ");
            b.Append(TileY.ToString());
            b.Append(" ");
            b.Append(Direction);
            b.Append(" ");
            b.Append("Monster");
            this.add(b);
        }

        /// <summary>
        /// Places on object on the furniture at a position. If the location is FarmHouse, then it will always be placed on the initial table.
        /// </summary>
        /// <param name="XPosition"></param>
        /// <param name="YPosition"></param>
        /// <param name="ObjectParentSheetIndex"></param>
        public virtual void addToTable(int XPosition, int YPosition, int ObjectParentSheetIndex)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addToTable ");
            b.Append(XPosition);
            b.Append(" ");
            b.Append(YPosition);
            b.Append(" ");
            b.Append(ObjectParentSheetIndex);
            this.add(b);
        }

        /// <summary>
        /// Adds the Return Scepter to the player's inventory.
        /// </summary>
        public virtual void addTool_ReturnScepter()
        {
            StringBuilder b = new StringBuilder();
            b.Append("addTool Wand");
            this.add(b);
        }

        /// <summary>
        /// 	Set multiple movements for an NPC. You can set True to have NPC walk the path continuously. Example: /advancedMove Robin false 0 3 2 0 0 2 -2 0 0 -2 2 0/
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="Loop"></param>
        /// <param name="TilePoints"></param>
        public virtual void advanceMove(NPC npc, bool Loop, List<Point> TilePoints)
        {
            StringBuilder b = new StringBuilder();
            b.Append("advancedMove ");
            b.Append(npc.Name);
            b.Append(" ");
            b.Append(Loop.ToString());
            b.Append(" ");
            for(int i = 0; i < TilePoints.Count; i++)
            {
                b.Append(TilePoints[i].X);
                b.Append(" ");
                b.Append(TilePoints[i].Y);
                if (i != TilePoints.Count - 1)
                {
                    b.Append(" ");
                }
            }
            this.add(b);
        }

        /// <summary>
        /// Modifies the ambient light level, with RGB values from 0 to 255. Note that it works by removing colors from the existing light ambience, so ambientLight 1 80 80 would reduce green and blue and leave the light with a reddish hue.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public virtual void setAmbientLight(int r, int g, int b)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("ambientLight ");
            builder.Append(r);
            builder.Append(" ");
            builder.Append(g);
            builder.Append(" ");
            builder.Append(b);
            this.add(builder);
        }

        /// <summary>
        /// Modifies the ambient light level, with RGB values from 0 to 255. Note that it works by removing colors from the existing light ambience, so ambientLight 1 80 80 would reduce green and blue and leave the light with a reddish hue.
        /// </summary>
        /// <param name="color"></param>
        public virtual void setAmbientLight(Color color)
        {
            this.setAmbientLight(color.R, color.G, color.B);
        }


    }
}
