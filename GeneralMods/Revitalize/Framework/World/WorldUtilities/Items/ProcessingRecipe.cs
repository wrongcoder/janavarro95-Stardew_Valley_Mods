using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.World.Objects.Items.Utilities;

namespace Omegasis.Revitalize.Framework.World.WorldUtilities.Items
{
    /// <summary>
    /// TODO finish filling this out.
    ///
    /// All the inputs and outputcs for machines, objects and buiildings for items.
    /// </summary>
    public class ProcessingRecipe
    {

        public string id = "";
        public GameTimeStamp timeToProcess = new GameTimeStamp();
        public List<ItemReference> inputs=new List<ItemReference>();
        public List<LootTableEntry> outputs=new List<LootTableEntry>();

        public ProcessingRecipe()
        {

        }

        public ProcessingRecipe(string Id, GameTimeStamp TimeToProcess, ItemReference Input, LootTableEntry Output)
        {
            this.id= Id;
            this.timeToProcess = TimeToProcess;
            this.inputs.Add(Input);
            this.outputs.Add(Output);
        }

        public ProcessingRecipe(string Id, GameTimeStamp TimeToProcess, ItemReference Input, List<LootTableEntry> Output)
        {
            this.id = Id;
            this.timeToProcess = TimeToProcess;
            this.inputs.Add(Input);
            this.outputs.AddRange(Output);
        }

        public ProcessingRecipe(string Id, GameTimeStamp TimeToProcess, List<ItemReference> Input, LootTableEntry Output)
        {
            this.id = Id;
            this.timeToProcess = TimeToProcess;
            this.inputs.AddRange(Input);
            this.outputs.Add(Output);
        }

        public ProcessingRecipe(string id, GameTimeStamp timeToProcess, List<ItemReference> inputs, List<LootTableEntry> outputs)
        {
            this.id = id;
            this.timeToProcess = timeToProcess;
            this.inputs = inputs;
            this.outputs = outputs;
        }

    }
}
