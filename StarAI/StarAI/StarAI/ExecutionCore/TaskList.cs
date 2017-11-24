using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarAI.ExecutionCore
{
    class TaskList
    {
        public static List<CustomTask> taskList = new List<CustomTask>();
        public static Task executioner = new Task(new Action(runTaskList));
       
        public static void runTaskList()
        {
            
            //myTask t = new myTask(StarAI.PathFindingCore.CropLogic.CropLogic.harvestSingleCrop);
             
            List<CustomTask> removalList = new List<CustomTask>();
            bool assignNewTask = true;
            for (int i= 0; i < taskList.Count;i++)
            {
                ModCore.CoreMonitor.Log("I value: " + i);
                ModCore.CoreMonitor.Log("Count: " + taskList.Count);
                ModCore.CoreMonitor.Log("GAOL AMOUNT: " + PathFindingCore.PathFindingLogic.goals.Count);
                CustomTask v = taskList[i];
                //  v.Start();

                v.runTask();

               
            }
            foreach(var v in removalList)
            {
                taskList.Remove(v);
            }

            
        }
    }
}
