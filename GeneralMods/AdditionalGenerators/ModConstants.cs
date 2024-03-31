using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdditionalGenerators
{
    public static class ModConstants
    {

        /// <summary>
        /// The unqualified object id. Also used for referencing the object for crafting recipe buying + crafting purposes.
        /// </summary>
        public static string BioFuelGeneratorObjectId = "Omegasis.AdditionalGenerators.Objects.BiofuelGenerator";
        /// <summary>
        /// The fully qualified object id for the biofuel generator.
        /// </summary>
        public static string BioFuelGeneratorQualifiedObjectId = "(BC)" + BioFuelGeneratorObjectId;

        /// <summary>
        /// The unqualified object id. Also used for referencing the object for crafting recipe buying + crafting purposes.
        /// </summary>
        public static string GeothermalGeneratorObjectId = "Omegasis.AdditionalGenerators.Objects.GeothermalGenerator";
        /// <summary>
        /// The fully qualified object id for the biofuel generator.
        /// </summary>
        public static string GeothermalGeneratorQualifiedObjectId = "(BC)" + GeothermalGeneratorObjectId;

    }
}
