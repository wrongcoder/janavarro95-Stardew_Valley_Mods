using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;

namespace Omegasis.Revitalize.Framework.Utilities
{
    /// <summary>
    /// Utilities based around machines added by Revitalize and other mods.
    /// </summary>
    public class MachineUtilities
    {

        public static Dictionary<string, List<ResourceInformation>> ResourcesForMachines;


        public static void InitializeResourceList()
        {

            ResourcesForMachines = new Dictionary<string, List<ResourceInformation>>()
            {
                /*
            {"Omegasis.Revitalize.Objects.Machines.BatteryBin" ,new List<ResourceInformation>(){
                new ResourceInformation(new StardewValley.Object((int)Enums.SDVObject.BatteryPack,1),1,1,1,1,1,1,0,0,0,0)
            } },
            {"Omegasis.Revitalize.Objects.Machines.Sandbox",new List<ResourceInformation>(){
                new ResourceInformation(ModCore.ObjectManager.GetItem(MiscEarthenResources.Sand,1),1,1,1,1,1,1,0,0,0,0)
            } }
                */
            };
        }

    }
}
