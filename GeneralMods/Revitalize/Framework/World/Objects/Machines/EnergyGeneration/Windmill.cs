using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Revitalize.Framework.Objects.InformationFiles;
using Revitalize.Framework.World.Objects.InformationFiles;
using Revitalize.Framework.Utilities;
using StardewValley;
using StardustCore.Animations;
using System.Xml.Serialization;
using Netcode;

namespace Revitalize.Framework.World.Objects.Machines.EnergyGeneration
{
    [XmlType("Mods_Revitalize.Framework.World.Objects.Machines.EnergyGeneration.Windmill")]
    public class Windmill : Machine
    {

        public readonly NetInt maxDaysToProduceBattery = new NetInt();
        public readonly NetInt daysRemainingToProduceBattery = new NetInt();

        public Windmill() { }

        public Windmill(BasicItemInformation info, Vector2 TileLocation) : base(info, TileLocation)
        {
            this.maxDaysToProduceBattery.Value = 12;
            this.daysRemainingToProduceBattery.Value = this.maxDaysToProduceBattery;
        }

        public override void updateWhenCurrentLocation(GameTime time, GameLocation environment)
        {
            base.updateWhenCurrentLocation(time, environment);
        }

        protected override void initNetFieldsPostConstructor()
        {
            base.initNetFieldsPostConstructor();
            this.NetFields.AddFields(this.maxDaysToProduceBattery, this.daysRemainingToProduceBattery);
        }


        public override Item getOne()
        {
            Windmill component = new Windmill(this.getItemInformation().Copy(), this.TileLocation);
            //component.containerObject = this.containerObject;
            //component.offsetKey = this.offsetKey;
            return component;
        }

        public override void DayUpdate(GameLocation location)
        {
            if (!this.getCurrentLocation().IsOutdoors) return;
            if (this.heldObject.Value != null) return;
            if (Game1.weatherIcon == Game1.weather_rain)
            {
                this.daysRemainingToProduceBattery.Value -= 2;
            }
            else if (Game1.weatherIcon == Game1.weather_lightning)
            {
                this.daysRemainingToProduceBattery.Value -= 3;
            }
            else if (Game1.weatherIcon == Game1.weather_debris)
            {
                this.daysRemainingToProduceBattery.Value -= 4;
            }
            else
            {
                this.daysRemainingToProduceBattery.Value -= 1;
            }
            if (this.daysRemainingToProduceBattery <= 0)
            {
                this.daysRemainingToProduceBattery.Value = this.maxDaysToProduceBattery;
                this.heldObject.Value = ObjectUtilities.getStardewObjectFromEnum(Enums.SDVObject.BatteryPack, 1);
            }
        }
    }
}
