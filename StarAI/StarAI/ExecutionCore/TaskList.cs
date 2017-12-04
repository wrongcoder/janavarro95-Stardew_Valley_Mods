using StarAI.PathFindingCore;
using StarAI.PathFindingCore.WaterLogic;
using StarAI.TaskCore.MapTransitionLogic;
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

        public static bool pathafterLocationChange;
        public static void runTaskList()
        {
           
            //myTask t = new myTask(StarAI.PathFindingCore.CropLogic.CropLogic.harvestSingleCrop);
             
            bool assignNewTask = true;

            if(TaskPrerequisites.BedTimePrerequisite.enoughTimeToDoTaskStatic() == false)
            {
                CustomTask task = WayPoints.pathToWayPointReturnTask("bed");
                if (task == null) ModCore.CoreMonitor.Log("SOMETHING WENT WRONG WHEN TRYING TO GO TO BED", LogLevel.Error);
                ModCore.CoreMonitor.Log("Not enough time remaining in day. Going home.", LogLevel.Alert);
                task.runTask();
                Utilities.tileExceptionList.Clear();
                taskList.Clear();
                removalList.Clear();
                return;
            }

            while(ranAllTasks()==false||TaskPrerequisites.BedTimePrerequisite.enoughTimeToDoTaskStatic()==false)
            {
                Utilities.tileExceptionList.Clear();
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

            Utilities.tileExceptionList.Clear();
            taskList.Clear();
            removalList.Clear();
            
        }

        public static void recalculateTask(CustomTask v)
        {
            object[] oArray = (object[])v.objectParameterDataArray;
            ModCore.CoreMonitor.Log("RECALCULATING: "+ v.taskMetaData.name);

            if (v.taskMetaData.name.Contains("Path to "))
            {
                Utilities.tileExceptionList.Clear();
                ModCore.CoreMonitor.Log("POKE DEW VALLEY: " + v.taskMetaData.name);
                string[] s = v.taskMetaData.name.Split(' ');
                ModCore.CoreMonitor.Log(s.ElementAt(s.Length-1));
                List<List<TileNode>> newPaths = new List<List<TileNode>>(); 
                newPaths = TaskCore.MapTransitionLogic.WarpGoal.getWarpChainReturn(Game1.player.currentLocation, s.ElementAt(s.Length-1));
                v.taskMetaData.cost = 0;
                int value = 0;
                foreach (var path in newPaths)
                {
                    value+= (path.Count * TaskMetaDataHeuristics.pathCostMultiplier);
                }
                object[] arr = (object[])v.objectParameterDataArray;
                arr[3] = newPaths;
                v.taskMetaData.cost = value;
                v.taskMetaData.pathsToTake = newPaths;
                ModCore.CoreMonitor.Log("IDK ANY MORE: " + v.taskMetaData.cost);
                return;
            }
            Utilities.tileExceptionList.Clear();
            try
            {
                Utilities.tileExceptionList.Clear();
                TileNode t = (TileNode)oArray[0];
                Utilities.tileExceptionList.Clear();
                ModCore.CoreMonitor.Log("Premtive calculate 1");
                v.taskMetaData.calculateTaskCost(t, false);
                object[] objArr = new object[10];
                objArr[0] = (object)t;
                objArr[1] = (object)v.taskMetaData.pathsToTake[0];
                int malcolm = 0;
                objArr[2] = (object)v.taskMetaData.pathsToTake[0].ElementAt(malcolm); //source of whatever is hit.
                try
                {
                    objArr[3] = oArray[3];
                }
                catch (Exception err2)
                {

                }
                try
                {
                    objArr[4] = oArray[4];
                }
                catch (Exception err2)
                {

                }
                try
                {
                    objArr[5] = oArray[5];
                }
                catch (Exception err2)
                {

                }
                v.objectParameterDataArray = objArr;
            }
            catch (Exception err)
            {
               
            }
            
            try
            {
                Utilities.tileExceptionList.Clear();
                List<TileNode> t = (List<TileNode>)oArray[0];
                ModCore.CoreMonitor.Log("Premtive calculate 2");
                foreach (var s in Utilities.tileExceptionList)
                {
                    ModCore.CoreMonitor.Log(s.actionType);
                }
                v.taskMetaData.calculateTaskCost(t, false);
                object[] objArr = new object[10];
                objArr[0] = (object)t; //List of trees to use for path calculations
                objArr[1] = (object)v.taskMetaData.pathsToTake[0]; //The path itself.
                int malcolm = 0;
                ModCore.CoreMonitor.Log("THIS IS MALCOLM:" + malcolm);
                objArr[2] = (object)v.taskMetaData.pathsToTake[0].ElementAt(malcolm); //source of whatever is hit.
                try
                {
                    objArr[3] = oArray[3];
                }
                catch (Exception err2)
                {

                }
                try
                {
                    objArr[4] = oArray[4];
                }
                catch (Exception err2)
                {

                }
                try
                {
                    objArr[5] = oArray[5];
                }
                catch (Exception err2)
                {

                }
                v.objectParameterDataArray = objArr;
                Utilities.tileExceptionList.Clear();
            }
            catch(Exception err)
            {

            }

            try
            {
                Utilities.tileExceptionList.Clear();
                List<List<TileNode>> t = (List<List<TileNode>>)oArray[3];
                ModCore.CoreMonitor.Log("Premtive calculate 3");
                foreach (var s in Utilities.tileExceptionList)
                {
                    ModCore.CoreMonitor.Log(s.actionType);
                }
                v.taskMetaData.calculateTaskCost(t, false);
                object[] objArr = new object[10];
                objArr[0] = (object)t; //List of trees to use for path calculations
                objArr[1] = (object)v.taskMetaData.pathsToTake; //The path itself.
                int malcolm = 0;
                ModCore.CoreMonitor.Log("THIS IS MALCOLM:" + malcolm);
                objArr[2] = (object)v.taskMetaData.pathsToTake[0].ElementAt(malcolm); //source of whatever is hit.
                try
                {
                    objArr[3] = oArray[3];
                }
                catch (Exception err2)
                {

                }
                try
                {
                    objArr[4] = oArray[4];
                }
                catch (Exception err2)
                {

                }
                try
                {
                    objArr[5] = oArray[5];
                }
                catch (Exception err2)
                {

                }
                v.objectParameterDataArray = objArr;
                Utilities.tileExceptionList.Clear();
            }
            catch(Exception err)
            {

            }
        }

        public static bool interruptionTasks(CustomTask v)
        {

            if (v.taskMetaData.bedTimePrerequisite.enoughTimeToDoTask() == false)
            {
                CustomTask task = WayPoints.pathToWayPointReturnTask("bed");
                if (task == null)
                {
                    ModCore.CoreMonitor.Log("SOMETHING WENT WRONG WHEN TRYING TO GO TO BED", LogLevel.Error);
                    return false;
                }
                ModCore.CoreMonitor.Log("Not enough time remaining in day. Going home and removing tasks.", LogLevel.Alert);
                task.runTask();
                return true;
            }

            if (v.taskMetaData.locationPrerequisite.isPlayerAtLocation() == false)
            {
                //Force player to move to that location, but also need the cost again....
                ModCore.CoreMonitor.Log("PLAYERS LOCATION:"+Game1.player.currentLocation.name);
                Utilities.tileExceptionList.Clear();
               CustomTask task= WarpGoal.getWarpChainReturnTask(Game1.player.currentLocation, v.taskMetaData.locationPrerequisite.location.name);
                if (task == null)
                {
                    ModCore.CoreMonitor.Log("SOMETHING WENT WRONG WHEN TRYING TO GO TO" + v.taskMetaData.locationPrerequisite.location.name, LogLevel.Error);
                    return false;
                }
                task.runTask();
                return true;

            }

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
