using StarAI.PathFindingCore;
using StarAI.PathFindingCore.WaterLogic;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Tools;
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

        public static List<CustomTask> removalList = new List<CustomTask>();
        public static void runTaskList()
        {
           
            //myTask t = new myTask(StarAI.PathFindingCore.CropLogic.CropLogic.harvestSingleCrop);
             
            bool assignNewTask = true;

            while(ranAllTasks()==false)
            {
            foreach (var task2 in taskList)
            {
                    if (removalList.Contains(task2)) continue;
                    recalculateTask(task2);               
                //task.taskMetaData = new TaskMetaData(task.taskMetaData.name, PathFindingCore.Utilities.calculatePathCost(task.objectParameterDataArray), task.taskMetaData.staminaPrerequisite, task.taskMetaData.toolPrerequisite);
            }
               // ModCore.CoreMonitor.Log("DONE CALCULATING JUNK NOW RUNNING TASK");
            //Some really cool delegate magic that sorts in place by the cost of the action!!!!
            taskList.Sort(delegate (CustomTask t1, CustomTask t2)
            {
                return t1.taskMetaData.cost.CompareTo(t2.taskMetaData.cost);
            });
                CustomTask v = taskList.ElementAt(0);
                int i = 0;
                while (removalList.Contains(v))
                {
                    v = taskList.ElementAt(i);
                    i++;
                }
                //  v.Start();
                bool recalculate= interruptionTasks(v);

                if (recalculate) {
                    recalculateTask(v);
                }
                if (v.taskMetaData.verifyAllPrerequisitesHit() == true)
                {
                    v.runTask();
                    Utilities.clearExceptionListWithName("Child");
                    Utilities.clearExceptionListWithName("Navigation");
                    removalList.Add(v);
                }
                else
                {
                    removalList.Add(v);
                }
            }


            taskList.Clear();
            removalList.Clear();
            
        }

        public static void recalculateTask(CustomTask v)
        {
            object[] oArray = (object[])v.objectParameterDataArray;

            try
            {
                TileNode t = (TileNode)oArray[0];
                ModCore.CoreMonitor.Log("Premtive calculate 1");
                v.taskMetaData.calculateTaskCost(t, false);
                object[] objArr = new object[3];
                objArr[0] = (object)t;
                objArr[1] = (object)v.taskMetaData.path;
                v.objectParameterDataArray = objArr;
            }
            catch (Exception err)
            {
                List<TileNode> t = (List<TileNode>)oArray[0];
                ModCore.CoreMonitor.Log("Premtive calculate 2");
                foreach (var s in Utilities.tileExceptionList)
                {
                    ModCore.CoreMonitor.Log(s.actionType);
                }
                v.taskMetaData.calculateTaskCost(t, false);
                object[] objArr = new object[3];
                objArr[0] = (object)t; //List of trees to use for path calculations
                objArr[1] = (object)v.taskMetaData.path; //The path itself.
                int malcolm = 0;
                ModCore.CoreMonitor.Log("THIS IS MALCOLM:" + malcolm);
                objArr[2] = (object)v.taskMetaData.path.ElementAt(malcolm); //source of whatever is hit.
                v.objectParameterDataArray = objArr;
                Utilities.tileExceptionList.Clear();
            }
        }

        public static bool interruptionTasks(CustomTask v)
        {
            if (v.taskMetaData.name == "Water Crop")
            {
                StardewValley.Tools.WateringCan w = new WateringCan();
                bool found = false;
                foreach (var item in Game1.player.items)
                {
                    if (item == null) continue;
                    if (item.GetType() == typeof(StardewValley.Tools.WateringCan))
                    {
                        w = (WateringCan)item;
                        found = true;
                    }
                }
                if (found == false)
                {
                    removalList.Add(v);
                    return false;
                }
                if (w.WaterLeft == 0)
                {
                    CustomTask waterRefill = WaterLogic.getAllWaterTilesTask(Game1.player.currentLocation);
                    ModCore.CoreMonitor.Log("No water in can. Going to refil");
                    waterRefill.runTask();
                    return true;
                }
                //
            }
            return false;
        }

        public static bool ranAllTasks()
        {
            foreach(CustomTask task in taskList)
            {
                if (removalList.Contains(task)) continue;
                else return false;
            }
            return true;
        }

        public static void printAllTaskMetaData()
        {
            ModCore.CoreMonitor.Log(taskList.Count.ToString());
            foreach (var task in taskList)
            {
                task.taskMetaData.printMetaData();
            }
        }
    }
}
