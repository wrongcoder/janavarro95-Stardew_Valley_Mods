using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.Revitalize.Framework.Constants.Ids.Objects
{
    public static class MachineIds
    {
        /// <summary>
        /// Prefix for machine ids.
        /// </summary>
        public const string PREFIX = "Revitalize.Objects.Machines.";

        public const string WindmillV1 = PREFIX + "WindmillV1";

        //Mining Drills.
        public const string CoalMiningDrill = PREFIX + "CoalMiningDrill";
        public const string ElectricMiningDrill = PREFIX + "ElectricMiningDrill";
        public const string NuclearMiningDrill = PREFIX + "NuclearMiningDrill";
        public const string MagicalMiningDrill = PREFIX + "MagicalMiningDrill";


        //Geode Crushers.
        public const string CoalAdvancedGeodeCrusher = PREFIX + "GeodeCrushers.CoalAdvancedGeodeCrusher";
        public const string ElectricAdvancedGeodeCrusher = PREFIX + "GeodeCrushers.ElectricAdvancedGeodeCrusher";
        public const string NuclearAdvancedGeodeCrusher = PREFIX + "GeodeCrushers.NuclearAdvancedGeodeCrusher";
        public const string MagicalAdvancedGeodeCrusher = PREFIX + "GeodeCrushers.MagicalAdvancedGeodeCrusher";

        //Charcoal Kilns
        public const string AdvancedCharcoalKiln = PREFIX + "CharcoalKilns.AdvancedCharcoalKiln";
        public const string DeluxCharcoalKiln = PREFIX + "CharcoalKilns.DeluxCharcoalKiln";
        public const string SuperiorCharcoalKiln = PREFIX + "CharcoalKilns.SuperiorCharcoalKiln";

        //Furnaces
        public const string ElectricFurnace = PREFIX + "ElectricFurnace";
        public const string NuclearFurnace = PREFIX + "NuclearFurnace";
        public const string MagicalFurnace = PREFIX + "MagicalFurnace";

        //Generated code below this point.
        public const string BurnerGenerator = PREFIX + "EnergyGeneration.BurnerGenerator";
        public const string AdvancedGenerator = "Revitalize.Objects.Machines.EnergyGeneration.AdvancedGenerator";
        public const string NuclearGenerator = "Revitalize.Objects.Machines.EnergyGeneration.NuclearGenerator";
		public const string AdvancedSolarPanel = "Revitalize.Objects.Machines.EnergyGeneration.AdvancedSolarPanel";
		public const string SuperiorSolarPanel = "Revitalize.Objects.Machines.EnergyGeneration.SuperiorSolarPanel";
    }
}
