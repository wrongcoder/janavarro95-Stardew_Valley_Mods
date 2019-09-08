using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PyTK.CustomElementHandler;
using StardustCore.Animations;
using Revitalize.Framework.Illuminate;
using Revitalize.Framework.Utilities;
using StardewValley;
using StardustCore.UIUtilities;
using Newtonsoft.Json;

namespace Revitalize.Framework.Objects
{
    public class BasicItemInformation
    {
        private string _name;
        public string name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
                this.requiresUpdate = true;
            }
        }

        private string _id;
        public string id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
                this.requiresUpdate = true;
            }
        }


        private string _description;
        public string description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
                this.requiresUpdate = true;
            }
        }

        private string _categoryName;
        public string categoryName
        {
            get
            {
                return this._categoryName;
            }
            set
            {
                this._categoryName = value;
                this.requiresUpdate = true;
            }
        }

        private Color _categoryColor;
        public Color categoryColor
        {
            get
            {
                return this._categoryColor;
            }
            set
            {
                this._categoryColor = value;
                this.requiresUpdate = true;
            }
        }

        private int _price;
        public int price
        {
            get
            {
                return this._price;
            }
            set
            {
                this._price = value;
                this.requiresUpdate = true;
            }
        }

        private int _edibility;
        public int edibility
        {
            get
            {
                return this._edibility;
            }
            set
            {
                this._edibility = value;
                this.requiresUpdate = true;
            }
        }



        private int _fragility;
        public int fragility
        {
            get
            {
                return this._fragility;
            }
            set
            {
                this._fragility = value;
                this.requiresUpdate = true;
            }
        }


        private bool _canBeSetIndoors;
        public bool canBeSetIndoors
        {
            get
            {
                return this._canBeSetIndoors;
            }
            set
            {
                this._canBeSetIndoors = value;
                this.requiresUpdate = true;
            }
        }



        private bool _canBeSetOutdoors;
        public bool canBeSetOutdoors
        {
            get
            {
                return this._canBeSetOutdoors;
            }
            set
            {
                this._canBeSetOutdoors = value;
                this.requiresUpdate = true;
            }
        }

        private bool _isLamp;
        public bool isLamp
        {
            get
            {
                return this._isLamp;
            }
            set
            {
                this._isLamp = value;
                this.requiresUpdate = true;
            }
        }

        private string _locationName;
        public string locationName
        {
            get
            {
                return this._locationName;

            }
            set
            {
                this._locationName = value;
                this.requiresUpdate = true;
            }
        }

        private AnimationManager _animationManager;
        public AnimationManager animationManager
        {
            get
            {
                return this._animationManager;
            }
            set
            {
                this._animationManager = value;
                this.requiresUpdate = true;
            }
        }

        private Vector2 _drawPosition;
        public Vector2 drawPosition
        {
            get
            {
                return this._drawPosition;
            }
            set
            {
                this._drawPosition = value;
                this.requiresUpdate = true;
            }
        }


        private Color _drawColor;
        public Color drawColor
        {
            get
            {
                return this._drawColor;
            }
            set
            {
                this._drawColor = value;
                this.requiresUpdate = true;
            }
        }


        private bool _ignoreBoundingBox;
        public bool ignoreBoundingBox
        {
            get
            {
                return this._ignoreBoundingBox;
            }
            set
            {
                this._ignoreBoundingBox = value;
                this.requiresUpdate = true;
            }
        }

        private InventoryManager _inventory;
        public InventoryManager inventory
        {
            get
            {
                return this._inventory;
            }
            set
            {
                this._inventory = value;
                this.requiresUpdate = true;
            }
        }


        private LightManager _lightManager;
        public LightManager lightManager
        {
            get
            {
                return this._lightManager;
            }
            set
            {
                this._lightManager = value;
                this.requiresUpdate = true;
            }
        }

        private Enums.Direction _facingDirection;
        public Enums.Direction facingDirection
        {
            get
            {
                return this._facingDirection;
            }
            set
            {
                this._facingDirection = value;
                this.requiresUpdate = true;
            }
        }


        private int _shakeTimer;
        public int shakeTimer
        {
            get
            {
                return this._shakeTimer;
            }
            set
            {
                this._shakeTimer = value;
                this.requiresUpdate = true;
            }
        }

        private Energy.EnergyManager _energyManager;
        public Energy.EnergyManager EnergyManager
        {
            get
            {
                return this._energyManager;
            }
            set
            {
                this._energyManager = value;
                this.requiresUpdate = true;
            }
        }

        [JsonIgnore]
        public bool requiresUpdate;
        public BasicItemInformation()
        {
            this.name = "";
            this.description = "";
            this.categoryName = "";
            this.categoryColor = new Color(0, 0, 0);
            this.price = 0;
            this.edibility = -300;
            this.canBeSetIndoors = false;
            this.canBeSetOutdoors = false;

            this.animationManager = new AnimationManager();
            this.drawPosition = Vector2.Zero;
            this.drawColor = Color.White;
            this.inventory = new InventoryManager();
            this.lightManager = new LightManager();

            this.facingDirection = Enums.Direction.Down;
            this.id = "";
            this.shakeTimer = 0;
            this.EnergyManager = new Energy.EnergyManager();
        }

        public BasicItemInformation(string name, string id, string description, string categoryName, Color categoryColor,int edibility, int fragility, bool isLamp, int price, bool canBeSetOutdoors, bool canBeSetIndoors, Texture2D texture, AnimationManager animationManager, Color drawColor, bool ignoreBoundingBox, InventoryManager Inventory, LightManager Lights,Energy.EnergyManager EnergyManager=null)
        {
            this.name = name;
            this.id = id;
            this.description = description;
            this.categoryName = categoryName;
            this.categoryColor = categoryColor;
            this.price = price;
            this.edibility = edibility;

            this.canBeSetOutdoors = canBeSetOutdoors;
            this.canBeSetIndoors = canBeSetIndoors;
            this.fragility = fragility;
            this.isLamp = isLamp;

            this.animationManager = animationManager;
            if (this.animationManager.IsNull)
            {
                this.animationManager = new AnimationManager(new Texture2DExtended(), new Animation(new Rectangle(0, 0, 16, 16)), false);
                this.animationManager.getExtendedTexture().texture = texture;
            }

            this.drawPosition = Vector2.Zero;
            this.drawColor = drawColor;
            this.ignoreBoundingBox = ignoreBoundingBox;
            this.inventory = Inventory ?? new InventoryManager();
            this.lightManager = Lights ?? new LightManager();
            this.facingDirection = Enums.Direction.Down;
            this.shakeTimer = 0;

            this.EnergyManager = EnergyManager ?? new Energy.EnergyManager();
        }

        /// <summary>
        /// Gets an x offset for shaking an object. Source code used from game.
        /// </summary>
        /// <returns></returns>
        public int shakeTimerOffset()
        {
            return (this.shakeTimer > 0 ? Game1.random.Next(-1, 2) : 0);
        }

        /// <summary>
        /// Returns a copy of this object.
        /// </summary>
        /// <returns></returns>
        public BasicItemInformation Copy()
        {
            return new BasicItemInformation(this.name, this.id,this.description, this.categoryName, this.categoryColor, this.edibility, this.fragility, this.isLamp, this.price, this.canBeSetOutdoors, this.canBeSetIndoors, this.animationManager.getTexture(), this.animationManager, this.drawColor, this.ignoreBoundingBox, this._inventory.Copy(), this._lightManager.Copy(),this._energyManager.Copy());
        }

        public bool requiresSyncUpdate()
        {
            return this.requiresUpdate || this.animationManagerRequiresUpdate() || this.inventoryManagerRequiresUpdate() || this.lightManagerRequiresUpdate() || this.energyManagerRequiresUpdate();
        }

        public void forceUpdate()
        {
            this.requiresUpdate = true;
        }
        private bool animationManagerRequiresUpdate()
        {
            if (this._animationManager == null) return false;
            else return this._animationManager.requiresUpdate;
        }
        private bool inventoryManagerRequiresUpdate()
        {
            if (this._inventory == null) return false;
            else return this._inventory.requiresUpdate;
        }
        private bool lightManagerRequiresUpdate()
        {
            if (this._lightManager == null) return false;
            else return this._lightManager.requiresUpdate;
        }

        private bool energyManagerRequiresUpdate()
        {
            if (this._energyManager == null) return false;
            else return this._energyManager.requiresUpdate;
        }

        public void cleanAfterUpdate()
        {
            this.requiresUpdate = false;
            this._inventory.requiresUpdate = false;
            this._animationManager.requiresUpdate = false;
            this._lightManager.requiresUpdate = false;
            this._energyManager.requiresUpdate = false;
        }
        
    }
}
