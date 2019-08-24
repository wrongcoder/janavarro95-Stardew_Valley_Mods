using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PyTK.CustomElementHandler;
using StardewValley;
using StardewValley.Objects;

namespace Revitalize.Framework.Objects.Items
{
    internal class MyTool : Tool, ISaveElement
    {
        public string something;
        public object getReplacement()
        {
            return new Chest(true);
        }

        public Dictionary<string, string> getAdditionalSaveData()
        {
            Dictionary<string, string> saveData = new Dictionary<string, string>();
            saveData.Add("something", "myValue");
            return saveData;
        }

        public void rebuild(Dictionary<string, string> additionalSaveData, object replacement)
        {
            this.something = additionalSaveData["something"];
            ModCore.log("What is my something: " + this.something);
        }

        protected override string loadDisplayName()
        {
            return "Pytk tool";
        }

        protected override string loadDescription()
        {
            return "My description";
        }

        public override Item getOne()
        {
            return new MyTool();
        }
    }
}
